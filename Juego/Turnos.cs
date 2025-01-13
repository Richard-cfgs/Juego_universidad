namespace Juego
{
    public class Turnos
    {
        public static int cant_jugadores;
        private static bool v = false;
        public static Dictionary<int,List<int>> players = new Dictionary<int,List<int>>();
//funcion para ir llevando a cada jugador y lo que puede hacer
        public static void turnos()
        {
            if(Canserbero.terminar == true)return;
            Console.Clear();    
            Compilar.compilar(0,0,0);
            while(true)
            {
//recorro cada jugador            
                for(int i=1 ; i<=cant_jugadores ; i++){
                    if(Actualizar.verificar_jugador_muerto(i) == 0)continue;
//verificar el uso de habilidades , el fin de los tiempos de habilidades y afectaciones por trampas
                    Actualizar.times(i);
//v es para que cada jugador pueda atacar una sola vez en su turno y llamo a la funcion que revisa que tipo de mov se ejecura
                    v = false;
                    hacer_mov(i); 
                }
//cuando todos los jugadores hagan su mov se mueven los npcs
                Compilar.inf("Se están moviendo los guardianes" , "blue");
                Npcs.npcs_attack();
                Npcs.mover_npcs();
                Npcs.npcs_attack();
                Canserbero.attack();
                Console.Clear();
                Compilar.compilar(0,0,0);
            }
        }
        public static void hacer_mov(int jugador)
        {
//jugador elige el pc a mover
            int id = elegir_pc(jugador);
//hacer movimientos del pc en su velocidad +1 para poder recorrer por todas las opciones
            Compilar.inf("presiona una tecla de mov (A,D,W,S), `Enter´ para usar la habilidad , `Barra Espaciadora´ para atacar o `Esc´ terminar turno" , "magenta");
            int speed = Pcs.pcs[id].speed+1;
            int count_mov = 0;
            for(int j=0 ; j<=speed ; j++)
            {
                Compilar.inf("elige un movimiento", "magenta");
//hacer un mov , habilidad o ataque
                while(true)
                {
                    ConsoleKeyInfo tecla = Console.ReadKey(true);
//ver si es una tecla de mov y si lo puedo hacer
                    int m = mov(tecla , id , count_mov);
                    if(m == 1){
                        count_mov++;
                        Actualizar.tomar_pcs(jugador,id);
                        Trampas.caer_trampa(id);
                        Console.Clear();
                        Compilar.compilar(0,0,0);
                        break;
                    }
                    if(m == 0)Compilar.inf("ya distes el max de mov" , "red");
                    if(m == 2)Compilar.inf("casilla inaxesible" , "red");
//sino es de mov ver si es de habilidad y ejecutarla
                    if(m == -1)
                    {
                        int h = ability(tecla , id);
                        if(h == 1){
                            Console.Clear();
                            Compilar.compilar(0,0,0);
                            break;
                        }
                        if(h == 0)Compilar.inf("la habilidad no esta lista aun" , "red");
//si no es de mov ni habilidad ver si es de ataque
                        if(h == -1)
                        {
                            int a = attack(tecla , id , jugador);
                            if(a == 1){
                                Console.Clear();
                                Compilar.compilar(0,0,0);
                                v = true;
                                break;
                            }
                            if(a == 0)Compilar.inf("no puedes atacar mas durante este turno" , "red");
                            if(a == -1)
                            {
                                if(tecla.Key == ConsoleKey.Escape)return;
                                Compilar.inf("no es un comando" , "red");
                            }
                        }
                    }
                }
            }
            Compilar.inf($"se ha acabado el turno del jugador {jugador}" , "red");
        }
        private static int attack(ConsoleKeyInfo tecla, int id , int jugador)
        {
            if(tecla.Key == ConsoleKey.Spacebar)
            {
                if(v == false)
                {
                    Compilar.inf("elige direccion(arriba-w, abajo-s, derecha-d, izquierda-a) para atacar" , "magenta");
                    while(true)
                    {
                        ConsoleKeyInfo d = Console.ReadKey(true);
                        int dx = 0 , dy = 0;
                        if(d.Key == ConsoleKey.W){dx = -1;Compilar.inf("has atacado hacia arriba", "yellow");}
                        if(d.Key == ConsoleKey.S){dx = 1;Compilar.inf("has atacado hacia abajo" , "yellow");}
                        if(d.Key == ConsoleKey.D){dy = 1;Compilar.inf("has atacado hacia la derecha" , "yellow");}
                        if(d.Key == ConsoleKey.A){dy = -1;Compilar.inf("has atacado hacia la izquierda" , "yellow");}
                        if(dx != 0 || dy != 0){
                            recorrer_ataque(dx , dy , id , jugador);
                            return 1;
                        }
                        Compilar.inf("comando incorrecto" , "red");
                    }
                }
                else return 0;
            }
            else return -1;
        }
        private static void recorrer_ataque(int dx , int dy , int id , int jugador)
        {
//iterar por las posiciones que llega el ataque
            for(int i=0 ; i<=Pcs.pcs[id].range ; i++)
            {
//x , y pos del ataque en esa iteracion
                int x = Pcs.pcs[id].posx + dx*i;
                int y = Pcs.pcs[id].posy + dy*i;
//si es un muro no puedo seguir el ataque
                if(Laberinto.mat[x,y] == 0)return;
                Console.Clear();
                Compilar.compilar(1, x, y);
                if(x == Laberinto.size/2 && y == Laberinto.size/2){
                    Canserbero.healthPoints_canserbero -= Pcs.pcs[id].attackPoints;
                    Canserbero.revisar_muerto_canserbero(id);
                }
//revisar si hay algun pc en esa posicion
                if(Pcs.pos_pcs[(x,y)].Count != 0)
                {
//itero por todos los pc q hay
                    for(int j=0 ; j<Pcs.pos_pcs[(x,y)].Count ; j++)
                    {
                        int index = Pcs.pos_pcs[(x,y)][j];
//si el pc que hay es el que ataco o un compañero no pasa nada o que no este controlado x nadie sino le quito de la vida la fuerza del que ataco
                        if(Pcs.pcs[index].jugador == Pcs.pcs[id].jugador)continue;
                        if(Pcs.pcs[index].jugador == 0)continue;
                        Pcs.pcs[index].healthPoints -= Pcs.pcs[id].attackPoints;
//si la vida es cero mandarlo a la pos inicial y esperar durante 5 turnos
                        Actualizar.revisar_muerto(index , true , jugador);
                    }
                }
//si hay npcs en la posicion hago lo mismo
                if(Npcs.pos_npcs[(x,y)].Count != 0)
                {
                    for(int j=0 ; j<Npcs.pos_npcs[(x,y)].Count ; j++)
                    {
                        int index = Npcs.pos_npcs[(x,y)][j];
                        Npcs.npcs[index].healthPoints -= Pcs.pcs[id].attackPoints;
                        Actualizar.revisar_muerto(index , false, id);
                    }
                }
            }
        }
        private static int mov(ConsoleKeyInfo tecla, int id, int cant_mov_realizados)
        {
            if(tecla.Key == ConsoleKey.A || tecla.Key == ConsoleKey.D || tecla.Key == ConsoleKey.W || tecla.Key == ConsoleKey.S)
            {
                if(cant_mov_realizados == Pcs.pcs[id].speed)return 0;
                else{
                    Pcs.pos_pcs[(Pcs.pcs[id].posx,Pcs.pcs[id].posy)].Remove(id);
                    int dx = 0, dy = 0;
                    if(tecla.Key == ConsoleKey.W)dx--;
                    if(tecla.Key == ConsoleKey.S)dx++;
                    if(tecla.Key == ConsoleKey.D)dy++;
                    if(tecla.Key == ConsoleKey.A)dy--;
                    if((dx != 0 || dy != 0) && Laberinto.verificar_pos(Pcs.pcs[id].posx+dx , Pcs.pcs[id].posy+dy) == 1){
                        Pcs.pcs[id].posx = Pcs.pcs[id].posx+dx;
                        Pcs.pcs[id].posy = Pcs.pcs[id].posy+dy;
                        Pcs.pos_pcs[(Pcs.pcs[id].posx,Pcs.pcs[id].posy)].Add(id);
                        return 1;
                    }
                    return 2;
                }
            }
            else return -1;
        }
        private static int ability(ConsoleKeyInfo tecla, int id)
        {
            if(tecla.Key == ConsoleKey.Enter)
            {
                if(Pcs.pcs[id].downTime > 0)return 0;
                else{
                    Pcs.us_ability(id);
                    return 1;
                }
            }
            else return -1;
        }
        private static int elegir_pc(int jugador)
        {
            while(true)
            {
                Compilar.inf($"Jugador {jugador} elija el id del personaje que quiera mover" ,  "magenta");
                char aux = Console.ReadKey(true).KeyChar;
                if(aux >= '0' && aux < '8')
                {
                    int id = aux - '0';
                    if(Pcs.pcs[id].jugador == jugador)return id;
                }
                Compilar.inf("Ese Héroe no existe o no es aliado suyo" , "red");
            }
        }
    }
}
namespace Juego
{
    public class Turnos
    {
        public static int cant_jugadores;
        public static int count_mov = 0;
        public static int personaje_en_juego = 0;
        private static bool v = false;
        public static Dictionary<int,List<int>> players = new Dictionary<int,List<int>>();
//funcion para ir llevando a cada jugador y lo que puede hacer
        public static void turnos()
        {
            if(Canserbero.terminar == true)return;
            while(true)
            {
//recorro cada jugador       
                personaje_en_juego = -1;   
                count_mov = 0;   
//rebajar tiempo a los pcs que usaron habilidades y dejaron de ser de algun jugador
                for(int i=0 ; i<Pcs.cant_pcs ; i++)
                {
                    if(Pcs.pcs[i].jugador == 0)Actualizar.timesnpcs(i);
                }
                for(int i=1 ; i<=cant_jugadores ; i++){
                    Actualizar.times(i);
                    Compilar.compilar(0,0,0);
                    if(Actualizar.verificar_jugador_muerto(i) == 1)
                    {
//verificar el uso de habilidades , el fin de los tiempos de habilidades y afectaciones por trampas
//v es para que cada jugador pueda atacar una sola vez en su turno y llamo a la funcion que revisa que tipo de mov se ejecura
                        Compilar.compilar(0,0,0);
                        v = false;
                        hacer_mov(i); 
                    }
                }
//cuando todos los jugadores hagan su mov se mueven los npcs
                Compilar.inf("Se están moviendo los guardianes y héroes" , "blue");
                Thread.Sleep(3000);
//marcar para que cada npc ataque una sola vez
                if(Canserbero.healthPoints_canserbero < 50)Canserbero.healthPoints_canserbero += 3;
                if(Canserbero.healthPoints_canserbero > 50)Canserbero.healthPoints_canserbero = 50;
                Npcs.mover_npcs();
                Pcs.caminar_pcs();
                Canserbero.attack();
            }
        }
        public static void hacer_mov(int jugador)
        {
//jugador elige el pc a mover
            int id = elegir_pc(jugador);
//verificar si hay algun pc para tomar como aliado en la pos inicial
            Actualizar.tomar_pcs(id);
//hacer movimientos del pc en su velocidad +1 para poder recorrer por todas las opciones
            count_mov = 0;
            personaje_en_juego = id;
            while(true)
            {
//hacer un mov , habilidad o ataque
                while(true)
                {
                    if(Pcs.pcs[id].jugador == 0)return;
                    Compilar.inf("Haga un movimiento" , "magenta");
                    ConsoleKeyInfo tecla = Console.ReadKey(true);
//ver si es una tecla de mov y si lo puedo hacer
                    int m = mov(tecla , id , count_mov);
                    if(m == 1){
                        count_mov++;
                        Actualizar.tomar_pcs(id);
                        Trampas.caer_trampa(id);
                        Compilar.compilar(0,0,0);
                        break;
                    }
//sino es de mov ver si es de habilidad y ejecutarla
                    if(m == -1)
                    {
                        int h = ability(tecla , id);
                        if(h == 1){
                            Console.Clear();
                            Compilar.compilar(0,0,0);
                            if(id == 6 && Pcs.pcs[6].downTime != 0)return;
                            break;
                        }
                        if(h == 0){
                            Compilar.inf("la habilidad no está lista aun, presiona Enter para continuar." , "red");
                            Actualizar.continuar();
                        }
//si no es de mov ni habilidad ver si es de ataque
                        if(h == -1)
                        {
                            int a = attack(tecla , id);
                            if(a == 1){
                                Compilar.compilar(0,0,0);
                                v = true;
                                break;
                            }
                            if(a == 2)continue;
                            if(a == 0){
                                Compilar.inf("no puedes atacar más durante este turno, presiona Enter para continuar." , "red");
                                Actualizar.continuar();
                            }
                            if(a == -1)
                            {
                                if(tecla.Key == ConsoleKey.Escape){
                                    personaje_en_juego = -1; 
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
        private static int attack(ConsoleKeyInfo tecla, int id)
        {
            if(tecla.Key == ConsoleKey.Spacebar)
            {
                if(v == false)
                {
                    while(true)
                    {
                        if(Pcs.pcs[id].range != 0)
                        {
                            Compilar.inf("elige direccion para atacar o Esc para cancelar" , "magenta");
                            ConsoleKeyInfo d = Console.ReadKey(true);
                            int dx = 0 , dy = 0;
                            if(d.Key == ConsoleKey.W || d.Key == ConsoleKey.UpArrow)dx--;
                            if(d.Key == ConsoleKey.S || d.Key == ConsoleKey.DownArrow)dx++;
                            if(d.Key == ConsoleKey.D || d.Key == ConsoleKey.RightArrow)dy++;
                            if(d.Key == ConsoleKey.A || d.Key == ConsoleKey.LeftArrow)dy--;
                            if(dx != 0 || dy != 0){
                                recorrer_ataque(dx , dy , id);
                                return 1;
                            }
                            if(d.Key == ConsoleKey.Escape)return 2;
                        }
                        else
                        {
                            recorrer_ataque(0 , 0 , id);
                            return 1;
                        }
                    }
                }
                else return 0;
            }
            else return -1;
        }
        private static void recorrer_ataque(int dx , int dy , int id)
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
                    int[] revisar = new int[8];
                    int count = 0;
//itero por todos los pc q hay
                    for(int j=0 ; j<Pcs.pos_pcs[(x,y)].Count ; j++)
                    {
                        int index = Pcs.pos_pcs[(x,y)][j];
//si el pc que hay es el que ataco o un compañero no pasa nada o que no este controlado x nadie sino le quito de la vida la fuerza del que ataco
                        if(Pcs.pcs[index].jugador == Pcs.pcs[id].jugador)continue;
                        if(Pcs.pcs[index].jugador == 0)continue;
                        Pcs.pcs[index].healthPoints -= Pcs.pcs[id].attackPoints;
                        revisar[count++] = index;
                    }
                    for(int j=0 ; j<count ; j++)Actualizar.revisar_muerto(revisar[j] , true , id);
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
            if(tecla.Key == ConsoleKey.A || tecla.Key == ConsoleKey.D || tecla.Key == ConsoleKey.W || tecla.Key == ConsoleKey.S || tecla.Key == ConsoleKey.UpArrow || tecla.Key == ConsoleKey.DownArrow || tecla.Key == ConsoleKey.RightArrow || tecla.Key == ConsoleKey.LeftArrow)
            {
                if(cant_mov_realizados >= Pcs.pcs[id].speed)return 0;
                else{
                    Pcs.pos_pcs[(Pcs.pcs[id].posx,Pcs.pcs[id].posy)].Remove(id);
                    int dx = 0, dy = 0;
                    if(tecla.Key == ConsoleKey.W || tecla.Key == ConsoleKey.UpArrow)dx--;
                    if(tecla.Key == ConsoleKey.S || tecla.Key == ConsoleKey.DownArrow)dx++;
                    if(tecla.Key == ConsoleKey.D || tecla.Key == ConsoleKey.RightArrow)dy++;
                    if(tecla.Key == ConsoleKey.A || tecla.Key == ConsoleKey.LeftArrow)dy--;
                    if((dx != 0 || dy != 0) && Laberinto.verificar_pos(Pcs.pcs[id].posx+dx , Pcs.pcs[id].posy+dy) == 1)
                    {
                        Pcs.pcs[id].posx = Pcs.pcs[id].posx+dx;
                        Pcs.pcs[id].posy = Pcs.pcs[id].posy+dy;
                        Pcs.pos_pcs[(Pcs.pcs[id].posx,Pcs.pcs[id].posy)].Add(id);
                        if(Pcs.pcs[id].posx+dx >= (Laberinto.size/2)-2 && Pcs.pcs[id].posx+dx <= (Laberinto.size/2)+2 && Pcs.pcs[id].posy+dy >= (Laberinto.size/2)-2 && Pcs.pcs[id].posy+dy <= (Laberinto.size/2)+2)
                        {
                            Pcs.pcs[id].healthPoints--;
                            Actualizar.revisar_muerto(id , true , -2);
                        }
                        return 1;
                    }
                    Pcs.pos_pcs[(Pcs.pcs[id].posx,Pcs.pcs[id].posy)].Add(id);
                    return 0;
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
            int pos = 0;
            while(true)
            {
                Compilar.inf_pcs(players[jugador][pos]);
                Compilar.inf("Elija el personaje que quiera mover" , "magenta");
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if(tecla.Key == ConsoleKey.D || tecla.Key == ConsoleKey.RightArrow)
                {
                    pos++;
                    if(pos >= players[jugador].Count)pos = 0;
                }
                if(tecla.Key == ConsoleKey.A || tecla.Key == ConsoleKey.LeftArrow)
                {
                    pos--;
                    if(pos < 0)pos = players[jugador].Count-1;
                }
                if(tecla.Key == ConsoleKey.Enter)
                {
                    return players[jugador][pos];
                }
            }
        }
    }
}
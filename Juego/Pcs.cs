namespace Juego
{
    public class Pcs
    {
        public static int pos_original_x = 0;
        public static int pos_original_y = 0;
        public static int[] d = {0,1,2,3};
        public static int cant_pcs = 8;
        public static bool[] pcs_principales = new bool[cant_pcs];
        public static int[] m = {0,1,2,3,4,5,6,7};
        public int id { get; set; }
        public string emoji{ get ; set; }
        public string name { get; set; }
        public int healthPoints { get; set; }
        public int attackPoints { get; set; }
        public int range { get; set; }
        public int speed { get; set; }     
        public int abilityTimeOriginal { get; set; }  
        public int abilityTime { get; set; }
        public int downTimeOriginal { get; set; }
        public int downTime { get; set; }
        public int affectedTurns { get; set; }
        public int posx { get; set; }
        public int posy { get; set; }
        public int jugador { get; set; }
        //propiedades que van a tener todo personaje
        public Pcs(int id, string emoji, string name, int healthPoints, int attackPoints, int range, int speed, int abilityTimeOriginal, int abilityTime, int downTimeOriginal, int downTime, int affectedTurns, int x, int y , int jugador)
        {
            this.id = id;
            this.emoji = emoji;
            this.name = name;
            this.healthPoints = healthPoints;
            this.attackPoints = attackPoints;
            this.range = range;
            this.speed = speed;
            this.abilityTimeOriginal = abilityTimeOriginal;
            this.abilityTime = abilityTime;
            this.downTimeOriginal = downTimeOriginal;
            this.downTime = downTime;
            this.affectedTurns = affectedTurns;
            posx = x;
            posy = y;
            this.jugador = jugador;
        }
        //lista donde voy a guardar los personajes
        public static List<Pcs>pcs = new List<Pcs>();
        //diccionario donde voy a guardar los indices de cada personaje en sus posiciones
        public static Dictionary<(int,int),List<int>>pos_pcs = new Dictionary<(int,int),List<int>>();
        public static void crear_pcs()
        {  
            for(int i=0 ; i<cant_pcs ; i++)
            {
                inf_pcs(i , true);
            }
        }  
        public static void inf_pcs(int id , bool v)
        {
//crear pos aleatorias para la creacion de personajes
                if(v == true)Generacion_Aleatoria.generar(2,1);
                int x = Generacion_Aleatoria.posx;
                int y = Generacion_Aleatoria.posy;
                if(id == 0){
                    //marine americano retirado, habilidad de poder tirar granadas a una distancia 2 que tiene un daño de area de 20 donde caiga y 10 al rededor a una casilla de distancia
                    pcs.Insert(0, new Pcs(0, "🧨", "Jake Thompson", 20, 10, 3, 5, 1, 0, 2, 0, 0, x, y, 0)); 
                    pos_pcs[(x,y)].Add(id);
                }
                if(id == 1){
                    //samuray, habilidad de moverse hasta 5 casillas atravezando paredes y eliminando a todo enemigo q esté a su paso
                    pcs.Insert(1, new Pcs(1, "👺","Kaito Yamamoto", 25, 15, 0, 8, 1, 0, 4, 0, 0, x, y, 0));
                    pos_pcs[(x,y)].Add(id);
                }
                if(id == 2){
                    //mago, habilidad de teletransportarse o teletransportar a cualquier enemigo exepto a canserbero hasta 5 casillas de distancia 
                    pcs.Insert(2, new Pcs(2, "🎩","Merlinus the Enchanter", 15, 20, 2, 3, 1, 0, 4, 0, 0, x, y, 0));
                    pos_pcs[(x,y)].Add(id);
                }
                if(id == 3){
                    //cientifico loco que crea un robot y lo hace su cuerpo, habilidad de endurecimiento duplicando su HP
                    pcs.Insert(3, new Pcs(3, "🤖", "Dr. Frankenstein", 40 , 5, 1, 5, 2, 0, 1, 0, 0, x, y, 0));
                    pos_pcs[(x,y)].Add(id);
                }
                if(id == 4){
                    //hombre lobo, habilidad de transormarse en lobo donde aumenta su vida en 20, su ataque en 10, y su velocidad en 10
                    pcs.Insert(4, new Pcs(4, "🐺", "Lupus", 15, 5, 0, 4, 3, 0, 2, 0, 0, x, y, 0));
                    pos_pcs[(x,y)].Add(id);
                }
                if(id == 5){
                    //jugador de NBA, habilidad de saltar los muros del laberinto
                    pcs.Insert(5, new Pcs(5, "🏀", "The Jumper Smith", 10, 1, 0, 5, 1, 0, 1, 0, 0, x, y, 0));
                    pos_pcs[(x,y)].Add(id);
                }
                if(id == 6){
                    //mutante, habilidad de entrar a la mente de cualquier jugador y controlarlo 
                    pcs.Insert(6, new Pcs(6, "🧠", "Cerebra", 15, 8, 5, 4, 1, 0, 3, 0, 0, x, y, 0));
                    pos_pcs[(x,y)].Add(id);
                }
                if(id == 7){
                    //demonio "personaje especial del laberinto infierno" habilidad de mover a todos los npcs, desventaja q no puede tomar a otros personajes como aliados
                    pcs.Insert(7, new Pcs(7, "😈", "Dante", 30, 20, 1, 10, 1 , 0, 1, 0, 0, x, y, 0));
                    pos_pcs[(x,y)].Add(id);
                }
        }
        public static void pos_pcs_elegidos(int jugador, int id)
        {
            int max = Laberinto.size-2;
            int[] x = {1,max,1,max};
            int[] y = {1,max,max,1};
            pos_pcs[(pcs[id].posx,pcs[id].posy)].Remove(id);
            pcs[id].posx = x[d[jugador-1]];
            pcs[id].posy = y[d[jugador-1]];
            Laberinto.mat[pcs[id].posx,pcs[id].posy] = 1;
            pos_pcs[(pcs[id].posx,pcs[id].posy)].Add(id);
        }
        public static void us_ability(int id)
        {
            if(id == 0 || id == 1)
            {
                while(true)
                {
                    
                    if(id == 0)Compilar.inf("Elija la dirección (W,S,D,A) para lanzar la granada o ESC para cancelar" , "magenta");
                    else Compilar.inf("elija la direccion (W,S,D,A) para utilizar el impulso o Esc para cancelar" , "magenta");

                    ConsoleKeyInfo d = Console.ReadKey(true);
                    int x = 0 , y = 0;
                    if(d.Key == ConsoleKey.W)x = -2;
                    if(d.Key == ConsoleKey.S)x = 2;
                    if(d.Key == ConsoleKey.D)y = 2;
                    if(d.Key == ConsoleKey.A)y = -2;
                    if(d.Key == ConsoleKey.Escape)return;
                    if(x == 0 && y == 0){
                        Compilar.inf("comando incorrecto" , "red");
                        Actualizar.continuar();
                    }
                    else{
                        if(pcs[id].abilityTimeOriginal == 0)pcs[id].abilityTime = 1;
                        else pcs[id].abilityTime = pcs[id].abilityTimeOriginal;
                        pcs[id].downTime = pcs[id].downTimeOriginal;
                        if(id == 0)abilidad_pc0(pcs[id].posx + x , pcs[id].posy + y);
                        if(id == 1)abilidad_pc1(x/2 , y/2);
                        return;
                    }
                }
            }
            if(id == 2)
            {
                while(true)
                {
                    Compilar.inf("elija id del Héroe a teletransportar o 8 para cancelar" , "magenta");
                    char aux = Console.ReadKey(true).KeyChar;
                    if(aux == '8')return;
                    if(aux >= '0' && aux < '8')
                    {
                        int index = aux-'0';
                        pos_original_x = pcs[index].posx;
                        pos_original_y = pcs[index].posy;
                        pos_pcs[(pos_original_x,pos_original_y)].Remove(index);
                        Console.Clear();
                        Compilar.compilar(2 , pos_original_x , pos_original_y);
                        while(true)
                        {
                            Compilar.inf("utiliza las teclas de mov para desplazarte hasta llegar a la pos deseada para la teletransportacion y presina Enter cuando quieras parar o Esc para volver a elegir Héroe" , "magenta");
                            ConsoleKeyInfo tecla = Console.ReadKey(true);                            
                            int x1 = 0, y1 = 0;
                            if(tecla.Key == ConsoleKey.Escape)break;
                            if(tecla.Key == ConsoleKey.W)x1--;
                            if(tecla.Key == ConsoleKey.S)x1++;
                            if(tecla.Key == ConsoleKey.D)y1++;
                            if(tecla.Key == ConsoleKey.A)y1--;     
                            int pos_x = pcs[index].posx+x1;                         
                            int pos_y = pcs[index].posy+y1;                         
                            if((x1 != 0 || y1 != 0) && Laberinto.verificar_pos(pos_x , pos_y) != -1 && pos_original_x-6 < pos_x && pos_x < pos_original_x+6 && pos_original_y-6 < pos_y && pos_y < pos_original_y+6){
                                pcs[index].posx = pos_x;
                                pcs[index].posy = pos_y;
                                Console.Clear();
                                Compilar.compilar(2 , pos_x , pos_y);
                            }
                            else
                            {
                                if(tecla.Key == ConsoleKey.Enter && Laberinto.verificar_pos(pcs[index].posx , pcs[index].posy) == 1){
                                    pos_pcs[(pcs[index].posx , pcs[index].posy)].Add(index);
                                    Console.Clear();
                                    Compilar.compilar(0,0,0);
                                    Trampas.caer_trampa(2);
                                    if(pcs[id].abilityTimeOriginal == 0)pcs[id].abilityTime = 1;
                                    else pcs[id].abilityTime = pcs[id].abilityTimeOriginal;
                                    pcs[id].downTime = pcs[id].downTimeOriginal;
                                    return;
                                }
                                else{
                                    Compilar.inf("posicion invalida o comando incorrecto" , "red");
                                    Actualizar.continuar();
                                }
                            }
                        }
                    }
                    else{
                        Compilar.inf("id erroneo" , "red");
                        Actualizar.continuar();
                    }
                }
            }
            if(id == 3){
                pcs[id].healthPoints *= 2;
                if(pcs[id].abilityTimeOriginal == 0)pcs[id].abilityTime = 1;
                else pcs[id].abilityTime = pcs[id].abilityTimeOriginal;
                pcs[id].downTime = pcs[id].downTimeOriginal;
            }
            if(id == 4){
                pcs[id].healthPoints += 5;
                pcs[id].attackPoints += 5;
                pcs[id].speed += 6;
                if(pcs[id].abilityTimeOriginal == 0)pcs[id].abilityTime = 1;
                else pcs[id].abilityTime = pcs[id].abilityTimeOriginal;
                pcs[id].downTime = pcs[id].downTimeOriginal;
                }
            if(id == 5)
            {
                while(true)
                {
                    Compilar.inf("presione una tecla de mov para dar el super salto de dos casillas de distancia o Esc para cancelar" , "magenta");
                    ConsoleKeyInfo tecla = Console.ReadKey(true);
                    int x1 = 0, y1 = 0;
                    if(tecla.Key == ConsoleKey.W)x1 = -2;
                    if(tecla.Key == ConsoleKey.S)x1 = 2;
                    if(tecla.Key == ConsoleKey.D)y1 = 2;
                    if(tecla.Key == ConsoleKey.A)y1 = -2;
                    if((x1 != 0 || y1 != 0) && Laberinto.verificar_pos(pcs[id].posx+x1 , pcs[id].posx+y1) == 1){
                        pos_pcs[(pcs[id].posx , pcs[id].posy)].Remove(id);
                        pcs[id].posx += x1;
                        pcs[id].posy += y1;
                        pos_pcs[(pcs[id].posx , pcs[id].posy)].Add(id);
                        if(pcs[id].abilityTimeOriginal == 0)pcs[id].abilityTime = 1;
                        else pcs[id].abilityTime = pcs[id].abilityTimeOriginal;
                        pcs[id].downTime = pcs[id].downTimeOriginal;
                        Trampas.caer_trampa(5);
                        return;
                    }
                    if(tecla.Key == ConsoleKey.Escape)return;
                    Compilar.inf("posición inaccesible" , "red");
                    Actualizar.continuar();
                }
            }
            if(id == 6)
            {
                while(true)
                {
                    Compilar.inf("elige el id del jugador que quieres controlar o 8 para cancelar" , "magenta");
                    char index = Console.ReadKey(true).KeyChar;
                    if(index == '8')return;
                    if(index >= '0' && index < '8')
                    {
                        Turnos.hacer_mov(index);
                        if(pcs[id].abilityTimeOriginal == 0)pcs[id].abilityTime = 1;
                        else pcs[id].abilityTime = pcs[id].abilityTimeOriginal;
                        pcs[id].downTime = pcs[id].downTimeOriginal;
                        return;
                    }
                }   
            }
            if(id == 7)
            {
                if(pcs[id].abilityTimeOriginal == 0)pcs[id].abilityTime = 1;
                else pcs[id].abilityTime = pcs[id].abilityTimeOriginal;
                pcs[id].downTime = pcs[id].downTimeOriginal;
                for(int i=0 ; i<Npcs.cant_npcs ; i++)
                {
                    int count_pasos = 0;
                    Console.Clear();
                    Compilar.compilar(1 , Npcs.npcs[i].posx , Npcs.npcs[i].posy);
                    while(true)
                    {
                        Compilar.inf("elige un comando de moverte para mover el guaridan señalado, Enter para pasar de guardian o Esc para terminar" , "magenta");
                        ConsoleKeyInfo tecla = Console.ReadKey(true);
                        int x1 = 0, y1 = 0;
                        if(count_pasos == Npcs.speed){
                            Compilar.inf("el guardian ya ha dado el max de pasos" , "red");
                            Actualizar.continuar();
                            break;
                        }
                        if(tecla.Key == ConsoleKey.W)x1--;
                        if(tecla.Key == ConsoleKey.S)x1++;
                        if(tecla.Key == ConsoleKey.D)y1++;
                        if(tecla.Key == ConsoleKey.A)y1--;
                        if((x1 != 0 || y1 != 0) && Laberinto.verificar_pos(Npcs.npcs[i].posx+x1 , Npcs.npcs[i].posx+y1) == 1){
                            Npcs.pos_npcs[(Npcs.npcs[i].posx , Npcs.npcs[i].posy)].Remove(i);
                            Npcs.npcs[i].posx += x1;
                            Npcs.npcs[i].posy += y1;
                            Npcs.pos_npcs[(Npcs.npcs[i].posx , Npcs.npcs[i].posy)].Add(i);
                            count_pasos++;
                            Console.Clear();
                            Compilar.compilar(1 , Npcs.npcs[i].posx , Npcs.npcs[i].posy);
                        }
                        if(tecla.Key == ConsoleKey.Enter)break;
                        if(tecla.Key == ConsoleKey.Escape)return;
                    }
                }
            }
        }
//usar la habilidad del pc1, veo todas las pos vecinas de donde cae la granada y en ella misma y quito vida 
        private static void abilidad_pc0(int x , int y)
        {
            bool v = false;
            for(int i=0 ; i<=4 ; i++)
            {
                int x1 , y1 , daño;
                if(i == 4){x1 = x;y1 = y;daño = 20;}
                else{x1 = x + Laberinto.dx[i];y1 = y + Laberinto.dy[i];daño = 10;}
                quitar_HP(x1,y1,daño,0);
                if(Laberinto.verificar_pos(x1,y1) != -1)v = true;
            }
            Console.Clear();
            if(v == false){
                Compilar.inf("explocion de granada fuera del laberinto" , "yellow");
                Actualizar.continuar();
            }
            else Compilar.compilar(3,x,y);
        }
//usar la habilidad del pc 1,itero desde la ultima posicion que puede llegar el pc para revisar cual es la max y despues voy caminando desde 0 hasta la maxima quitando vida a los enemigos
        private static void abilidad_pc1(int x , int y)
        {
            int max = 0;
            for(int i=5 ; i>=0 ; i--){
                int x1 = pcs[1].posx + x*i;
                int y1 = pcs[1].posy + y*i;
                if(Laberinto.verificar_pos(x1,y1) == 1){
                    max = i;
                    break;
                }
            }
            int px = pcs[1].posx;
            int py = pcs[1].posy;
            pos_pcs[(pcs[1].posx,pcs[1].posy)].Remove(1);
            for(int i=0 ; i<=max ; i++)
            {
                int x1 = px + x*i;
                int y1 = py + y*i;
                if(Laberinto.verificar_pos(x1,y1) == 1)
                {
                    pcs[1].posx = x1;
                    pcs[1].posy = y1;
                    quitar_HP(x1 , y1 , 10 , 1);
                }
                if(Laberinto.verificar_pos(x1,y1) == -1)
                {
                    Compilar.inf("has chocado con el fin del laberinto" , "red");
                    Actualizar.continuar();
                    break;
                }
                Thread.Sleep(3000);
                Console.Clear();
                Compilar.compilar(1,x1,y1);
            }
            pos_pcs[(pcs[1].posx,pcs[1].posy)].Add(1);
            Console.Clear();
            Compilar.compilar(0,0,0);
            Trampas.caer_trampa(1);
        }
//metodo para revisar una pos y quitar Hp a todos los que esten un daño predefinido
        public static void quitar_HP(int x1 , int y1 , int daño , int id_asesino)
        {
            if(x1 == Laberinto.size/2 && y1 == Laberinto.size/2){
                    Canserbero.healthPoints_canserbero -= daño;
                    Canserbero.revisar_muerto_canserbero(id_asesino);
                }
            if(Laberinto.verificar_pos(x1,y1) == 1){
                if(pos_pcs[(x1,y1)].Count != 0)
                {
                    for(int j=0 ; j<pos_pcs[(x1,y1)].Count ; j++)
                    {
                        int index = pos_pcs[(x1,y1)][j];
                        pcs[index].healthPoints -= daño;
                        Actualizar.revisar_muerto(index,true,id_asesino);
                    }                
                }
                if(Npcs.pos_npcs[(x1,y1)].Count != 0)
                {
                    for(int j=0 ; j<Npcs.pos_npcs[(x1,y1)].Count ; j++)
                    {
                        int index = Npcs.pos_npcs[(x1,y1)][j];
                        Npcs.npcs[index].healthPoints -= daño;
                        Actualizar.revisar_muerto(index,false,id_asesino);
                    }
                }
            }
        }
        public static void caminar_pcs()
        {
            for(int i=0 ; i<cant_pcs ; i++)
            {
                if(pcs[i].jugador == 0)
                {
                    int[] m = {0 , 1 , 2 , 3};
                    Laberinto.barajear_direcciones(m);
                    for(int j=0 ; j<4 ; j++){
                        int x = pcs[i].posx + Laberinto.dx[j];
                        int y = pcs[i].posy + Laberinto.dy[j];
                        if(Laberinto.verificar_pos(x,y) == 1){
                            Thread.Sleep(1000);
                            Compilar.compilar(1,pcs[i].posx,pcs[i].posy);
                            pos_pcs[(pcs[i].posx,pcs[i].posy)].Remove(i);
                            pcs[i].posx = x;
                            pcs[i].posy = y;
                            pos_pcs[(pcs[i].posx,pcs[i].posy)].Add(i);
                            Thread.Sleep(1000);
                            Compilar.compilar(1,pcs[i].posx,pcs[i].posy);
                        }
                    }
                }
            }
        }
    }
}
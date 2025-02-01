namespace Juego
{
    public class Npcs
    {
        public static int speed = 3;
        public static int range = 1;
        public static int fuerza = 5;
        public static int cant_npcs = 20;
        public static int[] m1 = new int[Npcs.cant_npcs];
        public static List<Npcs>npcs = new List<Npcs>();
        public static Dictionary<(int,int), List<int>> pos_npcs = new Dictionary<(int,int),List<int>>();
        public int healthPoints { get; set; } 
        public int posx { get; set; }
        public int posy { get; set; }
        public int direccion { get; set; }
        public Npcs(int healthPoints, int posx, int posy ,int d)
        {
            this.healthPoints = healthPoints;
            this.posx = posx;
            this.posy = posy;
            direccion = d;
        }
        public static void crear_npcs()
        {   
//add los npcs a la lista y guardo sus pos en un diccionario
            for(int i=0 ; i<cant_npcs ; i++)
            {   
//elegir nuevas pos aleatorias 
                Generacion_Aleatoria.generar(4,1);
                npcs.Insert(i, new Npcs(10, Generacion_Aleatoria.posx, Generacion_Aleatoria.posy, i%4));
                pos_npcs[(Generacion_Aleatoria.posx,Generacion_Aleatoria.posy)].Add(i);             
            }
        }
        public static void mover_npcs()
        {
            bool[,] ya_atacaron = new bool[cant_npcs,Pcs.cant_pcs];
//un for de 0 a 2 y en cada iteracion cada guardian da un paso  
            for(int q=0 ; q<speed ; q++){
//un for hata la cantidad de guardianes y veo en q direccion puede dar el paso
                for(int i=0 ; i<cant_npcs ; i++)
                {
                    if(npcs[i].healthPoints <= 0)continue;
//en la primera iteracion reviso si puedo atacar a alguien
                    if(q == 0)npcs_attack(ya_atacaron , i);
                    int x = npcs[i].posx + Laberinto.dx[npcs[i].direccion];
                    int y = npcs[i].posy + Laberinto.dy[npcs[i].direccion];
//verifico si el movimiento es posible
                    if(Laberinto.verificar_pos(x,y) == 1)
                    {
                        pos_npcs[(npcs[i].posx,npcs[i].posy)].Remove(i);
                        npcs[i].posx = x;
                        npcs[i].posy = y;
                    }
//si no lo hay barajeo las posibles direcciones del guaridan y veo cual es posible tomar
                    else
                    {
                        int[] mov = {0, 1, 2, 3};
                        Laberinto.barajear_direcciones(mov);
                        for(int k=0 ; k<4 ; k++){
                            x = npcs[i].posx + Laberinto.dx[mov[k]];
                            y = npcs[i].posy + Laberinto.dy[mov[k]];
                            if(Laberinto.verificar_pos(x,y) == 1)
                            {
                                pos_npcs[(npcs[i].posx,npcs[i].posy)].Remove(i);
                                npcs[i].posx = x;
                                npcs[i].posy = y;
                                npcs[i].direccion = mov[k];
                                break;
                            }
                        }
                    }
                    pos_npcs[(npcs[i].posx,npcs[i].posy)].Add(i);
                    npcs_attack(ya_atacaron , i);
                }
            }
        }
        public static void npcs_attack(bool[,] ya_atacaron , int id)
        {
//iterar por las pos a las que llega el ataque
            for(int i=0 ; i<=range ; i++)
            {
//si el rango es 1 reviso si hay alguien en esa pos
                int[] revisar = new int[8];
                int count = 0;
                if(i == 0)
                {
                    foreach(int id_pc in Pcs.pos_pcs[(npcs[id].posx , npcs[id].posy)])
                    {
                        if(Pcs.pcs[id_pc].jugador == 0)continue;
                        if(Pcs.pcs[id_pc].healthPoints <= 0)continue;
                        if( ya_atacaron[id,id_pc] )continue;
                        ya_atacaron[id,id_pc] = true;
                        Compilar.inf($"El Héroe {id_pc} ha sido atacado por los guardianes" , "yellow");
                        Thread.Sleep(2000);
                        Compilar.compilar(1,npcs[id].posx,npcs[id].posy);
                        Thread.Sleep(2000);
                        Pcs.pcs[id_pc].healthPoints -= fuerza;
                        revisar[count++] = id_pc;
                    }
                }
                else
                {
                    for(int j=0 ; j<4 ; j++)
                    {
                        int x = npcs[id].posx + Laberinto.dx[j];
                        int y = npcs[id].posy + Laberinto.dy[j];
                        if(Laberinto.verificar_pos(x,y) == 1)
                        {
                            foreach(int id_pc in Pcs.pos_pcs[(x,y)])
                            {
                                if(Pcs.pcs[id_pc].jugador == 0)continue;
                                if(Pcs.pcs[id_pc].healthPoints <= 0)continue;
                                if( ya_atacaron[id,id_pc] )continue;
                                ya_atacaron[id,id_pc] = true;
                                Compilar.inf($"El Héroe {id_pc} ha sido atacado por los guardianes" , "yellow");
                                Thread.Sleep(2000);
                                Compilar.compilar(1,npcs[id].posx,npcs[id].posy);
                                Thread.Sleep(2000);
                                Pcs.pcs[id_pc].healthPoints -= fuerza;
                                revisar[count++] = id_pc;
                            }
                        }
                    }
                }
                for(int j=0 ; j<count ; j++)Actualizar.revisar_muerto(revisar[j] , true , -1);
            }
        }
    }
}
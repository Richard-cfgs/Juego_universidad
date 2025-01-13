namespace Juego
{
    public class Npcs
    {
        public static int speed = 3;
        public static int range = 2;
        public static int fuerza = 100;
        public static int cant_npcs = 20;
        public static int[] m1 = new int[Npcs.cant_npcs];
        public static List<Npcs>npcs = new List<Npcs>();
        public static Dictionary<(int,int), List<int>> pos_npcs = new Dictionary<(int,int),List<int>>();
        public int healthPoints { get; set; } 
        public int attackPoints { get; set; }
        public int posx { get; set; }
        public int posy { get; set; }
        public int direccion { get; set; }
        public Npcs(int healthPoints, int attackPoints, int posx, int posy ,int d)
        {
            this.healthPoints = healthPoints;
            this.attackPoints = attackPoints;
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
                npcs.Insert(i, new Npcs(5, 5, Generacion_Aleatoria.posx, Generacion_Aleatoria.posy, i%4));
                pos_npcs[(Generacion_Aleatoria.posx,Generacion_Aleatoria.posy)].Add(i);             
            }
        }
        public static void mover_npcs()
        {
//un for de 0 a 2 y en cada iteracion cada guardian da un paso  
            for(int q=0 ; q<speed ; q++){
//un for hata la cantidad de guardianes y veo en q direccion puede dar el paso
                for(int i=0 ; i<cant_npcs ; i++)
                {
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
                    Console.Clear();
                    Compilar.compilar(0,0,0);
                }
            }
        }
        public static void npcs_attack()
        {
//iterar por las pos a las que llega el ataque
            for(int i=0 ; i<=range ; i++)
            {
//revisar a que personaje le llega
                for(int j=0 ; j<Pcs.cant_pcs ; j++)
                {
//si no pertenece a ningun jugador que no lo ataquen
                    if(Pcs.pcs[j].jugador == 0)continue;
//si el rango es 0 revisar esa pos sino reviso todas posibles para atacar
                    if(i == 0)quitar_vida_npc(Pcs.pcs[j].posx , Pcs.pcs[j].posy , j);
                    else{
                        for(int q=0 ; q<4 ; q++)
                        {    
                            int x = Pcs.pcs[j].posx + Laberinto.dx[q]*i;
                            int y = Pcs.pcs[j].posy + Laberinto.dy[q]*i;
                            quitar_vida_npc(x,y,j);
                        }
                    }
                }
                Console.Clear();
                Compilar.compilar(0,0,0);
            }
        }
        private static void quitar_vida_npc(int x , int y , int id)
        {
            if(Laberinto.mat[x,y] == 1)
            {
                if(pos_npcs[(x,y)].Count != 0)
                {
                    Compilar.inf("Has sido atacado por los guardianes" , "yellow");
                    Pcs.pcs[id].healthPoints -= fuerza*pos_npcs[(x,y)].Count;
                    Actualizar.revisar_muerto(id,true,-1);
                    Console.Clear();
                    Compilar.compilar(1,x,y);
                    Thread.Sleep(1000);
                    Console.Clear();
                    Compilar.compilar(1,Pcs.pcs[id].posx,Pcs.pcs[id].posy);
                }
            }
        }
    }
}
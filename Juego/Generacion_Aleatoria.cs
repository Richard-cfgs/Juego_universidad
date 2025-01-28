namespace Juego
{
    public class Generacion_Aleatoria
    {
        public static int posx;
        public static int posy;
        public static Random ram = new Random();
        public static void generar(int a, int cond)
        {
//las posiciones del centro del laberinto
            int inf = (Laberinto.size/2)-4 , sup = (Laberinto.size/2)+4;
//les doy las posiciones iniciales del centro para obligar a entrar al while
            posx = posy = inf;
//mientras las pos esten en el centro busco otras nuevas
            while(posx >= inf && posx <= sup && posy >= inf && posy <= sup)
            {
                posx = ram.Next(a , Laberinto.size-(a+1));
                posy = ram.Next(a , Laberinto.size-(a+1));
            }
//ahora reviso si el lab en la pos no es lo que quiero busco las adyacentes
            if(Laberinto.mat[posx,posy] != cond){
                for(int j=0 ; j<4 ; j++)
                {
                    int x1 = posx + Laberinto.dx[j];
                    int y1 = posy + Laberinto.dy[j];
                    if(Laberinto.mat[x1,y1] == cond)
                    {
                        posx = x1;
                        posy = y1;
                        break;
                    }
                }
            }
            if(Laberinto.mat[posx,posy] != cond)generar(a, cond);
        }
        public static void iniciar_lista()
        {
            for(int i=0 ; i<Laberinto.size ; i++){
                for(int j=0 ; j<Laberinto.size ; j++){
                    if(Laberinto.mat[i,j] == 1){
                        Npcs.pos_npcs[(i,j)] = new List<int>();
                        Pcs.pos_pcs[(i,j)] = new List<int>();
                    }
                }
            }
        }
    }
}
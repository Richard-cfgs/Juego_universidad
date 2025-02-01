namespace Juego
{
    public class Laberinto
    {
        public static int cant_caminos = 20;
        public static int size = 39;
        public static int[] dx = { 1 , -1 , 0 , 0};
        public static int[] dy = { 0 , 0 , 1 , -1};
        public static int[,] mat = new int[size,size];
        public static Random ram = new Random();

        public static void crear()
        {   
//generar el laberinto 
            generar_laberinto(size/2 , size/2);
//abrir el centro del laberinto
            for(int i=size/2-2 ; i<=size/2+2 ; i++){
                for(int j=size/2-2 ; j<=size/2+2 ; j++){
                    mat[i,j] = 1;
                }    
            }
//abrir muros para crear caminos con ciclos
            generar_caminos();   
        }
        public static void generar_laberinto(int x , int y)
        {
//marcar la pos como camino
            mat[x , y] = 1;
//posibles direcciones    
            int[] movimientos = {0 , 1 , 2 , 3};
//barajerar las direcciones
            barajear_direcciones(movimientos);
//elegir las dirrecciones barajeadas
            for(int i=0 ; i<4 ; i++){
//elegir las nuevas pos de 2 en 2
                int x1 = x + (dx[movimientos[i]]*2);
                int y1 = y + (dy[movimientos[i]]*2);
                if(verificar_pos(x1 , y1) == 0)
                {
//marcar la pos en 1 movimiento
                    mat[x+dx[movimientos[i]],y+dy[movimientos[i]]] = 1;
                    generar_laberinto(x1 , y1);
                }
            }
        }
//verificar si una pos esta dentro del laberinto
        public static int verificar_pos(int x , int y)
        {
            if(x > 0 && y > 0 && x < Laberinto.size-1 && y < Laberinto.size-1)return mat[x,y];
            return -1;
        }
//barajear las direcciones para que quede en un array desordenado
        public static void barajear_direcciones(int[] array)
        {
            for(int i=array.Length-1 ; i>0 ; i--)
            {
                int b = ram.Next(0 , i+1);
                int a = array[i];
                array[i] = array[b];
                array[b] = a;
            }
        }
//generar caminos aleatorios
        public static void generar_caminos()
        {
            for(int i=0 ; i < cant_caminos ; i++)
            {
//tomar pos aleatorias que sean muros
                Generacion_Aleatoria.generar(2,0);
                mat[Generacion_Aleatoria.posx,Generacion_Aleatoria.posy] = 1;
            }
        }
    }
}
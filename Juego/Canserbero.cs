using Spectre.Console;
namespace Juego
{    
    public class Canserbero
    {
        public static int healthPoints_canserbero = 50;
        public static void attack()
        {
            int[] revisar = new int[8];
            int count = 0;
            for(int i=Laberinto.size/2-2 ; i<=Laberinto.size/2+2 ; i++){
                for(int j=Laberinto.size/2-2 ; j<=Laberinto.size/2+2 ; j++){
                    if(Pcs.pos_pcs[(i,j)].Count != 0)
                    {
                        for(int q=0 ; q<Pcs.pos_pcs[(i,j)].Count ; q++)
                        {
                            int id = Pcs.pos_pcs[(i,j)][q];
                            if(Pcs.pcs[id].jugador == 0)continue;
                            Pcs.pcs[id].healthPoints -= 7;
                            Compilar.inf($"El Héroe {id} a sido atacado por Canserbero" , "yellow");
                            Thread.Sleep(3000);
                            revisar[count++] = id;
                        }
                    }
                }
            }
            for(int i=0 ; i<count ; i++)Actualizar.revisar_muerto(revisar[i] , true , -2);
        }
        public static void revisar_muerto_canserbero(int id)
        {
            Thread.Sleep(3000);
            if(healthPoints_canserbero <= 0){
                Console.Clear();
                int x = Console.WindowWidth/2;
                Console.CursorLeft = x - 15;
                AnsiConsole.Markup("[blue]EL JUGADOR [/]");
                AnsiConsole.Markup($"[yellow]{Pcs.pcs[id].jugador} HA GANADO [/]");
                AnsiConsole.Markup($"[blue]Y EL HÉROE VENCEOR FUE {Pcs.pcs[id].name} 🎇🎆🔥[/]");
                Thread.Sleep(5000);
                Console.WriteLine("\n\n\n\n\n");
                Thread.Sleep(10000);
                Environment.Exit(0);
            }
        }
    }
}
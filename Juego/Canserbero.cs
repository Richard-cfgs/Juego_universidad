using Spectre.Console;
namespace Juego
{    
    public class Canserbero
    {
        public static int healthPoints_canserbero = 50;
        public static bool terminar = false;
        public static void attack()
        {
            for(int i=Laberinto.size/2-2 ; i<=Laberinto.size/2+2 ; i++){
                for(int j=Laberinto.size/2-2 ; j<=Laberinto.size/2+2 ; j++){
                    if(Pcs.pos_pcs[(i,j)].Count != 0)
                    {
                        for(int q=0 ; q<Pcs.pos_pcs[(i,j)].Count ; q++)
                        {
                            int id = Pcs.pos_pcs[(i,j)][q];
                            if(Pcs.pcs[id].jugador == 0)continue;
                            Pcs.pcs[id].healthPoints -= 10;
                            Actualizar.revisar_muerto(id , true , -1);
                        }
                    }
                }    
            }
        }
        public static int revisar_muerto_canserbero(int id)
        {
            Thread.Sleep(3000);
            if(healthPoints_canserbero <= 0){
                Console.Clear();
                int x = Console.WindowWidth/2;
                Console.SetCursorPosition(x-30,x);
                AnsiConsole.Markup("[magenta]EL JUGADOR [/]");
                AnsiConsole.Markup($"[yellow]{Pcs.pcs[id].jugador} HA GANADO [/]");
                AnsiConsole.Markup($"[red]Y EL HÉROE VENCEOR FUE {Pcs.pcs[id].name} 🎇🎆🔥[/]");
                Thread.Sleep(1000000);
                terminar = true;
                Turnos.turnos();
                return 1;
            }
            return 0;
        }
    }
}
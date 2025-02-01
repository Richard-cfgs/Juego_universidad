using Spectre.Console;
namespace Juego
{
    public class Ajustar_pantalla
    {
        public static void size()
        {
            AnsiConsole.MarkupLine("[blue]Abra la aplicacíon en pantalla completa y presione cualquier tecla para continuar.[/]");
            Console.ReadLine();
            while(true)
            {
                Console.Clear();
//instrucciones
                AnsiConsole.MarkupLine("[blue]Presione (Control) + (-) o (Control) + (+) para configurar el tamaño de la pantalla hasta poder ver los puntos amarrillos en las esquinas, presione cualquier tecla para actualizar o Enter para continuar.[/]");
                int largo = Console.WindowWidth;
                int ancho = Console.WindowHeight;
//puntos en las esquinas
                Console.Write("🔴");
                if(largo > 180)
                {
                    Console.CursorLeft = 180;
                    Console.Write("🔴");
                }
                if(ancho > 40)
                {
                    Console.SetCursorPosition(0,40);
                    Console.Write("🔴");
                }
                if(largo > 180 && ancho > 40)
                {
                    Console.SetCursorPosition(180,40);
                    Console.Write("🔴");
                }

                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if(tecla.Key == ConsoleKey.Enter)return;
            }
        }
    }
}
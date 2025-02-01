using Spectre.Console;
namespace Juego
{
    public class Compilar
    {
        public static int laberinto_ancho = 40;
        public static void compilar(int tipo, int x, int y)
        {
            Console.Clear();
            Console.SetCursorPosition(0,0);
            for(int i=0 ; i<Laberinto.size ; i++)
            {
                for(int j=0 ; j<Laberinto.size ; j++)
                {
                    if(tipo == 1 && x == i && y == j)Console.Write("⬜"); 
                    else
                    {
                        if(tipo == 2 && ((((Math.Abs(i-Pcs.pos_original_x) == 6 && Math.Abs(j-Pcs.pos_original_y) <= 6) || (Math.Abs(i-Pcs.pos_original_x) <= 6 && Math.Abs(j-Pcs.pos_original_y) == 6)) && Laberinto.mat[i,j] == 0) || (i == x && j == y))){
                            if(((Math.Abs(i-Pcs.pos_original_x) == 6 && Math.Abs(j-Pcs.pos_original_y) <= 6) || (Math.Abs(i-Pcs.pos_original_x) <= 6 && Math.Abs(j-Pcs.pos_original_y) == 6)) && Laberinto.mat[i,j] == 0)Console.Write("🟨");
                            if(i == x && j == y)Console.Write("⬜");
                        }
                        else
                        {
                            if(tipo == 3 && ((i == x && j == y) || (i+1 == x && j == y) || (i-1 == x && j == y) || (i == x && j-1 == y) || (i == x && j+1 == y))){
                                Console.Write("⬜");
                            }  
                            else
                            {
                                if(i == j && i == Laberinto.size/2)
                                {
                                    if(Pcs.pos_pcs[(i,j)].Count == 0)Console.Write("👾");
                                    else Console.Write("🎆");
                                }
                                else{
                                    if(Laberinto.mat[i,j] == 0)
                                    {
                                        if(i == 0 || i == Laberinto.size-1 || j == 0 || j == Laberinto.size-1)Console.Write("🟨");
                                        else Console.Write("🟥");
                                    }
                                    else
                                    {
                                        if(Pcs.pos_pcs[(i,j)].Count >= 1)
                                        {
                                            if(Npcs.pos_npcs[(i,j)].Count >= 1)Console.Write("🎆");
                                            else{
                                                for(int q=0 ; q<Pcs.pos_pcs[(i,j)].Count ; q++){
                                                    if(Pcs.pos_pcs[(i,j)][0] != Pcs.pos_pcs[(i,j)][q]){
                                                        Console.Write("🎆");
                                                        break;
                                                    }
                                                    else if(q == Pcs.pos_pcs[(i,j)].Count-1)Console.Write(Pcs.pcs[Pcs.pos_pcs[(i,j)][0]].emoji);
                                                }
                                            }
                                        }
                                        else{
                                            if(Npcs.pos_npcs[(i,j)].Count >= 1)
                                            {
                                                if(Npcs.pos_npcs[(i,j)].Count == 1)Console.Write("👻");
                                                if(Npcs.pos_npcs[(i,j)].Count > 1)Console.Write("💀");
                                            }
                                            else Console.Write("  ");
                                        }
                                    }
                                }
                            }
                        }   
                    }
                }
                Console.WriteLine();
            }
            inf_pcs(-1);
        }
        public static void inf_pcs(int p)
        {
            int inicio_y = 0; 
            int inicio_x = laberinto_ancho*2;    
            for(int i=1 ; i<=Turnos.cant_jugadores ; i++)
            {
                Console.SetCursorPosition(inicio_x,inicio_y);
                AnsiConsole.Markup($"[blue]Jugador {i} - Héroes: [/]");
                foreach(int id in Turnos.players[i])
                {
                    if(p == id || Turnos.personaje_en_juego == id)AnsiConsole.Markup($"[yellow underline]{Pcs.pcs[id].id}{Pcs.pcs[id].emoji} [/]");
                    else AnsiConsole.Markup($"[blue]{Pcs.pcs[id].id}{Pcs.pcs[id].emoji} [/]");
                }
                inicio_y++;
            }
            inicio_y++;
            Console.SetCursorPosition(inicio_x,inicio_y);
            AnsiConsole.MarkupLine($"[blue]Canserbero.HP: {Canserbero.healthPoints_canserbero}[/]");
            inicio_y++;
            for(int id=0 ; id < Pcs.cant_pcs ; id++)
            {
                Console.SetCursorPosition(inicio_x, inicio_y + 1);
                if(Turnos.personaje_en_juego == id)AnsiConsole.MarkupLine($"[yellow underline]Héroe:{Pcs.pcs[id].id} {Pcs.pcs[id].emoji}[/]");
                else AnsiConsole.MarkupLine($"[blue]Héroe:{Pcs.pcs[id].id} {Pcs.pcs[id].emoji}[/]");
                if(Pcs.pcs[id].healthPoints > 0)
                {
                    Console.SetCursorPosition(inicio_x, inicio_y + 2);
                    AnsiConsole.MarkupLine($"[blue]HP: {Pcs.pcs[id].healthPoints}[/]");
                    Console.SetCursorPosition(inicio_x, inicio_y + 3);
                    AnsiConsole.MarkupLine($"[blue]AP: {Pcs.pcs[id].attackPoints}[/]");
                    Console.SetCursorPosition(inicio_x, inicio_y + 4);
                    AnsiConsole.MarkupLine($"[blue]R: {Pcs.pcs[id].range}[/]");
                    Console.SetCursorPosition(inicio_x, inicio_y + 5);
                    if(Turnos.personaje_en_juego == id)
                    {
                        if(Pcs.pcs[id].speed - Turnos.count_mov <= 0)AnsiConsole.MarkupLine($"[red]S: {0}[/]");
                        else AnsiConsole.MarkupLine($"[blue]S: {Pcs.pcs[id].speed - Turnos.count_mov}[/]");
                    }
                    else AnsiConsole.MarkupLine($"[blue]S: {Pcs.pcs[id].speed}[/]");
                    Console.SetCursorPosition(inicio_x, inicio_y + 6);
                    if(id == 0)AnsiConsole.MarkupLine($"[blue]H: Lanzar Granadas[/]");
                    if(id == 1)AnsiConsole.MarkupLine($"[blue]H: Impulso[/]");
                    if(id == 2)AnsiConsole.MarkupLine($"[blue]H: T_Transportacion[/]");
                    if(id == 3)AnsiConsole.MarkupLine($"[blue]H: Endurecimiento[/]");
                    if(id == 4)AnsiConsole.MarkupLine($"[blue]H: Transformacion[/]");
                    if(id == 5)AnsiConsole.MarkupLine($"[blue]H: Super Salto[/]");
                    if(id == 6)AnsiConsole.MarkupLine($"[blue]H: Control Mental[/]");
                    if(id == 7)AnsiConsole.MarkupLine($"[blue]H: Orden[/]");
                    Console.SetCursorPosition(inicio_x, inicio_y + 7);
                    if(Pcs.pcs[id].affectedTurns != 0)AnsiConsole.MarkupLine($"[red]Affected Turns: {Pcs.pcs[id].affectedTurns}[/]");
                    else AnsiConsole.MarkupLine($"[blue]Affected Turns: {Pcs.pcs[id].affectedTurns}[/]");
                    Console.SetCursorPosition(inicio_x, inicio_y + 8);
                    AnsiConsole.MarkupLine($"[blue]AT Original: {Pcs.pcs[id].abilityTimeOriginal}[/]");
                    Console.SetCursorPosition(inicio_x, inicio_y + 9);
                    if(Pcs.pcs[id].abilityTime != 0)AnsiConsole.MarkupLine($"[red]AT Actual: {Pcs.pcs[id].abilityTime}[/]");
                    else AnsiConsole.MarkupLine($"[blue]AT Actual: {Pcs.pcs[id].abilityTime}[/]");
                    Console.SetCursorPosition(inicio_x, inicio_y + 10);
                    AnsiConsole.MarkupLine($"[blue]DT Original: {Pcs.pcs[id].downTimeOriginal}[/]");
                    Console.SetCursorPosition(inicio_x, inicio_y + 11);
                    if(Pcs.pcs[id].downTime != 0)AnsiConsole.MarkupLine($"[red]DT Actual: {Pcs.pcs[id].downTime}[/]");
                    else AnsiConsole.MarkupLine($"[blue]DT Actual: {Pcs.pcs[id].downTime}[/]");
                }
                inicio_x += 23;
                if(id == 3){inicio_y += 13; inicio_x = laberinto_ancho*2;}
            }
        }
        public static void inf(string info , string color)
        {
            int ancho = Console.WindowWidth;
            int x = (laberinto_ancho*2)-1;
            int y = 32;
            bool v = false;
            int aux = 0;
            for(int i=0 ; i<ancho ; i++)
            {
                aux++;
                Console.SetCursorPosition(x+aux,y);
                if(i >= info.Length)
                {  
                    Console.Write(" ");
                    if(x + aux >= ancho-1){y++;aux = 0;}
                    if(y == 34)return;
                }
                else 
                {
                    AnsiConsole.MarkupLine($"[{color}]{info[i]}[/]");
                    if(v == true && info[i] == ' ')
                    {
                        aux = 0;
                        y++;
                        v = false;
                    }
                    if(x + aux + 15 > ancho)v = true;
                }
                if(y == 34)return;
            }
        }        
    }
}
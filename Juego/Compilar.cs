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
                    //Console.SetCursorPosition(j , j);
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
                                if(i == j && i == Laberinto.size/2)Console.Write("👾");
                                else{
                                    if(Laberinto.mat[i,j] == 0)
                                    {
                                        if(Pcs.pos_pcs.ContainsKey((i,j)) == true)Console.WriteLine("error1");
                                        if(Npcs.pos_npcs.ContainsKey((i,j)) == true)Console.WriteLine("error2");
                                        if(Trampas.trampa[i,j] != 0)Console.WriteLine("error3");
                                        if(i == 0 || i == Laberinto.size-1 || j == 0 || j == Laberinto.size-1)Console.Write("🟨");
                                        else Console.Write("🟥");
                                    }
                                    else
                                    {
                                        if(Pcs.pos_pcs[(i,j)].Count >= 1)
                                        {
                                            if(Npcs.pos_npcs[(i,j)].Count >= 1)Console.Write("⚔️ ");
                                            else{
                                                for(int q=0 ; q<Pcs.pos_pcs[(i,j)].Count ; q++){
                                                    if(Pcs.pos_pcs[(i,j)][0] != Pcs.pos_pcs[(i,j)][q]){
                                                        Console.Write("⚔️ ");
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
                                            else Console.Write("⬛");
                                        }
                                    }
                                }
                            }
                        }   
                    }
                }
                Console.WriteLine();
            }
            inf_pcs();
        }
        public static void inf_pcs()
        {
            int inicio_y = 0; 
            int inicio_x = laberinto_ancho*2; 
            for(int i=1 ; i<=Turnos.cant_jugadores ; i++)
            {
                //Console.WriteLine($"{i} {Pcs.pcs_principales[Pcs.pcs[i].id]}");
                Console.SetCursorPosition(inicio_x,inicio_y);
                Console.Write($"Jugador {i} - Héroes: ");
                foreach(int id in Turnos.players[i])
                {
                    Console.Write($"{Pcs.pcs[id].id}{Pcs.pcs[id].emoji} ");
                }
                inicio_y++;
            }
            inicio_y++;
            for(int id=0 ; id < Pcs.cant_pcs ; id++)
            {
                Console.SetCursorPosition(inicio_x, inicio_y + 1);
                Console.WriteLine($"Héroe:{Pcs.pcs[id].id} {Pcs.pcs[id].emoji}");
                Console.SetCursorPosition(inicio_x, inicio_y + 2);
                Console.WriteLine($"HP: {Pcs.pcs[id].healthPoints}");
                Console.SetCursorPosition(inicio_x, inicio_y + 3);
                Console.WriteLine($"AP: {Pcs.pcs[id].attackPoints}");
                Console.SetCursorPosition(inicio_x, inicio_y + 4);
                Console.WriteLine($"R: {Pcs.pcs[id].range}");
                Console.SetCursorPosition(inicio_x, inicio_y + 5);
                if(Turnos.personaje_en_juego == id)Console.WriteLine($"S: {Pcs.pcs[id].speed - Turnos.count_mov}");
                else Console.WriteLine($"S: {Pcs.pcs[id].speed}");
                Console.SetCursorPosition(inicio_x, inicio_y + 6);
                if(id == 0)Console.WriteLine($"H: Lanzar Granadas");
                if(id == 1)Console.WriteLine($"H: Impulso");
                if(id == 2)Console.WriteLine($"H: T_Transportacion");
                if(id == 3)Console.WriteLine($"H: Endurecimiento");
                if(id == 4)Console.WriteLine($"H: Transformacion");
                if(id == 5)Console.WriteLine($"H: Super Salto");
                if(id == 6)Console.WriteLine($"H: Control Mental");
                if(id == 7)Console.WriteLine($"H: Orden");
                Console.SetCursorPosition(inicio_x, inicio_y + 7);
                Console.WriteLine($"AT: {Pcs.pcs[id].abilityTimeOriginal}");
                Console.SetCursorPosition(inicio_x, inicio_y + 8);
                Console.WriteLine($"DT: {Pcs.pcs[id].downTimeOriginal}");
                inicio_x += 23;
                if(id == 3){inicio_y += 10; inicio_x = laberinto_ancho*2;}
            } 
        }
        public static void inf(string info , string color)
        {
            int ancho = Console.WindowWidth;
            int x = laberinto_ancho*2;
            int y = 28;
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
                    if(y == 30)return;
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
                if(y == 30)return;
            }
        }        
/**
//muro
                    if(Laberinto.mat[i,j] == 0)
                    {   
                        if(Pcs.pos_pcs.ContainsKey((i,j)) == true)Console.WriteLine("error");
                        if(Npcs.pos_npcs.ContainsKey((i,j)) == true)Console.WriteLine("error");
                        if(Trampas.trampa[i,j] != 0)Console.WriteLine("error");
                        AnsiConsole.Markup("[red]#   [/]");
                    }
//camino
                    else
                    {
//camino , personaje
                        if(Pcs.pos_pcs[(i,j)].Count != 0)
                        {
//camino , personaje , guardian
                            if(Npcs.pos_npcs[(i,j)].Count != 0)
                            {
//camino , personaje , guardian , trampa
                                if(Trampas.trampa[i,j] != 0)
                                {
                                    AnsiConsole.Markup("[green]pgt [/]");
                                }
//camino , personaje , guardian , no trampa
                                else
                                {
                                    AnsiConsole.Markup("[green]pg  [/]");   
                                }
                            }
//camino , personaje , no guardian
                            else
                            {
//camino , personaje , no guardian , trampa
                                if(Trampas.trampa[i,j] != 0)
                                {
                                    AnsiConsole.Markup("[yellow]pt  [/]");
                                }
//camino , personaje , no guardian , no trampa
                                else
                                {
                                    AnsiConsole.Markup("[yellow]p   [/]");
                                }
                            }
                        }
//camino , no personaje
                        else
                        {
//camino , no personaje , guardian
                            if(Npcs.pos_npcs[(i,j)].Count != 0)
                            {
//camino , no personaje , guardian , trampa
                                if(Trampas.trampa[i,j] != 0)
                                {
                                    AnsiConsole.Markup("[blue]gt  [/]");
                                }
//camino , no personaje , guardian , no trampas
                                else
                                {
                                    AnsiConsole.Markup("[blue]g   [/]");
                                }
                            }
//camino , no personaje , no guardian , trampas
                            else
                            {
                                if(Trampas.trampa[i,j] != 0)
                                {
                                    Console.Write("t   ");
                                }
//camino , no personaje , no guardian , no trampa
                                else
                                {
                                    Console.Write("    ");
                                }
                            }
                        }
                    }
                    **/
    }
}
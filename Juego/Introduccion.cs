using System.Data.SqlTypes;
using Spectre.Console;
namespace Juego
{
    public class Introduccion
    {
        public static string[,] heroes = new string[8,15];
        private static void historia()
        {
//introduccion a la trampa del juego
            string txt1 = almacen1();
            string txt2 = almacen2();            
            Escribir(txt1, 0);
            Escribir(txt2, 0);
            Console.WriteLine();
            AnsiConsole.MarkupLine("[magenta bold]Presione Esc para regresar al Menu[/]");
            while(true)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if(tecla.Key == ConsoleKey.Escape){
                    menu();
                    return;
                }
            }          
        }
        private static void nuevo_juego()
        {
            elegir_pc_iniciales();
            Turnos.turnos();
        }
        private static void como_jugar()
        {
            string txt1 = almacen5();
            Escribir(txt1, 0);
            Console.WriteLine();
            AnsiConsole.MarkupLine("[magenta bold]Presione Esc para regresar al Menu[/]");
            while(true)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if(tecla.Key == ConsoleKey.Escape){
                    menu();
                    return;
                }
            }
        }
//escribir los textos con un retraso para simular que se van escribiendo
        private static void Escribir(string texto, int tipo)
        {
            int ancho = Console.WindowWidth;
            bool v = false;
            foreach (char x in texto)
            {
                if(tipo == 0)AnsiConsole.Markup($"[blue]{x}[/]");
                if(tipo == 1)AnsiConsole.Markup($"[yellow italic]{x}[/]");
                if(tipo == 2)AnsiConsole.Markup($"[cyan]{x}[/]");
                if(tipo == 3)AnsiConsole.Markup($"[yellow]{x}[/]");
                //Thread.Sleep(00000001);
                if(v == true && x == ' ')
                {
                    Console.WriteLine();
                    v = false;
                }
                if(Console.CursorLeft + 15 > ancho)v = true;
            }
            Console.WriteLine();
        }
        private static void elegir_pc_iniciales()
        {
//barajear las pos donde se van a crear los pcs elegidos
            Laberinto.barajear_direcciones(Pcs.d);
//no elegir dos veces el mismo pc
            bool[] v = new bool[10]; 
//guardar las propiedades de cada heroe para imprimir
            asignar();
            int pos = 0;
            Turnos.cant_jugadores = 0;
            while(Turnos.cant_jugadores < 4)
            {
                Console.Clear();
                AnsiConsole.MarkupLine($"[yellow bold]Jugador {Turnos.cant_jugadores} elija su Héroe)[/]");
//heroes a elegir
                for(int j=0 ; j<8 ; j++)
                {
                    if(pos == j && v[pos] == false)Console.WriteLine($"> {heroes[j,0]}");
                    if(pos == j && v[pos] == true)AnsiConsole.MarkupLine($"[strikethrough]> {heroes[j,0]}[/]");
                    if(pos != j && v[j] == false)Console.WriteLine($"  {heroes[j,0]}");
                    if(pos != j && v[j] == true)AnsiConsole.MarkupLine($"[strikethrough]  {heroes[j,0]}[/]");
                }
                if(pos == 8)Console.WriteLine($"> Comenzar partida");
                else Console.WriteLine($"  Comenzar partida");
                if(pos == 9)Console.WriteLine($"> Salir al menú principal");
                else Console.WriteLine($"  Salir al menú principal");
//descripcion de los heroes
                if(pos < 8)
                {
                    Console.WriteLine();
                    for(int j=0 ; j<10 ; j++)Console.WriteLine(heroes[pos,j]);
                }
                while(true)
                {
                    ConsoleKeyInfo tecla = Console.ReadKey(true);
                    if(tecla.Key == ConsoleKey.S || tecla.Key == ConsoleKey.DownArrow){
                        pos++;
                        if(pos > 9)pos = 0;
                        break;
                    }
                    if(tecla.Key == ConsoleKey.W || tecla.Key == ConsoleKey.UpArrow){
                        pos--;
                        if(pos < 0)pos = 9;
                        break;
                    }
                    if(tecla.Key == ConsoleKey.Enter)
                    {
                        if(pos < 8)
                        {
                            if(!v[pos])
                            {
                                Turnos.cant_jugadores++;
                                Turnos.players[Turnos.cant_jugadores] = new List<int>();
                                Turnos.players[Turnos.cant_jugadores].Add(pos);
                                Pcs.pcs[pos].jugador = Turnos.cant_jugadores;
                                Pcs.pos_pcs_elegidos(Turnos.cant_jugadores,pos);
                                Pcs.pcs_principales[pos] = true;
                                v[pos] = true;
                            }
                            break;
                        }
                        if(pos == 8)
                        {
                            if(Turnos.cant_jugadores == 0)break;
                            else
                            {
                                Turnos.turnos();
                                return;
                            }
                        }
                        if(pos == 9)
                        {
                            Turnos.players.Clear();
                            menu();
                            return;
                        }
                    }
                }
            }
        }
        public static void menu()
        {
            int inicio = Console.WindowWidth/2;
            Console.Clear();
            int pos = 0;
            while(true)
            {
                Console.Clear();
                string aux = "Menus :";
                Console.CursorLeft = inicio-(aux.Length/2);
                Console.WriteLine(aux);
                if(pos == 0)aux = "> nuevo juego :";
                else aux = "  nuevo juego :";
                Console.CursorLeft = inicio - (aux.Length/2);
                Console.WriteLine(aux);
                if(pos == 1)aux = "> Ver Historia :";
                else aux = "  Ver Historia :";
                Console.CursorLeft = inicio - (aux.Length/2);
                Console.WriteLine(aux);
                if(pos == 2)aux = "> Como Jugar :";
                else aux = "  Como Jugar :";
                Console.CursorLeft = inicio - (aux.Length/2);
                Console.WriteLine(aux);
                Console.WriteLine("\n\n\n");
                if(pos == 3)aux = "> Salir :";
                else aux = "  Salir :";
                Console.CursorLeft = inicio - (aux.Length/2);
                Console.WriteLine(aux);
                Console.WriteLine();
                while(true)
                {
                    ConsoleKeyInfo tecla = Console.ReadKey(true);
                    if(tecla.Key == ConsoleKey.S || tecla.Key == ConsoleKey.DownArrow){
                        pos++;
                        if(pos > 3)pos = 0;
                        break;
                    }
                    if(tecla.Key == ConsoleKey.W || tecla.Key == ConsoleKey.UpArrow){
                        pos--;
                        if(pos < 0)pos = 3;
                        break;
                    }
                    if(tecla.Key == ConsoleKey.Enter)
                    {
                        if(pos == 0)
                        {
                            nuevo_juego();
                            return;
                        }
                        if(pos == 1)historia();
                        if(pos == 2)como_jugar();
                        if(pos == 3)
                        {
                            aux = "Saliendo.....";
                            Console.CursorLeft = inicio - (aux.Length/2);
                            AnsiConsole.MarkupLine($"[red]{aux}[/]");
                            return;
                        }
                    }
                }
            }
        }
        private static void asignar()
        {
            for(int i=0 ; i<8 ; i++)
            {
                
                heroes[i,0] = $"{Pcs.pcs[i].name} {Pcs.pcs[i].emoji}";
                heroes[i,3] = $"Puntos de Vida(HP): {Pcs.pcs[i].healthPoints}";
                heroes[i,4] = $"Puntos de Fuerza(AP): {Pcs.pcs[i].attackPoints}";
                heroes[i,5] = $"Rango de Ataque(R): {Pcs.pcs[i].range}";
                heroes[i,6] = $"Distancia Máxima por Movimiento(S): {Pcs.pcs[i].speed}";
                heroes[i,8] = $"Tiempo de Duración(AT): {Pcs.pcs[i].abilityTime}";
                heroes[i,9] = $"Tiempo de Enfriamiento(DT): {Pcs.pcs[i].downTimeOriginal}";
                if(Pcs.pcs[i].name == "Jake Thompson")
                {
                    heroes[i,1] = "Nación: Estados Unidos de América";
                    heroes[i,2] = "Descripción: Marine americano retirado, conocido por su valentía en una misión suicida donde acabó con un ejército enemigo. Ahora, lucha por su país en el Laberinto del Infierno.";
                    heroes[i,7] = "Habilidad Especial(A): Puede lanzar granadas a una distancia de 1 casilla, causando 20 puntos de daño en el punto de impacto y 10 puntos de daño en las casillas circundantes.";
                }
                if(Pcs.pcs[i].name == "Kaito Yamamoto")
                {
                    heroes[i,1] = "Nación: Japón";
                    heroes[i,2] = "Descripción: Samurái legendario, maestro de la espada y las artes marciales. Lucha por el honor de su nación y la gloria de sus ancestros.";
                    heroes[i,7] = "Habilidad Especial (A): Puede moverse hasta 5 casillas en una direccion atravesando paredes y quitar 10 de HP a todo enemigo que esté a su paso sin activar trampas.";
                }
                if(Pcs.pcs[i].name == "Merlinus the Enchanter")
                {
                    heroes[i,1] = "Nación: Reino Unido";
                    heroes[i,2] = "Descripción: Mago poderoso y sabio, conocido por sus habilidades de teletransportación y manipulación de la realidad. Lucha por el conocimiento y la magia.";
                    heroes[i,7] = "Habilidad Especial (A): Puede teletransportarse o teletransportar a cualquier Héroe hasta 3 casillas de distancia.";
                }
                if(Pcs.pcs[i].name == "Dr. Frankenstein")
                {
                    heroes[i,1] = "Nación: Alemania";
                    heroes[i,2] = "Descripción: Científico loco que ha creado un robot para servir como su cuerpo. Lucha por la ciencia y la innovación.";
                    heroes[i,7] = "Habilidad Especial (A): Puede endurecer su cuerpo, duplicando sus puntos de vida temporalmente.";
                }

                if(Pcs.pcs[i].name == "Lupus")
                {
                    heroes[i,1] = "Nación: Italia";
                    heroes[i,2] = "Descripción: Hombre lobo con un pasado oscuro y misterioso. Lucha por la libertad y la redención.";
                    heroes[i,7] = "Habilidad Especial (A): Puede transformarse en lobo, aumentando su vida en 20, su ataque en 10 y su velocidad en 10.";
                }
                if(Pcs.pcs[i].name == "The Jumper Smith")
                {
                    heroes[i,1] = "Nación: Canada";
                    heroes[i,2] = "Descripción: Jugador de la NBA, conocido por su habilidad para saltar y evadir obstáculos. Lucha por la diversión y la superación personal.";
                    heroes[i,7] = "Habilidad Especial (A): Puede saltar sobre los muros del laberinto, evitando obstáculos.";
                }
                if(Pcs.pcs[i].name == "Cerebra")
                {
                    heroes[i,1] = "Nación: Rusia";
                    heroes[i,2] = "Descripción: Mutante con habilidades psíquicas, capaz de controlar la mente de otros. Lucha por la dominación y el control.";
                    heroes[i,7] = "Habilidad Especial (A): Puede entrar en la mente de cualquier jugador y controlarlo durante un turno.";
                }
                if(Pcs.pcs[i].name == "Dante")
                {
                    heroes[i,1] = "Nación: Infierno";
                    heroes[i,2] = "Descripción: Demonio astuto y manipulador, busca la fuente de poder del laberinto para convertirse en el nuevo guardián.";
                    heroes[i,7] = "Habilidad Especial (A): Puede hacerse pasar por un guardia, evitando ser atacado por los guardianes y no caer en trampas, aunque no puede tomar a otros personajes como aliados.";
                }
            }
        }
        private static string almacen1()
        {
            return "En las profundidades del infierno, más allá de los ríos de lava y las montañas de ceniza, se encuentra un laberinto legendario conocido como ``El Laberinto del Infierno´´. Este lugar no es un simple laberinto; es una prisión eterna custodiada por Canserbero, el temible perro de tres cabezas que protege una fuente de poder inmensa en su centro. Se dice que quien controle esta fuente obtendrá un poder capaz de alterar el destino de los mundos.";
        }
        private static string almacen2()
        {
            return "Las naciones del mundo han descubierto la existencia de esta fuente y, movidas por la ambición, han enviado a sus mejores guerreros para reclamarla. Cada uno de estos héroes tiene habilidades únicas y una misión clara: llegar al centro del laberinto, derrotar a Canserbero y obtener el poder para su nación. Sin embargo, el laberinto está lleno de criaturas infernales, trampas mortales y enigmas imposibles. Además, Dante, un demonio que escapó del infierno, también busca la fuente para convertirse en el nuevo guardián y destronar a Canserbero.";
        }
        private static string almacen5()
        {
            return "El `Laberinto del Infierno´ es un juego multijugador para un máximo de 4 personas donde el deber de cada uno es llegar al centro del laberinto, ganarle a `Canserbero´ y llegar a su posición, para eso puedes tomar aliados parandote en las posiciones de héroes que no esten controlados por ningún jugador o luchar contra héroes que hayan tomado otros jugadores, ademas si mueres serás revivido en una posicion inicial dentro de 5 turnos pero perderas a todos los aliados que tengas, 👻 significa que hay un guardian 💀 más de uno, los guardianes merodean el laberitno, cuando todos los jugadores terminar sus turnos ellos pueden atacar a una distancia maxima de 2 y moverse";
        }
    }
}
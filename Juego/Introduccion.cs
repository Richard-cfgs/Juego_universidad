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
                AnsiConsole.MarkupLine("[red underline]Comando incorrecto[/]");
            }          
        }
        private static void héroes()
        {
//darle los valores al string heroes para mostrar en consola
//mostrar pcs
            asignar();
            AnsiConsole.MarkupLine("[yellow underline italic]Héroes:[/]");
            for(int i=0 ; i<8 ; i++)
            {
                Console.WriteLine();
                for(int j=0 ; j<10 ; j++)
                {
                    if(j == 3)AnsiConsole.MarkupLine("[blue italic]Características:[/]");
                    if(j == 0)Escribir(heroes[i,j], 1);
                    else Escribir(heroes[i,j], 2);
                }
            }
            Console.WriteLine();
            AnsiConsole.MarkupLine("[magenta bold]Presione Esc para regresar al Menu[/]");
            while(true)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if(tecla.Key == ConsoleKey.Escape){
                    menu();
                    return;
                }
                AnsiConsole.MarkupLine("[red underline]Comando incorrecto[/]");
            }
        }
        private static void nuevo_juego()
        {
            AnsiConsole.MarkupLine($"[blue]¿Cuantos Héroes están dispuestos a luchar por su país? solo pueden viajar juntos en esta aventura un maximo de 4 guerreros.[/]");
            elegir_pc_iniciales();
            AnsiConsole.MarkupLine($"[blue]Perfecto, ahora ustedes {Turnos.cant_jugadores} serán transportados al infierno.[/]");
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
                AnsiConsole.MarkupLine("[red underline]Comando incorrecto[/]");
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
                Thread.Sleep(00000001);
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
            Turnos.cant_jugadores = 1;
//barajear las pos donde se van a crear los pcs elegidos
            Laberinto.barajear_direcciones(Pcs.d);
            bool[] v = new bool[8]; 
            while(true)
            {
                Turnos.players[Turnos.cant_jugadores] = new List<int>();
                if(Turnos.cant_jugadores > 4){
                    Turnos.cant_jugadores--;
                    AnsiConsole.MarkupLine("[red underline]YA estan elegidos la máxima cantidad de guerreros, HA JUGAR[/]");
                    return;
                }
                Console.WriteLine();
                AnsiConsole.MarkupLine($"[magenta bold]Jugador {Turnos.cant_jugadores} elija su Héroe tecleando su identificador(numero al lado del nombre del Héroe)[/]");
                char aux = Console.ReadKey(true).KeyChar;
                if (aux >= '0' && aux < '8' && v[aux-'0'] == false){
                    int num = aux - '0';
                    Turnos.players[Turnos.cant_jugadores].Add(num);
                    Pcs.pcs[num].jugador = Turnos.cant_jugadores;
                    Pcs.pos_pcs_elegidos(Turnos.cant_jugadores,num);
                    Pcs.pcs_principales[num] = true;
                    v[num] = true;
                    AnsiConsole.MarkupLine($"[blue]El jugador {Turnos.cant_jugadores} eligió a {Pcs.pcs[num].name}[/]");
                    AnsiConsole.MarkupLine("[magenta bold]Presione `Tab´ para añadir otro jugador o `Esc´ para finalizar[/]");
                    Turnos.cant_jugadores++;
                    while(true){
                        ConsoleKeyInfo tecla = Console.ReadKey(true);
                        if(tecla.Key == ConsoleKey.Tab)
                        {
                            break;
                        }
                        if(tecla.Key == ConsoleKey.Escape){
                            Turnos.cant_jugadores--;
                            return;
                        }
                        AnsiConsole.MarkupLine("[red underline]Comando erroneo, intente nuevamente, solo puedes usar `Tabulador´ para añadir otro jugador y `Escape´ si ya todos estan listos para Ganar[/]");
                    }
                }
                else{
                    AnsiConsole.MarkupLine("[red underline]El identificador no pertenece a la lista de Héroes o ya fue eligido por otro jugador, por favor elija otro[/]");
                }
            }
        }
        public static int menu()
        {
            int inicio = Console.WindowWidth/2;
            Console.Clear();
            while(true)
            {
                string aux = "Menus :";
                Console.CursorLeft = inicio-(aux.Length/2);
                AnsiConsole.MarkupLine($"[yellow underline bold]{aux}[/]");
                aux = "1-  nuevo juego :";
                Console.CursorLeft = inicio - (aux.Length/2);
                AnsiConsole.MarkupLine($"[blue bold]{aux}[/]");
                aux = "2-  Ver Héroes :";
                Console.CursorLeft = inicio - (aux.Length/2);
                AnsiConsole.MarkupLine($"[blue bold]{aux}[/]");
                aux = "3-  Ver Historia :";
                Console.CursorLeft = inicio - (aux.Length/2);
                AnsiConsole.MarkupLine($"[blue bold]{aux}[/]");
                aux = "4- Como Jugar :";
                Console.CursorLeft = inicio - (aux.Length/2);
                AnsiConsole.MarkupLine($"[blue bold]{aux}[/]");
                Console.WriteLine("\n\n\n");
                aux = "5- Salir :";
                Console.CursorLeft = inicio - (aux.Length/2);
                AnsiConsole.MarkupLine($"[red]{aux}[/]");
                Console.WriteLine();
                while(true)
                {
                    aux = "Elija una opcion usando los números del 1-4";
                    Console.CursorLeft = inicio - (aux.Length/2);
                    AnsiConsole.MarkupLine($"[red]{aux}[/]");
                    char opcion = Console.ReadKey(true).KeyChar;
                    if(opcion == '5'){
                        aux = "Saliendo.....";
                        Console.CursorLeft = inicio - (aux.Length/2);
                        AnsiConsole.MarkupLine($"[red]{aux}[/]");
                        return 0;
                    }
                    if(opcion == '4'){
                        como_jugar();
                        break;
                    }
                    if(opcion == '3'){
                        historia();
                        break;
                    }
                    if(opcion == '2'){
                        héroes();
                        break;
                    }
                    if(opcion == '1'){
                        nuevo_juego();
                        return 1;
                    }
                    AnsiConsole.MarkupLine("[yellow]Comando incorrecto[/]");
                }
            }
        }
        private static void asignar()
        {
            for(int i=0 ; i<8 ; i++)
            {
                
                heroes[i,0] = $"{Pcs.pcs[i].id} {Pcs.pcs[i].name} {Pcs.pcs[i].emoji}";
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
                    heroes[i,7] = "Habilidad Especial (A): Puede entrar en la mente de cualquier jugador y controlarlo temporalmente.";
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
            return "El `Laberinto del Infierno´ es un juego multijugador para un máximo de 4 personas donde el deber de cada uno es llegar al centro del laberinto, ganarle a `Canserbero´ y llegar a su posición, para eso puedes tomar aliados parandote en las posiciones de héroes que no esten controlados por ningún jugador o luchar contra héroes que hayan tomado otros jugadores, ademas si mueres serás revivido en una posicion inicial dentro de 5 turnos pero perderas a todos los aliados que tengas, 👻 significa que hay un guardian 💀 más de uno, los guardianes merodean el laberitno, cuando todos los jugadores terminar sus turnos ellos atacan a una distancia maxima de 2, se mueven 3casillas y vuelven a atacar";
        }
    }
}
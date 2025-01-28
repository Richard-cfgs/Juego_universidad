namespace Juego
{
    public class Actualizar
    {
        public static int[] jugadores_muertos = new int[8];
//revisar si el pc o npc atacado murio
        public static void revisar_muerto(int index_muerto , bool tipo , int index_asesino)
        {
//si es in pc
            if(tipo == true)
            {
                if(Pcs.pcs[index_muerto].healthPoints <= 0)
                {
                    Pcs.pcs[index_muerto].healthPoints = 0;
                    if(Pcs.pcs_principales[index_muerto] == true)
                    {
//si el que muere es un pc principal recorro a todos sus aliados y le igualo el jugador a 0
                        int jugador_muerto = Pcs.pcs[index_muerto].jugador;
                        foreach(int id_aliados in Turnos.players[Pcs.pcs[index_muerto].jugador])
                        {
                            if(Pcs.pcs[id_aliados].jugador != jugador_muerto)Pcs.pcs[id_aliados].jugador = 0;
                        }
//limpio la lista de sus aliados
                        Turnos.players[jugador_muerto].Clear();
//add al pc principal
                        Turnos.players[jugador_muerto].Add(index_muerto);
//le igualo a los 5 turnos que no puede jugar
                        jugadores_muertos[jugador_muerto] = 7;
//y elimino el pc de la posicion
                        Pcs.pos_pcs[(Pcs.pcs[index_muerto].posx,Pcs.pcs[index_muerto].posy)].Remove(index_muerto);
                    }
//y no es el personaje principal de ningun jugador lo elimino y se lo dos al jugador que lo mato
                    if(Pcs.pcs_principales[index_muerto] == false)
                    {
//verificar quien controla al pc y quitarselo
                        Turnos.players[Pcs.pcs[index_muerto].jugador].Remove(index_muerto);
//guardo su pos
                        int x = Pcs.pcs[index_muerto].posx;
                        int y = Pcs.pcs[index_muerto].posy;
//eliminar el pc de la lista
                        Pcs.pcs.RemoveAt(index_muerto);
//agregar el pc creado nuevamente a la lista y le doy su vieja pos
                        Pcs.inf_pcs(index_muerto , false);
                        Pcs.pcs[index_muerto].posx = x;
                        Pcs.pcs[index_muerto].posy = y;
//lo agrego a la lista del que lo mató
                        if(index_asesino != -1){
                            Compilar.inf($"el héroe {Pcs.pcs[index_muerto].name} se ha unido al jugador {Pcs.pcs[index_asesino].jugador}" , "yellow");
                            Turnos.players[Pcs.pcs[index_asesino].jugador].Add(index_muerto);
                            Pcs.pcs[index_muerto].jugador = Pcs.pcs[index_asesino].jugador;
                        }
                        Pcs.pcs[index_muerto].jugador = 0;
                    }
                }
            }
            else
            {
                if(Npcs.npcs[index_muerto].healthPoints <= 0)
                {
//eliminar el indice del npc de las pos de los npc              
                    Npcs.pos_npcs[(Npcs.npcs[index_muerto].posx,Npcs.npcs[index_muerto].posy)].Remove(index_muerto);
//eliminar el npc de la lista
                    Npcs.npcs.RemoveAt(index_muerto);
                    Npcs.cant_npcs--;
                }
            }
        }
        public static int verificar_jugador_muerto(int jugador)
        {
            if(jugadores_muertos[jugador] > 1){
                jugadores_muertos[jugador]--;
                Compilar.inf($"jugador {jugador} herido, presiona Enter para continuar" , "yellow");
                continuar();
                return 0;
            }
            else{
                if(jugadores_muertos[jugador] == 1){
                    Compilar.inf($"jugador {jugador} terminó de sanar, presiona Enter para continuar" , "yellow");
                    continuar();
                    jugadores_muertos[jugador] = 0;
                    int index = Turnos.players[jugador][0];
                    Pcs.pcs.RemoveAt(index);
                    Pcs.inf_pcs(index,false);
                    Laberinto.barajear_direcciones(Pcs.d);
                    Pcs.pos_pcs_elegidos(jugador,index);
                    Pcs.pcs[index].jugador = jugador;
                }
                return 1;
            }
        }
        public static void times(int jugador)
        {
//disminuir tiempo de enfriamiento de habilidad de todos los pcs del jugador i y los turnos afectados
            for(int i=0 ; i<Turnos.players[jugador].Count ; i++)
            {
                int id = Turnos.players[jugador][i];
//si el tiempo de enfriamiento de su habilidad +1 es menor que el que le di al usar la habilidad es que aun esta activa su habilidad sino es que se acabo y le quito los atributos que le dio
                if(Pcs.pcs[id].abilityTime > 1)Pcs.pcs[id].abilityTime--;
                else
                {
                    if(Pcs.pcs[id].abilityTime == 1)
                    {
                        if(id == 3)Pcs.pcs[id].healthPoints /= 2;
                        if(id == 4){Pcs.pcs[id].healthPoints -= 5; Pcs.pcs[id].attackPoints	-= 5; Pcs.pcs[id].speed -= 8;}
                        Pcs.pcs[id].abilityTime = 0;
                    }
                    else if(Pcs.pcs[id].downTime > 0)Pcs.pcs[id].downTime--;
                }
                if(Pcs.pcs[id].affectedTurns > 1)Pcs.pcs[id].affectedTurns--;
                else
                {
                    if(Pcs.pcs[id].affectedTurns == 1)
                    {
                        Pcs.pcs[id].healthPoints *= 2;
                        Pcs.pcs[id].attackPoints *= 2;
                        Pcs.pcs[id].speed *= 2;
                        Pcs.pcs[id].affectedTurns = 0;
                    }    
                }
            }
        }
        public static void tomar_pcs(int id)
        {
            foreach(int i in Pcs.pos_pcs[(Pcs.pcs[id].posx , Pcs.pcs[id].posy)])
            {
                if(Pcs.pcs[i].jugador == 0 && Pcs.pcs[i].jugador != Pcs.pcs[id].jugador)
                {
                    Turnos.players[Pcs.pcs[id].jugador].Add(i);
                    Pcs.pcs[i].jugador = Pcs.pcs[id].jugador;
                }
            }
        }
        public static void continuar()
        {
            while(true)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if(tecla.Key == ConsoleKey.Enter)return;
            }
        }
    }
}
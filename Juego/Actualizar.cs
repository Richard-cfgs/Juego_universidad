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
                    else
                    {
//si el que muere es un pc principal quito todos los pc que tenga ese jugador y lo saco durante 5 turnos y hago lo mismo que un psc normal
                        Turnos.players[Pcs.pcs[index_muerto].jugador].Clear();
                        Turnos.players[Pcs.pcs[index_muerto].jugador].Add(index_muerto);
                        jugadores_muertos[Pcs.pcs[index_muerto].jugador] = 7;
                        Pcs.pos_pcs[(Pcs.pcs[index_muerto].posx,Pcs.pcs[index_muerto].posy)].Remove(index_muerto);
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
                Compilar.inf($"jugador {jugador} herido" , "yellow");
                return 0;
            }
            else{
                if(jugadores_muertos[jugador] == 1){
                    Compilar.inf($"jugador {jugador} terminó de sanar" , "yellow");
                    jugadores_muertos[jugador] = 0;
                    int index = Turnos.players[jugador][0];
                    Pcs.pcs.RemoveAt(index);
                    Pcs.inf_pcs(index,false);
                    Laberinto.barajear_direcciones(Pcs.d);
                    Pcs.pos_pcs_elegidos(jugador,index);;
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
        public static void tomar_pcs(int jugador , int id)
        {
            for(int i=0 ; i<Pcs.pos_pcs[(Pcs.pcs[id].posx,Pcs.pcs[id].posy)].Count ; i++)
            {
                if(Pcs.pcs[i].jugador == 0 && Pcs.pcs[i].jugador != jugador)Turnos.players[jugador].Add(i);
            }
        }
    }
}
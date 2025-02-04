namespace Juego
{
    public class Trampas
    {
        public static bool[,] desactivar_trampa = new bool[Laberinto.size,Laberinto.size];
        public static int cant_trampas = 50;
        public static int cant_trampas_distintas = 4;
        public static int[,] trampa = new int[Laberinto.size,Laberinto.size];
        public static void caer_trampa(int id)
        {
//verifico si caes en alguna trampa 
            if(trampa[Pcs.pcs[id].posx,Pcs.pcs[id].posy] != 0 && desactivar_trampa[Pcs.pcs[id].posx,Pcs.pcs[id].posy] == false){
//desactivar la trampa 
                desactivar_trampa[Pcs.pcs[id].posx,Pcs.pcs[id].posy] = true;
//ejecutar la accion de la trampa
                trampas(id,trampa[Pcs.pcs[id].posx,Pcs.pcs[id].posy]);
                Compilar.compilar(0 , 0 , 0);
            }
        }
        public static void trampas(int iden_p , int iden_t)
        {
            //pasas por un espectro y te quita la mitad de la vida, fuerza y rapidez durante 3 turnos
            if(iden_t == 1){
                //si el personaje no esta afectado lo afecto y le igualo a 3 los turnos afectados
                Compilar.inf("pasas por un espectro y te quita la mitad de la vida, fuerza y rapidez durante 3 turnos, presione Enter para continuar" , "yellow");
                Actualizar.continuar();
                if(Pcs.pcs[iden_p].affectedTurns == 0){
                    Pcs.pcs[iden_p].healthPoints /= 2;
                    Pcs.pcs[iden_p].attackPoints /= 2;
                    Pcs.pcs[iden_p].speed /= 2;
                    if(Pcs.pcs[iden_p].speed == 0)Pcs.pcs[iden_p].speed = 1;
                    if(Pcs.pcs[iden_p].attackPoints == 0)Pcs.pcs[iden_p].attackPoints = 1;
                    Pcs.pcs[iden_p].affectedTurns = 3;
                    Actualizar.revisar_muerto(iden_p , true , -1);
                }
                //sino solo le aumento 3 a los turnos afectados
                else{
                    Pcs.pcs[iden_p].affectedTurns += 3;
                } 
            }
            //brota lava del suelo y le quito 5 a la vida al personaje
            if(iden_t == 2){
                Compilar.inf("brota lava del suelo y le quito 5 a la vida al personaje, presione Enter para continuar" , "yellow");
                Actualizar.continuar();
                Pcs.pcs[iden_p].healthPoints = Pcs.pcs[iden_p].healthPoints-5;
                Actualizar.revisar_muerto(iden_p , true , -1);
            }
    //el suelo se agrieta y caes en una pos aleatoria
            if(iden_t == 3){
                Compilar.inf("el suelo se agrieta y caes en una pos aleatoria, presione Enter para continuar" , "yellow");
                Actualizar.continuar();
                Generacion_Aleatoria.generar(1,1);
                Pcs.pos_pcs[(Pcs.pcs[iden_p].posx , Pcs.pcs[iden_p].posy)].Remove(iden_p);
                Pcs.pcs[iden_p].posx = Generacion_Aleatoria.posx;    
                Pcs.pcs[iden_p].posy = Generacion_Aleatoria.posy;    
                Pcs.pos_pcs[(Pcs.pcs[iden_p].posx , Pcs.pcs[iden_p].posy)].Add(iden_p);
                Actualizar.tomar_pcs(iden_p);
            }
            //gas toxico que evita q uses tu habilidad durante 3 turnos      
            if(iden_t == 4){
                Compilar.inf("gas toxico que evita q uses tu habilidad durante 3 turnos, presione Enter para continuar" , "yellow");
                Actualizar.continuar();
                Pcs.pcs[iden_p].downTime += 3;
            }
        }
//generar trampas aleatorias
        public static void pos_trampas()
        {
//generar nuevas pos aleatorias que cumplan con la condicion
            for(int i=0 ; i<cant_trampas ; i++)
            { 
                Generacion_Aleatoria.generar(2,1);
                trampa[Generacion_Aleatoria.posx,Generacion_Aleatoria.posy] = (i%cant_trampas_distintas)+1;
            }
        }
    }
}
namespace Juego
{
    class Program
    {
        static void Main()
        {
//crear laberinto
            Laberinto.crear();
//inicializar lista de las pos de npsc y pcs
            Generacion_Aleatoria.iniciar_lista();
//crear las posiciones donde estan las trampas
            Trampas.pos_trampas();
//generar personajes aleatoriamente
            Pcs.crear_pcs();
//generar npsc aleatoriamente 
            Npcs.crear_npcs();
//introduccion al juego
            Introduccion.menu();
        }
    }
}

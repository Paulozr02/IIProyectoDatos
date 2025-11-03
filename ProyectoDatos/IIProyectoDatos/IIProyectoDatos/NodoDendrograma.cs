namespace IIProyectoDatos;

public class NodoDendrograma
{
    public string nombre;
    public double distancia;
    public List<NodoDendrograma> hijos;

    public NodoDendrograma(string nombre, double distancia)
    {
        /*

        Al ser el dendodragma una representacion grafica, necesitamos en su estructura
        el nombre, que seria la etiqueta, la distancia que es el enlace que tiene con sus hijos y
        claramente la lista de hijos que es necesaria para recorrerlo completamente y ademas,
        poder exportarlo (JSON).
        
        */
        
        this.nombre = nombre;
        this.distancia = distancia;
        hijos = new List<NodoDendrograma>();
    }
}
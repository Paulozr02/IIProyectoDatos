namespace IIProyectoDatos;

public class NodoDendrograma
{
    public string nombre;
    public double distancia;
    public List<NodoDendrograma> hijos;

    public NodoDendrograma(string nombre, double distancia)
    {
        this.nombre = nombre;
        this.distancia = distancia;
        hijos = new List<NodoDendrograma>();
    }
}
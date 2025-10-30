namespace IIProyectoDatos;

public class Vector
{
    private double[] datos;

    public Vector(int tamano)
    {
        datos = new double[tamano];
    }

    public int longitud => datos.Length;

    public double Obtener(int pos)
    {
        return datos[pos];
    }

    public void Asignar(int pos, double valor)
    {
        datos[pos] = valor;
    }
}
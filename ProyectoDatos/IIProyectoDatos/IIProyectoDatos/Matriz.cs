namespace IIProyectoDatos;

public class Matriz
{
    private double[,] matriz;

    public Matriz(int filas, int cols)
    {
        matriz = new double[filas, cols];
    }
    public int filas => matriz.GetLength(0);
    public int cols => matriz.GetLength(1);

    public double Obtener(int fila, int colum)
    {
        return matriz[fila, colum];
    }

    public void Asignar(int fila, int colum, double valor)
    {
        matriz[fila, colum] = valor;
    }
}
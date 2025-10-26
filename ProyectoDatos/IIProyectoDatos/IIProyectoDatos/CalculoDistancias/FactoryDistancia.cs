namespace IIProyectoDatos;

public class FactoryDistancia
{
    public static InterfaceDistancia Crear(string tipo)
    {
        if (tipo == "Euclidiana") return new DistanciaEuclidiana();
        if (tipo == "Manhattan") return new DistanciaManhattan();
        if (tipo == "Coseno") return new DistanciaCoseno();
        if (tipo == "Hamming") return new DistanciaHamming();
        throw new Exception("Tipo de distancia no valido");
    }
}
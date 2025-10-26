using System.Diagnostics.Tracing;

namespace IIProyectoDatos;

public static class FactoryNormalizador
{
    public static interfaceNormalizador Crear(string tipo)
    {
        if (tipo == "MinMax") return new NormalizadorMinMax();
        if (tipo == "ZScore") return new NormalizadorZScore();
        if (tipo == "Log") return new NormalizadorLogaritmica();
        throw new Exception("Tipo de normalizador no es valido");
    }
}
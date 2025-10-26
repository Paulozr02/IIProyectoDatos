using System;
using System.IO;      
using IIProyectoDatos;

class Program 
{
    static void Main()
    {
        Console.WriteLine("Proyecto II - Clustering Jerárquico");
        
        string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string ruta = Path.Combine(escritorio, "peliculasPrueba.csv");
        if (!File.Exists(ruta))
        {
            Console.WriteLine("No se encuentra");
            return;
        }
        else
        {
            Console.WriteLine("Leyendo data");
        }

        string normalizador = "ZScore";       
        string distancia = "Manhattan";       

        var controladora = new Controladora();
        controladora.EjecucionPrograma(ruta, normalizador, distancia);

        Console.WriteLine("Listo");
    }
}

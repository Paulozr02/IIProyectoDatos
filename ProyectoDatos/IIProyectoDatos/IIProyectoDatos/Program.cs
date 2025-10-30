using System;
using System.IO;
using IIProyectoDatos;

class Program
{
    static void Main()
    {
        string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string ruta = Path.Combine(escritorio, "datos.csv"); 

        Console.WriteLine("Normalizador: 1) MinMax  2) ZScore  3) Log");
        string opcionNorma = Console.ReadLine();
        opcionNorma = opcionNorma == "1" ? "MinMax" : opcionNorma == "2" ? "ZScore" : "Log";
        string normalizador = opcionNorma;

        Console.WriteLine("Distancia: 1) Euclidiana  2) Manhattan  3) Coseno  4) Hamming");
        string opcionDis = Console.ReadLine();
        opcionDis = opcionDis == "1" ? "Euclidiana" : opcionDis == "2" ? "Manhattan" : opcionDis == "3" ? "Coseno" : "Hamming";
        string distancia = opcionDis;

        var controladora = new Controladora();
        controladora.EjecucionPrograma(ruta, normalizador, distancia);

        Console.WriteLine("Archivo Json creado correctamente");
    }
}
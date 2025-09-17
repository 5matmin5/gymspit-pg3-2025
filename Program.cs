//napiš program který se tě zeptá na barvu vlastnosti, zvíře, jídlo a místo. Poté ti vygeneruje krátký příběh s použitím těchto slov.
using System;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Vítej v Mad Libs! Odpověz na následující otázky:");
        
        Console.Write("Zadej barvu: ");
        string barva = Console.ReadLine();
        
        Console.Write("Zadej zvíře: ");
        string zvire = Console.ReadLine();
       
        Console.Write("Zadej jídlo: ");
        string jidlo = Console.ReadLine();
       
        Console.Write("Zadej místo: ");
        string misto = Console.ReadLine();
        string pribeh = $"Jednoho dne jsem viděl/a {barva} {zvire}, které jedlo {jidlo} v {misto}. Bylo to neuvěřitelné!";
       
        Console.WriteLine("\nTady je tvůj příběh:");
        Console.WriteLine(pribeh);
    }
}

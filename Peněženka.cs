using System;
using System.Collections.Generic;
using System.IO;

class Polozka
{
    public int Hodnota;
    public string Nazev;
    public string Kategorie;
}

class Program
{
    const string CESTA = "data.txt";
    const int MAX = 100;

    static List<Polozka> seznam = new List<Polozka>();

    static void Main()
    {
        NactiZeSouboru();

        bool run = true;
        while (run)
        {
            Console.WriteLine("\n--- PENĚŽENKA ---");
            Console.WriteLine("1 - Vypsat položky");
            Console.WriteLine("2 - Přidat položku");
            Console.WriteLine("3 - Upravit položku");
            Console.WriteLine("4 - Smazat položku");
            Console.WriteLine("5 - Výpis s filtrem");
            Console.WriteLine("6 - Řazení");
            Console.WriteLine("0 - Konec");

            Console.Write("Volba: ");
            switch (Console.ReadLine())
            {
                case "1": Vypis(null); break;
                case "2": Pridat(); break;
                case "3": Upravit(); break;
                case "4": Smazat(); break;
                case "5": Filtr(); break;
                case "6": Razeni(); break;
                case "0": run = false; break;
            }
        }

        UlozDoSouboru();
    }

    // =========================
    static void NactiZeSouboru()
    {
        if (!File.Exists(CESTA)) return;

        string[] radky = File.ReadAllLines(CESTA);
        for (int i = 0; i < radky.Length; i += 3)
        {
            seznam.Add(new Polozka
            {
                Hodnota = int.Parse(radky[i]),
                Nazev = radky[i + 1],
                Kategorie = radky[i + 2]
            });
        }
    }

    static void UlozDoSouboru()
    {
        List<string> radky = new List<string>();
        foreach (var p in seznam)
        {
            radky.Add(p.Hodnota.ToString());
            radky.Add(p.Nazev);
            radky.Add(p.Kategorie);
        }
        File.WriteAllLines(CESTA, radky);
    }

    // =========================
    static void Vypis(string filtr)
    {
        Console.WriteLine("\n{0,-5} {1,-10} {2,-20} {3,-15}",
            "#", "Hodnota", "Název", "Kategorie");

        int prijmy = 0, vydaje = 0, pCount = 0, vCount = 0;
        int maxP = int.MinValue, minP = int.MaxValue;
        int maxV = int.MinValue, minV = int.MaxValue;

        Dictionary<string, int> katSoucty = new Dictionary<string, int>();

        for (int i = 0; i < seznam.Count; i++)
        {
            var p = seznam[i];

            if (filtr == null || p.Nazev.Contains(filtr, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("{0,-5} {1,-10} {2,-20} {3,-15}",
                    i, p.Hodnota, p.Nazev, p.Kategorie);
            }

            if (p.Hodnota >= 0)
            {
                prijmy += p.Hodnota;
                pCount++;
                maxP = Math.Max(maxP, p.Hodnota);
                minP = Math.Min(minP, p.Hodnota);
            }
            else
            {
                vydaje += p.Hodnota;
                vCount++;
                maxV = Math.Max(maxV, p.Hodnota);
                minV = Math.Min(minV, p.Hodnota);
            }

            if (!katSoucty.ContainsKey(p.Kategorie))
                katSoucty[p.Kategorie] = 0;
            katSoucty[p.Kategorie] += p.Hodnota;
        }

        Console.WriteLine("\n--- Statistiky ---");
        Console.WriteLine($"Příjmy: {pCount}, součet {prijmy}, min {minP}, max {maxP}");
        Console.WriteLine($"Výdaje: {vCount}, součet {vydaje}, min {minV}, max {maxV}");

        Console.WriteLine("\n--- Kategorie ---");
        foreach (var k in katSoucty)
            Console.WriteLine($"{k.Key}: {k.Value}");
    }

    // =========================
    static void Pridat()
    {
        if (seznam.Count >= MAX) return;

        Console.Write("Hodnota: ");
        if (!int.TryParse(Console.ReadLine(), out int h)) return;

        Console.Write("Název: ");
        string n = Console.ReadLine();

        Console.Write("Kategorie: ");
        string k = Console.ReadLine();

        seznam.Add(new Polozka { Hodnota = h, Nazev = n, Kategorie = k });
    }

    static void Upravit()
    {
        Console.Write("Číslo řádku: ");
        if (!int.TryParse(Console.ReadLine(), out int i)) return;
        if (i < 0 || i >= seznam.Count) return;

        Console.Write("Nová hodnota: ");
        if (!int.TryParse(Console.ReadLine(), out int h)) return;

        Console.Write("Nový název: ");
        string n = Console.ReadLine();

        Console.Write("Nová kategorie: ");
        string k = Console.ReadLine();

        seznam[i].Hodnota = h;
        seznam[i].Nazev = n;
        seznam[i].Kategorie = k;
    }

    static void Smazat()
    {
        Console.Write("Číslo řádku: ");
        if (!int.TryParse(Console.ReadLine(), out int i)) return;
        if (i < 0 || i >= seznam.Count) return;

        seznam.RemoveAt(i);
    }

    static void Filtr()
    {
        Console.Write("Hledaný text: ");
        string f = Console.ReadLine();
        Vypis(f);
    }

    static void Razeni()
    {
        Polozka[] kopie = (Polozka[])seznam.ToArray().Clone();

        Console.WriteLine("1 - podle hodnoty | 2 - podle názvu");
        bool podleHodnoty = Console.ReadLine() == "1";

        Array.Sort(kopie, (a, b) =>
            podleHodnoty ? a.Hodnota.CompareTo(b.Hodnota)
                          : a.Nazev.CompareTo(b.Nazev));

        Console.Write("Sestupně? (a/n): ");
        if (Console.ReadLine() == "a")
            Array.Reverse(kopie);

        Console.WriteLine("\n--- Seřazený výpis ---");
        foreach (var p in kopie)
            Console.WriteLine($"{p.Hodnota,-10} {p.Nazev,-20} {p.Kategorie}");
    }
}

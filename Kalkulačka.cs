class Program
{
    static void Main()
    {
        while (true)
        {
            // Načtení prvního čísla
            Console.Write("Zadej první číslo: ");
            string vstup1 = Console.ReadLine();
            double a;

            if (!double.TryParse(vstup1, out a))
            {
                Console.WriteLine("Neplatný vstup! Zkus to znovu.\n");
                continue;
            }

            // Načtení druhého čísla
            Console.Write("Zadej druhé číslo: ");
            string vstup2 = Console.ReadLine();
            double b;

            if (!double.TryParse(vstup2, out b))
            {
                Console.WriteLine("Neplatný vstup! Zkus to znovu.\n");
                continue;
            }

            // Výběr operace
            Console.Write("Zadej operaci (+, -, *, /) nebo 'konec' pro ukončení: ");
            string operace = Console.ReadLine();

            if (operace == "konec")
            {
                Console.WriteLine("Program ukončen.");
                break;
            }

            double vysledek = 0;
            bool chyba = false;

            // Výpočet podle operace
            if (operace == "+")
            {
                vysledek = a + b;
            }
            else if (operace == "-")
            {
                vysledek = a - b;
            }
            else if (operace == "*")
            {
                vysledek = a * b;
            }
            else if (operace == "/")
            {
                if (b == 0)
                {
                    Console.WriteLine("Chyba: nelze dělit nulou!");
                    chyba = true;
                }
                else
                {
                    vysledek = a / b;
                }
            }
            else
            {
                Console.WriteLine("Neplatná operace!");
                chyba = true;
            }

            // Výpis výsledku (jen pokud nedošlo k chybě)
            if (!chyba)
            {
                Console.WriteLine("Výsledek: " + vysledek);
            }

            Console.WriteLine(); // prázdný řádek pro přehlednost
        }
    }
}


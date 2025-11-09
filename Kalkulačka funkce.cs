using System;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            PrintMenu();

            char operace = ReadOperation();

            if (operace == 'k') // 'k' = konec
            {
                Console.WriteLine("Program ukončen.");
                break;
            }

            Console.WriteLine();
            Console.WriteLine("Zadej první číslo:");
            double a = ReadDouble();

            Console.WriteLine("Zadej druhé číslo:");
            bool musiBytNenulove = (operace == '/'); // při dělení nesmí být 0
            double b = ReadDouble(musiBytNenulove);

            double vysledek = Compute(operace, a, b);

            PrintResult(operace, a, b, vysledek);

            Console.WriteLine("\nStiskni libovolnou klávesu pro pokračování...");
            Console.ReadKey();
        }
    }

    static void PrintMenu()
    {
        Console.WriteLine("=== KALKULÁTOR ===");
        Console.WriteLine("Vyber operaci:");
        Console.WriteLine(" +  sčítání");
        Console.WriteLine(" -  odčítání");
        Console.WriteLine(" *  násobení");
        Console.WriteLine(" /  dělení");
        Console.WriteLine(" k  konec");
        Console.WriteLine();
    }

    static char ReadOperation()
    {
        while (true)
        {
            Console.Write("Zadej operaci: ");
            string vstup = Console.ReadLine().Trim();

            if (vstup.Length == 1)
            {
                char op = vstup[0];
                if (op == '+' || op == '-' || op == '*' || op == '/' || op == 'k')
                {
                    return op;
                }
            }

            Console.WriteLine("Neplatná operace! Zkus to znovu.\n");
        }
    }

    static double ReadDouble(bool nonZero = false)
    {
        while (true)
        {
            Console.Write("Zadej číslo: ");
            string vstup = Console.ReadLine();

            if (double.TryParse(vstup, out double cislo))
            {
                if (!nonZero || cislo != 0)
                {
                    return cislo;
                }
                else
                {
                    Console.WriteLine("Číslo nesmí být nula! Zkus to znovu.\n");
                }
            }
            else
            {
                Console.WriteLine("Neplatný vstup! Zkus to znovu.\n");
            }
        }
    }

    static double Compute(char operation, double operand1, double operand2)
    {
        switch (operation)
        {
            case '+': return operand1 + operand2;
            case '-': return operand1 - operand2;
            case '*': return operand1 * operand2;
            case '/': return operand1 / operand2;
            default: return 0; // teoreticky by sem program neměl dojít
        }
    }

    static void PrintResult(char operation, double operand1, double operand2, double result)
    {
        Console.WriteLine($"\nVýsledek: {operand1} {operation} {operand2} = {result}");
    }
}

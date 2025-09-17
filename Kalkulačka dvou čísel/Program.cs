// napiš program, který se zaptá na dvě čísla a odělá s nima součin, součet, rozdíl, podíl a další matematické operace
using System;
double num1, num2;

Console.WriteLine("Zadejte první číslo:");
while (!double.TryParse(Console.ReadLine(), out num1))
{
    Console.WriteLine("Neplatný vstup. Zadejte prosím číslo:");
}

Console.WriteLine("Zadejte druhé číslo:");
while (!double.TryParse(Console.ReadLine(), out num2))
{
    Console.WriteLine("Neplatný vstup. Zadejte prosím číslo:");
}

Console.WriteLine($"Součet: {num1 + num2}");
Console.WriteLine($"Rozdíl: {num1 - num2}");
Console.WriteLine($"Součin: {num1 * num2}");

if (num2 != 0)
{
    Console.WriteLine($"Podíl: {num1 / num2}");
}
else
{
    Console.WriteLine("Podíl: Chyba – dělení nulou není povoleno.");
}

Console.WriteLine($"Mocnina prvního čísla na druhé: {Math.Pow(num1, num2)}");
Console.WriteLine($"Mocnina druhého čísla na první: {Math.Pow(num2, num1)}");
Console.WriteLine($"Odmocnina prvního čísla: {Math.Sqrt(num1)}");
Console.WriteLine($"Odmocnina druhého čísla: {Math.Sqrt(num2)}");

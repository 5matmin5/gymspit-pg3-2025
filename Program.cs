using System;
using System.Collections.Generic;

namespace SimpleMonopoly
{
	// --- 1. HRÁČ ---
	public class Player
	{
		// Vlastnosti (Properties) hráče
		public string Name { get; set; } // Jméno hráče
		public int Money { get; set; } // Aktuální stav peněz
		public int Position { get; set; } // Na jakém políčku hráč stojí (index)

		// Zkratka pro zjištění, jestli hráč prohrál (má méně než 0 peněz)
		public bool IsBankrupt => Money < 0;

		// Konstruktor - volá se při vytváření nového hráče
		public Player(string name, int startingMoney)
		{
			Name = name;
			Money = startingMoney; // Startovní peníze zvolené na začátku
			Position = 0; // Všichni začínají na políčku 0 (Start)
		}
	}

	// --- 2. HERNÍ PLOCHA (POLÍČKA) ---
	// Abstraktní třída - slouží jen jako šablona pro konkrétní políčka
	public abstract class Space
	{
		public string Name { get; set; }

		public Space(string name)
		{
			Name = name;
		}

		// Metoda, která se spustí, když sem hráč stoupne. 
		// Slovo 'abstract' znamená, že konkrétní políčka si ji musí naprogramovat po svém.
		public abstract void OnLand(Player player);
	}

	// Speciální políčko pro Start
	public class StartSpace : Space
	{
		public StartSpace() : base("Start") { }

		public override void OnLand(Player player)
		{
			Console.WriteLine("Jsi na Startu. Odpočívej.");
			// Průchozí peníze za start se obvykle řeší při pohybu, tady hráč jen stojí.
		}
	}

	// Klasická nemovitost, která se dá koupit
	public class PropertySpace : Space
	{
		public int Price { get; set; } // Cena za nákup
		public int Rent { get; set; } // Nájem, který platí cizí hráči
		public Player Owner { get; set; } // Majitel (pokud je null, nikomu to nepatří)

		public PropertySpace(string name, int price, int rent) : base(name)
		{
			Price = price;
			Rent = rent;
			Owner = null; // Na začátku nemá majitele
		}

		public override void OnLand(Player player)
		{
			// Pokud pozemek nikomu nepatří
			if (Owner == null)
			{
				Console.WriteLine($"{Name} je volná. Cena: {Price}, nájem: {Rent}, tvůj zůstatek: {player.Money}.");
				Console.WriteLine("Chceš koupit? Napiš 'a' pro ANO, cokoliv jiného pro NE:");

				string input = Console.ReadLine(); // Načte odpověď od uživatele

				if (input != null && input.ToLower() == "a")
				{
					if (player.Money >= Price) // Kontrola, jestli má hráč dost peněz
					{
						player.Money -= Price; // Odečtení peněz
						Owner = player; // Nastavení nového majitele
						Console.WriteLine($"Paráda! Koupil jsi {Name}. Zůstatek: {player.Money}");
					}
					else
					{
						Console.WriteLine("Nemáš dost peněz!");
					}
				}
			}
			// Pokud pozemek patří někomu jinému
			else if (Owner != player)
			{
				Console.WriteLine($"Tento pozemek vlastní {Owner.Name}! Platíš nájem {Rent}.");
				player.Money -= Rent; // Hráč platí
				Owner.Money += Rent; // Majitel dostává peníze
			}
			// Pokud pozemek patří hráči na tahu
			else
			{
				Console.WriteLine("Vítej na svém pozemku! Tady jsi v bezpečí.");
			}
		}
	}

	// --- 3. JÁDRO HRY ---
	public class Game
	{
		private List<Player> players = new List<Player>(); // Seznam hráčů
		private List<Space> board = new List<Space>(); // Seznam políček (herní deska)
		private Random dice = new Random(); // Generátor náhodných čísel (kostka)
		private int startingMoney = 1500; // uchováváme zvolenou hodnotu pro výpočet nájmu

		public void Setup()
		{
			// Zeptáme se na počet hráčů
			int playerCount = 0;
			while (true)
			{
				Console.WriteLine("Zadej počet hráčů (minimálně 2):");
				string? countInput = Console.ReadLine();
				if (int.TryParse(countInput, out playerCount) && playerCount >= 2)
				{
					break;
				}
				Console.WriteLine("Neplatný počet. Zkus to znovu.");
			}

			// Zeptáme se na startovní peníze (pro všechny hráče stejná částka)
			startingMoney = 0;
			while (true)
			{
				Console.WriteLine("Zadej výši startovních peněz pro každého hráče (kladné celé číslo, např. 1500):");
				string? moneyInput = Console.ReadLine();
				if (int.TryParse(moneyInput, out startingMoney) && startingMoney >= 0)
				{
					break;
				}
				Console.WriteLine("Neplatná částka. Zkus to znovu.");
			}

			// Zeptáme se na jména hráčů
			for (int i = 1; i <= playerCount; i++)
			{
				while (true)
				{
					Console.WriteLine($"Zadej jméno hráče {i}:");
					string? name = Console.ReadLine()?.Trim();
					if (!string.IsNullOrEmpty(name))
					{
						players.Add(new Player(name, startingMoney));
						break;
					}
					Console.WriteLine("Jméno nesmí být prázdné. Zkus to znovu.");
				}
			}

			// Vytvoříme herní desku (pořadí v seznamu určuje pořadí na desce)
			board.Add(new StartSpace());

			// Pomocná lokální lambda pro výpočet nájmu podle ceny a startovních peněz
			int CalcRent(int price)
			{
				// základní návrh: price / 8
				int baseRent = Math.Max(1, price / 8);
				// dolní limit: price / 20
				int minRent = Math.Max(1, price / 20);
				// horní limit: startingMoney / 6 (aby jediný nájem hráče nezruinoval)
				int maxRent = Math.Max(1, startingMoney / 6);

				return Math.Clamp(baseRent, minRent, maxRent);
			}

			// Přidání políček s automatickým výpočtem nájmu
			board.Add(new PropertySpace("Václavák", 400, CalcRent(400)));
			board.Add(new PropertySpace("Příkopy", 350, CalcRent(350)));
			board.Add(new PropertySpace("Hlavní Nádraží", 200, CalcRent(200)));
			board.Add(new PropertySpace("Národní", 300, CalcRent(300)));
			board.Add(new PropertySpace("Karlův Most", 450, CalcRent(450)));
			board.Add(new PropertySpace("Staromák", 500, CalcRent(500)));
			board.Add(new PropertySpace("Vinohrady", 250, CalcRent(250)));
			board.Add(new PropertySpace("Žižkov", 150, CalcRent(150)));
			board.Add(new PropertySpace("Smíchov", 350, CalcRent(350)));
			board.Add(new PropertySpace("Anděl", 300, CalcRent(300)));
			board.Add(new PropertySpace("Letná", 200, CalcRent(200)));
			board.Add(new PropertySpace("Holešovice", 250, CalcRent(250)));
			board.Add(new PropertySpace("Vršovice", 150, CalcRent(150)));
			board.Add(new PropertySpace("Florenc", 400, CalcRent(400)));
			board.Add(new PropertySpace("Náměstí Míru", 450, CalcRent(450)));
			board.Add(new PropertySpace("Hradčany", 500, CalcRent(500)));
			board.Add(new PropertySpace("Vršovice", 150, CalcRent(150)));
		}

		public void Play()
		{
			int currentPlayerIndex = 0; // Začíná hráč na indexu 0

			Console.WriteLine("=== VÍTEJTE V MONOPOLY ===");

			// Hra běží neustále dokola, dokud nezbude jen jeden hráč (vítěz)
			while (players.Count > 1)
			{
				// Ošetření situace kdy currentPlayerIndex může být mimo rozsah (po odstranění hráče)
				if (currentPlayerIndex >= players.Count)
				{
					currentPlayerIndex = currentPlayerIndex % players.Count;
				}

				Player current = players[currentPlayerIndex]; // Hráč, který je právě na tahu

				Console.WriteLine($"\n--- Hraje {current.Name} (Peníze: {current.Money}) ---");
				Console.WriteLine("Stiskni ENTER pro hod kostkou...");
				Console.ReadLine(); // Zastaví program a čeká na stisk Enteru

				// Hod dvěma kostkami (1 až 6)
				int roll = dice.Next(1, 7) + dice.Next(1, 7);
				Console.WriteLine($"Padlo ti {roll}.");

				// Uložení staré pozice pro kontrolu průchodu Startem
				int oldPosition = current.Position;

				// Výpočet nové pozice. Operátor % (modulo) zajistí, že po posledním políčku hráč skočí zase na začátek.
				current.Position = (current.Position + roll) % board.Count;

				// Kontrola, jestli hráč prošel Startem (nová pozice je menší než stará)
				if (current.Position < oldPosition)
				{
					Console.WriteLine("Prošel jsi Startem! Získáváš 100.");
					current.Money += 100;
				}

				// Zjištění, na jaké políčko hráč stoupl
				Space landedOn = board[current.Position];
				Console.WriteLine($"{current.Name} stoupl na políčko: {landedOn.Name}");

				// Spuštění události políčka (nákup, placení nájmu atd.)
				landedOn.OnLand(current);

				// Kontrola bankrotu po zaplacení nájmů
				if (current.IsBankrupt)
				{
					Console.WriteLine($"\n!!! {current.Name} ZKRACHOVAL !!!");
					players.Remove(current); // Vyřazení hráče ze hry

					// Po odstranění hráče upravíme index tak, aby zůstal v platném rozsahu.
					if (players.Count > 0)
					{
						currentPlayerIndex = currentPlayerIndex % players.Count;
					}
				}
				else
				{
					// Posun na dalšího hráče (opět použito modulo pro cyklování od posledního k prvnímu)
					currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
				}
			}

			// Smyčka skončila (v seznamu zbyl jen jeden hráč)
			if (players.Count == 1)
			{
				Console.WriteLine($"\n=== KONEC HRY! Vítězem se stává {players[0].Name} ===");
			}
			else
			{
				Console.WriteLine("\n=== KONEC HRY! Bez vítěze ===");
			}
		}
	}

	// --- 4. HLAVNÍ SPOUŠTĚČ ---
	class Program
	{
		static void Main()
		{
			Game monopoly = new Game(); // Vytvoření instance hry
			monopoly.Setup(); // Příprava hráčů a desky
			monopoly.Play(); // Spuštění hlavní herní smyčky
		}
	}
}
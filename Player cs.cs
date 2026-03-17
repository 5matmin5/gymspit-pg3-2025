using System.IO;

namespace Lecture18
{
    internal class Player : Character
    {
        private TextReader input;
        private TextWriter? prompt;

        public Player(string name, int maxHitPoints, int attack, int defense, TextReader input, TextWriter? prompt = null) :
            base(name, maxHitPoints, attack, defense)
        {
            this.input = input;
            this.prompt = prompt;
        }

        protected override string ChooseAction()
        {
            while (true)
            {
                if (prompt != null)
                {
                    prompt.WriteLine("Choose an action:");
                    prompt.WriteLine("(A)ttack (attack {0})", Attack);
                    // ÚPRAVA: Přidána možnost vybrat obranu v uživatelském rozhraní
                    prompt.WriteLine("(D)efense (increases armor)");
                    // ÚPRAVA: Doplněno info, že čekání trochu léčí
                    prompt.WriteLine("(W)ait (heals slightly)");
                }

                string? choice = input.ReadLine();
                if (choice == null)
                {
                    // Pokud vstup skončil (např. EOF), zvolíme bezpečnou výchozí akci (čekání).
                    return TURN_CHOICE_WAIT;
                }

                choice = choice.Trim().ToLowerInvariant();

                switch (choice)
                {
                    case "a":
                    case "attack":
                        return TURN_CHOICE_ATTACK;

                    // ÚPRAVA: Zpracování vstupu pro novou akci obrany
                    case "d":
                    case "defense":
                        return TURN_CHOICE_DEFENSE;

                    case "w":
                    case "wait":
                        return TURN_CHOICE_WAIT;
                }

                if (prompt != null)
                {
                    prompt.WriteLine("Invalid choice!");
                }
            }
        }
    }
}
        
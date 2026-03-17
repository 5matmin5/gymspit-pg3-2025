using System;
using System.IO;

namespace Lecture18
{
    internal abstract class Character
    {
        protected const string TURN_CHOICE_ATTACK = "attack";
        protected const string TURN_CHOICE_WAIT = "wait";
        // ÚPRAVA 1: Přidána nová akce pro obranu (Větší úprava)
        protected const string TURN_CHOICE_DEFENSE = "defense";

        private string name;
        private int hitPoints;
        private int maxHitPoints;
        private int attack;
        private int defense;

        // ÚPRAVA 1: Přidána vlastnost armorBonus
        private int armorBonus;

        public string Name => name;
        public int HitPoints => hitPoints;
        public int MaxHitPoints => maxHitPoints;
        public bool Alive => hitPoints > 0;
        public int Attack => attack;
        public int Defense => defense;

        public Character(string name, int maxHitPoints, int attack, int defense)
        {
            this.name = name;
            this.maxHitPoints = maxHitPoints;
            this.attack = attack;
            this.defense = defense;
            Reset();
        }

        public void Reset()
        {
            hitPoints = maxHitPoints;
            // ÚPRAVA 1: Reset armor bonusu na začátku boje (nebo při restartu)
            armorBonus = 0;
        }

        protected abstract string ChooseAction();

        public void TakeTurn(TextWriter output, Character enemy, Die die)
        {
            string action = ChooseAction();

            switch (action)
            {
                case TURN_CHOICE_ATTACK:
                    AttackEnemy(output, enemy, die);
                    break;

                case TURN_CHOICE_WAIT:
                    Wait(output, die);
                    break;

                // ÚPRAVA 1: Zpracování nové obranné akce
                case TURN_CHOICE_DEFENSE:
                    DefenseAction(output);
                    break;

                default:
                    output.WriteLine("{0} does nothing...", name);
                    break;
            }
        }

        private void AttackEnemy(TextWriter output, Character enemy, Die die)
        {
            // ÚPRAVA 1: Snížení armorBonus při vlastním útoku
            if (armorBonus > 0)
            {
                armorBonus--;
            }

            int attackRoll = attack + die.Roll();
            enemy.ReceiveAttack(output, attackRoll, die);
        }

        private void ReceiveAttack(TextWriter output, int attackRoll, Die die)
        {
            // ÚPRAVA 1: K obraně se přičítá i nasbíraný armorBonus
            int defenseRoll = defense + armorBonus + die.Roll();
            int damage = attackRoll - defenseRoll;

            if (damage > 0)
            {
                hitPoints -= damage;
                output.WriteLine("{0} takes {1} damage!", name, damage);
            }
            else
            {
                output.WriteLine("{0} takes no damage!", name);
            }
        }

        private void Wait(TextWriter output, Die die)
        {
            output.WriteLine("{0} waits and rolls a die...", name);
            int roll = die.Roll();
            output.WriteLine("They rolled a {0}!", roll);

            // ÚPRAVA 2: Při čekání se postavě doplní menší množství životů (Menší úprava)
            // Použijeme např. polovinu hodu kostkou (minimálně 1 HP).
            int healAmount = Math.Max(1, roll / 2);
            if (hitPoints < maxHitPoints)
            {
                // Math.Min zaručí, že HP nepřelezou přes maximum
                hitPoints = Math.Min(maxHitPoints, hitPoints + healAmount);
                output.WriteLine("{0} rests and heals for {1} HP! (Current HP: {2}/{3})", name, healAmount, hitPoints, maxHitPoints);
            }
        }

        // ÚPRAVA 1: Metoda pro provedení obranné akce
        private void DefenseAction(TextWriter output)
        {
            armorBonus += 2; // Zvýšíme bonus k obraně
            output.WriteLine("{0} takes a defensive stance! Armor bonus increased to {1}.", name, armorBonus);
        }
    }
}
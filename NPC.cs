using System;

namespace Lecture18
{
    internal class NPC : Character
    {
        private Random random;

        public NPC(string name, int maxHitPoints, int attack, int defense, Random random) :
            base(name, maxHitPoints, attack, defense)
        {
            this.random = random;
        }

        protected override string ChooseAction()
        {
            // ÚPRAVA 1: Zajímavější umělá inteligence (Větší úprava)
            // NPC se nyní rozhoduje logicky na základě stavu svých životů.

            double healthPercentage = (double)HitPoints / MaxHitPoints;

            // Podmínka 1: NPC má 30 % životů nebo méně (Kritický stav)
            if (healthPercentage <= 0.3)
            {
                // V ohrožení života se NPC většinou stáhne, aby se vyléčilo (Wait),
                // nebo se alespoň zkusí opevnit (Defense).
                if (random.NextDouble() < 0.7) // 70% šance
                {
                    return TURN_CHOICE_WAIT;
                }
                else // 30% šance
                {
                    return TURN_CHOICE_DEFENSE;
                }
            }
            // Podmínka 2: NPC má víc než 30 % životů (Dobrý stav)
            else
            {
                // Zde primárně útočí, ale občas takticky použije obranu, aby tě naštvalo.
                int roll = random.Next(100);
                if (roll < 75) // 75% šance na útok
                {
                    return TURN_CHOICE_ATTACK;
                }
                else // 25% šance na obranu
                {
                    return TURN_CHOICE_DEFENSE;
                }
            }
        }
    }
}
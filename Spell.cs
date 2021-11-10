using System;
using System.Collections.Generic;

namespace southfury_csharp_dojo
{
    enum SpellType
    {
        Fireball,
        Frostball,
        HealingTouch
    }

    class Spell
    {
        public SpellType type;
        public bool selfTargeted;
        public int minDamage;
        public int maxDamage;
        public int coolDown;

        public Spell(SpellType type, bool selfTargeted, int minDamage, int maxDamage, int coolDown)
        {
            this.type = type;
            this.selfTargeted = selfTargeted;
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.coolDown = coolDown;
        }

        public void Cast(Enemy target = null, Player player = null)
        {
            switch (this.type)
            {
                case SpellType.Fireball:
                    this.DoDamage(target);
                    break;
                case SpellType.Frostball:
                    this.DoDamage(target);
                    break;
                case SpellType.HealingTouch:
                    int healedHp = this.CalculateDamage();
                    player.Heal(healedHp);
                    break;
            }
        }

        public int CalculateDamage()
        {
            Random random = new Random();
            int damage = random.Next(this.minDamage, this.maxDamage + 1);
            return damage;
        }

        public void DoDamage(Enemy target)
        {
            int damage = CalculateDamage();
            target.hpCurrent = target.hpCurrent - damage;
            Console.WriteLine(this.type.ToString() + " hits " + target.type.ToString() + " for " + damage + " damage.");
            target.UpdateState();
        }
    }
}
using System;
using System.Collections.Generic;

namespace southfury_csharp_dojo
{
    class Spell
    {
        public string name;
        public bool selfTargeted;
        public int minDamage;
        public int maxDamage;
        public int cooldown;
        public bool isOnCooldown = false;
        public int currentCooldown = 0;
        public int manaCost;

        public Spell(string name, bool selfTargeted, int minDamage, int maxDamage, int cooldown, int manaCost)
        {
            this.name = name;
            this.selfTargeted = selfTargeted;
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.cooldown = cooldown;
            this.manaCost = manaCost;
        }

        public void Cast(Player player, Enemy target = null)
        {
            if (this.selfTargeted)
            {
                int healedHp = this.CalculateDamage();
                player.Heal(healedHp);
            }
            else
            {
                this.DoDamage(target);
            }
            this.StartCooldown();
            player.DecreaseMana(this.manaCost);
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
            Console.WriteLine(this + " hits " + target.type.ToString() + " for " + damage + " damage.");
            target.UpdateState();
        }

        public void StartCooldown()
        {
            this.currentCooldown = this.cooldown;
            this.isOnCooldown = true;
        }

        public void DecreaseCooldown()
        {
            if (this.currentCooldown > 0)
            {
                this.currentCooldown--;
                if (this.currentCooldown == 0)
                {
                    this.isOnCooldown = false;
                }
            }
        }
    }
}
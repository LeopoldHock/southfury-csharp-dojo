using System;
using System.Collections.Generic;

namespace southfury_csharp_dojo
{
    enum PlayerAction
    {
        Attack,
        Spell,
        Flee
    }

    class Player
    {
        public string name;
        public int hpTotal = 50;
        public int hpCurrent;
        public int manaTotal = 40;
        public int manaCurrent;
        public int manaRegenBasic = 2;
        public bool dead = false;
        public int minAttackDamage = 6;
        public int maxAttackDamage = 14;
        public Combat currentCombat;
        public List<Spell> spellBook = new List<Spell>();

        public Player(bool askForName = true)
        {
            // Initialize name
            if (askForName)
            {
                Console.WriteLine("Name your character..");
                this.name = Console.ReadLine();
            }
            else
            {
                this.name = "Nameless Hero";
            }
            // Initialize stats
            this.hpCurrent = this.hpTotal;
            this.manaCurrent = this.manaTotal;
            // Initialize spell book
            this.spellBook.Add(new Spell("Fireball", false, 16, 26, 2, 12));
            this.spellBook.Add(new Spell("Frostball", false, 7, 13, 2, 8));
            this.spellBook.Add(new Spell("Healing Touch", true, 18, 25, 3, 15));
        }

        public void PerformAttack(Enemy target)
        {
            Random random = new Random();
            int damage = random.Next(this.minAttackDamage, this.maxAttackDamage + 1);
            target.hpCurrent = target.hpCurrent - damage;
            Console.WriteLine("You hit " + target.type + " for " + damage + " damage!");
            target.UpdateState();
        }

        public void UpdateState()
        {
            if (this.hpCurrent <= 0)
            {
                this.hpCurrent = 0;
                this.dead = true;
            }
        }

        public Spell CastSpell(Spell spell)
        {
            if (spell.selfTargeted)
            {
                spell.Cast(this);
            }
            else
            {
                Enemy target = this.currentCombat.PickTargetEnemy();
                spell.Cast(this, target: target);
            }
            return spell;
        }

        public void Heal(int healedHp)
        {
            this.hpCurrent += healedHp;
            if (this.hpCurrent > this.hpTotal)
            {
                this.hpCurrent = this.hpTotal;
            }
            Console.WriteLine("You healed for " + healedHp + " HP.");
        }

        public void RegenerateMana()
        {
            this.manaCurrent += this.manaRegenBasic;
            if (this.manaCurrent > this.manaTotal)
            {
                this.manaCurrent = this.manaTotal;
            }
        }

        public void DecreaseMana(int value)
        {
            this.manaCurrent -= value;
            if (this.manaCurrent < 0)
            {
                this.manaCurrent = 0;
            }
        }

        public bool CanAffordManaCost(Spell spell)
        {
            return this.manaCurrent >= spell.manaCost;
        }

        public bool AllSpellsOnCooldown()
        {
            bool result = true;
            foreach (Spell spell in this.spellBook)
            {
                if (!spell.isOnCooldown)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public bool AllSpellsAreNotAffordable()
        {
            bool result = true;
            foreach (Spell spell in this.spellBook)
            {
                if (this.CanAffordManaCost(spell))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public void ShowSpellbook()
        {
            Console.WriteLine("Your Mana is " + this.manaCurrent + ".");
            for (int i = 0; i < this.spellBook.Count; i++)
            {
                string output = "(" + i + ") " + this.spellBook[i].name + ", Mana cost: " + this.spellBook[i].manaCost;
                if (this.spellBook[i].isOnCooldown)
                {
                    output += " [Cooldown left: " + (this.spellBook[i].currentCooldown) + " Turns]";
                }
                Console.WriteLine(output);
            }
        }
    }
}
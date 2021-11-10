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
            // Initialize spell book
            this.spellBook.Add(new Spell(SpellType.Fireball, false, 16, 21, 2));
            this.spellBook.Add(new Spell(SpellType.Frostball, false, 7, 13, 2));
            this.spellBook.Add(new Spell(SpellType.HealingTouch, true, 18, 25, 3));
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

        public void CastSpell(Spell spell)
        {
            if (spell.selfTargeted)
            {
                spell.Cast(player: this);
            }
            else
            {
                Enemy target = this.currentCombat.PickTargetEnemy();
                spell.Cast(target: target);
            }
        }

        public void Heal(int healedHp)
        {
            int newHp = hpCurrent += healedHp;
            this.hpCurrent += healedHp;
            if (this.hpCurrent > this.hpTotal)
            {
                this.hpCurrent = this.hpTotal;
            }
            Console.WriteLine("You healed for " + healedHp + " HP.");
        }
    }
}
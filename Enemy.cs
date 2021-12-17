using System;
using System.Collections.Generic;

namespace southfury_csharp_dojo
{
    class Enemy
    {
        public Guid id;
        public EnemyType type;
        public int hpTotal = 25;
        public int hpCurrent;
        public int minAttackDamage = 3;
        public int maxAttackDamage = 8;
        public bool dead = false;

        public Enemy()
        {
            this.id = Guid.NewGuid();
            this.hpCurrent = this.hpTotal;
            Random r = new Random();
            int randomNumber = r.Next(3);
            this.type = (EnemyType)r.Next(0, Enum.GetNames(typeof(EnemyType)).Length);
        }

        public void PerformAttack(Player playerTarget)
        {
            Random random = new Random();
            int damage = random.Next(this.minAttackDamage, this.maxAttackDamage + 1);
            playerTarget.hpCurrent = playerTarget.hpCurrent - damage;
            Console.WriteLine("You are hit by " + this.type + " for " + damage + " damage!");
            playerTarget.UpdateState();
        }

        public void UpdateState()

        {
            if (this.hpCurrent <= 0)
            {
                this.dead = true;
            }
        }
    }

    enum EnemyType
    {
        Murloc,
        Bandit,
        Gnoll
    }
}
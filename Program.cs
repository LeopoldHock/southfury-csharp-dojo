using System;
using System.Collections.Generic;
// SOUTHFURY CODING DOJO - The RPG

namespace southfury_csharp_dojo
{
    class southfury_csharp_dojo
    {
        static void Main()
        {
            Console.WriteLine("Game starting.");
            Player player = new Player();
            Console.WriteLine("Welcome, " + player.name + "! Your adventure begins on the shores of Westfall...");
            Combat combat = new Combat(player);
        }
    }

    class Player
    {
        public string name;
        public int hpTotal = 50;
        public int hpCurrent;

        public Player()
        {
            Console.WriteLine("Name your character..");
            this.name = Console.ReadLine();
            this.hpCurrent = this.hpTotal;
        }
    }

    class Enemy
    {
        public Guid id;
        public EnemyType type;
        public int hpTotal = 25;
        public int hpCurrent;

        public Enemy()
        {
            this.id = Guid.NewGuid();
            this.hpCurrent = this.hpTotal;
            Random r = new Random();
            int randomNumber = r.Next(3);
            if (randomNumber == 0)
            {
                this.type = EnemyType.Bandit;
            }
            else if (randomNumber == 1)
            {
                this.type = EnemyType.Gnoll;
            }
            else
            {
                this.type = EnemyType.Murloc;
            }
        }
    }

    enum EnemyType
    {
        Murloc,
        Bandit,
        Gnoll
    }

    class Combat
    {
        public Player player;
        public List<Enemy> enemies = new List<Enemy>();

        public Combat(Player player)
        {
            this.player = player;
            Random random = new Random();
            int numberOfEnemies = random.Next(2, 5);
            for (int i = 0; i < numberOfEnemies; i++)
            {
                Enemy enemy = new Enemy();
                this.enemies.Add(enemy);
            }
            Console.WriteLine("Watch yourself! " + this.enemies.Count + " enemies appear!");
        }
    }
}
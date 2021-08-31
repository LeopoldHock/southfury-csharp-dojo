using System;
using System.Collections.Generic;

namespace southfury_csharp_dojo
{
    class Program
    {
        static void Main()
        {
            // Start the game - gameController will only be used to store data
            Store store = new Store();
            //Random random = new Random();
            //int numberOfGnolls = random.Next(2, 5);
            int numberOfGnolls = 3;

            //establish name
            Console.WriteLine("Game starts. Please name your hero:");
            string heroName = Console.ReadLine();
            store.player = new Player(heroName);
            Console.WriteLine("Welcome, " + store.player.heroName + ".");

            Console.WriteLine("You venture into the dark forest...");

            //establish gnoll amount
            for (int i = 0; i < numberOfGnolls; i++)
            {
                Gnoll newGnoll = new Gnoll();
                store.gnolls.Add(newGnoll);
            }

            Console.WriteLine("A group of " + numberOfGnolls + " wild Gnolls appears!");

            while (numberOfGnolls > 0)
            {
                Console.WriteLine("Gnoll HP: " + store.gnolls[0].hpCurrent + "/" + store.gnolls[0].hpTotal + " " + store.gnolls[1].hpCurrent + "/" + store.gnolls[1].hpTotal + " " + store.gnolls[2].hpCurrent + "/" + store.gnolls[2].hpTotal + " ");
                Console.WriteLine("You gotta whack 'em! Which one do you wanna hit?");
                string gnollToHit = Console.ReadLine();
                int gnollIndex = int.Parse(gnollToHit);
                gnollIndex--;
                Gnoll target = store.gnolls[gnollIndex];
                store.player.Attack(target, gnollIndex);
                Console.WriteLine(numberOfGnolls);


                for (int n = 0; n < store.gnolls.Count; n++)
                {
                    if (store.gnolls[n].aliveStatus == false)
                    {
                        numberOfGnolls--;
                    }
                }

            }
        }
    }

    public class Store
    {
        public Player player;
        public List<Gnoll> gnolls = new List<Gnoll>();
    }

    public class Player
    {
        public string heroName;
        public int playerHPTotal = 100;
        public int playerHPCurrent;

        public Player(string name)
        {
            this.heroName = name;
        }

        public void Attack(Gnoll target, int index)
        {
            if (target.aliveStatus == true)
            {
                Random random = new Random();
                int damage = random.Next(23, 37);
                target.TakeHit(damage, index);
            }
            else
            {
                Console.WriteLine("Stop! He's already dead!");
            }

        }
    }

    public class Gnoll
    {
        public int hpTotal = 50;
        public int hpCurrent;
        public bool aliveStatus = true;

        public Gnoll()
        {
            this.hpCurrent = hpTotal;
        }

        public void TakeHit(int damage, int index)
        {
            Console.WriteLine("You hit gnoll " + (index + 1) + " for " + damage + " damage!");
            this.hpCurrent = this.hpCurrent - damage;
            if (this.hpCurrent <= 0)
            {
                aliveStatus = false;
                Console.WriteLine("Gnoll " + (index + 1) + " is ded.");
            }
        }
    }
}
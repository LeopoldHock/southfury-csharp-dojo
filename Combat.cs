using System;
using System.Collections.Generic;

namespace southfury_csharp_dojo
{
    class Combat
    {
        public Player player;
        public List<Enemy> enemies = new List<Enemy>();
        public int turnCounter = 1;

        public Combat(Player player)
        {
            this.player = player;
            player.currentCombat = this;
            Random random = new Random();
            int numberOfEnemies = random.Next(2, 4);
            for (int i = 0; i < numberOfEnemies; i++)
            {
                Enemy enemy = new Enemy();
                this.enemies.Add(enemy);
            }
            Console.WriteLine("Watch yourself! " + this.enemies.Count + " enemies appear!");
            this.StartTurn();
        }

        public void StartTurn()
        {
            // Update Spell Cooldowns
            foreach (Spell spell in player.spellBook)
            {
                if (spell.isOnCooldown)
                {
                    spell.DecreaseCooldown();
                }
            }

            // Stat regeneration
            player.RegenerateMana();

            // Display status
            Console.WriteLine("Turn " + this.turnCounter + " starts.");
            Console.WriteLine("Your HP is " + player.hpCurrent + ".");
            Console.WriteLine("Your Mana is " + player.manaCurrent + ".");

            foreach (Enemy enemy in this.enemies)
            {
                if (enemy.dead)
                {
                    Console.WriteLine(enemy.type + " (DEAD)");
                }
                else
                {
                    Console.WriteLine(enemy.type + " (HP: " + enemy.hpCurrent + ")");
                }
            }
            Console.WriteLine("");
            this.AskForPlayerInput();
        }

        public void AskForPlayerInput()
        {
            string actionString = "";
            for (int i = 0; i < Enum.GetNames(typeof(PlayerAction)).Length; i++)
            {
                actionString += Enum.GetNames(typeof(PlayerAction))[i] + " (" + i + ")";
                if (i < Enum.GetNames(typeof(PlayerAction)).Length - 1)
                {
                    actionString += ", ";
                }
            }
            Console.WriteLine("Available actions: " + actionString);
            Console.WriteLine("What do you want to do?");
            this.ReadPlayerInput();
        }

        public void ReadPlayerInput()
        {
            String input = Console.ReadLine();
            // check whether the player entered a number
            int number;
            if (int.TryParse(input, out number))
            {
                if (Enum.IsDefined(typeof(PlayerAction), number))
                {
                    PlayerAction action = (PlayerAction)number;
                    this.PerformPlayerAction(action);
                }
                else
                {
                    this.AskForPlayerInput();
                }
            }
            else
            {
                this.AskForPlayerInput();
            }
        }

        public void PerformPlayerAction(PlayerAction action)
        {
            switch (action)
            {
                case PlayerAction.Attack:
                    this.player.PerformAttack(this.PickTargetEnemy());
                    this.EnemyAttack(player);
                    break;
                case PlayerAction.Spell:
                    if (this.player.AllSpellsOnCooldown())
                    {
                        Console.WriteLine("All your spells are currently on cooldown!\n");
                        this.AskForPlayerInput();
                    }
                    else if (this.player.AllSpellsAreNotAffordable())
                    {
                        Console.WriteLine("You cannot afford to cast any of your spells!\n");
                        this.AskForPlayerInput();
                    }
                    else
                    {
                        this.PlayerChoseSpell();
                        this.EnemyAttack(player);
                    }
                    break;
                case PlayerAction.Flee:
                    Console.WriteLine("You run for your life!");
                    this.EndCombat();
                    break;
            }
        }

        public void PlayerChoseSpell()
        {
            Console.WriteLine("Choose a spell (0-" + (this.player.spellBook.Count - 1) + "): ");
            this.player.ShowSpellbook();
            string input = Console.ReadLine();
            int number;
            if (int.TryParse(input, out number))
            {
                if (number >= 0 && number < this.player.spellBook.Count)
                {
                    Spell spell = this.player.spellBook[number];
                    if (spell.isOnCooldown)
                    {
                        Console.WriteLine(spell.name + " is on cooldown.");
                        this.PlayerChoseSpell();
                    }
                    else if (!player.CanAffordManaCost(spell))
                    {
                        Console.WriteLine("You don't have enough mana.");
                        this.PlayerChoseSpell();
                    }
                    else
                    {
                        this.player.CastSpell(spell);
                    }
                }
                else
                {
                    this.PlayerChoseSpell();
                }
            }
            else
            {
                this.PlayerChoseSpell();
            }
        }

        public Enemy PickTargetEnemy()
        {
            List<Enemy> availableTargets = new List<Enemy>();
            foreach (Enemy enemy in this.enemies)
            {
                if (enemy.dead)
                {
                    continue;
                }
                availableTargets.Add(enemy);
            }
            Console.WriteLine("Pick a target (0-" + (availableTargets.Count - 1) + "): ");
            for (int i = 0; i < availableTargets.Count; i++)
            {
                Console.WriteLine("(" + i + ") " + availableTargets[i].type + " (HP: " + availableTargets[i].hpCurrent + ")");
            }
            String input = Console.ReadLine();
            int number;
            if (int.TryParse(input, out number))
            {
                if (number >= 0 && number < availableTargets.Count)
                {
                    return availableTargets[number];
                }
                else
                {
                    return this.PickTargetEnemy();
                }
            }
            else
            {
                return this.PickTargetEnemy();
            }
        }

        public void EnemyAttack(Player playerTarget)
        {
            foreach (Enemy enemy in this.enemies)
            {
                if (enemy.dead)
                {
                    continue;
                }
                else
                {
                    enemy.PerformAttack(playerTarget);
                }
            }
            this.EndTurn();
        }

        public void EndTurn()
        {
            if (player.hpCurrent <= 0)
            {
                Console.WriteLine("WASTED!");
                EndCombat();
                return;
            }

            Console.WriteLine("Turn " + this.turnCounter + " ends.");
            Console.WriteLine("--------------");
            this.turnCounter++;
            if (this.CheckVictoryCondition())
            {
                Console.WriteLine("You were victorious!");
                this.EndCombat();
            }
            else
            {
                this.StartTurn();
            }
        }

        public void EndCombat()
        {
            this.player.currentCombat = null;
            Console.WriteLine("Combat ends.");
        }

        public bool CheckVictoryCondition()
        {
            bool allEnemiesDead = true;
            foreach (Enemy enemy in this.enemies)
            {
                if (enemy.dead == false)
                {
                    allEnemiesDead = false;
                    break;
                }
            }
            if (allEnemiesDead)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
using System;
using System.Collections.Generic;
// SOUTHFURY CODING DOJO - The RPG

/////////////////////////////////
// TO DO
/////////////////////////////////
// Features:
// Nerf spells (add mana/cooldown)
// Add spell effects

// QoL/Usability:

// Behind the scenes:
// Clean up!

namespace southfury_csharp_dojo
{
    class southfury_csharp_dojo
    {
        static void Main()
        {
            Console.WriteLine("Game starting.");
            Player player = new Player(askForName: false);
            player.spellBook.Add(new Spell(SpellType.Fireball, false, 16, 21, 2));
            player.spellBook.Add(new Spell(SpellType.Frostball, false, 7, 13, 2));
            player.spellBook.Add(new Spell(SpellType.HealingTouch, true, 18, 25, 3));
            Console.WriteLine("Welcome, " + player.name + "! Your adventure begins on the shores of Westfall...");
            Combat combat = new Combat(player);
        }
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
            if (askForName)
            {
                Console.WriteLine("Name your character..");
                this.name = Console.ReadLine();
            }
            else
            {
                this.name = "Nameless Hero";
            }
            this.hpCurrent = this.hpTotal;
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


    enum PlayerAction
    {
        Attack,
        Spell,
        Flee
    }

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
            int numberOfEnemies = random.Next(2, 5);
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
            Console.WriteLine("Turn " + this.turnCounter + " starts.");
            Console.WriteLine("Your HP is " + player.hpCurrent);
            if (player.hpCurrent <= 0)
            {
                Console.WriteLine("WASTED!");
                EndCombat();
                return;
            }

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
                    this.PlayerChoseSpell();
                    this.EnemyAttack(player);
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
            for (int i = 0; i < this.player.spellBook.Count; i++)
            {
                Console.WriteLine("(" + i + ") " + this.player.spellBook[i].type.ToString());
            }
            string input = Console.ReadLine();
            int number;
            if (int.TryParse(input, out number))
            {
                if (number >= 0 && number < this.player.spellBook.Count)
                {
                    Spell spell = this.player.spellBook[number];
                    this.player.CastSpell(spell);
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
                    if (this.enemies[number].dead)
                    {
                        Console.WriteLine("That target is dead already, you crazy bastard!");
                        return this.PickTargetEnemy();
                    }
                    else
                    {
                        return this.enemies[number];
                    }
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
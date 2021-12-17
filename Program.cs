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
            Console.WriteLine("Welcome, " + player.name + "! Your adventure begins on the shores of Westfall...");
            Combat combat = new Combat(player);
        }
    }
}
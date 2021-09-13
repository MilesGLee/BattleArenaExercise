using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    /// <summary>
    /// Represents any entity that exists in game
    /// </summary>
    struct Character
    {
        public string name;
        public float health;
        public float attackPower;
        public float defensePower;
    }

    class Game
    {
        bool gameOver;
        int currentScene;
        Character player;
        List<Character> enemies = new List<Character>();
        private int currentEnemyIndex = 0;
        private Character currentEnemy;

        /// <summary>
        /// Function that starts the main game loop
        /// </summary>
        public void Run()
        {
            Start();
            while (!gameOver) 
            {
                Update();
            }
            End();
        }

        /// <summary>
        /// Function used to initialize any starting values by default
        /// </summary>
        public void Start()
        {
            currentScene = 0;
            currentEnemyIndex = 0;
            enemies.Clear();
            Character slime;
            slime.name = "Slime";
            slime.health = 10;
            slime.attackPower = 1;
            slime.defensePower = 0;
            enemies.Add(slime);
            Character zomb;
            zomb.name = "Zom-B";
            zomb.health = 15;
            zomb.attackPower = 5;
            zomb.defensePower = 2;
            enemies.Add(zomb);
            Character kris;
            kris.name = "guy named Kris";
            kris.health = 25;
            kris.attackPower = 10;
            kris.defensePower = 5;
            enemies.Add(kris);
            currentEnemy = slime;
        }

        /// <summary>
        /// This function is called every time the game loops.
        /// </summary>
        public void Update()
        {
            DisplayCurrentScene();
        }

        /// <summary>
        /// This function is called before the applications closes
        /// </summary>
        public void End()
        {
            
        }

        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, string option1, string option2)
        {
            string input = "";
            int inputReceived = 0;

            while (inputReceived != 1 && inputReceived != 2)
            {//Print options
                Console.WriteLine(description);
                Console.WriteLine("1. " + option1);
                Console.WriteLine("2. " + option2);
                Console.Write("> ");

                //Get input from player
                input = Console.ReadLine();

                //If player selected the first option...
                if (input == "1" || input == option1)
                {
                    //Set input received to be the first option
                    inputReceived = 1;
                }
                //Otherwise if the player selected the second option...
                else if (input == "2" || input == option2)
                {
                    //Set input received to be the second option
                    inputReceived = 2;
                }
                //If neither are true...
                else
                {
                    //...display error message
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey();
                }

                Console.Clear();
            }
            return inputReceived;
        }

        /// <summary>
        /// Calls the appropriate function(s) based on the current scene index
        /// </summary>
        void DisplayCurrentScene()
        {
            if (currentScene == 0)
            {
                DisplayMainMenu();
            }
            if (currentScene == 1)
            {
                DisplayCombatScene();
            }
            if (currentScene == 2) 
            {
                DisplayFinalScene();
            }
        }

        /// <summary>
        /// Displays the menu that allows the player to start or quit the game
        /// </summary>
        void DisplayMainMenu()
        {
            GetPlayerName();
            CharacterSelection();
            currentScene++;
        }

        void DisplayCombatScene() 
        {
            Battle(ref currentEnemy);
            if (currentEnemy.health <= 0 && currentEnemyIndex < enemies.Count - 1)
            {
                currentEnemyIndex++;
                currentEnemy = enemies[currentEnemyIndex];
            }
            else if (currentEnemyIndex == enemies.Count - 1 && currentEnemy.health <= 0) 
            {
                currentScene++;
            }
            Console.Clear();
        }

        void DisplayFinalScene() 
        {
            Console.Clear();
            int endInput = GetInput("Play Again?", "Yah", "Nah");
            if (endInput == 1)
            {
                Start();
                Console.Clear();
            }
            else if (endInput == 2)
            {
                gameOver = true;
                Console.WriteLine("Begone ya pansey...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Displays text asking for the players name. Doesn't transition to the next section
        /// until the player decides to keep the name.
        /// </summary>
        void GetPlayerName()
        {
            Console.WriteLine("Welcome! Please enter your name.");
            Console.Write("> ");
            string inputName = Console.ReadLine();
            player.name = inputName;
        }

        /// <summary>
        /// Gets the players choice of character. Updates player stats based on
        /// the character chosen.
        /// </summary>
        public void CharacterSelection()
        {
            int characterChoice = GetInput($"Nice to meet you {player.name}. Please select a character.", "Wizard", "Knight");
            if (characterChoice == 1)
            {
                player.health = 50;
                player.attackPower = 25;
                player.defensePower = 5;
            }
            else if (characterChoice == 2) 
            {
                player.health = 75;
                player.attackPower = 15;
                player.defensePower = 10;
            }
        }

        /// <summary>
        /// Prints a characters stats to the console
        /// </summary>
        /// <param name="character">The character that will have its stats shown</param>
        void DisplayStats(Character character)
        {
            Console.WriteLine($"Name: {character.name}");
            Console.WriteLine($"Health: {character.health}");
            Console.WriteLine($"Attack Power: {character.attackPower}");
            Console.WriteLine($"Defense Power: {character.defensePower}");
        }

        /// <summary>
        /// Calculates the amount of damage that will be done to a character
        /// </summary>
        /// <param name="attackPower">The attacking character's attack power</param>
        /// <param name="defensePower">The defending character's defense power</param>
        /// <returns>The amount of damage done to the defender</returns>
        float CalculateDamage(float attackPower, float defensePower)
        {
            return attackPower - defensePower;
        }

        /// <summary>
        /// Deals damage to a character based on an attacker's attack power
        /// </summary>
        /// <param name="attacker">The character that initiated the attack</param>
        /// <param name="defender">The character that is being attacked</param>
        /// <returns>The amount of damage done to the defender</returns>
        public void Attack(ref Character attacker, ref Character defender)
        {
            float damage = CalculateDamage(attacker.attackPower, defender.defensePower);
            if (damage > 0)
            {
                defender.health -= damage;
                Console.WriteLine($"{defender.name} took {damage} damage from {attacker.name} and has {defender.health} health left");
            }
        }

        /// <summary>
        /// Simulates one turn in the current monster fight
        /// </summary>
        public void Battle(ref Character monster)
        {
            DisplayStats(player);
            Console.WriteLine("");
            DisplayStats(monster);
            Console.WriteLine("");
            int combatChoice = GetInput($"A {monster.name} stands in front of you! What will you do?", "Attack", "Dodge");
            if (combatChoice == 1) 
            {
                Attack(ref player, ref currentEnemy);
                Attack(ref currentEnemy, ref player);
                Console.ReadKey();
            }
            else if (combatChoice == 2) 
            {
                Console.Clear();
                Console.WriteLine("You dodged the enemies attack!");
                Console.ReadKey();
                Console.Clear();
                Battle(ref currentEnemy);
            }
            if (monster.health <= 0)
            {
                Console.Clear();
                Console.WriteLine($"You slayed the {monster.name}.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        /// <summary>
        /// Checks to see if either the player or the enemy has won the current battle.
        /// Updates the game based on who won the battle..
        /// </summary>
        void CheckBattleResults()
        {
            Console.WriteLine($"{player.name}'s health: {player.health}");
            Console.WriteLine($"{currentEnemy.name}'s health: {currentEnemy.health}");
        }

    }
}

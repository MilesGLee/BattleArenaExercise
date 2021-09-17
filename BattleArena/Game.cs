using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    public struct Item 
    {
        public string Name;
        public float StatBoost;
    }

    class Game
    {
        private bool _gameOver;
        private int _currentScene;
        private Player _player;
        private Entity[] _enemies;
        private int _currentEnemyIndex;
        private Entity _currentEnemy;
        private string _playerName;
        private Item[] _wizardItems;
        private Item[] _knightItems;

        public void Run()
        {
            Start();
            
            while (!_gameOver) 
            {
                Update();
            }
            End();
        }

        public void Start()
        {
            _gameOver = false;
            _currentScene = 0;
            InitializeEnemies();
            InitializeItems();
        }

        public void InitializeItems() 
        {
            //Wizard Items
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 5 };
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 15 };

            //Knight Items
            Item wand = new Item { Name = "Wand", StatBoost = 1025 };
            Item shoes = new Item { Name = "Shoes", StatBoost = 9000.05f};

            //Initialize Arrays
            _wizardItems = new Item[] { bigWand, bigShield};
            _knightItems = new Item[] { wand, shoes};
        }

        public void InitializeEnemies() 
        {
            _currentEnemyIndex = 0;

            Entity fraawg = new Entity("Fraawg", 42590, 15, 573);
        }

        public void Update()
        {
            
        }

        public void End()
        {
            
        }

        public void CharacterSelection() 
        {
            int choice = GetInput($"Nice to meet you {_playerName}. Please select a character", "Wizard", "Knight");

            if (choice == 1)
            {
                _player = new Player(_playerName, 50, 25000, 25000, _wizardItems);
                _currentScene++;
            }
            else if (choice == 2) 
            {
                _player = new Player(_playerName, 75, 15, 10, _knightItems);
                _currentScene++;
            }
        }

        public void DisplayStats(Entity character) 
        {
            Console.WriteLine($"Name: {character.Name}");
            Console.WriteLine($"Health: {character.Health}");
            Console.WriteLine($"Attack: {character.AttackPower}");
            Console.WriteLine($"Defense: {character.DefensePower}");
        }

        int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputReceived = -1;
            while (inputReceived == -1) 
            {
                Console.WriteLine(description);
                for (int i = 0; i < options.Length; i++) 
                {
                    Console.WriteLine($"{(i+1)}. {options[i]}");
                }
                Console.Write(">");
                input = Console.ReadLine();

                if (int.TryParse(input, out inputReceived)) 
                {
                    inputReceived--;
                    if (inputReceived < 0 || inputReceived >= options.Length)
                    {
                        inputReceived = -1;

                        Console.WriteLine("Invalid Input");
                        Console.ReadKey(true);
                    }
                }
            }
        }

        public void Battle() 
        {
            float damageDealt = 0;

            DisplayStats(_player);
            DisplayStats(_currentEnemy);

            int choice = GetInput($"A {_currentEnemy.Name} stands in front of you! What will you do?", "Attack", "Equip Item");

            if (choice == 1)
            {

            }
            else if (choice == 2) 
            {

            }

            damageDealt = _currentEnemy.Attack(_player);
            Console.WriteLine($"The {_currentEnemy.Name} dealt {damageDealt} damage!");

            Console.ReadKey(true);
            Console.Clear();
        }
    }
}

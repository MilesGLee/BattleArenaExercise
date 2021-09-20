using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArena
{
    public enum ItemType 
    {
        DEFENSE,
        ATTACK,
        NONE
    }

    public enum Scene 
    {
        STARTMENU,
        NAMECREATION,
        CHARACTERSELECTION,
        BATTLE,
        RESTARTMENU
    }

    public struct Item 
    {
        public string Name;
        public float StatBoost;
        public ItemType Type;
    }

    class Game
    {
        private bool _gameOver;
        private Scene _currentScene;
        private Player _player;
        private Entity[] _enemies;
        private int _currentEnemyIndex;
        private Entity _currentEnemy;
        private string _playerName;
        private Item[] _wizardItems;
        private Item[] _knightItems;

        int[] AppendToArray(int[] arr, int value) 
        {
            //Create a dummy array with one more element than our previous array.
            int[] tempArray = new int[arr.Length + 1];

            //Copy the old array values to the new array.
            for (int i = 0; i < arr.Length; i++) 
            {
                tempArray[i] = arr[i];
            }

            //Set the last index of array to be the new value
            tempArray[tempArray.Length - 1] = value;

            //Return new array.
            return tempArray;
        }

        public void Run()
        {
            int[] numbers = new int[4] { 1, 2, 3, 4 };

            numbers = AppendToArray(numbers, 5);

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
            Item bigWand = new Item { Name = "Big Wand", StatBoost = 5, Type = ItemType.ATTACK};
            Item bigShield = new Item { Name = "Big Shield", StatBoost = 15, Type = ItemType.DEFENSE};

            //Knight Items
            Item wand = new Item { Name = "Wand", StatBoost = 10, Type = ItemType.ATTACK};
            Item shoes = new Item { Name = "Shoes", StatBoost = 90f, Type = ItemType.DEFENSE};

            //Initialize Arrays
            _wizardItems = new Item[] { bigWand, bigShield};
            _knightItems = new Item[] { wand, shoes};
        }

        public void InitializeEnemies() 
        {
            _currentEnemyIndex = 0;

            Entity fraawg = new Entity("Fraawg", 42, 15, 573);

            Entity sassafrazzz = new Entity("Sassafrazzz", 74, 17, 59);

            Entity wompus = new Entity("Wompus with gun", 99, 55, 530);

            _enemies = new Entity[] {fraawg, sassafrazzz, wompus};

            _currentEnemy = _enemies[_currentEnemyIndex];
        }

        public void Update()
        {
            DisplayCurrentScnee();
        }

        public void End()
        {
            Console.WriteLine("Guh BAh");
            Console.ReadKey(true);
        }

        public void Save() 
        {
            StreamWriter writer = new StreamWriter("SaveData.txt");
            writer.WriteLine(_currentEnemyIndex);
            _player.Save(writer);
            _currentEnemy.Save(writer);

            writer.Close();
        }

        public bool Load() 
        {
            bool loadSuccessful = true;
            if (!File.Exists("SaveData.txt"))
                loadSuccessful = false;
            StreamReader reader = new StreamReader("SaveData.txt");
            if (!int.TryParse(reader.ReadLine(), out _currentEnemyIndex))
                loadSuccessful = false;

            string job = reader.ReadLine();

            if (job == "Wizard")
                _player = new Player(_wizardItems);
            else if (job == "Knight")
                _player = new Player(_knightItems);
            else
                loadSuccessful = false;

            _player.Job = job;

            if (!_player.Load(reader))
                loadSuccessful = false;
            if (!_currentEnemy.Load(reader))
                loadSuccessful = false;

            _enemies[_currentEnemyIndex] = _currentEnemy;

            reader.Close();

            return loadSuccessful;
        }

        void DisplayCurrentScnee() 
        {
            switch (_currentScene) 
            {
                case Scene.STARTMENU:
                    DisplayStartMenu();
                    break;
                case Scene.NAMECREATION:
                    GetPlayerName();
                    break;
                case Scene.CHARACTERSELECTION:
                    CharacterSelection();
                    break;
                case Scene.BATTLE:
                    Battle();
                    break;
                case Scene.RESTARTMENU:
                    DisplayRestartMenu();
                    break;
            }
        }

        public void CharacterSelection() 
        {
            int choice = GetInput($"Nice to meet you {_playerName}. Please select a character", "Wizard", "Knight");

            if (choice == 0)
            {
                _player = new Player(_playerName, 50, 25, 25, _wizardItems, "Wizard");
                _currentScene++;
            }
            else if (choice == 1) 
            {
                _player = new Player(_playerName, 75, 15, 10, _knightItems, "Knight");
                _currentScene++;
            }
        }

        void DisplayRestartMenu() 
        {
            int choice = GetInput("Play Again?", "Yes", "No");

            if (choice == 0)
            {
                _currentScene = 0;
                InitializeEnemies();
            }
            else if (choice == 1)
                _gameOver = true;
        }

        public void DisplayStartMenu() 
        {
            int choice = GetInput("Welcome to Lodis' All Star Brawl", "Start new game", "Load game");
            if (choice == 0)
                _currentScene = Scene.NAMECREATION;
            if (choice == 1) 
            {
                if (Load())
                {
                    Console.WriteLine("Load Successful");
                    Console.ReadKey();
                    Console.Clear();
                    _currentScene = Scene.BATTLE;
                }
                else 
                {
                    Console.WriteLine("Load Failed");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        void GetPlayerName() 
        {
            Console.WriteLine("Please enter your name.");
            Console.Write(">");
            _playerName = Console.ReadLine();

            Console.Clear();

            int choice = GetInput($"YOu've entered {_playerName}. Are you sure you want to keep this name?", "Yes", "No");

            if (choice == 0) 
            {
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
                else 
                {
                    inputReceived = -1;
                    Console.WriteLine("Invalid Input.");
                    Console.ReadKey(true);

                    Console.Clear();
                }

                Console.Clear();
            }

            return inputReceived;
        }

        public void DisplayEquipItemMenu() 
        {
            int choice = GetInput("Select an item to equip", _player.GetItemNames());

            if (!_player.tryEquipItem(choice))
                Console.WriteLine("You couldn't find that item in your bag.");

            Console.WriteLine($"You equipped {_player.CurrentItem.Name}!");
        }

        void CheckBattleResults() 
        {
            if (_player.Health <= 0)
            {
                Console.WriteLine($"You were slain by the {_currentEnemy.Name}");
                Console.ReadKey();
                Console.Clear();
                _currentScene = Scene.RESTARTMENU;
            }
            else if (_currentEnemy.Health <= 0) 
            {
                Console.WriteLine($"You slayed the {_currentEnemy.Name}");
                Console.ReadKey();
                Console.Clear();
                _currentEnemyIndex++;

                if (_currentEnemyIndex >= _enemies.Length) 
                {
                    _currentScene = Scene.RESTARTMENU;
                    Console.WriteLine("YOu've slain all the enemies!");
                    return;
                }

                _currentEnemy = _enemies[_currentEnemyIndex];
            }
        }

        public void Battle()
        {
            float damageDealt = 0;

            DisplayStats(_player);
            DisplayStats(_currentEnemy);

            int choice = GetInput($"A {_currentEnemy.Name} stands in front of you! What will you do?", "Attack", "Equip Item", "Remove current item", "Save Game", "Load Game");

            if (choice == 0)
            {
                damageDealt = _player.Attack(_currentEnemy);
                Console.WriteLine($"You dealt {damageDealt} damage.");
            }
            else if (choice == 1)
            {
                DisplayEquipItemMenu();
                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            else if (choice == 2)
            {
                if (!_player.TryRemoveCurrentItem())
                    Console.WriteLine("You don't have anything equipped");
                else
                {
                    Console.WriteLine("YOu placed the item in your bag.");
                }
            }
            else if (choice == 3)
            {
                Save();
                Console.WriteLine("Saved Game");
                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            else if (choice == 4) 
            {
                Load();
                Console.WriteLine("Loaded Game");
                Console.ReadKey(true);
                Console.Clear();
                return;
            }
            damageDealt = _currentEnemy.Attack(_player);
            Console.WriteLine($"The {_currentEnemy.Name} dealt {damageDealt} damage!");

            Console.ReadKey(true);
            Console.Clear();
        }
    }
}

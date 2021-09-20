using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArena
{
    class Player : Entity
    {
        private Item[] _items;
        private Item _currentItem;
        private int _currentItemIndex;
        private string _job;

        public override float DefensePower 
        {
            get 
            {
                if (_currentItem.Type == ItemType.DEFENSE)
                    return base.DefensePower + CurrentItem.StatBoost;
                return base.DefensePower;
            }
        }
        public override float AttackPower
        {
            get
            {
                if (_currentItem.Type == ItemType.ATTACK)
                {
                    return base.AttackPower + CurrentItem.StatBoost;
                }
                return base.AttackPower;
            }
        }

        public Item CurrentItem 
        {
            get 
            {
                return _currentItem;
            }
        }

        public string Job 
        {
            get 
            {
                return _job;
            }
            set 
            {
                _job = value;
            }
        }

        public Player() 
        {
            _items = new Item[0];
            _currentItem.Name = "Nothing";
        }

        public Player(Item[] items) : base()
        {
            
            _currentItem.Name = "Nothing";
            _items = items;
            _currentItemIndex = -1;
        }

        public Player(string name, float health, float attackPower, float defensePower, Item[] items, string job) : base(name, health, attackPower, defensePower)
        {
            _items = items;
            _currentItem.Name = "Nothing";
            _job = job;
            _currentItemIndex = -1;
        }

        public bool tryEquipItem(int index) 
        {
            if (index >= _items.Length || index < 0)
                return false;
            _currentItemIndex = index;

            //_currentItem = _items[index];

            _currentItem = _items[_currentItemIndex];

            return true;
        }

        public bool TryRemoveCurrentItem()
        {
            if (CurrentItem.Name == "Nothing")
                return false;
            _currentItem = new Item();
            _currentItem.Name = "Nothing";
            _currentItemIndex = 1;
            return true;
        }

        public string[] GetItemNames() 
        {
            string[] itemNames = new string[_items.Length];

            for (int i = 0; i < _items.Length; i++) 
            {
                itemNames[i] = _items[i].Name;
            }

            return itemNames;
        }

        public override void Save(StreamWriter writer)
        {
            writer.WriteLine(_job);
            base.Save(writer);
            writer.WriteLine(_currentItemIndex);
        }

        public override bool Load(StreamReader reader)
        {
            if(!base.Load(reader))
                return false;
            if (!int.TryParse(reader.ReadLine(), out _currentItemIndex))
                return false;
            return tryEquipItem(_currentItemIndex);
        }
    }
}

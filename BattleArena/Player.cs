using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    class Player : Entity
    {
        private Item[] _items;
        private Item _currentItem;

        public Item CurrentItem 
        {
            get 
            {
                return _currentItem;
            }
        }

        public Player(string name, float health, float attackPower, float defensePower, Item[] items) : base(name, health, attackPower, defensePower)
        {
            _items = items;
            _currentItem.Name = "Nothing";
        }

        public bool tryEquipItem(int index) 
        {
            if (index >= _items.Length || index < 0)
                return false;

            _currentItem = _items[index];

            return true;
        }

        public bool TryRemoveCurrentItem()
        {
            if (CurrentItem.Name == "Nothing")
                return false;
            _currentItem = new Item();
            _currentItem.Name = "Nothing";
            return true;
        }
    }
}

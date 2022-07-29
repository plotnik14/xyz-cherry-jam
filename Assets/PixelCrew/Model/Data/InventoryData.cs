using PixelCrew.Model.Definition;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private int _inventoryMaxSize = 10;
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public Action<string, int> OnChange;

        public bool Add(string id, int value)
        {          
            if (value <= 0) return false;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return false;

            if (itemDef.IsStackable)
            {
                if (!AddStackable(id, value)) return false;
            }
            else
            {
                
                if (!AddNotStackable(id, value)) return false;
            }

            OnChange?.Invoke(id, Count(id));
            return true;
        }

        private bool AddStackable(string id, int value)
        {
            var item = GetItem(id);

            if (item == null)
            {
                if (_inventory.Count >= _inventoryMaxSize)
                {
                    Debug.Log($"Inventory is full. Max size:{_inventoryMaxSize}");
                    return false;
                }

                item = new InventoryItemData(id);
                _inventory.Add(item);
            }

            item.Value += value;
            return true;
        }

        private bool AddNotStackable(string id, int value)
        {
            if (_inventory.Count >= _inventoryMaxSize)
            {
                Debug.Log($"Inventory is full. Max size:{_inventoryMaxSize}");
                return false;
            }

            var item = new InventoryItemData(id, value);
            _inventory.Add(item);
            return true;
        }

        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            var item = GetItem(id);

            if (item == null) return;

            item.Value -= value;

            if (item.Value <= 0)
            {
                _inventory.Remove(item);
            }

            OnChange?.Invoke(id, Count(id));
        }

        private InventoryItemData GetItem(string id)
        {
            foreach (InventoryItemData item in _inventory)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }

        public int Count(string id)
        {
            var count = 0;
            foreach (var item in _inventory)
            {
                if (item.Id == id)
                {
                    count += item.Value;
                }
            }
            return count;
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] public string Id;
        public int Value;

        public InventoryItemData(string id)
        {
            Id = id; 
        }

        public InventoryItemData(string id, int value)
        {
            Id = id;
            Value = value;
        }
    }
}

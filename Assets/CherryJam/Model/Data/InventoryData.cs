﻿using System;
using System.Collections.Generic;
using System.Linq;
using CherryJam.Model.Definition;
using CherryJam.Model.Definition.Repositories.Items;
using UnityEngine;

namespace CherryJam.Model.Data
{
    [Serializable]
    public class InventoryData
    {
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
                if (_inventory.Count >= DefsFacade.I.Player.InventorySize)
                {
                    Debug.Log($"Inventory is full. Max size:{DefsFacade.I.Player.InventorySize}");
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
            if (_inventory.Count >= DefsFacade.I.Player.InventorySize)
            {
                Debug.Log($"Inventory is full. Max size:{DefsFacade.I.Player.InventorySize}");
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

        public InventoryItemData[] GetAll(params ItemTag[] tags)
        {
            var retValue = new List<InventoryItemData>();

            foreach (var item in _inventory)
            {
                var itemDef = DefsFacade.I.Items.Get(item.Id);
                var allRequirementsMet = tags.All(tag => itemDef.HasTag(tag));
                if (allRequirementsMet)
                {
                    retValue.Add(item);
                }
            }
            
            return retValue.ToArray();
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

        public bool HasEnough(params ItemWithCount[] items)
        {
            var joinedItems = new Dictionary<string, int>();
            
            foreach (var item in items)
            {
                if (joinedItems.ContainsKey(item.ItemId))
                    joinedItems[item.ItemId] += item.Count;
                else 
                    joinedItems.Add(item.ItemId, item.Count);
            }

            foreach (var kvp in joinedItems)
            {
                var count = Count(kvp.Key);
                if (count < kvp.Value) return false;
            }

            return true;
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

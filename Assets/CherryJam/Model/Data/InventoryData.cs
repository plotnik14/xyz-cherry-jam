using System;
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

        public bool Add(string id, int value, int maxValue = -1)
        {          
            if (value <= 0) return false;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid)
            {
                Debug.LogWarning("Trying to add item with empty id to inventory");
                return false;
            }

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

            var maxCount = maxValue < 0 ? itemDef.MaxCount : maxValue;
            
            if (item.Value > maxCount)
                item.Value = maxCount;

            OnChange?.Invoke(id, Count(id));
            return true;
        }
        
        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.I.Items.Get(id);

            if (itemDef.IsVoid)
            {
                Debug.LogWarning("Trying to remove item with empty id to inventory");
                return;
            }

            var item = GetItem(id);

            if (item == null) return;

            item.Value -= value;

            if (item.Value <= 0)
                _inventory.Remove(item);
            
            OnChange?.Invoke(id, Count(id));
        }

        public InventoryItemData[] GetAllWithTags(params ItemTag[] tags)
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
            foreach (var item in _inventory)
                if (item.Id == id) return item;
            
            return null;
        }

        public int Count(string id)
        {
            foreach (var item in _inventory)
                if (item.Id == id) return item.Value;

            return 0;
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

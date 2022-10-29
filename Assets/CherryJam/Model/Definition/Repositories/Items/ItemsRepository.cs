using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CherryJam.Model.Definition.Repositories.Items
{
    [CreateAssetMenu(menuName = "Defs/Items", fileName = "Items")]
    public class ItemsRepository : DefRepository<ItemDef>
    {
        public ItemDef[] AllWithTag(ItemTag tag)
        {
            List<ItemDef> itemsWithTag = new List<ItemDef>();

            foreach (var itemDef in _collection)
            {
                if (itemDef.HasTag(tag))
                    itemsWithTag.Add(itemDef);
            }

            return itemsWithTag.ToArray();
        }

#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _collection;
#endif
    }

    [Serializable]
    public struct ItemDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private ItemTag[] _tags;
        [SerializeField] private int _maxCount;

        public string Id => _id;
        public int MaxCount => _maxCount;

        public bool IsVoid => string.IsNullOrEmpty(_id);

        public bool HasTag(ItemTag tag)
        {
            return _tags?.Contains(tag) ?? false;
        }
    }
}

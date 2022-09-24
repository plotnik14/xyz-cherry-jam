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
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemTag[] _tags;
        [SerializeField] private ItemWithCount _price;

        public string Id => _id;
        public bool IsStackable => HasTag(ItemTag.Stackable);
        public Sprite Icon => _icon;
        public ItemWithCount Price => _price;

        public bool IsVoid => string.IsNullOrEmpty(_id);

        public bool HasTag(ItemTag tag)
        {
            return _tags?.Contains(tag) ?? false;
        }
    }
}

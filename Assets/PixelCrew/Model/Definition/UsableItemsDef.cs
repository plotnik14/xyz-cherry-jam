using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Model.Definition
{
    [CreateAssetMenu(menuName = "Defs/UsableItemsDef", fileName = "UsableItemsDef")]
    public class UsableItemsDef : ScriptableObject
    {
        [SerializeField] private UsableDef[] _items;

        public UsableDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id)
                {
                    return itemDef;
                }
            }
            return default;
        }
    }

    [Serializable]
    public struct UsableDef
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private float _value;

        public string Id => _id;
        public float Value => _value;

        public bool IsVoid => string.IsNullOrEmpty(_id);
    }
}
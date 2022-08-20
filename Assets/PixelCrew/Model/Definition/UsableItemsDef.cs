using System;
using UnityEngine;

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
        [SerializeField] private UseActionDef _action;

        public string Id => _id;
        public float Value => _value;
        public UseActionDef Action => _action;

        public bool IsVoid => string.IsNullOrEmpty(_id);
    }

    public enum UseActionDef
    {
        Heal,
        BoostSpeed
    }
}
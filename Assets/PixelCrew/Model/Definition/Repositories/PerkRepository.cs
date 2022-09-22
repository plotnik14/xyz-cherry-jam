using System;
using System.Collections.Generic;
using PixelCrew.Model.Definition.Repositories.Items;
using UnityEngine;

namespace PixelCrew.Model.Definition.Repositories
{
    [CreateAssetMenu(menuName = "Defs/Repositories/Perks", fileName = "Perks")]
    public class PerkRepository : DefRepository<PerkDef>
    {
        public List<PerkDef> Get(List<string> ids)
        {
            // if (ids.Count == 0) return default;
            var perkDefs = new List<PerkDef>();

            foreach (var itemDef in _collection)
            {
                if (ids.Contains(itemDef.Id)) 
                    perkDefs.Add(itemDef);
            }
            
            return perkDefs;
        }
    }
    
    [Serializable]
    public struct PerkDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _info;
        [SerializeField] private ItemWithCount _price;
        [SerializeField] private float _cooldown;

        public string Id => _id;
        public Sprite Icon => _icon;
        public string Info => _info;
        public ItemWithCount Price => _price;
        public float Cooldown => _cooldown;
    }
}
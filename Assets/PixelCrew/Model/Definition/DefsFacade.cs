using System;
using UnityEngine;

namespace PixelCrew.Model
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;

        public InventoryItemsDef Items => _items;

        private static DefsFacade _instance;
        public static DefsFacade I => _instance ?? LoadDefs();

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }       
    }
}

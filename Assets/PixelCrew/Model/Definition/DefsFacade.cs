using UnityEngine;

namespace PixelCrew.Model.Definition
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;
        [SerializeField] private ThrowableItemsDef _throwableItems;
        [SerializeField] private UsableItemsDef _usableItems;
        [SerializeField] private PlayerDef _player;

        public InventoryItemsDef Items => _items;
        public ThrowableItemsDef ThrowableItems => _throwableItems;
        public UsableItemsDef UsableItems => _usableItems;
        public PlayerDef Player => _player;

        private static DefsFacade _instance;
        public static DefsFacade I => _instance ?? LoadDefs();

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }       
    }
}

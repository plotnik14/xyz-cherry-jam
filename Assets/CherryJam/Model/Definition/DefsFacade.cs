using CherryJam.Model.Definition.Player;
using CherryJam.Model.Definition.Repositories;
using CherryJam.Model.Definition.Repositories.Items;
using UnityEngine;

namespace CherryJam.Model.Definition
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private ItemsRepository _items;
        [SerializeField] private ThrowableRepository _throwableItems;
        [SerializeField] private UsableRepository _usableItems;
        [SerializeField] private PotionRepository _potions;
        [SerializeField] private PerkRepository _perks;
        [SerializeField] private PlayerDef _player;

        public ItemsRepository Items => _items;
        public ThrowableRepository ThrowableItems => _throwableItems;
        public UsableRepository UsableItems => _usableItems;
        public PotionRepository Potions => _potions;
        public PerkRepository Perks => _perks;

        public PlayerDef Player => _player;

        private static DefsFacade _instance;
        public static DefsFacade I => _instance ?? LoadDefs();

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }       
    }
}

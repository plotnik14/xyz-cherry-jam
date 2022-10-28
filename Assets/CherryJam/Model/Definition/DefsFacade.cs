using CherryJam.Model.Definition.Player;
using CherryJam.Model.Definition.Repositories.Items;
using UnityEngine;

namespace CherryJam.Model.Definition
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private ItemsRepository _items;
        [SerializeField] private PlayerDef _player;

        public ItemsRepository Items => _items;
        public PlayerDef Player => _player;

        private static DefsFacade _instance;
        public static DefsFacade I => _instance ?? LoadDefs();

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }       
    }
}

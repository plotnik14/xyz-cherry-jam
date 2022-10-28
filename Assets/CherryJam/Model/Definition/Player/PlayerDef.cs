using UnityEngine;

namespace CherryJam.Model.Definition.Player
{
    [CreateAssetMenu(menuName = "Defs/PlayerDef", fileName = "PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int _inventorySize;
        [SerializeField] private int _maxHP;
        
        public int InventorySize => _inventorySize;
        public int MaxHP => _maxHP;
    }
}

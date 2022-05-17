using UnityEngine;

namespace PixelCrew.Components
{
    public class CollectItemComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToCollect;
        [SerializeField] private HeroInventory _inventory;

        private const int SilverCoinValue = 1;
        private const int GoldCoinValue = 10;

        public void CollectCoin()
        {
            if (_objectToCollect.CompareTag("SilverCoin"))
            {
                _inventory.AddCoins(SilverCoinValue);
            }
            else if (_objectToCollect.CompareTag("GoldCoin"))
            {
                _inventory.AddCoins(GoldCoinValue);
            }
            else
            {
                Debug.LogError($"Untagged coin:{gameObject.name}");
            }
        }
    }

}

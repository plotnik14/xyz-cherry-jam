using UnityEngine;

namespace PixelCrew.Components
{
    public class CollectItemComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToCollect;

        private const int SilverCoinValue = 1;
        private const int GoldCoinValue = 10;

        public void CollectCoin(GameObject go)
        {
            var inventory = go.GetComponent<HeroInventory>();

            if (_objectToCollect.CompareTag("SilverCoin"))
            {
                inventory.AddCoins(SilverCoinValue);
            }
            else if (_objectToCollect.CompareTag("GoldCoin"))
            {
                inventory.AddCoins(GoldCoinValue);
            }
            else
            {
                Debug.LogError($"Untagged coin:{gameObject.name}");
            }
        }
    }

}
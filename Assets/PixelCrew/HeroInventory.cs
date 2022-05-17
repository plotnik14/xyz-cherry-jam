using UnityEngine;

namespace PixelCrew
{
    public class HeroInventory : MonoBehaviour
    {
        [SerializeField] private int _coins;

        public void Awake()
        {
            _coins = 0;
        }

        public void AddCoins(int coinsCount)
        {
            _coins += coinsCount;
            PrintCoinsCount();
        }

        private void PrintCoinsCount()
        {
            Debug.Log($"Coins count:{_coins}");
        }
    }
}


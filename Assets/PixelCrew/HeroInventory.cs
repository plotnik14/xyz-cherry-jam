using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew
{
    public class HeroInventory : MonoBehaviour
    {
        private GameSession _session;

        public void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }

        public void AddCoins(int coinsCount)
        {
            _session.Data.Coins += coinsCount;
            //PrintCoinsCount();
        }

        public void LoseCoins(int coinsCount)
        {
            _session.Data.Coins -= coinsCount;
            //PrintCoinsCount();
        }

        public void AddSwords(int swordsCount)
        {
            _session.Data.SwordsCount += swordsCount;
            PrintSwordsCount();
        }

        public void DecreaseSwords(int swordsCount)
        {
            _session.Data.SwordsCount -= swordsCount;
            PrintSwordsCount();
        }

        public int GetSwordsCount()
        {
            return _session.Data.SwordsCount;
        }

        public int GetCoinsCount()
        {
            return _session.Data.Coins;
        }

        private void PrintCoinsCount()
        {
            Debug.Log($"Coins count:{_session.Data.Coins}");
        }

        private void PrintSwordsCount()
        {
            Debug.Log($"Swords count:{_session.Data.SwordsCount}");
        }
    }
}


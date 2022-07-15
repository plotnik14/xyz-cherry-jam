using System;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Hp;
        public bool IsArmed;
        public int SwordsCount;
    
        public PlayerData Clone()
        {
            return new PlayerData
            {
                Coins = Coins,
                Hp = Hp,
                IsArmed = IsArmed,
                SwordsCount = SwordsCount
            };
        }
    }
}


﻿using System;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Hp;
        public bool IsArmed;
        public int SwordsCount;
    
        public PlayerData GetCopy()
        {          
            var clonedData = new PlayerData();

            clonedData.Coins = Coins;
            clonedData.Hp = Hp;
            clonedData.IsArmed = IsArmed;
            clonedData.SwordsCount = SwordsCount;

            return clonedData;
        }
    }
}


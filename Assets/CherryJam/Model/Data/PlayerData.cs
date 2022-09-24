﻿using System;
using CherryJam.Model.Data.Properties;
using UnityEngine;

namespace CherryJam.Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;

        public InventoryData Inventory => _inventory;

        public IntProperty Hp = new IntProperty();
        public PerksData Perks = new PerksData();
        public LevelData Levels = new LevelData();

        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}


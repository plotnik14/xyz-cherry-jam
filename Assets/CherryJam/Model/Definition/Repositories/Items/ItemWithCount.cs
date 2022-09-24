﻿using System;
using UnityEngine;

namespace CherryJam.Model.Definition.Repositories.Items
{
    [Serializable]
    public struct ItemWithCount
    {
        [InventoryId] [SerializeField] private string _itemId;
        [SerializeField] private int _count;

        public string ItemId => _itemId;
        public int Count => _count;
    }
}
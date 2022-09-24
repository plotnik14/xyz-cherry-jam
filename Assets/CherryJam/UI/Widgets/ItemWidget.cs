﻿using CherryJam.Model.Definition;
using CherryJam.Model.Definition.Repositories.Items;
using UnityEngine;
using UnityEngine.UI;

namespace CherryJam.UI.Widgets
{
    public class ItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;

        public void SetData(ItemWithCount price)
        {
            var def = DefsFacade.I.Items.Get(price.ItemId);
            _icon.sprite = def.Icon;

            _value.text = price.Count.ToString();
        }
    }
}
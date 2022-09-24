﻿using CherryJam.Model;
using CherryJam.Model.Data;
using CherryJam.Model.Definition;
using CherryJam.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace CherryJam.UI.Hud.QuickInventory
{
    public class InventoryItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selection;
        [SerializeField] private Text _value;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private int _index;

        private void Start()
        {
            var session = GameSession.Instance;
            var index = session.QuickInventory.SelectedIndex;
            _trash.Retain(index.SubscribeAndInvoke(OnIndexChanged));
        }

        private void OnIndexChanged(int newValue, int _)
        {
            _selection.SetActive(_index == newValue);
        }

        public void SetData(InventoryItemData item, int index)
        {
            _index = index;
            var def = DefsFacade.I.Items.Get(item.Id);
            _icon.sprite = def.Icon;
            _value.text = def.IsStackable ? item.Value.ToString() : string.Empty;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
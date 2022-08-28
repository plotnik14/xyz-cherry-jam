using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definition;
using PixelCrew.Model.Definition.Repositories.Items;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Model.Models
{
    public class QuickInventoryModel : IDisposable
    {
        private readonly PlayerData _data;

        public InventoryItemData[] Inventory { get; private set; }

        public readonly IntProperty SelectedIndex = new IntProperty();

        public Action OnChanged;

        public InventoryItemData SelectedItem
        {
            get
            {
                if (Inventory.Length > 0 && Inventory.Length > SelectedIndex.Value)
                    return Inventory[SelectedIndex.Value];
                
                return null;
            }
        }

        public ItemDef SelectedDef => DefsFacade.I.Items.Get(SelectedItem?.Id);

        public QuickInventoryModel(PlayerData data)
        {
            _data = data;

            Inventory = _data.Inventory.GetAll(ItemTag.Usable);
            _data.Inventory.OnChange += OnInventoryChanged;
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }
        
        private void OnInventoryChanged(string id, int value)
        {
            Inventory = _data.Inventory.GetAll(ItemTag.Usable);
            SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
            OnChanged?.Invoke();
        }

        public void SetNextItem()
        {
            SelectedIndex.Value = (int) Mathf.Repeat(SelectedIndex.Value + 1, Inventory.Length);
        }
        
        public void Dispose()
        {
            _data.Inventory.OnChange -= OnInventoryChanged;
        }
    }
}
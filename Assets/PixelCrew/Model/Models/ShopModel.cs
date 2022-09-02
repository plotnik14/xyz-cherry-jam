using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definition;
using PixelCrew.Model.Definition.Repositories.Items;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Models
{
    public class ShopModel : IDisposable
    {
        public readonly StringProperty InterfaceSelection = new StringProperty();
        public event Action OnSelectionChanged; 

        private readonly PlayerData _data;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public ShopModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = GetAllItemsForSale()[0].Id;
            _disposable.Retain(InterfaceSelection.Subscribe((x, y) => OnSelectionChanged?.Invoke()));
        }
        
        public IDisposable Subscribe(Action call)
        {
            OnSelectionChanged += call;
            return new ActionDisposable(() => OnSelectionChanged -= call);
        }

        public ItemDef[] GetAllItemsForSale()
        {
            return DefsFacade.I.Items.AllWithTag(ItemTag.ForSale);
        }

        public void BuyItem(string id)
        {
            var def = DefsFacade.I.Items.Get(id);
            var isEnoughResources = _data.Inventory.HasEnough(def.Price);

            if (isEnoughResources)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Inventory.Add(id, 1);
            }
        }

        public ItemDef GetSelectedItemDef()
        {
            return DefsFacade.I.Items.Get(InterfaceSelection.Value);
        }

        public void Dispose()
        {
            _disposable.Dispose();   
        }
    }
}
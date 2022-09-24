using CherryJam.Model;
using CherryJam.Model.Definition.Repositories.Items;
using CherryJam.UI.Widgets;
using CherryJam.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace CherryJam.UI.Windows.Shop
{
    public class ShopWindow : AnimatedWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private ItemWidget _price;
        [SerializeField] private Transform _itemsContainer;

        private PredefinedDataGroup<ItemDef, ShopInventoryItemWidget> _dataGroup;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private GameSession _session;
        private ItemDef[] _itemsForSale;
        
        protected override void Start()
        {
            base.Start();
            _session = GameSession.Instance;
            
            _disposable.Retain(_buyButton.onClick.Subscribe(OnBuy));
            _disposable.Retain(_session.ShopModel.Subscribe(OnSelectionChanged));
            
            _dataGroup = new PredefinedDataGroup<ItemDef, ShopInventoryItemWidget>(_itemsContainer);
            _itemsForSale = _session.ShopModel.GetAllItemsForSale();
            
            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            _dataGroup.SetData(_itemsForSale);
            _price.SetData(_session.ShopModel.GetSelectedItemDef().Price);
        }
        
        private void OnBuy()
        {
            _session.ShopModel.BuyItem(_session.ShopModel.InterfaceSelection.Value);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}
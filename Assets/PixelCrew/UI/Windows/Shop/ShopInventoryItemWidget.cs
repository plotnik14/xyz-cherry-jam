using PixelCrew.Model;
using PixelCrew.Model.Definition.Repositories.Items;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Shop
{
    public class ShopInventoryItemWidget : MonoBehaviour, IItemRenderer<ItemDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _isSelected;
        [SerializeField] private GameObject _isLocked;
        
        private GameSession _session;
        private ItemDef _data;

        private void Start()
        {
            _session = GameSession.Instance;
            
            if (_session != null)
                UpdateView();
        }
        
        public void SetData(ItemDef data, int index)
        {
            _data = data;
            
            if (_session != null)
                UpdateView();
        }
        
        private void UpdateView()
        {
            _icon.sprite = _data.Icon;
            _isSelected.SetActive(_data.Id == _session.ShopModel.InterfaceSelection.Value);
            _isLocked.SetActive(false); // for the future logic
        }
        
        public void OnSelect()
        {
            _session.ShopModel.InterfaceSelection.Value = _data.Id;
        }
    }
}
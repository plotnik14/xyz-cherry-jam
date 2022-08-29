using PixelCrew.Model;
using PixelCrew.Model.Definition;
using PixelCrew.Model.Definition.Repositories.Items;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class SingleInventoryItemWidget : MonoBehaviour
    {
        [InventoryId][SerializeField] private string _itemId;
        [SerializeField] private Image _icon;
        [SerializeField] private Text _value;
        
        private GameSession _session;
        
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Inventory.OnChange += OnInventoryChanged;

             _session.Data.Inventory.Count(_itemId);
             UpdateValue(_session.Data.Inventory.Count(_itemId));
        }

        private void OnInventoryChanged(string itemId, int value)
        {
            if (itemId == _itemId)
                UpdateValue(value);
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChange -= OnInventoryChanged;
        }

        private void UpdateValue(int value)
        {
            _value.text = value.ToString("0");
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            var itemDef = DefsFacade.I.Items.Get(_itemId);
            _icon.sprite = itemDef.Icon;
            UpdateValue(0);
        }
#endif
        
    }
}
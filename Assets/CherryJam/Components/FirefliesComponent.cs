using System.Collections.Generic;
using CherryJam.Model;
using CherryJam.Model.Definition.Repositories.Items;
using UnityEngine;

namespace CherryJam.Components
{
    public class FirefliesComponent : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> _fireflies;

        private int _firefliesMax;
        private int _currentFireflies;

        private GameSession _session;
        
        private void Start()
        {
            _session = GameSession.Instance;
            _session.Data.Inventory.OnChange += OnInventoryChanged;
            
            _firefliesMax = _session.Data.Inventory.Count(ItemId.FireflyCaptured.ToString());
            _currentFireflies = _session.Data.Inventory.Count(ItemId.FireflyToUse.ToString());
            
            UpdateView();
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == ItemId.FireflyCaptured.ToString())
                _firefliesMax = _session.Data.Inventory.Count(ItemId.FireflyCaptured.ToString());
            
            if (id == ItemId.FireflyToUse.ToString())
                _currentFireflies = _session.Data.Inventory.Count(ItemId.FireflyToUse.ToString());
            
            UpdateView();
        }

        private void UpdateView()
        {
            // Deactivate all
            foreach (var firefly in _fireflies)
            {
                firefly.color = new Color(255, 255, 255, 0);
                firefly.gameObject.SetActive(false);
            }
            
            // Activate
            for (var i = 0; i < _firefliesMax; i++)
            {
                _fireflies[i].gameObject.SetActive(true);
            }
            
            for (var i = 0; i < _currentFireflies; i++)
            {
                _fireflies[i].color = new Color(255, 255, 255, 1);;
            }
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChange -= OnInventoryChanged;
        }
    }
}
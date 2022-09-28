using System;
using System.Collections.Generic;
using CherryJam.Model;
using UnityEngine;

namespace CherryJam.Components
{
    public class FirefliesComponent : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> _fireflies;
        // [SerializeField] private Color _activeColor = Color.white;
        // [SerializeField] private Color _inactiveColor = Color.black;

        private int _firefliesMax;
        private int _currentFireflies;

        private const string FireflyMaxId = "FireflyMax";
        private const string FireflyId = "Firefly";
        
        private GameSession _session;
        
        private void Start()
        {
            _session = GameSession.Instance;
            _session.Data.Inventory.OnChange += OnInventoryChanged;
            
            _firefliesMax = _session.Data.Inventory.Count(FireflyMaxId);
            _currentFireflies = _session.Data.Inventory.Count(FireflyId);
            
            UpdateView();
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == FireflyMaxId)
                _firefliesMax = _session.Data.Inventory.Count(FireflyMaxId);
            
            if (id == FireflyId)
                _currentFireflies = _session.Data.Inventory.Count(FireflyId);
            
            UpdateView();
        }

        private void UpdateView()
        {
            // Deactivate all
            foreach (var firefly in _fireflies)
            {
                // firefly.color = _inactiveColor;
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
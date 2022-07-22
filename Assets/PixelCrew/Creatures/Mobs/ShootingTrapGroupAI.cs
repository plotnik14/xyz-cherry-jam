using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using PixelCrew.Utils;
using PixelCrew.Components;

namespace PixelCrew.Creatures 
{
    public class ShootingTrapGroupAI : MonoBehaviour
    {      
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private Cooldown _cooldown;
        [SerializeField] private ShootingTrapAI[] _shootingTraps;
        [SerializeField] private UnityEvent _onDie;

        private List<ShootingTrapAI> _activeTraps;
        private int _index;

        private void Start()
        {
            _activeTraps = new List<ShootingTrapAI>(_shootingTraps);
            _index = 0;
        }

        private void Update()
        {
            if (_activeTraps.Count == 0)
            {
                _onDie?.Invoke();
                return;
            }

            if (!_vision.IsTouchingLayer) return;
            if (!_cooldown.IsReady) return;

            PerformAttack();
            NextTrap();
        }
        
        private void PerformAttack()
        {
            var activeTrap = _activeTraps[_index];
            if (activeTrap == null) // It was killed by player
            {
                _activeTraps.RemoveAt(_index);
                return;
            }

            activeTrap.RangeAttack();
            _cooldown.Reset();
        }

        private void NextTrap()
        {
            _index++;
            if (_index >= _activeTraps.Count)
            {
                _index = 0;
            }
        }
    }
}
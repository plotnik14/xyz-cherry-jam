using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using PixelCrew.Utils;
using PixelCrew.Components;

namespace PixelCrew.Creatures 
{
    public class ShootingTrapGroupAI : MonoBehaviour
    {      
        [SerializeField] private ColliderCheck _vision;
        [SerializeField] private Cooldown _cooldown;
        [SerializeField] private List<ShootingTrapAI> _shootingTraps;
        [SerializeField] private UnityEvent _onDie;

        private int _index;

        private void Start()
        {
            _index = 0;
        }

        private void Update()
        {
            if (_shootingTraps.Count == 0)
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
            var activeTrap = _shootingTraps[_index];
            if (activeTrap == null) // It was killed by player
            {
                _shootingTraps.RemoveAt(_index);
                return;
            }

            activeTrap.RangeAttack();
            _cooldown.Reset();
        }

        private void NextTrap()
        {
            _index++;
            if (_index >= _shootingTraps.Count)
            {
                _index = 0;
            }
        }
    }
}
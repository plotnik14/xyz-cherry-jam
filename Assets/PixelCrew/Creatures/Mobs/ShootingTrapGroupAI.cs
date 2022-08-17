using System.Collections.Generic;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Creatures.Mobs 
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

            // Another option of remove logic
            //foreach (var trap in _shootingTraps)
            //{
            //    var health = trap.GetComponent<HealthComponent>();
            //    health.OnDie.AddListener( () => OnTrapDie(trap));
            //}
        }

        //private void OnTrapDie(ShootingTrapAI trap)
        //{
        //    _shootingTraps.Remove(trap);
        //}

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
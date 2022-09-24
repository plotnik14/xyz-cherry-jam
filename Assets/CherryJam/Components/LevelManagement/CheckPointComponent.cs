﻿using CherryJam.Components.GoBased;
using CherryJam.Model;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components.LevelManagement
{
    [RequireComponent(typeof(SpawnComponent))]
    public class CheckPointComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private SpawnComponent _heroSpawner;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnchecked;

        private GameSession _session;

        public string Id => _id;
        
        private void Start()
        {
            _session = GameSession.Instance;
            
            if (_session.IsChecked(_id))
            {
                _setChecked?.Invoke();
            }
            else
            {
                _setUnchecked?.Invoke();
            }
        }

        public void Check()
        {
            _session.SetChecked(_id);
            _setChecked?.Invoke();
        }

        public void SpawnHero()
        {
            _heroSpawner.Spawn();
        }
    }
}
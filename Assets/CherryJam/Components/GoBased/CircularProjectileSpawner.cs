﻿using System;
using System.Collections;
using CherryJam.Creatures.Weapons;
using CherryJam.Utils;
using CherryJam.Utils.ObjectPool;
using UnityEngine;

namespace CherryJam.Components.GoBased
{
    public class CircularProjectileSpawner : MonoBehaviour, IProjectileSpawner
    {
        [SerializeField] private bool _usePool = true;
        [SerializeField] private CircularProjectileSettings[] _settings;
        public int Stage { get; set; }
        
        [ContextMenu("Launch!")]
        public void LaunchProjectiles()
        {
            StartCoroutine(SpawnProjectiles());
        }

        private IEnumerator SpawnProjectiles()
        {
            var setting = _settings[Stage];
            var sectorAngle = 2 * Mathf.PI / setting.BurstCount;

            for (var i = 0; i < setting.BurstCount; i++)
            {
                var angle = sectorAngle * i;
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                StartCoroutine(SpawnBurst(direction, setting));
                
                yield return new WaitForSeconds(setting.DelayBetweenBursts);
            }
        }

        private IEnumerator SpawnBurst(Vector2 direction, CircularProjectileSettings setting)
        {
            for (var j = 0; j < setting.ItemsPerBurst; j++)
            {
                var instance = _usePool 
                    ? Pool.Instance.Get(setting.Prefab.gameObject, transform.position) 
                    : SpawnUtils.Spawn(setting.Prefab.gameObject, transform.position);
                
                
                var projectile = instance.GetComponent<DirectionalProjectile>();
                projectile.Launch(direction);

                yield return new WaitForSeconds(setting.DelayBetweenItems);
            }
        }
        
        [Serializable]
        public struct CircularProjectileSettings
        {
            [SerializeField] private DirectionalProjectile _prefab;
            [SerializeField] private int _burstCount;
            [SerializeField] private float _delayBetweenBursts;
            
            [Space][Header("Burst Config")]
            [SerializeField] private int _itemsPerBurst;
            [SerializeField] private float _delayBetweenItems;

            public DirectionalProjectile Prefab => _prefab;
            public int BurstCount => _burstCount;
            public int ItemsPerBurst => _itemsPerBurst;
            public float DelayBetweenBursts => _delayBetweenBursts;
            public float DelayBetweenItems => _delayBetweenItems;
        }
    }
}
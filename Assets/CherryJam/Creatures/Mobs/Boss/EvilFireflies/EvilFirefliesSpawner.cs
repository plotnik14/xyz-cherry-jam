using System.Collections.Generic;
using CherryJam.Components.GoBased;
using UnityEngine;

namespace CherryJam.Creatures.Mobs.Boss.EvilFireflies
{
    public class EvilFirefliesSpawner : SpawnComponent
    {
        [SerializeField] private List<EvilFireflyConfig> _fireflyConfigs;
        
        public void SpawnFireflies()
        {
            foreach (var config in _fireflyConfigs)
            {
                var instance = SpawnInactive();
                var firefly = instance.GetComponent<EvilFirefly>();
                firefly.SetWayPoints(config.Waypoints);
                instance.SetActive(true);
            }
        }
    }
}
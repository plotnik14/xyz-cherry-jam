using System.Collections.Generic;
using UnityEngine;

namespace CherryJam.Components.GoBased
{
    public class MultipleSpawnComponent : SpawnComponent
    {
        [SerializeField] private List<Transform> _spawnPoints;

        public void SpawnMultiple()
        {
            foreach (var spawnPoint in _spawnPoints)
            {
                _target = spawnPoint;
                Spawn();
            }
        }
    }
}
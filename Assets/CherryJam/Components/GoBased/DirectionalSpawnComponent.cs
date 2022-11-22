using CherryJam.Creatures.Weapons;
using CherryJam.Utils;
using CherryJam.Utils.ObjectPool;
using UnityEngine;

namespace CherryJam.Components.GoBased
{
    public class DirectionalSpawnComponent : SpawnComponent
    {
        [ContextMenu("Spawn")]
        public void Spawn(Vector2 direction)
        {
            var instance = SpawnInactive();
            var projectile = instance.GetComponent<DirectionalProjectile>();
            instance.SetActive(true);
            projectile.Launch(direction);
        }
    }
}
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
            var instance = _usePool
                ? Pool.Instance.Get(_prefab, _target.position, transform.lossyScale)
                : SpawnUtils.Spawn(_prefab, _target.position);
            
            instance.transform.localScale = transform.lossyScale;

            var projectile = instance.GetComponent<DirectionalProjectile>();
            projectile.Launch(direction);
            
            instance.SetActive(true);
        }
    }
}
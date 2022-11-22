using CherryJam.Utils;
using CherryJam.Utils.ObjectPool;
using UnityEngine;

namespace CherryJam.Components.GoBased
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] protected Transform _target;
        [SerializeField] protected GameObject _prefab;
        [SerializeField] protected bool _usePool;
        [SerializeField] protected bool _useSpawnerScale = true;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var instance = SpawnInactive();
            instance.SetActive(true);
        }

        protected GameObject SpawnInactive()
        {
            var instance = _usePool
                ? GetInstanceFromPool()
                : GetNewInstance();

            return instance;
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }

        private GameObject GetInstanceFromPool()
        {
            return _useSpawnerScale
                ? Pool.Instance.Get(_prefab, _target.position, transform.lossyScale)
                : Pool.Instance.Get(_prefab, _target.position);
        }

        private GameObject GetNewInstance()
        {
            var instance = SpawnUtils.Spawn(_prefab, _target.position);
            
            if (_useSpawnerScale)
                instance.transform.localScale = transform.lossyScale;

            return instance;
        }
    }
}

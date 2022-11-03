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
            var instance = _usePool
            ? Pool.Instance.Get(_prefab, _target.position, _useSpawnerScale ? transform.lossyScale : Vector3.zero)
            : SpawnUtils.Spawn(_prefab, _target.position);
            
            if (_useSpawnerScale)
                instance.transform.localScale = transform.lossyScale;
            
            instance.SetActive(true);
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }
    }
}

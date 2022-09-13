using PixelCrew.Utils;
using PixelCrew.Utils.ObjectPool;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _usePool;

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var instance = _usePool
            ? Pool.Instance.Get(_prefab, _target.position, transform.lossyScale)
            : SpawnUtils.Spawn(_prefab, _target.position);
            
            instance.transform.localScale = transform.lossyScale;
            instance.SetActive(true);
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }
    }
}

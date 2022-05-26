using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;

        public void Spawn()
        {
            var instance = Instantiate(_prefab, _target.position, Quaternion.identity);
            instance.transform.localScale = transform.lossyScale;
        }

        [ContextMenu("Spawn")]
        private void TestSpawn()
        {
            Spawn();
        }
    }
}

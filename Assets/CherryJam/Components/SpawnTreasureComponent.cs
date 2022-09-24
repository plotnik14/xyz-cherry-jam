using UnityEngine;

namespace CherryJam.Components
{
    internal class SpawnTreasureComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _silverCoinPrefab;
        [SerializeField] private GameObject _goldCoinPrefab;
        [SerializeField][Range(1, 10)] private int _coinsCount;
        [SerializeField][Range(1, 100)] private int _chanceOfGoldCoin;
        [SerializeField] private float _spawnCircleRadius;

        public void SpawnTreasure()
        {
            var random = new System.Random();           

            for (int i = 0; i < _coinsCount; i++)
            {
                var isGoldCoin = random.Next(1, 101) < _chanceOfGoldCoin;
                var prefab = isGoldCoin ? _goldCoinPrefab : _silverCoinPrefab;
                SpawnByPrefabInCircle(prefab, i);
            }
        }

        private void SpawnByPrefabInCircle(GameObject prefab, int index)
        {
            var angle = index * Mathf.PI * 2f / _coinsCount;
            var newX = transform.position.x + Mathf.Cos(angle) * _spawnCircleRadius;
            var newY = transform.position.y + Mathf.Sin(angle) * _spawnCircleRadius;
            var newPosition = new Vector3(newX, newY, transform.position.z);

            var instance = Instantiate(prefab, newPosition, Quaternion.identity);
            instance.transform.localScale = transform.lossyScale;
        }
    }
}

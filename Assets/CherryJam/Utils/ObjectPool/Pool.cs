using System.Collections.Generic;
using UnityEngine;

namespace CherryJam.Utils.ObjectPool
{
    public class Pool : MonoBehaviour
    {
        private readonly Dictionary<int, Queue<PoolItem>> _items = new Dictionary<int, Queue<PoolItem>>();

        private static Pool _instance;

        public static Pool Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                var go = new GameObject("### MAIN_POOL ###");
                _instance = go.AddComponent<Pool>();
                return _instance;
            }
        }
        
        public GameObject Get(GameObject go, Vector3 position, Vector3 scale)
        {
            return GetOrCreateItem(go, position, scale);
        }
        
        public GameObject Get(GameObject go, Vector3 position)
        {
            return GetOrCreateItem(go, position, Vector3.zero);
        }

        private GameObject GetOrCreateItem(GameObject go, Vector3 position, Vector3 scale)
        {
            var id = go.GetInstanceID();
            var queue = RequireQueue(id);

            if (queue.Count > 0)
            {
                var pooledItem = queue.Dequeue();
                pooledItem.transform.position = position;

                if (scale != Vector3.zero)
                    pooledItem.transform.localScale = scale;
                
                pooledItem.gameObject.SetActive(true);
                pooledItem.Restart();
                return pooledItem.gameObject;
            }

            var instance = SpawnUtils.Spawn(go, position, gameObject.name);
            var poolItem = instance.GetComponent<PoolItem>();
            poolItem.Retain(id, this);
            return instance;
        }

        private Queue<PoolItem> RequireQueue(int id)
        {
            if (!_items.TryGetValue(id, out var queue))
            {
                queue = new Queue<PoolItem>();
                _items.Add(id, queue);
            }

            return queue;
        }

        public void Release(int id, PoolItem poolItem)
        {
            poolItem.gameObject.SetActive(false);
            
            var queue = RequireQueue(id);
            queue.Enqueue(poolItem);
        }
    }
}
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Utils.ObjectPool
{
    public class PoolItem : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onRestart;
        
        private int _id;
        private Pool _pool;

        public void Restart()
        {
            _onRestart?.Invoke();
        }
        
        public void Retain(int id, Pool pool)
        {
            _id = id;
            _pool = pool;
        }

        public void Release()
        {
            _pool.Release(_id, this);
        }
    }
}
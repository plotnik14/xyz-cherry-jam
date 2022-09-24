using CherryJam.Utils.ObjectPool;
using UnityEngine;

namespace CherryJam.Components.GoBased
{
    public class DisableObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;

        public void DestroyObject()
        {
            Destroy(_objectToDestroy);
        }

        public void ReleaseOrDestroy(GameObject go)
        {
            var poolItem = go.GetComponent<PoolItem>();
            
            if (poolItem != null)
                poolItem.Release();
            else
                Destroy(go);
        }

        public void TurnOff(GameObject go)
        {
            var rb = go.GetComponent<Rigidbody2D>();
            rb.simulated = false;
        }
    }
}
using CherryJam.Utils;
using UnityEngine;

namespace CherryJam.Components.ColliderBased
{
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.IsInLayer(_layer)) return;
            if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag)) return;
            
            _action?.Invoke(other.gameObject);  
        }
    }
}

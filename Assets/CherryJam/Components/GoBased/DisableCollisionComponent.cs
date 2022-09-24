using System.Collections;
using UnityEngine;

namespace CherryJam.Components.GoBased
{
    public class DisableCollisionComponent : MonoBehaviour
    {
        [SerializeField] private Collider2D _current;
        [SerializeField] private float _disableTime = 0.5f;

        private Coroutine _activeCoroutine;
        
        public void DisableCollision(GameObject target)
        {
            if (_activeCoroutine != null) return;

            _activeCoroutine = StartCoroutine(DisableCollisionCoroutine(target));
        }

        private IEnumerator DisableCollisionCoroutine(GameObject target)
        {
            Physics2D.IgnoreCollision(_current, target.GetComponent<Collider2D>());
            yield return new WaitForSeconds(_disableTime);
            Physics2D.IgnoreCollision(_current, target.GetComponent<Collider2D>(), false);
            _activeCoroutine = null;
        }
    }
}
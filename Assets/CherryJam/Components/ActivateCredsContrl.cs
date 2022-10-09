using UnityEngine;

namespace CherryJam.Components
{
    public class ActivateCredsContrl : MonoBehaviour
    {
        [SerializeField] private GameObject _container;

        public void Activate()
        {
            _container.SetActive(true);
        }

    }
}
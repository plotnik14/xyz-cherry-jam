using CherryJam.Model;
using CherryJam.Model.Definition.Repositories.Items;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components
{
    public class UpdateHubComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _OnAllCollected;

        private void Start()
        {
            CheckFireflies();
        }

        private void CheckFireflies()
        {
            var firefliesMax = GameSession.Instance.Data.Inventory.Count(ItemId.FireflyCaptured.ToString());
            if (firefliesMax == 3)
            {
                _OnAllCollected?.Invoke();
            }
        }
    }
}
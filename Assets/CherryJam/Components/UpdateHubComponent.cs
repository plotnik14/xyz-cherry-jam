using System;
using CherryJam.Model;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components
{
    public class UpdateHubComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _OnAllCollected;
        
        private const string FireflyMaxId = "FireflyMax";

        private void Start()
        {
            CheckFireflies();
        }

        private void CheckFireflies()
        {
            var firefliesMax = GameSession.Instance.Data.Inventory.Count(FireflyMaxId);
            if (firefliesMax == 3)
            {
                _OnAllCollected?.Invoke();
            }
        }
    }
}
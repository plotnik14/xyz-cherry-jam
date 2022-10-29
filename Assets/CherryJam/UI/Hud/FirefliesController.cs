using CherryJam.Model;
using CherryJam.Model.Definition.Repositories.Items;
using UnityEngine;

namespace CherryJam.UI.Hud
{
    public class FirefliesController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private GameSession _session;
        private int _firefliesMax;
        private static readonly int FirefliesCountKey = Animator.StringToHash("fireflies-count");

        private void Start()
        {
            _session = GameSession.Instance;
            _session.Data.Inventory.OnChange += OnInventoryChanged;
            
            _firefliesMax = _session.Data.Inventory.Count(ItemId.FireflyToUse.ToString());
            UpdateView();
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == ItemId.FireflyToUse.ToString())
            {
                _firefliesMax = _session.Data.Inventory.Count(ItemId.FireflyToUse.ToString());
                UpdateView();
            }
        }
        
        private void UpdateView()
        {
            _animator.SetInteger(FirefliesCountKey, _firefliesMax);
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChange -= OnInventoryChanged;
        }
    }
}
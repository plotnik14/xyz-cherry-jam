using PixelCrew.Creatures.Hero;
using PixelCrew.Model.Definition;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Collectables
{
    public class AddToInventoryComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _count;

        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onFail;

        public void Add (GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null)
            {
                if (hero.AddToInventory(_id, _count))
                {
                    _onSuccess?.Invoke();
                }
                else
                {
                    _onFail?.Invoke();
                }
            }
        }
    }
}

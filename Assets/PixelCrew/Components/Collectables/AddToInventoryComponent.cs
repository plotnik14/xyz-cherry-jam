using PixelCrew.Creatures;
using PixelCrew.Model.Definition;
using System;
using UnityEngine;

namespace PixelCrew.Components
{
    public class AddToInventoryComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add (GameObject go)
        {
            var hero = go.GetComponent<Hero>();
            if (hero != null)
            {
                hero.AddToInventory(_id, _count);
            }
        }
    }
}

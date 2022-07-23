using PixelCrew.Creatures;
using System;
using UnityEngine;

namespace PixelCrew.Components
{
    public class AddToInventoryComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
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

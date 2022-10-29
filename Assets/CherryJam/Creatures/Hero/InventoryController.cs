using CherryJam.Model;
using CherryJam.Model.Data;
using CherryJam.Model.Definition.Repositories.Items;
using UnityEngine;

namespace CherryJam.Creatures.Hero
{
    public class InventoryController : MonoBehaviour
    {
        private InventoryData Inventory => GameSession.Instance.Data.Inventory;
        
        public bool Add(string id, int value)
        {
            if (ItemId.FireflyToUse.ToString() == id)
                return AddFireflies(id, value);
            
            return Inventory.Add(id, value);
        }

        private bool AddFireflies(string id, int value)
        {
            var firefliesCaptured = Inventory.Count(ItemId.FireflyCaptured.ToString());
            return Inventory.Add(id, value, firefliesCaptured);
        }
    }
}
using System;
using CherryJam.Model.Definition.Repositories.Items;
using UnityEngine;

namespace CherryJam.Model.Definition.Repositories
{
    [CreateAssetMenu(menuName = "Defs/Repositories/Throwable", fileName = "Throwable")]
    public class ThrowableRepository : DefRepository<ThrowableDef>
    {
    }

    [Serializable]
    public struct ThrowableDef : IHaveId
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private GameObject _projectile;

        public string Id => _id;
        public GameObject Projectile => _projectile;
    }
}
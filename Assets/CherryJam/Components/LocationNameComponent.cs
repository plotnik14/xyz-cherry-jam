using System;
using CherryJam.Model.Data.Properties;
using UnityEngine;

namespace CherryJam.Components
{
    public class LocationNameComponent : MonoBehaviour
    {
        [SerializeField] private string _locationName;

        public StringProperty LocationName = new StringProperty();

        private void Start()
        {
            LocationName.Value = _locationName;
        }
    }
}
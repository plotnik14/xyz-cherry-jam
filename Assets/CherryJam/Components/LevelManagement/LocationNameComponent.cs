using CherryJam.Model.Data.Properties;
using CherryJam.Model.Definition.Localization;
using UnityEngine;

namespace CherryJam.Components.LevelManagement
{
    public class LocationNameComponent : MonoBehaviour
    {
        [SerializeField] private string _locationName;

        public string LocationName => _locationName;

        private void Awake()
        {
            _locationName = Localize(_locationName);
        }

        private string Localize(string locationName)
        {
            return string.IsNullOrEmpty(locationName) 
                ? locationName 
                : LocalizationManager.I.Localize(locationName);
        }
    }
}
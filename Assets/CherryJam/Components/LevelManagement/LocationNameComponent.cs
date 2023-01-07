using CherryJam.Model.Data.Properties;
using CherryJam.Model.Definition.Localization;
using UnityEngine;

namespace CherryJam.Components.LevelManagement
{
    public class LocationNameComponent : MonoBehaviour
    {
        [SerializeField] private string _locationName;

        public StringProperty LocationName = new StringProperty();

        private void Start()
        {
            var locationName = Localize(_locationName);
            LocationName.Value = locationName;
        }

        private string Localize(string locationName)
        {
            return string.IsNullOrEmpty(locationName) 
                ? locationName 
                : LocalizationManager.I.Localize(locationName);
        }
    }
}
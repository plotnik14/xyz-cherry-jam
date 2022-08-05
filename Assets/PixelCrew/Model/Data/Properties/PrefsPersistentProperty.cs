using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    public abstract class PrefsPersistentProperty<TPropetryType> : PersistentProperty<TPropetryType>
    {
        protected string Key;

        protected PrefsPersistentProperty(TPropetryType defaultValue, string key) : base(defaultValue)
        {
            Key = key;
        }
    }
}

using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public abstract class PersistentProperty<TPropertyType>
    {
        [SerializeField] protected TPropertyType _value; // for display
        protected TPropertyType _storedValue; // saved on disk
        private TPropertyType _defaultValue;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue);

        public event OnPropertyChanged OnChanged;

        public PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public TPropertyType Value
        {
            get => _storedValue;
            set
            {
                var isEqual = _storedValue.Equals(value);
                if (isEqual) return;

                var oldValue = _storedValue;
                Write(value);
                _storedValue = _value = value;

                OnChanged?.Invoke(value, oldValue);
            }
        }

        protected void Init()
        {
            _storedValue = _value = Read(_defaultValue);
        }

        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);

        public void Validate()
        {
            if (!_storedValue.Equals(_value))
            {
                Value = _value;
            }
        }
    }
}

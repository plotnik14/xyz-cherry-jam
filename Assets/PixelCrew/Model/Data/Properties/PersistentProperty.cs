using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public abstract class PersistentProperty<TPropetryType>
    {
        [SerializeField] protected TPropetryType _value; // for display
        private TPropetryType _storedValue; // saved on disk
        private TPropetryType _defaultValue;

        public delegate void OnPropertyChanged(TPropetryType newValue, TPropetryType oldValue);

        public event OnPropertyChanged OnChanged;

        public PersistentProperty(TPropetryType defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public TPropetryType Value
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

        protected abstract void Write(TPropetryType value);
        protected abstract TPropetryType Read(TPropetryType defaultValue);

        public void Validate()
        {
            if (!_storedValue.Equals(_value))
            {
                Value = _value;
            }
        }
    }
}

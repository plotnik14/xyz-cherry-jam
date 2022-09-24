namespace CherryJam.Model.Data.Properties
{
    public abstract class PersistentProperty<TPropertyType> : ObservableProperty<TPropertyType>
    {
        protected TPropertyType _storedValue; // saved on disk
        private TPropertyType _defaultValue;

        public PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public override TPropertyType Value
        {
            get => _storedValue;
            set
            {
                var isEqual = _storedValue.Equals(value);
                if (isEqual) return;

                var oldValue = _storedValue;
                Write(value);
                _storedValue = _value = value;

                InvokeChangedEvent(value, oldValue);
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

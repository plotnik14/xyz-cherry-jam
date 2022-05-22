namespace PixelCrew
{
    public class SimpleModel
    {
        public int Value { get; private set; }

        private int _increaseValue;
        private int _decreaseValue;

        public SimpleModel(int increaseValue, int decreaseValue)
        {
            _increaseValue = increaseValue;
            _decreaseValue = decreaseValue;
        }

        public void Increase()
        {
            Value += _increaseValue;
        }

        public void Decrease()
        {
            Value -= _decreaseValue;
        }
    }
}

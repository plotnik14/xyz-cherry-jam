using NUnit.Framework;
using CherryJam;

namespace Tests.EditMode
{
    internal class SimpleModelTest
    {
        private SimpleModel _model;

        [SetUp]
        public void Init()
        {
            _model = new SimpleModel(2, 3);
        }

        [Test]
        public void TestIncreaseValue()
        {
            _model.Increase();
            Assert.AreEqual(2, _model.Value);
        }

        [Test]
        public void TestDecreaseValue()
        {
            _model.Decrease();
            Assert.AreEqual(-3, _model.Value);
        }

        [TearDown]
        public void TearDown()
        {
            // release resources 
        }
    }
}

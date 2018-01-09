using NUnit.Framework;

namespace PsakeBuildCsharp.UnitTests.Tests
{
    [TestFixture]
    public class SomeTests
    {
        [Test]
        public void IWillPass()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void IWillFail()
        {
            Assert.AreEqual(true, false);
        }
    }
}

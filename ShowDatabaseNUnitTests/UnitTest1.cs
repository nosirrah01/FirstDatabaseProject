using NUnit.Framework;
using NewUserConsoleApp;

namespace ShowDatabaseNUnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
        public void Test2()
        {
            UiClass.DisplayTable("David");
            Assert.Pass();
        }
    }
}
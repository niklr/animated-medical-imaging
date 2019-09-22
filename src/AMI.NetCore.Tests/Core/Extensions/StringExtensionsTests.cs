using AMI.Core.Extensions.StringExtensions;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Extensions
{
    [TestFixture]
    public class StringExtensionsTests : BaseTest
    {
        [TestCase("test", "", "test")]
        [TestCase("test", " ", "test")]
        [TestCase("test", "#", "#test#")]
        public void StringExtensions_Embed(string value, string separator, string expected)
        {
            Assert.AreEqual(expected, value.Embed(separator));
        }
    }
}

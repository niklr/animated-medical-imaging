using AMI.Core.Extensions.ArrayExtensions;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.Extensions
{
    [TestFixture]
    public class ArrayExtensionsTests : BaseTest
    {
        [TestCase(new string[] { }, ",", "#", "")]
        [TestCase(new string[] { "test1" }, ",", "#", "#test1#")]
        [TestCase(new string[] { "test1", "test2" }, ",", "#", "#test1#,#test2#")]
        public void ArrayExtensions_ToString(string[] values, string separator1, string separator2, string expected)
        {
            Assert.AreEqual(expected, values.ToString(separator1, separator2));
        }
    }
}

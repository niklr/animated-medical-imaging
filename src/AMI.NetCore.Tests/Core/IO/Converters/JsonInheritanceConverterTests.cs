using AMI.Core.Entities.Results.Commands.ProcessObject;
using AMI.Core.Entities.Shared.Commands;
using AMI.Core.IO.Serializers;
using NUnit.Framework;

namespace AMI.NetCore.Tests.Core.IO.Converters
{
    [TestFixture]
    public class JsonInheritanceConverterTests : BaseTest
    {
        [Test]
        public void JsonInheritanceConverter_Test()
        {
            // Assemble
            var serializer = GetService<IDefaultJsonSerializer>();
            var command = new ProcessObjectCommand()
            {
                Id = "1234",
                AmountPerAxis = 1,
                OutputSize = 0
            };

            // Act
            var commandSerialized = serializer.Serialize(command);
            var commandDeserialized = serializer.Deserialize<BaseCommand>(commandSerialized);

            // Assert
            Assert.AreEqual(typeof(ProcessObjectCommand), commandDeserialized.GetType());
        }
    }
}

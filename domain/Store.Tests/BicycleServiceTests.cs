using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Tests
{
    public class BicycleServiceTests
    {
        [Fact]
        public void GetAllByQuery_WithSerial_CallsGetAllBySerialNumber()
        {
            var bicycleReposStub = new Mock<IBicycleRepos>();
            bicycleReposStub.Setup(x => x.GetAllBySerialNumber(It.IsAny<string>()))
                            .Returns(new [] {new Bicycle (1, "", "", "")});

            bicycleReposStub.Setup(x => x.GetAllByTitleOrProducer(It.IsAny<string>()))
                            .Returns(new[] { new Bicycle(2, "", "", "") });

            var bicycleService = new BicycleService(bicycleReposStub.Object);

            var validSerial = "Serial: 1234567";

            var actual = bicycleService.GetAllByQuery(validSerial);

            Assert.Collection(actual, bicycle => Assert.Equal(1, bicycle.ID));
        }

        [Fact]
        public void GetAllByQuery_WithProducer_CallsGetAllByTitleOrProducer()
        {
            var bicycleReposStub = new Mock<IBicycleRepos>();
            bicycleReposStub.Setup(x => x.GetAllBySerialNumber(It.IsAny<string>()))
                            .Returns(new[] { new Bicycle(1, "", "", "") });

            bicycleReposStub.Setup(x => x.GetAllByTitleOrProducer(It.IsAny<string>()))
                            .Returns(new[] { new Bicycle(2, "", "", "") });

            var bicycleService = new BicycleService(bicycleReposStub.Object);

            var invalidSerial = "1234567";

            var actual = bicycleService.GetAllByQuery(invalidSerial);

            Assert.Collection(actual, bicycle => Assert.Equal(2, bicycle.ID));
        }
    }
}

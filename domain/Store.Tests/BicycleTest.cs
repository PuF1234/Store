namespace Store.Tests
{
    public class BicycleTest
    {
        [Fact]
        public void IsSerial_WithNull_ReturnFalse()
        {
            bool actual = Bicycle.IsSerial(null);

            Assert.False(actual);
        }
        [Fact]
        public void IsSerial_WithBlankStrings_ReturnFalse()
        {
            bool actual = Bicycle.IsSerial("   ");

            Assert.False(actual);
        }
        [Fact]
        public void IsSerial_WithInvalidSerial_ReturnFalse()
        {
            bool actual = Bicycle.IsSerial("Serial: 123");

            Assert.False(actual);
        }
        [Fact]
        public void IsSerial_WithSerial7_ReturnTrue()
        {
            bool actual = Bicycle.IsSerial("serial: 1231231");

            Assert.True(actual);
        }
        [Fact]
        public void IsSerial_WithSerial10_ReturnTrue()
        {
            bool actual = Bicycle.IsSerial("serial: 1231231123");

            Assert.True(actual);
        }
        [Fact]
        public void IsSerial_WithTrashStart_ReturnFalse()
        {
            bool actual = Bicycle.IsSerial("xxx. serial: 1231231 yyy");

            Assert.False(actual);
        }
    }
}
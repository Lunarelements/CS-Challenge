using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1;

namespace JokeUnitTests
{
    [TestClass]
    public class PrinterUnitTest
    {
        [TestMethod]
        public void ValidateTextualInput_ValidInput_True()
        {
            // Arrange
            Printer printer = new Printer();

            // Act
            printer.Value("This is a test!");

            //Assert
            Assert.AreEqual(printer.printValue, "This is a test!");
        }
    }
}

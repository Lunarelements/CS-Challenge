using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp1;

namespace JokeUnitTests
{
    [TestClass]
    public class ProgramUnitTest
    {
        [TestMethod]
        public void ValidateTextualInput_ValidInput_True()
        {
            // Arrange
            string[] input = { "This", "is", "input" };
            string[] valid = { "This", "is", "input" };
            string invalidMessage = "Something went wrong!";

            // Act
            bool output = Program.ValidateTextualInput(input, valid, invalidMessage);

            //Assert
            Assert.IsTrue(output);
        }

        [TestMethod]
        public void ValidateTextualInput_InvalidInput_False()
        {
            // Arrange
            string[] input = { "This", "is", "input" };
            string[] valid = { "This", "is", "output" };
            string invalidMessage = "Something went wrong!";

            // Act
            bool output = Program.ValidateTextualInput(input, valid, invalidMessage);

            //Assert
            Assert.IsFalse(output);
        }

        [TestMethod]
        public void ValidateNumericInput_ValidInput_True()
        {
            // Arrange
            string input = "7";
            int min = 1;
            int max = 9;
            string invalidMessage = "Something went wrong!";

            // Act
            bool output = Program.ValidateNumericInput(input, min, max, invalidMessage);

            //Assert
            Assert.IsTrue(output);
        }

        [TestMethod]
        public void ValidateNumericInput_InvalidInput_False()
        {
            // Arrange
            string input = "10";
            int min = 1;
            int max = 9;
            string invalidMessage = "Something went wrong!";

            // Act
            bool output = Program.ValidateNumericInput(input, min, max, invalidMessage);

            //Assert
            Assert.IsFalse(output);
        }
    }
}

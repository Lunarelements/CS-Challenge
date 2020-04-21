using System;

namespace ConsoleApp1
{
    /// <summary>Class <c>Printer</c> provides different ways to handle output.</summary>
    public class Printer
    {
        public object printValue;

        /// <summary>Sets the printValue of the printer.</summary>
        /// <param><c>value</c> is the string that we wish to set the buffer to.</param>
        /// <see>Fluent interface Design Pattern</see>
        public Printer Value(string value)
        {
            printValue = value;
            return this;
        }

        /// <summary>Prints the current print value to the console.</summary>
        public void PrintToConsole()
        {
            Console.WriteLine(printValue);
        }

        /// <summary>Prints the inputted array to console on seperate lines.</summary>
        /// <param><c>toPrint</c> is the string array that we wish to print.</param>
        public void PrintArrayToConsole(string[] toPrint)
        {
            foreach(string value in toPrint)
            {
                Console.WriteLine(value);
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the object.</returns>
        public override string ToString()
        {
            return $"Print value: {printValue}";
        }
    }
}

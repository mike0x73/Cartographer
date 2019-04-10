using System;
using System.Threading.Tasks;
using Cartographer;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var cartographer = new Cartographer.Cartographer(@"C:\Users\Mike\Documents\test.log")
            {
                PrintToConsole = true,
                PrintContextData = false,                
            };

            cartographer.Log("Hello Test 1", LoggingLevel.Info);
            TestMethod(cartographer);

            var testTask = Task.Factory.StartNew(() =>
            {
                cartographer.Log("Hello from a new task.", LoggingLevel.Error);
                TestMethod(cartographer);
            });
            
            Console.ReadLine();
        }

        static void TestMethod(Cartographer.Cartographer cartographer)
        {
            cartographer.Log("Hello from another method", LoggingLevel.Info);
        }
    }
}

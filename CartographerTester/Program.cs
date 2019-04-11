using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CartographerTester
{
    public class Program
    {
        static void Main(string[] args)
        {
            var filePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents\\Cartographer\\Tests.log";

            var cartographer = new Cartographer.Cartographer(filePath)
            {
                PrintContextData = true,
                PrintToConsole = true,
                LoggingLevelToPrint = Cartographer.LoggingLevel.Debug,
                MaxFileSize = 2048,
                UseStackTrace = false,
            };

            cartographer.Log("Happy logging", Cartographer.LoggingLevel.Info);
            cartographer.Log("Don't print me", Cartographer.LoggingLevel.Trace);

            var multiMessage = new List<string>()
            {
                "Testing multiline logging",
                "Hope this is working",
                "Last effort",
            }.ToArray();

            cartographer.Log(multiMessage, Cartographer.LoggingLevel.Error);

            try
            {
                throw new Exception("Manully thrown exception", new Exception());                
            }
            catch(Exception e)
            {
                cartographer.Log($"Caught an exception with message: {e.Message}", Cartographer.LoggingLevel.Error, e);
            }

            var testTask = Task.Factory.StartNew(() =>
            {
                DougForcett(cartographer);
            });
            
            Console.ReadLine();
        }

        private static void DougForcett(Cartographer.Cartographer cartographer)
        {
            cartographer.Log("Testing logging from different method", Cartographer.LoggingLevel.Info);
        }
    }
}

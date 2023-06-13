using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer_Database
{
    public class ExceptionHandler
    {
        private static readonly ExceptionHandler _instance = new ExceptionHandler();

        public static ExceptionHandler Instance
        {
            get { return _instance; }
        }

        public void FileHandler(string message, string StackTrace)
        {
            Console.WriteLine($"Error: {message} at {StackTrace}. Error code: 500");
        }
    }
}

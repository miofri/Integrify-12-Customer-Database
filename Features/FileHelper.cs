using System.Reflection.Metadata.Ecma335;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer_Database
{
    public class FileHelper
    {
        private string _csvPath;
        private static readonly FileHelper _instance = new FileHelper();

        private FileHelper()
        {
            _csvPath = "customers.csv";
        }

        public static FileHelper Instance
        {
            get { return _instance; }
        }

        public void AddToCSV(Customer customer)
        {
            using var writer = new StreamWriter(_csvPath, append: true);
            string line =
                $"{customer.GetUserId},{customer.Email},{customer.Address},{customer.FirstName},{customer.LastName}";
            writer.WriteLine(line);
            Console.WriteLine("=== Data has been added to CSV. ===\n");
        }

        public void UpdateOrDeleteFromCSV(List<Customer> customerList)
        {
            File.WriteAllText(_csvPath, string.Empty);

            using var writer = new StreamWriter(_csvPath);
            foreach (Customer customer in customerList)
            {
                string line =
                    $"{customer.GetUserId},{customer.Email},{customer.Address},{customer.FirstName},{customer.LastName}";
                writer.WriteLine(line);
            }
            Console.WriteLine("=== Data in CSV has been updated. ===\n");
        }
    }
}

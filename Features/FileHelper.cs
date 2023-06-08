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
        private List<Customer> _customersFromCSV = new List<Customer>();

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
                $"{customer.GetUserId},{customer.Address},{customer.Email},{customer.FirstName},{customer.LastName}";
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
                    $"{customer.GetUserId},{customer.Address},{customer.Email},{customer.FirstName},{customer.LastName}";
                writer.WriteLine(line);
            }
            Console.WriteLine("=== Data in CSV has been updated. ===\n");
        }

        public List<Customer> ReadExistingDataFromCsv()
        {
            if (File.Exists(_csvPath))
            {
                using var reader = new StreamReader(_csvPath);
                while (!reader.EndOfStream)
                {
                    try
                    {
                        string line = reader.ReadLine() ?? "";
                        string[] eachLine = line.Split("\n");
                        for (int i = 0; i < eachLine.Length; i++)
                        {
                            string[] eachData = line.Split(",");
                            _customersFromCSV.Add(
                                new Customer(
                                    eachData[1],
                                    eachData[2],
                                    eachData[3],
                                    eachData[4],
                                    eachData[0]
                                )
                            );
                        }
                    }
                    catch (OutOfMemoryException)
                    {
                        Console.WriteLine("Warning: Out of memory!");
                    }
                }
            }
            else
            {
                Console.WriteLine("CSV file does not exist.");
            }
            return _instance._customersFromCSV;
        }

        public void PrintCustomersFromCSV()
        {
            if (_customersFromCSV.Count == 0)
            {
                Console.WriteLine(
                    "No data has been extracted from CSV. Please call ReadExistingDataFromCSV first."
                );
            }
            else
            {
                Console.WriteLine("=== Customer extracted from CSV ===");
                foreach (Customer item in _customersFromCSV)
                {
                    Console.WriteLine(
                        $"{item.GetUserId} {item.Address} {item.Email} {item.FirstName} {item.LastName}"
                    );
                }
            }
        }
    }
}

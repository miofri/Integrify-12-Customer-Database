using System.Reflection.Metadata.Ecma335;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace Customer_Database
{
    public class FileHelper : IEnumerable<Customer>
    {
        private static readonly FileHelper _instance = new FileHelper();
        private string _csvPath;
        private List<Customer> _customersFromCSV;

        private FileHelper()
        {
            _customersFromCSV = new List<Customer>();
            _csvPath = "customers.csv";
        }

        public static FileHelper Instance
        {
            get { return _instance; }
        }

        public void InitialiseCSV(CustomerDatabase database)
        {
            File.WriteAllText(_csvPath, string.Empty);
            var defaultData = _instance.ReadExistingDataFromCsv("default.csv");
            defaultData.ToList().ForEach(customer => database.AddCustomer(customer));
        }

        public void AddToCSV(Customer customer, string path)
        {
            using var writer = new StreamWriter(path, append: true);
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

        public IEnumerable<Customer> ReadExistingDataFromCsv(string path)
        {
            if (File.Exists(path))
            {
                using var reader = new StreamReader(path);
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine() ?? "";
                    string[] eachData = line.Split(',');

                    if (eachData.Length == 5)
                    {
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

        public IEnumerator<Customer> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

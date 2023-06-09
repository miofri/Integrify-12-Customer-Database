using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Customer_Database
{
    public class CustomerDatabase : IEnumerable<Customer>
    {
        private List<Customer> _customers;
        private static readonly CustomerDatabase _instance = new CustomerDatabase();
        private FileHelper _fileHelperInstance;
        public static CustomerDatabase Instance
        {
            get { return _instance; }
        }

        private CustomerDatabase()
        {
            _customers = new List<Customer>();
            _fileHelperInstance = FileHelper.Instance;
        }

        public bool AddCustomer(Customer newCustomer)
        {
            if (_customers.Find(customer => customer.Email == newCustomer.Email) != null)
            {
                Console.WriteLine("Email exists in database!\n");
                return false;
            }
            _customers.Add(newCustomer);
            Console.WriteLine($"Customer {newCustomer.FirstName} {newCustomer.LastName} added.\n");
            _fileHelperInstance.AddToCSV(newCustomer, "customers.csv");
            return true;
        }

        public void UpdateCustomer(Customer replacementCustomer, System.Guid idToChange)
        {
            try
            {
                var cusToChangeIndex = _customers.FindIndex(id => idToChange == id.GetUserId);
                Console.WriteLine(
                    $"Updated customer {_customers[cusToChangeIndex].FirstName} {_customers[cusToChangeIndex].LastName} into {replacementCustomer.FirstName} {replacementCustomer.LastName}\n"
                );
                _customers[cusToChangeIndex] = replacementCustomer;
                _fileHelperInstance.UpdateOrDeleteFromCSV(_customers);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Customer not found!\n");
                return;
            }
        }

        public bool DeleteCustomer(System.Guid idToDelete)
        {
            try
            {
                var cusToDeleteIndex = _customers.FindIndex(
                    customer => customer.GetUserId == idToDelete
                );
                Console.WriteLine(
                    $"\nDeleted customer {_customers[cusToDeleteIndex].FirstName} {_customers[cusToDeleteIndex].LastName}\n"
                );
                _customers.RemoveAt(cusToDeleteIndex);
                _fileHelperInstance.UpdateOrDeleteFromCSV(_customers);
                return true;
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Customer not found!\n");
                return false;
            }
        }

        public void PrintAllCustomers()
        {
            Console.WriteLine("=== Printing all customers ===");
            Console.WriteLine(
                string.Join(
                    '\n',
                    _customers.Select(
                        customer =>
                            $"{customer.Email} {customer.Address} {customer.FirstName} {customer.LastName}"
                    )
                )
            );
        }

        public IEnumerable<Customer> EnumerableAllCustomers()
        {
            return _instance._customers;
        }

        public IEnumerable<Customer> FindCustomerById(System.Guid customerId)
        {
            return _customers.Where(customer => customer.GetUserId == customerId);
        }

        public IEnumerable<Customer> FindCustomerBySearchTerm(string searchTerm)
        {
            return _customers.Where(
                customer =>
                    customer.FirstName.Contains(searchTerm)
                    || customer.LastName.Contains(searchTerm)
                    || customer.Email.Contains(searchTerm)
            );
        }

        public IEnumerator<Customer> GetEnumerator()
        {
            return _customers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

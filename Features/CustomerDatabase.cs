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
        public static CustomerDatabase Instance
        {
            get { return _instance; }
        }

        private CustomerDatabase()
        {
            _customers = new List<Customer>();
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
            return true;
        }

        public void UpdateCustomer(Customer customerToUpdate, System.Guid idToChange)
        {
            try
            {
                var cusToChangeIndex = _customers.FindIndex(id => idToChange == id.GetUserId);
                _customers[cusToChangeIndex] = customerToUpdate;
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
                _customers.RemoveAt(cusToDeleteIndex);
                return true;
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Customer not found!\n");
                return false;
            }
        }

        public IEnumerable<Customer> EnumerableAllCustomers()
        {
            return _customers;
        }

        public void PrintAllCustomers()
        {
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

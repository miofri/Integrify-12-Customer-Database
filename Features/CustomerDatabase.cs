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
        private ExceptionHandler _exceptionHandler;
        private Stack<Action> _undoHistory;
        private Stack<Action> _redoHistory;
        public static CustomerDatabase Instance
        {
            get { return _instance; }
        }

        private CustomerDatabase()
        {
            _customers = new List<Customer>();
            _exceptionHandler = ExceptionHandler.Instance;
            _fileHelperInstance = FileHelper.Instance;
            _undoHistory = new Stack<Action>();
            _redoHistory = new Stack<Action>();
        }

        public bool AddCustomer(Customer newCustomer)
        {
            var saveCustomer = new Customer(
                newCustomer.Address,
                newCustomer.Email,
                newCustomer.FirstName,
                newCustomer.LastName,
                newCustomer.GetUserId.ToString()
            );
            if (_customers.Find(customer => customer.Email == newCustomer.Email) != null)
            {
                Console.WriteLine("Email exists in database!\n");
                return false;
            }
            _customers.Add(newCustomer);

            Console.WriteLine($"Customer {newCustomer.FirstName} {newCustomer.LastName} added.\n");

            _fileHelperInstance.AddToCSV(newCustomer, "customers.csv");
            _undoHistory.Push(() => DeleteCustomer(saveCustomer.GetUserId));
            return true;
        }

        public bool UpdateCustomer(Customer replacementCustomer, System.Guid idToChange)
        {
            try
            {
                var cusToChangeIndex = _customers.FindIndex(id => idToChange == id.GetUserId);

                var savedCustomer = _customers[cusToChangeIndex];
                var saveCustomerForUndo = new Customer(
                    savedCustomer.Address,
                    savedCustomer.Email,
                    savedCustomer.FirstName,
                    savedCustomer.LastName,
                    savedCustomer.GetUserId.ToString()
                );

                Console.WriteLine(
                    $"Updated customer {_customers[cusToChangeIndex].FirstName} {_customers[cusToChangeIndex].LastName} into {replacementCustomer.FirstName} {replacementCustomer.LastName}\n"
                );

                replacementCustomer.GetUserId = _customers[cusToChangeIndex].GetUserId;
                _customers[cusToChangeIndex] = replacementCustomer;
                _fileHelperInstance.UpdateOrDeleteFromCSV(_customers);
                _undoHistory.Push(() => UpdateCustomer(saveCustomerForUndo, idToChange));
                return true;
            }
            catch (Exception ex)
            {
                _exceptionHandler.FileHandler(ex.Message, ex.StackTrace ?? "");
                return false;
            }
        }

        public bool DeleteCustomer(System.Guid idToDelete)
        {
            try
            {
                var cusToDeleteIndex = _customers.FindIndex(
                    customer => customer.GetUserId == idToDelete
                );
                if (cusToDeleteIndex == -1)
                {
                    // Customer with the specified idToDelete does not exist
                    return false;
                }
                var savedCustomer = _customers[cusToDeleteIndex];
                var saveCustomerForUndo = new Customer(
                    savedCustomer.Address,
                    savedCustomer.Email,
                    savedCustomer.FirstName,
                    savedCustomer.LastName,
                    savedCustomer.GetUserId.ToString()
                );

                Console.WriteLine(
                    $"\nDeleted customer {_customers[cusToDeleteIndex].FirstName} {_customers[cusToDeleteIndex].LastName}\n"
                );

                _customers.RemoveAt(cusToDeleteIndex);
                _fileHelperInstance.UpdateOrDeleteFromCSV(_customers);
                _undoHistory.Push(() => AddCustomer(saveCustomerForUndo));
                return true;
            }
            catch (Exception ex)
            {
                _exceptionHandler.FileHandler(ex.Message, ex.StackTrace ?? "");
                return false;
            }
        }

        public bool Undo()
        {
            if (_undoHistory.Count > 0)
            {
                var action = _undoHistory.Pop();
                action.Invoke();

                Console.WriteLine($"Undo action: {action.Method}");

                return true;
            }
            return false;
        }

        public bool Redo()
        {
            if (_undoHistory.Count > 0)
            {
                var action = _undoHistory.Pop();
                action.Invoke();

                Console.WriteLine($"Redo action: {action.Method}");

                return true;
            }
            return false;
        }

        public void PrintAllCustomers()
        {
            Console.WriteLine("=== Printing all customers ===");

            try
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
            catch (Exception ex)
            {
                _exceptionHandler.FileHandler(ex.Message, ex.StackTrace ?? "");
            }
        }

        public IEnumerable<Customer> EnumerableAllCustomers()
        {
            return _instance._customers;
        }

        public IEnumerable<Customer> FindCustomerById(System.Guid customerId)
        {
            try
            {
                return _customers.Where(customer => customer.GetUserId == customerId);
            }
            catch (Exception ex)
            {
                _exceptionHandler.FileHandler(ex.Message, ex.StackTrace ?? "");
                return Enumerable.Empty<Customer>();
            }
        }

        public IEnumerable<Customer> FindCustomerBySearchTerm(string searchTerm)
        {
            try
            {
                return _customers.Where(
                    customer =>
                        customer.FirstName.Contains(searchTerm)
                        || customer.LastName.Contains(searchTerm)
                        || customer.Email.Contains(searchTerm)
                );
            }
            catch (Exception ex)
            {
                _exceptionHandler.FileHandler(ex.Message, ex.StackTrace ?? "");
                return Enumerable.Empty<Customer>();
            }
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

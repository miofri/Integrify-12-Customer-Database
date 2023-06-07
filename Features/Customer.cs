using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer_Database
{
    public class Customer
    {
        private System.Guid _id;
        public string Address;
        public string FirstName;
        public string LastName;
        public string Email;

        public Customer(string newAddress, string newFirstName, string newLastName, string newEmail)
        {
            _id = Guid.NewGuid();
            Address = newAddress;
            FirstName = newFirstName;
            LastName = newLastName;
            Email = newEmail;
        }

        public System.Guid GetUserId
        {
            get { return _id; }
        }
    }
}

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
        public string Email;
        public string FirstName;
        public string LastName;

        public Customer(
            string newAddress,
            string newEmail,
            string newFirstName,
            string newLastName,
            string newId = ""
        )
        {
            Address = newAddress;
            FirstName = newFirstName;
            LastName = newLastName;
            Email = newEmail;
            if (newId == "")
            {
                _id = Guid.NewGuid();
            }
            else
            {
                _id = new Guid(newId);
            }
        }

        public System.Guid GetUserId
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}

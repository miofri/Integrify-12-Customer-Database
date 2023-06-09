using System.Data.Common;
// See https://aka.ms/new-console-template for more information
using Customer_Database;

public class Program
{
    static void Main()
    {
        CustomerDatabase Database = CustomerDatabase.Instance;
        FileHelper FileHelper = FileHelper.Instance;
        FileHelper.InitialiseCustomersFromCSV(Database);

        var cus1 = new Customer("address1", "friidu@mail.com", "Friidu", "Kesuma");
        var cus2 = new Customer("address2", "friidumio@mail.com", "Friidu", "Mio");
        var cus3 = new Customer("address3", "mio@mail.com", "Mio", "Kesuma");
        var cus4 = new Customer("address4", "babkes@mail.com", "Bab", "Kes");
        var cus5 = new Customer("address5", "kesbab@mail.com", "Kes", "Bab");

        Database.AddCustomer(cus1);
        Database.AddCustomer(cus2);
        Database.AddCustomer(cus3);
        Database.AddCustomer(cus4);

        IEnumerable<Customer> searchResult = Database.FindCustomerBySearchTerm("Kesuma");
        // Testing search:
        // Console.WriteLine(
        //     string.Join('\n', searchResult.Select(c => $"{c.Address} {c.FirstName} {c.LastName}"))
        // );
        Database.PrintAllCustomers();
        Database.DeleteCustomer(cus3.GetUserId);
        Database.UpdateCustomer(cus5, cus1.GetUserId);
        Database.PrintAllCustomers();
    }
}

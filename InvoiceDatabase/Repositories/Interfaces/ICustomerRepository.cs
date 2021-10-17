using InvoiceDatabase.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceDatabase.Repositories
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customer);
        void DeleteCustomer(int id);
        Task<Customer> GetCustomer(string id);
        Task<List<Customer>> GetCustomerList(string sortColumn, string sortDirection, int OffsetValue, int PagingSize, string searchText, string date = null);
        void UpdateCustomer(Customer customer);
    }
}
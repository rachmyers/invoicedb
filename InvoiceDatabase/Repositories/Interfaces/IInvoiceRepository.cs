using InvoiceDatabase.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceDatabase.Repositories
{
    public interface IInvoiceRepository
    {
        void AddInvoice(Invoice invoice);
        void DeleteInvoice(int id);
        Task<Invoice> GetInvoice(int id);
        Task<List<Invoice>> GetInvoiceList(string sortColumn, string sortDirection, int OffsetValue, int PagingSize, string searchText, string date = null);
        void UpdateInvoice(Invoice invoice);
    }
}
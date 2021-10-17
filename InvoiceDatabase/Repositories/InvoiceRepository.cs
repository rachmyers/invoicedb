using InvoiceDatabase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceDatabase.Repositories
{
    /// <summary>
    /// This class is responsible for directly interacting with the Invoices table in the database
    /// </summary>
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly string _connectionString;

        public InvoiceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Invoice> GetInvoice(int id)
        {
            Invoice invoice = new Invoice();

            using (SqlConnection conn = new SqlConnection())
            {
                await conn.OpenAsync();

                SqlCommand com = new SqlCommand("sp_GetInvoice", conn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@id", id);

                SqlDataReader sdr = com.ExecuteReader();

                while (sdr.Read())
                {
                    invoice.InvoiceId = sdr.GetInt32(0);
                    invoice.CustomerId = sdr.GetInt32(1);
                    invoice.InvoiceDate = sdr.GetDateTime(2);
                    invoice.WorkCompleted = sdr.GetString(3);
                    invoice.AmountBilled = sdr.GetDouble(4);
                    invoice.IsPaid = sdr.GetInt32(5) == 0 ? false : true;
                    invoice.Address = sdr.GetString(6);
                }

                conn.Close();
            }
            return invoice;
        }

        public async Task<List<Invoice>> GetInvoiceList(string sortColumn, string sortDirection, int OffsetValue, int PagingSize, string searchText, string date = null)
        {
            List<Invoice> invoices = new List<Invoice>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;

                await conn.OpenAsync();

                SqlCommand com = new SqlCommand("sp_GetInvoiceList", conn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@sortColumn", sortColumn);
                com.Parameters.AddWithValue("@sortOrder", sortDirection);
                com.Parameters.AddWithValue("@OffsetValue", OffsetValue);
                com.Parameters.AddWithValue("@PagingSize", PagingSize);
                com.Parameters.AddWithValue("@SearchText", searchText);
                com.CommandType = CommandType.StoredProcedure;

                SqlDataReader sdr = com.ExecuteReader();

                while (sdr.Read())
                {
                    Invoice invoice = new Invoice();

                    invoice.InvoiceId = sdr.GetInt32(0);
                    invoice.CustomerId = sdr.GetInt32(1);
                    invoice.InvoiceDate = sdr.GetDateTime(2);
                    invoice.AmountBilled = sdr.GetDouble(3);
                    invoice.WorkCompleted = sdr.GetString(4);
                    invoice.IsPaid = sdr.GetBoolean(5);
                    invoice.Address = sdr.GetString(10) + " " + sdr.GetString(11) + ", " + sdr.GetString(12) + " " + sdr.GetString(13);

                    invoices.Add(invoice);
                }

                await conn.CloseAsync();
            }

            return invoices;
        }
    

        public async void AddInvoice(Invoice invoice)
        {

            using (SqlConnection conn = new SqlConnection())
            {

                await conn.OpenAsync();
                SqlCommand com = new SqlCommand("sp_InsertInvoice", conn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@customerId", invoice.CustomerId);
                com.Parameters.AddWithValue("@date", invoice.InvoiceDate);
                com.Parameters.AddWithValue("@notes", invoice.WorkCompleted);
                com.Parameters.AddWithValue("@amount", invoice.AmountBilled);
                com.Parameters.AddWithValue("@isPaid", invoice.IsPaid);

                await com.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        public async void UpdateInvoice(Invoice invoice)
        {
            using (SqlConnection conn = new SqlConnection())
            {

                await conn.OpenAsync();
                SqlCommand com = new SqlCommand("sp_UpdateInvoice", conn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@customerId", invoice.CustomerId);
                com.Parameters.AddWithValue("@date", invoice.InvoiceDate);
                com.Parameters.AddWithValue("@notes", invoice.WorkCompleted);
                com.Parameters.AddWithValue("@amount", invoice.AmountBilled);
                com.Parameters.AddWithValue("@isPaid", invoice.IsPaid);

                await com.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        public async void DeleteInvoice(int id)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                await conn.OpenAsync();
                SqlCommand com = new SqlCommand("sp_DeleteInvoice", conn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@id", id);

                await com.ExecuteNonQueryAsync();

                conn.Close();
            }
        }
    }
}

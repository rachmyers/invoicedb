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
    /// This class is responsible for directly interacting with the Customer table in the database
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Gets a customer from the database by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomer(string id)
        {
            Customer customer = new Customer();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;

                await conn.OpenAsync();

                SqlCommand com = new SqlCommand("sp_GetCustomerList", conn);
                com.CommandType = CommandType.StoredProcedure;

                SqlDataReader sdr = com.ExecuteReader();

                while (sdr.Read())
                {
                    customer.Address = sdr.GetString(5);
                    customer.City = sdr.GetString(6);
                    customer.State = sdr.GetString(7);
                    customer.ZipCode = sdr.GetString(8);
                    customer.CustomerId = sdr.GetInt32(0);
                    customer.FirstName = sdr.GetString(1);
                    customer.LastName = sdr.GetString(2);
                    customer.Email = sdr.GetString(3);
                    customer.Phone = sdr.GetString(4);
                }

                await conn.CloseAsync();
            }

            return customer;
        }

        public async Task<List<Customer>> GetCustomerList(string sortColumn, string sortDirection, int OffsetValue, int PagingSize, string searchText, string date = null)
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;

                await conn.OpenAsync();

                SqlCommand com = new SqlCommand("sp_GetCustomerList", conn);
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
                    Customer customer = new Customer();
                    //Get the customer's street address, city, state, and zip code
                    customer.Address = sdr.GetString(5) + " " + sdr.GetString(6) + ", " 
                           + sdr.GetString(7) + " " + sdr.GetString(8);

                    customer.CustomerId = sdr.GetInt32(0);
                    customer.FirstName = sdr.GetString(1);
                    customer.LastName = sdr.GetString(2);
                    customer.Email = sdr.GetString(3);
                    customer.Phone = sdr.GetString(4);

                    customers.Add(customer);
                }

                await conn.CloseAsync();
            }

            return customers;
        }

        public async void AddCustomer(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection())
            {

                await conn.OpenAsync();
                SqlCommand com = new SqlCommand("sp_InsertCustomer", conn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@firstName", customer.FirstName);
                com.Parameters.AddWithValue("@lastName", customer.LastName);
                com.Parameters.AddWithValue("@email", customer.Email);
                com.Parameters.AddWithValue("@phone", customer.Phone);
                com.Parameters.AddWithValue("@address", customer.Address);
                com.Parameters.AddWithValue("@city", customer.City);
                com.Parameters.AddWithValue("@state", customer.State);
                com.Parameters.AddWithValue("@zipCode", customer.ZipCode);

                await com.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        public async void UpdateCustomer(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection())
            {

                await conn.OpenAsync();
                SqlCommand com = new SqlCommand("sp_UpdateCustomer", conn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@firstName", customer.FirstName);
                com.Parameters.AddWithValue("@lastName", customer.LastName);
                com.Parameters.AddWithValue("@email", customer.Email);
                com.Parameters.AddWithValue("@phone", customer.Phone);
                com.Parameters.AddWithValue("@address", customer.Address);
                com.Parameters.AddWithValue("@city", customer.City);
                com.Parameters.AddWithValue("@state", customer.State);
                com.Parameters.AddWithValue("@zipCode", customer.ZipCode);

                await com.ExecuteNonQueryAsync();

                conn.Close();
            }
        }

        public async void DeleteCustomer(int id)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                await conn.OpenAsync();
                SqlCommand com = new SqlCommand("sp_DeleteCustomer", conn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@id", id);

                await com.ExecuteNonQueryAsync();

                conn.Close();
            }
        }
    }
}

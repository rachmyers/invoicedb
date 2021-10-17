using InvoiceDatabase.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceDatabase.Controllers
{
    /// <summary>
    /// This class is used to interact with the DataTables JQuery library API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersAPIController : Controller
    {
        //Use dependency injection to access the repository class
        private ICustomerRepository _repo;

        public CustomersAPIController(ICustomerRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> GetInvoices()
        {
            try
            {
                //API values to send to the stored procedure
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();


                //Check if the sort column contains a space, then remove space
                bool hasSpace = sortColumn.Contains(" ");

                if (hasSpace)
                {
                    sortColumn = string.Concat(sortColumn.Where(c => !char.IsWhiteSpace(c)));
                }

                //Remove forward slash from column name
                if (sortColumn.Contains("/"))
                {
                    sortColumn = sortColumn.Replace("/", "");
                }

                //API values to send to the stored procedure
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                //Value from the Date field
                string date = Request.Form["startDate"].FirstOrDefault();

                //Default the sort column to PunchDate if one isn't selected
                if (string.IsNullOrEmpty(sortColumn))
                {
                    sortColumn = "PunchDate";
                }

                //Default the sort direction to desc if one isn't selected or if it's the initial page load
                if (string.IsNullOrEmpty(sortColumnDirection) || draw == "1")
                {
                    sortColumnDirection = "desc";
                }



                var entryData = (from entry in await _repo.GetCustomerList(sortColumn, sortColumnDirection, skip, pageSize, searchValue, date) select entry);

                //Check if any records exist. If yes, then add to data. If not, then send empty data string
                if (entryData.Any())
                {
                    recordsTotal = entryData.First().FilterTotalCount;
                    var jsonEntryData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = entryData };
                    return Ok(jsonEntryData);
                }
                //Send empty data string if no data exists
                var emptyJsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" };
                return Ok(emptyJsonData);

            }
            catch (Exception e)
            {
                _logger.LogError("Error thrown in the ReportAPIController: " + e.Message);
                throw;
            }

        }
    }
}

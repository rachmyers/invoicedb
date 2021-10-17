using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceDatabase.Models
{
    public class Invoice
    {
        public int FilterTotalCount { get; set; }
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public double AmountBilled { get; set; }
        public bool IsPaid { get; set; }
        public string WorkCompleted { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Address { get; set; }
    }
}

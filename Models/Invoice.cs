using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InvoicingSystem.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public List<InvoiceItem> Items { get; set; }

        [Range(0, double.MaxValue)]
        public double TotalAmount { get; set; }

        [Range(0, double.MaxValue)]
        public double Discount { get; set; }

        [Range(0, double.MaxValue)]
        public double Tax { get; set; }

        [Range(0, double.MaxValue)]
        public double FinalAmount { get; set; }

        [Required]
        public string PaymentOption { get; set; }

        public DateTime InvoiceDate { get; set; }
    }
}

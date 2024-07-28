namespace InvoicingSystem.Models
{
    public class InvoiceItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice => Quantity * (Price - Discount);
    }
}

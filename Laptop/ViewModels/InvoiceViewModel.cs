namespace Laptop.ViewModels
{
    public class InvoiceViewModel
    {
        public int ProductVarId { get; set; }
        public int SupplierId { get; set; }
        public DateTime? CreateDate { get; set; }
      
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}

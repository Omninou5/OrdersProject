namespace ManagementApplication.Models
{
    public class OrderProviderItem
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime? Date { get; set; }
        public string ProviderName { get; set; } // Название поставщика

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}

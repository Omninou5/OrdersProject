using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementApplication.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указан номер заказа")]
        public string Number { get; set; }


        [Required(ErrorMessage = "Не указана дата/время заказа")]
        [Column(TypeName = "datetime2(7)")]
        public DateTime? Date { get; set; }


        [Range(0, int.MaxValue, ErrorMessage = "Выберите поставщика")]
        public int ProviderId { get; set; }


        [Required(ErrorMessage = "Не добавлен элемент заказа")]
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
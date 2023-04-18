using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementApplication.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }


        [Required(ErrorMessage = "Не указано название элемента заказа")]
        public string Name { get; set; }


        [Range(0.001, 9999.999, ErrorMessage = "Введите значение от 0.01 до 9999.999")]
        [Column(TypeName = "decimal(18,3)")]
        public decimal Quantity { get; set; } // Количество (Вес) заказа


        [Required(ErrorMessage = "Не указана единица измерения заказа")]
        public string Unit { get; set; } // Единица измерения заказа (кг, гр)


        public Order? Order { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Models
{
    public class OrderItems
    {

        [Key]
        public int OrderItem { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
    }
}

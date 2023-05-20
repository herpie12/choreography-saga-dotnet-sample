using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Shared.Contracts.OrderCreatedEvent;

namespace OrderAPI.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public List<OrderItems> OrderItems { get; set; }
        public int WalletId { get; set; }
        public decimal TotalAmount { get; set; }

        [DefaultValue(OrderStatus.pending)]
        public OrderStatus status { get; set; }
    }
}


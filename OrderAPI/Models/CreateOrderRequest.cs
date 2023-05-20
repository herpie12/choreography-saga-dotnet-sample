using System.Collections.Generic;
using System.ComponentModel;
using static Shared.Contracts.OrderCreatedEvent;

namespace OrderAPI.Models
{
    public class CreateOrderRequest
    {
        public int UserId { get; set; }
        public List<OrderItemRequest> OrderItems { get; set; }
        public int WalletId { get; set; }
        public decimal TotalAmount { get; set; }

        [DefaultValue(OrderStatus.pending)]
        public OrderStatus status { get; set; }
    }

    public class OrderItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
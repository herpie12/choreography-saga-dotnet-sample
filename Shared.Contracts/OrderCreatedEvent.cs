﻿namespace Shared.Contracts
{
    public class OrderCreatedEvent
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public int WalletId { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderStatus status { get; set; }
        public enum OrderStatus
        {
            pending,
            completed,
            rejected
        }
    }
}

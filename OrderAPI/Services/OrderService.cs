using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Db;
using OrderAPI.Models;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBus _bus;
        private readonly OrderDbContext _OrderContext;

        public OrderService(IBus bus, OrderDbContext OrderContext)
        {
            _bus = bus;
            _OrderContext = OrderContext;
        }
        public async Task CreateOrderAsync(CreateOrderRequest createOrderRequest)
        {
            var newOrder = MapToOrder(createOrderRequest);

            _OrderContext.Order.Add(newOrder);
            var isSaved = _OrderContext.SaveChangesAsync();

            if (isSaved.Result > 0)
            {
                await _bus.PubSub.PublishAsync(new OrderCreatedEvent
                {
                    UserId = newOrder.UserId,
                    OrderId = newOrder.OrderId,
                    WalletId = newOrder.WalletId,
                    TotalAmount = newOrder.TotalAmount,
                    status = newOrder.status
                });
            }
        }
        public Task RejectOrderAsync(int orderId, string reason)
        {
            throw new System.NotImplementedException();
        }
        public Task CompleteOrderAsync(int orderId)
        {
            var order = _OrderContext.Order.FirstOrDefault(x => x.OrderId == orderId);

            if (order == null)
            {
                throw new ArgumentOutOfRangeException("order not found");
            }

            order.status = OrderCreatedEvent.OrderStatus.completed;
            _OrderContext.SaveChanges();

            return Task.CompletedTask;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _OrderContext.Order.Include(o => o.OrderItems).ToListAsync();
        }

        public static Order MapToOrder(CreateOrderRequest createOrderRequest)
        {
            return new Order
            {
                status = createOrderRequest.status,
                UserId = createOrderRequest.UserId,
                TotalAmount = createOrderRequest.TotalAmount,
                WalletId = createOrderRequest.WalletId,
                OrderItems = createOrderRequest.OrderItems.Select(x =>
                     new OrderItems() { Quantity = x.Quantity, Price = x.Price, ProductId = x.ProductId }).ToList()
            };
        }
    }
}

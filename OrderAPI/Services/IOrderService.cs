using OrderAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderAPI.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
        Task CreateOrderAsync(CreateOrderRequest request);
        Task CompleteOrderAsync(int orderId);
        Task RejectOrderAsync(int orderId, string reason);
    }
}
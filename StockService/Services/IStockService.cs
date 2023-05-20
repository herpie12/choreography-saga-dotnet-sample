using System.Threading.Tasks;

namespace StockService.Services
{
    public interface IStockService
    {
        Task ReserveStocksAsync(int orderId);
        Task<bool> ReleaseStocksAsync(int orderId);
    }
}
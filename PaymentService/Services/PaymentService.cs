using System;
using System.Threading.Tasks;

namespace PaymentService.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<Tuple<bool, string>> DoPaymentAsync(int walletId, int userId, decimal totalAmount)
        {
            var payment = Tuple.Create(true, "Completed");

            return Task.FromResult(payment);
        }
    }
}
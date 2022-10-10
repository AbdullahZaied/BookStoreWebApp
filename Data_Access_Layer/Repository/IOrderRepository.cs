using Data.Access.Layer.Data;
using Data.Access.Layer.Models;

namespace Data.Access.Layer.Repository
{
    public interface IOrderRepository
    {
        Task<int> GetBookAmountByIdAsync(int bookId);
        Task SetBookAmountByIdAsync(int bookId, int newAmount);
        Task<int> OrderBookByIdAsync(OrderModelData order);
        Task<Order> GetOrderByIdAsync(int orderId);
    }
}

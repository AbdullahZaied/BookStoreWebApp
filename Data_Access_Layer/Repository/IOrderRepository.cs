using Data.Access.Layer.Data;
using Data.Access.Layer.Models;

namespace Data.Access.Layer.Repository
{
    public interface IOrderRepository
    {
        Task<int> GetBookAmountByIdAsync(int bookId);
        Task<int?> SetBookAmountByIdAsync(int bookId, int newAmount);
        Task<int> OrderBookByIdAsync(OrderModelData order);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<int?> DeleteOrderByIdAsync(int orderId);
        Task<List<Order>> GetAllOrdersOfUserAsync(string userId);
        Task<int> UpdateOrderByIdAsync(Order order);
    }
}

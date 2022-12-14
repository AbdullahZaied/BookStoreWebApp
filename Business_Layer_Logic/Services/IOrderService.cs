using Business.Logic.Layer.Models;

namespace Business.Logic.Layer.Services
{
    public interface IOrderService
    {
        Task<int> OrderBookById(OrderModelBusiness order);
        Task<OrderModelBusiness> GetOrderByIdAsync(int orderId);
        Task<int?> DeleteOrderByIdAsync(int orderId);
        Task<List<OrderModelBusiness>> GetAllOrdersOfUserAsync();
        Task<int?> UpdateOrderByIdAsync(int orderId, OrderModelBusiness order);
    }
}

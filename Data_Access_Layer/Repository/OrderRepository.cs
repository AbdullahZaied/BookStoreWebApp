using Data.Access.Layer.Data;
using Data.Access.Layer.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Data.Access.Layer.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookStoreContext _dbContext;

        public OrderRepository(BookStoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetBookAmountByIdAsync(int bookId)
        {
            int amount = await _dbContext.Stocks
                .Where(x => x.BookId == bookId)
                .Select(x => x.StockAmount)
                .SingleOrDefaultAsync();
            return amount;
        }

        public async Task<int?> SetBookAmountByIdAsync(int bookId, int newAmount)
        {
            var bookStock = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.BookId == bookId);
            if (bookStock != null)
            {
                bookStock.StockAmount = newAmount;
                var result = await _dbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _dbContext.Orders
                                    .Where(x => x.Id == orderId)
                                    .FirstOrDefaultAsync();
            return order;
        }

        public async Task<int?> DeleteOrderByIdAsync(int orderId)
        {
            var order = await _dbContext.Orders.Where(x => x.Id == orderId).FirstOrDefaultAsync();
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
            return order.Id;
        }

        public async Task<int> OrderBookByIdAsync(OrderModelData order)
        {
            var newOrder = new Order()
            {
                CreatedAt = order.CreatedAt,
                OrderAmount = order.OrderAmount,
                UserId = order.UserId,
                BookId = order.BookId
            };

            _dbContext.Orders.Add(newOrder);
            await _dbContext.SaveChangesAsync();

            return newOrder.Id;
        }

        public async Task<List<Order>> GetAllOrdersOfUserAsync(string userId)
        {
            var orders = await _dbContext.Orders.Where(x => x.UserId == userId).ToListAsync();

            return orders;
        }

        public async Task<int> UpdateOrderByIdAsync(Order order)
        {
            var orderFromEntity = await _dbContext.Orders.FindAsync(order.Id);
            
            orderFromEntity.BookId = order.BookId;
            orderFromEntity.OrderAmount = order.OrderAmount;
            orderFromEntity.UpdatedAt = order.UpdatedAt;
            return await _dbContext.SaveChangesAsync();
        }
    }
}

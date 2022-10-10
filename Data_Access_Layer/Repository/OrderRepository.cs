using Data.Access.Layer.Data;
using Data.Access.Layer.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task SetBookAmountByIdAsync(int bookId, int newAmount)
        {
            var bookStock = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.BookId == bookId);
            if (bookStock != null)
            {
                bookStock.StockAmount = newAmount;
                await _dbContext.SaveChangesAsync();
            }
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
    }
}

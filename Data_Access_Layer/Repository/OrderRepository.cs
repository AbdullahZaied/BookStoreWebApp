using Data.Access.Layer.Data;
using Data.Access.Layer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

        public async Task SetBookAmountByIdAsync(int bookId, int newAmount)
        {
            var bookStock = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.BookId == bookId);
            bookStock.StockAmount = newAmount;
            if (bookStock != null)
            {
                _dbContext.Update(bookStock);
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

        public async Task<int> UpdateOrderByIdAsync(Order order)
        {
            var orderFromEntity = await _dbContext.Orders.FindAsync(order.Id);
            
            orderFromEntity.BookId = order.BookId;
            orderFromEntity.OrderAmount = order.OrderAmount;
            orderFromEntity.UpdatedAt = order.UpdatedAt;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            int success = 0;
            IDbContextTransaction tx = null;

            try
            {
                using (tx = await _dbContext.Database.BeginTransactionAsync())
                {
                    success = await _dbContext.SaveChangesAsync();
                    tx.Commit();
                    return success;
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Stock)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];
                        }

                        // Refresh original values to bypass next concurrency check
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException("Unable to save changes. The stock was updated by another user. " + entry.Metadata.Name);
                    }
                }
                throw ex;
            }
            catch (DbUpdateException ex)
            {
                SqlException s = ex.InnerException as SqlException;
                var errorMessage = $"{ex.Message}" + " {ex?.InnerException.Message}" + " rolling back…";
                tx.Rollback();
            }
            return success;
        }
    }
}

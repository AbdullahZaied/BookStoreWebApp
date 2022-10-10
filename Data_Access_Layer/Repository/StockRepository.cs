using Data.Access.Layer.Data;
using Data.Access.Layer.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Data.Access.Layer.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly BookStoreContext _dbContext;

        public StockRepository(BookStoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Stock>> GetStockAsync()
        {
            var stocklist = await _dbContext.Stocks.ToListAsync();

            return stocklist;
        }

        public async Task<Stock?> GetStockByIdAsync(int bookId)
        {
            var bookStock = await _dbContext.Stocks
                                    .Where(x => x.BookId == bookId)
                                    .FirstOrDefaultAsync();
            return bookStock;
        }

        public async Task<int?> SetStockByIdAsync(Stock stockInfo)
        {
            var bookStock = await _dbContext.Stocks
                                    .FirstOrDefaultAsync(x => x.BookId == stockInfo.BookId);
            if (bookStock != null)
            {
                bookStock.StockedBy = stockInfo.StockedBy;
                bookStock.StockedAt = stockInfo.StockedAt;
                bookStock.StockAmount = stockInfo.StockAmount;
                await _dbContext.SaveChangesAsync();
            }

            return bookStock?.BookId;
        }

        public async Task<int?> CreateStockAsync(Stock stockInfo)
        {
            var newStock = new Stock()
            {
                BookId = stockInfo.BookId,
                StockAmount = stockInfo.StockAmount,
                StockedBy = stockInfo.StockedBy,
                StockedAt = stockInfo.StockedAt
            };

            _dbContext.Stocks.Add(newStock);
            await _dbContext.SaveChangesAsync();

            return newStock.BookId;
        }
    }
}

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
    }
}

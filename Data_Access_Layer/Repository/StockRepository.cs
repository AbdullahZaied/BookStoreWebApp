using Data.Access.Layer.Data;
using Data.Access.Layer.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}

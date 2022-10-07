using Data.Access.Layer.Data;

namespace Data.Access.Layer.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly BookStoreContext _dbContext;

        public StockRepository(BookStoreContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}

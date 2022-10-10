using Data.Access.Layer.Data;

namespace Data.Access.Layer.Repository
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetStockAsync();
        Task<Stock?> GetStockByIdAsync(int bookId);
        Task<int?> SetStockByIdAsync(Stock stockInfo);
        Task<int?> CreateStockAsync(Stock stockInfo);
    }
}

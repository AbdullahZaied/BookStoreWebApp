using Business.Logic.Layer.Models;

namespace Business.Logic.Layer.Services
{
    public interface IStockService
    {
        Task<List<StockModelBusiness>> GetStockAsync();
        Task<StockModelBusiness> GetStockByIdAsync(int bookId);
        Task<int?> SetStockByIdAsync(StockModelBusiness stock);
        Task<int?> CreateStockAsync(StockModelBusiness stock);
        Task<int?> DecreaseStockByIdAsync(StockModelBusiness stock);
        Task<int?> IncreaseStockByIdAsync(StockModelBusiness stock);
    }
}
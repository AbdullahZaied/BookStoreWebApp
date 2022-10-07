using Business.Logic.Layer.Models;

namespace Business.Logic.Layer.Services
{
    public interface IStockService
    {
        Task<List<StockModelBusiness>> GetStockAsync();
        Task<StockModelBusiness> GetStockByIdAsync(int bookId);
        Task<int?> SetStockByIdAsync(StockModelBusiness stock);
    }
}
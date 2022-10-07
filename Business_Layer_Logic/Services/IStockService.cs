using Business.Logic.Layer.Models;
using Data.Access.Layer.Data;

namespace Business.Logic.Layer.Services
{
    public interface IStockService
    {
        Task<List<StockModelBusiness>> GetStockAsync();
        Task<StockModelBusiness> GetStockByIdAsync(int bookId);
    }
}
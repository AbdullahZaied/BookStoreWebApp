using Data.Access.Layer.Data;

namespace Data.Access.Layer.Repository
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetStockAsync();
    }
}

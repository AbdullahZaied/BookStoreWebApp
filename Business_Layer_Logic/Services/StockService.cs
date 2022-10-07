using AutoMapper;
using Business.Logic.Layer.Models;
using Data.Access.Layer.Repository;

namespace Business.Logic.Layer.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;

        public StockService(IStockRepository stockRepository, IMapper mapper)
        {
            _stockRepository = stockRepository;
            _mapper = mapper;
        }

        public async Task<List<StockModelBusiness>> GetStockAsync()
        {
            var stocklist = await _stockRepository.GetStockAsync();
            return _mapper.Map<List<StockModelBusiness>>(stocklist);
        }

        public async Task<StockModelBusiness> GetStockByIdAsync( int bookId)
        {
            var bookStock = await _stockRepository.GetStockByIdAsync(bookId);
            return _mapper.Map<StockModelBusiness>(bookStock);
        }
    }
}

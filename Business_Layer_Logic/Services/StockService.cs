using AutoMapper;
using Business.Logic.Layer.Models;
using Data.Access.Layer.Data;
using Data.Access.Layer.Repository;

namespace Business.Logic.Layer.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public StockService(IStockRepository stockRepository,
                            IAccountService accountService,                
                            IMapper mapper)
        {
            _stockRepository = stockRepository;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<List<StockModelBusiness>> GetStockAsync()
        {
            var stocklist = await _stockRepository.GetStockAsync();
            return _mapper.Map<List<StockModelBusiness>>(stocklist);
        }

        public async Task<StockModelBusiness> GetStockByIdAsync(int bookId)
        {
            var bookStock = await _stockRepository.GetStockByIdAsync(bookId);
            return _mapper.Map<StockModelBusiness>(bookStock);
        }

        public async Task<int?> SetStockByIdAsync(StockModelBusiness stock)
        {
            stock.StockedBy = _accountService.GetCurrentUserId();
            stock.StockedAt = DateTime.Now;
            var newStock = await _stockRepository.SetStockByIdAsync(_mapper.Map<Stock>(stock));
            return newStock;
        }
    }
}

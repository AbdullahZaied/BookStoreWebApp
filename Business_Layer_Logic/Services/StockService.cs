using AutoMapper;
using Business.Logic.Layer.Models;
using Data.Access.Layer.Data;
using Data.Access.Layer.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            var id = await _stockRepository.SetStockByIdAsync(_mapper.Map<Stock>(stock));
            return id;
        }

        public async Task<int?> CreateStockAsync(StockModelBusiness stock)
        {
            var bookStock = await _stockRepository.GetStockByIdAsync(stock.BookId);
            if(bookStock != null)
            {
                return null;
            }
            stock.StockedBy = _accountService.GetCurrentUserId();
            stock.StockedAt = DateTime.Now;
            var id = await _stockRepository.CreateStockAsync(_mapper.Map<Stock>(stock));
            return id;
        }

        public async Task<int?> DecreaseStockByIdAsync(StockModelBusiness stock)
        {   
            var bookStock = await _stockRepository.GetStockByIdAsync(stock.BookId);
            if(bookStock.StockAmount - stock.DecStockBy < 0)
            {
                return null;
            }
            else
            {
                stock.StockAmount = bookStock.StockAmount - stock.DecStockBy;
                var id = await SetStockByIdAsync(stock);
                return id;
            }

        }
        public async Task<int?> IncreaseStockByIdAsync(StockModelBusiness stock)
        {
            var bookStock = await _stockRepository.GetStockByIdAsync(stock.BookId);
            
            if(bookStock == null)
            {
                return null;
            }
            else
            {
                stock.StockAmount = bookStock.StockAmount + stock.IncStockBy;
                var id = await SetStockByIdAsync(stock);
                return id;
            }
        }
    }
}

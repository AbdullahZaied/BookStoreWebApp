using AutoMapper;
using Business.Logic.Layer.Models;
using Data.Access.Layer.Models;
using Data.Access.Layer.Repository;

namespace Business.Logic.Layer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAccountService _accountService;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository,
            IAccountService accountService,
            IStockService stockService,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _accountService = accountService;
            _stockService = stockService;
            _mapper = mapper;
        }

        public async Task<int> OrderBookById(OrderModelBusiness order)
        {
            int stockAmount = await _orderRepository.GetBookAmountByIdAsync(order.BookId);
            order.CreatedAt = DateTime.Now;
            order.UserId = _accountService.GetCurrentUserId();

            if (stockAmount >= order.OrderAmount)
            {
                try
                {
                    var newAmount = stockAmount - order.OrderAmount;
                    await _orderRepository.SetBookAmountByIdAsync(order.BookId, newAmount);
                    var id = await _orderRepository.OrderBookByIdAsync(_mapper.Map<OrderModelData>(order));

                    return id;
                }
                catch(Exception ex)
                { 
                    throw new ArgumentException(ex.Message);
                }
            }
            else
            {
                return -1;
            }
        }

        public async Task<OrderModelBusiness> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            var oderBusiness = _mapper.Map<OrderModelBusiness>(order);
            if(oderBusiness == null || (oderBusiness.UserId != _accountService.GetCurrentUserId()))
            {
                return null;
            }
            return oderBusiness;
        }

        public async Task<int?> DeleteOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            var stock = await _stockService.GetStockByIdAsync(order.BookId);
            var oderBusiness = _mapper.Map<OrderModelBusiness>(order);
            if (oderBusiness == null || (oderBusiness.UserId != _accountService.GetCurrentUserId()))
            {
                return null;
            }
            else
            {
                await _orderRepository.SetBookAmountByIdAsync(order.BookId, order.OrderAmount + stock.StockAmount);
                var deleted = await _orderRepository.DeleteOrderByIdAsync(orderId);
                return deleted;
            }

        }

        public async Task<List<OrderModelBusiness>> GetAllOrdersOfUserAsync()
        {
            var userId = _accountService.GetCurrentUserId();

            if(userId == null)
            {
                return null;
            }
            var orders = _orderRepository.GetAllOrdersOfUserAsync(userId);
            var ordersList = _mapper.Map<List<OrderModelBusiness>>(orders);
            return ordersList;
        }
    }
}

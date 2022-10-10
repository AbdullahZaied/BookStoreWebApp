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
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IAccountService accountService, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _accountService = accountService;
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
    }
}

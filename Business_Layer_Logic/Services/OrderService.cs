using AutoMapper;
using Business.Logic.Layer.Models;
using Data.Access.Layer.Data;
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

        private async Task<string> BackToStock(int orderId)
        {
            var currentOrder = await _orderRepository.GetOrderByIdAsync(orderId);
            if (currentOrder == null || (currentOrder.UserId != _accountService.GetCurrentUserId()))
            {
                return null;
            }
            var stockOfCurrentOrderedBook = await _stockService.GetStockByIdAsync(currentOrder.BookId);
            var newAmount = stockOfCurrentOrderedBook.StockAmount + currentOrder.OrderAmount;
            var done = await _orderRepository.SetBookAmountByIdAsync(currentOrder.BookId, newAmount);
            if(done == 1)
            {
                return "ok";
            }
            else
            {
                return null;
            }
        }

        public async Task<int?> UpdateOrderByIdAsync(int orderId, OrderModelBusiness order)
        {
            var proceed = await BackToStock(orderId);
            var orderData = _mapper.Map<Order>(order);

            if (proceed == null)
            {
                return null;
            }
            else
            {
                var stock = await _stockService.GetStockByIdAsync(order.BookId);
             
                orderData.UpdatedAt = DateTime.Now;
                orderData.UserId = _accountService.GetCurrentUserId();
                orderData.Id = orderId;
                
                if(order.OrderAmount > stock.StockAmount)
                {
                    return null;
                }

                var newBookAmount = stock.StockAmount - order.OrderAmount;
                await _orderRepository.SetBookAmountByIdAsync(order.BookId, newBookAmount);
                var updated = await _orderRepository.UpdateOrderByIdAsync(orderData);
                
                return updated;
            }

        }

        public async Task<List<OrderModelBusiness>> GetAllOrdersOfUserAsync()
        {
            var userId = _accountService.GetCurrentUserId();

            if(userId == null)
            {
                return null;
            }
            var orders = await _orderRepository.GetAllOrdersOfUserAsync(userId);
            var ordersList = _mapper.Map<List<OrderModelBusiness>>(orders);
            return ordersList;
        }
    }
}

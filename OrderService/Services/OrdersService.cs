﻿using AutoMapper;
using OrderService.Clients;
using OrderService.DTO;
using OrderService.Kafka;
using OrderService.Models;
using OrderService.Repositories;
using static OrderService.Services.OrdersService;

namespace OrderService.Services
{
    public class OrdersService : IOrdersService
    {
      
            private readonly IOrderRepository _repository;
            private readonly IProductServiceClient _productServiceClient;
            private readonly ICustomerServiceClient _customerServiceClient;
            private readonly IInventoryServiceClient _inventoryServiceClient;
            private readonly KafkaProducer _kafkaProducer;
            private readonly IMapper _mapper;

            public OrdersService(IOrderRepository repository,
                IProductServiceClient productServiceClient, ICustomerServiceClient customerServiceClient, 
                IMapper mapper, KafkaProducer kafkaProducer, IInventoryServiceClient inventoryServiceClient)
            {
                _repository = repository;
                _productServiceClient = productServiceClient;
                _customerServiceClient = customerServiceClient;
                _mapper = mapper;
                _kafkaProducer = kafkaProducer;
            _inventoryServiceClient = inventoryServiceClient;
            }

            public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
            {
                var orders = await _repository.GetAllAsync();
                var orderDtos = new List<OrderDTO>();

                foreach (var order in orders)
                {
                    var productDetails = await _productServiceClient.GetProductDetailsAsync(order.ProductId);
                    var customerDetails = await _customerServiceClient.GetCustomerDetailsAsync(order.CustomerId);

                    var orderDto = _mapper.Map<OrderDTO>(order);
                    orderDto.ProductDetails = productDetails;
                    orderDto.CustomerDetails = customerDetails;

                    orderDtos.Add(orderDto);
                }

                return orderDtos;
            }

            public async Task<OrderDTO?> GetOrderByIdAsync(string id)
            {
                var order = await _repository.GetByIdAsync(id);
                if (order == null) return null;

                var productDetails = await _productServiceClient.GetProductDetailsAsync(order.ProductId);
                var customerDetails = await _customerServiceClient.GetCustomerDetailsAsync(order.CustomerId);

                var orderDto = _mapper.Map<OrderDTO>(order);
                orderDto.ProductDetails = productDetails;
                orderDto.CustomerDetails = customerDetails;

                return orderDto;
            }

            public async Task<OrderDTO?> CreateOrderAsync(OrderDTO orderDto)
            {
                var productExists = await _productServiceClient.ProductExistsAsync(orderDto.ProductId);
                var customerExists = await _customerServiceClient.CustomerExistsAsync(orderDto.CustomerId);

                if (!productExists || !customerExists)
                {
                    return null;
                }

                var stockOK = await PlaceOrder(orderDto.ProductId, orderDto.Quantity);

            if (!stockOK)
            {
                return null;
            }

                var order = _mapper.Map<Order>(orderDto);
                await _repository.AddAsync(order);

            var productDetails = await _productServiceClient.GetProductDetailsAsync(order.ProductId);
            var customerDetails = await _customerServiceClient.GetCustomerDetailsAsync(order.CustomerId);

            var oderDtoReturn = _mapper.Map<OrderDTO>(order);
            oderDtoReturn.ProductDetails = productDetails;
            oderDtoReturn.CustomerDetails = customerDetails;

            return oderDtoReturn;
            }

            public async Task<bool> UpdateOrderAsync(string id, OrderDTO orderDto)
            {
                var productExists = await _productServiceClient.ProductExistsAsync(orderDto.ProductId);
                var customerExists = await _customerServiceClient.CustomerExistsAsync(orderDto.CustomerId);

                if (!productExists || !customerExists)
                {
                    return false;
                }

                var existingOrder = await _repository.GetByIdAsync(id);
                if (existingOrder == null) return false;

                _mapper.Map(orderDto, existingOrder);
                await _repository.UpdateAsync(existingOrder);
                return true;
            }

            public async Task<bool> DeleteOrderAsync(string id)
            {
                return await _repository.DeleteAsync(id);
            }

        public async Task DeleteOrdersByCustomerIdAsync(int customerId)
        {
            await _repository.DeleteByCustomerIdAsync(customerId);
        }



        public async Task<bool> PlaceOrder(int productId, int quantity)
            {
                var stock = await _inventoryServiceClient.GetStock(productId);
                if (stock == null || stock.Quantity < quantity) return false;

                _kafkaProducer.ProduceOrderPlacedEvent(productId, quantity);
                return true;
         }

        }
    }
    

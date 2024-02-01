using Core.DTOs.Order;
using Core.Helper;
using Core.IRepos;
using E_Commerce_Platform.Controllers;
using Foods.Core.Helper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace E_Commerce.Tests
{
    public class OrderControllerTest
    {
        private readonly OrderController _controller;
        private readonly Mock<IOrderRepo> _repoMock;

        public OrderControllerTest()
        {
            _repoMock = new Mock<IOrderRepo>();
            _controller = new OrderController(_repoMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkObjectResult_WithCustomerOrders()
        {
            // Arrange
            int customerId = 1;
            int pageNumber = 1;
            int pageSize = 10;
            var listOrders = new List<OrderGetDTO>();
            var expectedOrders = new PaginatedList<OrderGetDTO>(listOrders,listOrders.Count,pageNumber,pageSize);

            _repoMock.Setup(repo => repo.GetCustomerOrdersAsync(customerId, pageNumber, pageSize))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _controller.Get(customerId, pageNumber, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualOrders = Assert.IsType<PaginatedList<OrderGetDTO>>(okResult.Value);
            Assert.Equal(expectedOrders, actualOrders);
        }
        [Fact]
        public void GetById_ReturnsOkObjectResult_WithOrder()
        {
            // Arrange
            int orderId = 1;
            var expectedOrder = new OrderGetDTO();
            _repoMock.Setup(repo => repo.GetById(orderId))
                .Returns(expectedOrder);

            // Act
            var result = _controller.GetById(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualOrder = Assert.IsType<OrderGetDTO>(okResult.Value);
            Assert.Equal(expectedOrder, actualOrder);
        }

        [Fact]
        public async Task GetAll_ReturnsOkObjectResult_WithOrders()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            var orderList = new List<OrderGetDTO>();
            var expectedOrders = new PaginatedList<OrderGetDTO>(orderList,orderList.Count,pageNumber,pageSize);

            _repoMock.Setup(repo => repo.GetOrdersAsync(pageNumber, pageSize))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _controller.GetAll(pageNumber, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualOrders = Assert.IsType<PaginatedList<OrderGetDTO>>(okResult.Value);
            Assert.Equal(expectedOrders, actualOrders);
        }

        [Fact]
        public async Task Create_ReturnsOkObjectResult_AfterCreatingOrder()
        {
            // Arrange
            var dto = new OrderAddDTO();
            var expectedResult = new object();

            _repoMock.Setup(repo => repo.Create(dto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public async Task Cancel_ReturnsOkObjectResult_AfterCancellingOrder()
        {
            // Arrange
            int orderId = 1;
            var expectedResult = new object();

            _repoMock.Setup(repo => repo.Cancel(orderId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Cancel(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResult, okResult.Value);
        }
    }
}

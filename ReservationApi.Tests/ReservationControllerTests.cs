using Microsoft.AspNetCore.Mvc;
using Moq;
using ReservationApi.Controllers;
using ReservationApi.Data.Models;
using ReservationApi.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ReservationApi.Tests
{
    public class ReservationControllerTests
    {
        private Mock<IReservationService> _mockResSvc;

        public ReservationControllerTests()
        {
            _mockResSvc = new Mock<IReservationService>();
        }

        [Fact]
        public async Task ReservtionController_Get_Reservation()
        {
            //arrange
            var newReservation = new Reservation
            {
                Id = Guid.NewGuid().ToString(),
                Name = "NewRes",
                Price = 1,
                FromDate = DateTime.Today.ToString(),
                ToDate = DateTime.Today.AddDays(1).ToString(),
                RoomId = "Room101"
            };

            _mockResSvc.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(newReservation);
            var controller = new ReservationController(_mockResSvc.Object);

            //act
            var result = await controller.GetAsync(newReservation.Id);
            var res = result.Result as OkObjectResult;

            Assert.Equal(newReservation, res.Value);
        }
    }
}

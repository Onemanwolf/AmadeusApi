using Moq;
using ReservationApi.Data.Intefaces;
using ReservationApi.Data.Models;
using ReservationApi.Services;
using System;
using Xunit;

namespace ReservationApi.Tests
{
    public class ReservationServiceTests
    {
        private Mock<IRepository<Reservation>> _mockRepo;

        public ReservationServiceTests()
        {
            _mockRepo = new Mock<IRepository<Reservation>>();
        }

        [Fact]
        public void ReservationService_Get_Reservation_Verify()
        {
            //arrange
            _mockRepo.Setup(x => x.GetAsync(It.IsAny<string>())).Verifiable();

            var reservationSvc = new ReservationService(_mockRepo.Object);

            //act
            var result = reservationSvc.GetAsync("1");

            //assert
            _mockRepo.Verify();
        }

        [Fact]
        public void ReservationService_Create_Reservation_Verify()
        {
            //arrange
            var reservation = new Reservation
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Special Meeting",
                RoomId = "101",
                FromDate = DateTime.Today.ToString(),
                ToDate = DateTime.Today.AddDays(1).ToString(),
                Price = 1
            };

            _mockRepo.Setup(x => x.InsertAsync(It.IsAny<Reservation>())).Verifiable();

            var reservationSvc = new ReservationService(_mockRepo.Object);

            //act
            var result = reservationSvc.CreateAsync(reservation);

            //assert
            _mockRepo.Verify();
        }
    }
}

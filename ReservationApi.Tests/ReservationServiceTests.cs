using Moq;
using ReservationApi.Data.Intefaces;
using ReservationApi.Data.Models;
using ReservationApi.Services;
using Xunit;

namespace ReservationApi.Tests
{
    public class ReservationServiceTests
    {
        [Fact]
        public void ReservationService_Get_Reservation_Verify()
        {
            //arrange
            var mockRepo = new Mock<IRepository<Reservation>>();
            mockRepo.Setup(x => x.GetAsync(It.IsAny<string>())).Verifiable();

            var reservationSvc = new ReservationService(mockRepo.Object);

            //act
            var result = reservationSvc.GetAsync("1");

            //assert
            mockRepo.Verify();
        }
    }
}

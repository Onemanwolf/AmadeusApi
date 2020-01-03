using Moq;
using ReservationApi.Models;
using ReservationApi.Services;
using System;
using Xunit;

namespace ReservationApi.Tests
{
    public class ReservationServiceTests
    {
        [Fact]
        public void ReservationService_Add_Reservation()
        {
            //arrange
            var resSvcMock = new Mock<IReservationService>();

            var newReservation = new Reservation
            {
                Id = "myid",
                Name = "ResName",
                RoomId = "roomid",
                Price = 2,
                FromDate = DateTime.Now.ToString(),
                ToDate = DateTime.Now.Add(new TimeSpan(1, 0, 0)).ToString()
            };

            resSvcMock.Setup(x => x.Create(newReservation)).Returns(newReservation);

            //act

            //assert
        }
    }
}

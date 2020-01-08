using MongoDB.Driver;
using Moq;
using ReservationApi.Tests.Mongo;
using Xunit;

namespace ReservationApi.Tests
{
    public class ReservationServiceTests
    {
        [Fact]
        public void ReservationService_Add_Reservation()
        {
            //arrange
            var mongoCollMock = new Mock<IFakeMongoCollection>();

            //mongoCollMock.Setup(x => x.Find(x => x.Id == It.IsAny<string>(), new FindOptions())).Verifiable();

            //var reservationSvc = new ReservationService(mongoCollMock.Object);

            //act
            //var result = reservationSvc.GetAsync("1");

            //assert
            mongoCollMock.Verify();

        }
    }
}

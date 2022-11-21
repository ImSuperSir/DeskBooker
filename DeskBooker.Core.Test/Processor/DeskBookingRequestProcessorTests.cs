using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;
using Moq;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        public readonly DeskBookingRequestProcessor _processor;
        private readonly DeskBookingRequest _request;
        private List<Desk> _availableDesks;
        private readonly Mock<IDeskBookRepository> _deskBookingRepositoryMock;
        private Mock<IDeskRepository> _deskRepositoryMock;

        public DeskBookingRequestProcessorTests()
        {
            _request = new DeskBookingRequest
            {
                FirstName = "Lauro",
                LastName = "Ramirez",
                Email = "imsupersir@outlook.com",
                Date = new DateTime(2021, 1, 14)
            };


            _availableDesks = new List<Desk>() { new Desk()};

            _deskBookingRepositoryMock = new Mock<IDeskBookRepository>();
            _deskRepositoryMock = new Mock<IDeskRepository>();
            _deskRepositoryMock.Setup(x => x.GetAvailableDesk(_request.Date))
                .Returns(_availableDesks);

            _processor = new DeskBookingRequestProcessor(_deskBookingRepositoryMock.Object
                , _deskRepositoryMock.Object);



        }


        [Fact]
        public void ShouldReturnDeskBookingResultWithRequestValues()
        {


            //Arrange


            //Act
            DeskBookingResult result =  _processor.BookDesk(_request);
            //Eval
            Assert.NotNull(result);
            Assert.Equal(_request.FirstName, result.FirstName);
            Assert.Equal(_request.LastName, result.LastName);
            Assert.Equal(_request.Email, result.Email);
            Assert.Equal(_request.Date, result.Date);

        }

        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        { 
            

            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void ShouldSaveDeskBooking()
        {
            //Prep
            DeskBooking saveDeskBooking = null;

            _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBooking>()))
                .Callback<DeskBooking>(z => 
                { 
                    saveDeskBooking = z;
                });


            //Act
            _processor.BookDesk(_request);


            //Eva
            _deskBookingRepositoryMock.Verify( x=> x.Save(It.IsAny<DeskBooking>()), Times.Once());

            Assert.NotNull(saveDeskBooking);
            
            Assert.Equal(_request.FirstName, saveDeskBooking.FirstName);
            Assert.Equal(_request.LastName, saveDeskBooking.LastName);
            Assert.Equal(_request.Email, saveDeskBooking.Email);
            Assert.Equal(_request.Date, saveDeskBooking.Date);

        }

        [Fact]
        public void ShouldNotSaveDeskBookingIfNoDeskAvailable()
        {
            //TODO: Ensure that the Desk is available

            //Prep

            _availableDesks.Clear();


            //Act
            _processor.BookDesk(_request);


            //Eva
            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Never());


        }
    }
}

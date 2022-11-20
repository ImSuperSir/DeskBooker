using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessor
    {
        private readonly IDeskBookRepository _deskBookRepository;

        public DeskBookingRequestProcessor(IDeskBookRepository iDeskBookRepository)
        {
            this._deskBookRepository = iDeskBookRepository;
        }

        public DeskBookingResult BookDesk(DeskBookingRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));


            _deskBookRepository.Save(Create<DeskBooking>(request));

            return Create<DeskBookingResult>(request);
            
        }

        private static T Create<T>(DeskBookingRequest request) where T : DeskBookingBase, new()
        {
            return new T
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Date = request.Date
            };
        }
    }
}
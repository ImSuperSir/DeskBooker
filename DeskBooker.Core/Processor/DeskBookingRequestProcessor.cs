using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessor
    {
        private readonly IDeskBookRepository _deskBookRepository;
        private readonly IDeskRepository _deskRepository;

        public DeskBookingRequestProcessor(IDeskBookRepository iDeskBookRepository, IDeskRepository deskRepository)
        {
            this._deskBookRepository = iDeskBookRepository;
            this._deskRepository = deskRepository;
        }

        public DeskBookingResult BookDesk(DeskBookingRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));


            var lAvailableDesk = _deskRepository.GetAvailableDesk(request.Date);

            if (lAvailableDesk.Count() > 0)
            {
                _deskBookRepository.Save(Create<DeskBooking>(request));
            }

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
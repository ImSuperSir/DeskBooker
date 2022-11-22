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


            var lAvailableDesks = _deskRepository.GetAvailableDesk(request.Date);
            
            var lResult = Create<DeskBookingResult>(request);

            if (lAvailableDesks.FirstOrDefault() is Desk lAvailableDesk)
            {
                var lDeskBooking = Create<DeskBooking>(request);
                lDeskBooking.DeskId = lAvailableDesk.Id;
                _deskBookRepository.Save(lDeskBooking);
                lResult.DeskBookingId = lDeskBooking.Id;
                lResult.Code = DeskBookingResultCode.Success;
            }
            else
            {
                lResult.Code = DeskBookingResultCode.NoDeskAvailable;
            }

            return lResult;
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
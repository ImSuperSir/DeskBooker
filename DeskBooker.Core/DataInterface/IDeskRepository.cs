using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeskBooker.Core.Domain;

namespace DeskBooker.Core.DataInterface
{
    public interface IDeskRepository
    {
        IEnumerable<Desk> GetAvailableDesk(DateTime date);
    
    }


}

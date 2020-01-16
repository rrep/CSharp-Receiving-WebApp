using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.Data.DTOs
{
    public class ReceiveRejectUnitDetail
    {
        public int ProductID { get; set; }
        public int OrderDetailID { get; set; }
        public int? ReceivedUnits { get; set; }
        public int? RejectedUnits { get; set; }
        public string RejectReason { get; set; }
        public int? SalvagedUnits { get; set; }
    }
}

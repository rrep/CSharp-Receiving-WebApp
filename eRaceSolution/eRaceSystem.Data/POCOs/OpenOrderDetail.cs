using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.Data.POCOs
{
    public class OpenOrderDetail
    {
        public int OrderDetailId { get; set; }
        public string ItemName { get; set; }
        public int ProductId { get; set; }
        public string UnitsOrdered { get; set; }
        public int OrderSize { get; set; }
        public int OrderUnitSize { get; set; }
        public string QuantityType { get; set; }
        public int? QuantityOutstanding { get; set; }

    }
}

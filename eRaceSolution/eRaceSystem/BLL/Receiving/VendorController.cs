using eRaceSystem.DAL;
using eRaceSystem.Data.Entities;
using eRaceSystem.Data.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.BLL.Receiving
{
    public class VendorController
    {
        public VendorInfo Vendor_FindByOrderId(int orderid)
        {
            using (var context = new ERaceContext())
            {

                var data = (from x in context.Orders
                           where x.OrderID == orderid
                           select new VendorInfo
                           {
                               Name = x.Vendor.Name,
                               Address = x.Vendor.Address,
                               Phone = x.Vendor.Phone,
                               Contact = x.Vendor.Contact
                           }).FirstOrDefault();
                return data;
            }
         }
    }
}

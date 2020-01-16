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
    public class OrderDetailController
    {
        public List<OpenOrderDetail> OrderDetail_FindOpenOrderDetail(int orderid)
        {
            using (var context = new ERaceContext())
            {
                var data = from x in context.OrderDetails
                           where x.Order.OrderID == orderid
                           select new OpenOrderDetail
                           {
                               OrderDetailId = x.OrderDetailID,
                               ItemName = x.Product.ItemName,
                               ProductId = x.Product.ProductID,
                               UnitsOrdered = x.Quantity + " x " + (x.OrderUnitSize > 1 ? "case" : "each") + " of ",
                               OrderUnitSize = x.OrderUnitSize,
                               OrderSize = x.OrderUnitSize * x.Quantity,
                               QuantityType = " x " + (x.OrderUnitSize > 1 ? "case" : "each") + " of ",
                               QuantityOutstanding = x.Quantity * x.OrderUnitSize - ((from y in context.ReceiveOrderItems
                                                                                     where y.OrderDetailID == x.OrderDetailID
                                                                                     select y.ItemQuantity).DefaultIfEmpty(0).Sum() < 0 ?
                                                                                                 0 : (from y in context.ReceiveOrderItems
                                                                                                      where y.OrderDetailID == x.OrderDetailID
                                                                                                      select y.ItemQuantity).DefaultIfEmpty(0).Sum())
                           };

                return data.ToList();
            }

        }
    }
}

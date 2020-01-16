using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eRaceSystem.DAL;
using eRaceSystem.Data.DTOs;
using eRaceSystem.Data.Entities;
using eRaceSystem.Data.POCOs;

namespace eRaceSystem.BLL.Receiving
{
    [DataObject]
    public class OrderController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<OpenOrder> List_OpenOrder()
        {
            using (var context = new ERaceContext())
            {
                var data = from x in context.Orders
                           where x.OrderNumber != null && x.Closed == false
                           select new OpenOrder
                           {
                               OrderId = x.OrderID,
                               VendorOrder = x.OrderID + " - " + x.Vendor.Name
                           };

                return data.ToList();
            }
        }

        public void ForceClose_Order(int orderid, string reason)
        {
            if (string.IsNullOrEmpty(reason))
            {
                throw new Exception("Reason required for force close.");
            }
            else
            {

            using (var context = new ERaceContext())
            {
                List<string> reasons = new List<string>();
                //Find Order by orderid
                //Validate order is not closed
                var existing = (from x in context.Orders
                                where x.OrderID == orderid && x.Closed == false
                                select x).FirstOrDefault();
                if (existing == null)
                {
                    throw new Exception("Order is not open or does not exist");
                }
                else
                {
                    //	Set Order.Closed to true
                    existing.Closed = true;
                    context.Entry(existing).Property(y => y.Closed).IsModified = true;
                    //	Set Order.Comment to reason string
                    existing.Comment = reason;
                    context.Entry(existing).Property(y => y.Comment).IsModified = true;
                    //	Find outstanding product balances related to that order ID using ReceiveOrderItem and OrderDetail Table
                    var productList = (from x in context.OrderDetails
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
                                           QuantityOutstanding = x.Quantity * x.OrderUnitSize - (((from y in context.ReceiveOrderItems
                                                                                                   where y.OrderDetailID == x.OrderDetailID
                                                                                                   select y.ItemQuantity).DefaultIfEmpty(0).Sum()) < 0 ?
                                                                                                 0 : (from y in context.ReceiveOrderItems
                                                                                                      where y.OrderDetailID == x.OrderDetailID
                                                                                                      select y.ItemQuantity).DefaultIfEmpty(0).Sum())
                                       }).ToList();

                        if (productList == null)
                        {
                            throw new Exception("Order is not open or does not exist");
                        }
                        else
                        {
                            foreach (var item in productList)
                            {
                                //	Find and decrement decrement QuantityOnOrder of the appropriate<Product> collection items by corresponding outstanding balances related to that OrderID
                                if (item.QuantityOutstanding > 0)
                                {
                                    var productToModify = (from z in context.Products
                                                           where z.ProductID == item.ProductId
                                                           select z).FirstOrDefault();
                                    if (productToModify == null)
                                    {
                                        reasons.Add(item.ItemName + " does not exist.");
                                    }
                                    else
                                    {
                                        productToModify.QuantityOnOrder -= item.QuantityOutstanding.Value;
                                        context.Entry(productToModify).Property(y => y.QuantityOnOrder).IsModified = true;
                                    }
                                }
                            }
                            //	If they exist, remove items from the UnOrderedItems collection.
                            //context.UnOrderedItems.RemoveRange(context.UnOrderedItems);
                            //	Save transaction if nothing screwed up in the loop
                            if (reasons.Count() > 0)
                                throw new BusinessRuleException("Transaction failed", reasons);
                            else
                                context.SaveChanges();
                        }
                    }
                }
            }
        }
        public void Receive_Order(int orderid, int employeeid, List<ReceiveRejectUnitDetail> itemList)
        {
            using (var context = new ERaceContext())
            {
                List<string> reasons = new List<string>();
                // Validate order has not been closed 
                var existing = (from x in context.Orders
                                where x.OrderID == orderid && x.Closed == false
                                select x).FirstOrDefault();
                if (existing == null)
                {
                    throw new Exception("Order is not open or does not exist");
                }
                else
                {
                    
                    //	Create new ReceiveOrder record
                    ReceiveOrder newReceiveOrder = new ReceiveOrder();
                    newReceiveOrder.OrderID = orderid;
                    newReceiveOrder.EmployeeID = employeeid;
                    newReceiveOrder.ReceiveDate = DateTime.Now;

                    //iterate through the list of ReceiveRejectUnitDetail
                    foreach (var item in itemList)
                    {
                        //check to see if the received amounts exceed acceptable qtys.
                        var productChosen = (from x in context.OrderDetails
                                           where x.OrderDetailID == item.OrderDetailID
                                           select new OpenOrderDetail
                                           {
                                               OrderDetailId = x.OrderDetailID,
                                               ItemName = x.Product.ItemName,
                                               ProductId = x.Product.ProductID,
                                               UnitsOrdered = x.Quantity + " x " + (x.OrderUnitSize > 1 ? "case" : "each") + " of ",
                                               OrderUnitSize = x.OrderUnitSize,
                                               OrderSize = x.OrderUnitSize * x.Quantity,
                                               QuantityType = " x " + (x.OrderUnitSize > 1 ? "case" : "each") + " of ",
                                               QuantityOutstanding = x.Quantity * x.OrderUnitSize - (((from y in context.ReceiveOrderItems
                                                                                                       where y.OrderDetailID == x.OrderDetailID
                                                                                                       select y.ItemQuantity).DefaultIfEmpty(0).Sum()) < 0 ?
                                                                                                 0 : (from y in context.ReceiveOrderItems
                                                                                                      where y.OrderDetailID == x.OrderDetailID
                                                                                                      select y.ItemQuantity).DefaultIfEmpty(0).Sum())
                                           }).FirstOrDefault();
                        //BLL VALIDATION SECTION for ITEM
                        if ((productChosen.QuantityOutstanding - (item.ReceivedUnits + item.SalvagedUnits)) <= -(productChosen.OrderUnitSize))
                        {
                            reasons.Add("Cannot accept overage exceeding a single Unit Size for product: " + productChosen.ItemName);
                        }
                        if ((item.SalvagedUnits+item.RejectedUnits)%productChosen.OrderUnitSize!=0)
                        {
                            reasons.Add("Individual salvage and reject totals do not match unit size for: " + productChosen.ItemName);
                        }
                        if (item.RejectedUnits > 0 && string.IsNullOrEmpty(item.RejectReason))
                        {
                            reasons.Add("Reason required for rejection of item: " + productChosen.ItemName);
                        }
                        if (item.RejectedUnits < 0 || item.ReceivedUnits < 0 || item.SalvagedUnits < 0)
                        {
                            reasons.Add("Quantities cannot be negative for: " + productChosen.ItemName);
                        }
                        if (reasons.Count() == 0)
                        {
                            //	Add New ReceiveOrderItem record to ReceiveOrderItem collection for each item in ReceiveRejectUnitDetail that has ReceivedUnits > 0
                            if (item.ReceivedUnits > 0 || item.SalvagedUnits > 0)
                            {
                                ReceiveOrderItem receiveLine = new ReceiveOrderItem();
                                receiveLine.OrderDetailID = item.OrderDetailID;
                                receiveLine.ItemQuantity = item.ReceivedUnits.Value + item.SalvagedUnits.Value;
                                newReceiveOrder.ReceiveOrderItems.Add(receiveLine);

                                //	Find and Decrement QuantityOnOrder for products in the Product collection by corresponding receive quantity
                                int effectedProductId = (from x in context.OrderDetails where x.OrderDetailID == item.OrderDetailID select x.ProductID).FirstOrDefault();
                                Product effectedProduct = context.Products.Find(effectedProductId);
                                effectedProduct.QuantityOnOrder -= item.ReceivedUnits.Value + item.SalvagedUnits.Value;
                                if (effectedProduct.QuantityOnOrder < 0)
                                    effectedProduct.QuantityOnOrder = 0;
                                context.Entry(effectedProduct).Property(y => y.QuantityOnOrder).IsModified = true;
                                effectedProduct.QuantityOnHand += item.ReceivedUnits.Value + item.SalvagedUnits.Value;
                                context.Entry(effectedProduct).Property(y => y.QuantityOnHand).IsModified = true;

                            }//	Add New ReturnOrderItem record to ReturnOrderItem collection for each item in ReceiveRejectUnitDetail that has RejectedUnits > 0
                            if (item.RejectedUnits > 0)
                            {
                                ReturnOrderItem newReturnItem = new ReturnOrderItem();
                                newReturnItem.ItemQuantity = item.RejectedUnits.Value;
                                newReturnItem.Comment = item.RejectReason;
                                newReturnItem.VendorProductID = item.ProductID.ToString();
                                newReturnItem.OrderDetailID = item.OrderDetailID;
                                newReturnItem.UnOrderedItem = (from x in context.OrderDetails where x.OrderDetailID == item.OrderDetailID select x.Product.ItemName).FirstOrDefault();
                                //stage it
                                newReceiveOrder.ReturnOrderItems.Add(newReturnItem);
                            }
                        }
                    }//end of foreach

                    //	Retrieve items from UnorderedItems collection
                    var unOrderItemsList = context.UnOrderedItems.ToList();
                    //create a ReturnOrderItem for each unordered item
                    foreach (var unordereditem in unOrderItemsList)
                    {
                        ReturnOrderItem newReturnItem = new ReturnOrderItem();
                        newReturnItem.ItemQuantity = unordereditem.Quantity;
                        newReturnItem.UnOrderedItem = unordereditem.ItemName;
                        newReturnItem.VendorProductID = unordereditem.VendorProductID;
                        newReturnItem.Comment = unordereditem.ItemName;
                        //stage it
                        newReceiveOrder.ReturnOrderItems.Add(newReturnItem);
                    }
                    //	Clear Context UnOrderedItems Table
                    context.UnOrderedItems.RemoveRange(context.UnOrderedItems);
                    //stage it
                    existing.ReceiveOrders.Add(newReceiveOrder);

                    int remainingLineCounter = 0;
                    //	If ALL OrderDetail.UnitSize * OrderDetail.Quantity =< ReceiveOrderItems.ItemQuantity.Sum(), set Order.Closed to true.
                    //possible solution: get a list of orderdetails
                    var orderDetails = (from y in context.OrderDetails
                                        where y.OrderID == existing.OrderID
                                        select y).ToList();
                    //foreach orderdetail, sum the receiveorderitem.itemquantity
                    foreach (OrderDetail item in orderDetails)
                    {
                        //get total amount that have been received per item
                        int existingTotal = ((from y in context.ReceiveOrderItems
                                            where y.OrderDetailID == item.OrderDetailID
                                            select y.ItemQuantity).DefaultIfEmpty(0).Sum());
                        //add the new value we've just added
                        int newQuantity = ((from z in itemList
                                           where z.OrderDetailID == item.OrderDetailID
                                           select z.ReceivedUnits.Value).FirstOrDefault());
                        //put them together for the true total
                        int receiveTotal = existingTotal + newQuantity;
                        // if the order detail total is less than the sum of received orders, 
                            //add a tick to the counter to stop it from closing
                        if (receiveTotal < (item.OrderUnitSize * item.Quantity))
                            remainingLineCounter++;
                    }

                    //if logic check passes close the order
                    if (remainingLineCounter == 0)
                    {
                        existing.Closed = true;
                        context.Entry(existing).Property(y => y.Closed).IsModified = true;
                        existing.Comment = "Received in full";
                        context.Entry(existing).Property(y => y.Comment).IsModified = true;
                    }
                    //	Save transaction
                    if (reasons.Count() > 0)
                    {
                       throw new BusinessRuleException("Receive Failed.", reasons);
                    }
                    else
                    {
                        context.SaveChanges();
                    }  
                }
            }
        }
    }
}

using DMIT2018Common.UserControls;
using eRaceSystem.BLL.Common;
using eRaceSystem.BLL.Receiving;
using eRaceSystem.Data.DTOs;
using eRaceSystem.Data.Entities;
using eRaceSystem.Data.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApp.Security;

namespace eRaceWebApp.WebPages.Receiving
{
    public partial class receivinghome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {   //add
            
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Clerk") || User.IsInRole("Food Service"))
                {
                    var username = User.Identity.Name;
                    SecurityController securitymgr = new SecurityController();
                    int? employeeid = securitymgr.GetCurrentUserEmployeeId(username);
                    if (employeeid.HasValue)
                    {
                        MessageUserControl.TryRun(() =>
                        {
                            EmployeeController sysmgr = new EmployeeController();
                            Employee info = sysmgr.Employee_Get(employeeid.Value);
                            EmployeeName.Text = info.FullName;
                        });
                    }
                    else
                    {
                        MessageUserControl.ShowInfo("UnRegistered User", "This user is not a registered employee");
                        EmployeeName.Text = "Unregistered User";
                    }
                }
                else
                {
                    //redirect to a page that states no authorization fot the request action
                    Response.Redirect("~/Security/AccessDenied.aspx");
                }
            }
            else
            {
                //redirect to login page
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        protected void UnOrderedItems_ClearTable()
        {
            UnOrderedItemsController unosysmgr = new UnOrderedItemsController();
            unosysmgr.Clear_UnOrderedItems();
        }
        protected void SearchOrder_Click(object sender, EventArgs e)
        {
            //clear UnOrderedItems
            UnOrderedItems_ClearTable();
            OpenOrderDetailList.DataSource = "";
            OpenOrderDetailList.DataBind();
            //create base value
            int searchOrderId = 0;
            //get selected orderID from DDL
            string searchOrderIdValue = OpenOrderDDL.SelectedValue;
            //make sure something is selected
            if (string.IsNullOrEmpty(searchOrderIdValue))
            {
                MessageUserControl.ShowInfo("Attention", "Please select an order.");
            }
            //make sure it is parsable
            else if (!int.TryParse(searchOrderIdValue, out searchOrderId))
            {
                MessageUserControl.ShowInfo("Attention", "Current Order is invalid. Perform lookup again.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    //access related vendor
                    VendorController vsysmgr = new VendorController();
                    VendorInfo chosenVendor = vsysmgr.Vendor_FindByOrderId(searchOrderId);
                    //populate vendor info labels
                    VendorAddressLabel.Text = chosenVendor.Address;
                    VendorContactLabel.Text = chosenVendor.Contact;
                    VendorNameLabel.Text = chosenVendor.Name;
                    VendorPhoneLabel.Text = chosenVendor.Phone;


                    //generate list and populate table
                    OrderDetailController odsysmgr = new OrderDetailController();
                    List<OpenOrderDetail> openOrderItems = odsysmgr.OrderDetail_FindOpenOrderDetail(searchOrderId);
                    OpenOrderDetailList.DataSource = openOrderItems;
                    OpenOrderDetailList.DataBind();
                    UnOrderedItemsList.DataBind();
                }, "Order Loaded", "Please enter received items.");

            }
        }
        protected void ForceClose_Click(object sender, EventArgs e)
        {
            //get reason for close
            string reasonForClose = ForceCloseTextBox.Text;
            int searchOrderId;
            if(!int.TryParse(OpenOrderDDL.SelectedValue, out searchOrderId))
            {
                MessageUserControl.ShowInfo("Attention", "Current Order is invalid. Perform lookup again.");
            }
            else if(string.IsNullOrEmpty(reasonForClose))
            {
                MessageUserControl.ShowInfo("Required Data", "Please add a reason for close.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    //get orderid
                    int orderid = int.Parse(OpenOrderDDL.SelectedValue);
                    //send to bll 
                    OrderController osysmgr = new OrderController();
                    osysmgr.ForceClose_Order(orderid, reasonForClose);
                    OpenOrderDDL.Items.Clear();
                    OpenOrderDDL.DataBind();
                    OpenOrderDDL.Items.Insert(0, "Please Choose an Order");
                    OpenOrderDetailList.DataBind();
                    UnOrderedItemsList.DataBind();
                }, "Close Order", "Order has been closed.");
            }
        }

        protected void ReceiveItems_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int orderid;
                //•	Collect Employee ID from Log - In
                var username = User.Identity.Name;
                SecurityController securitymgr = new SecurityController();
                int employeeid = securitymgr.GetCurrentUserEmployeeId(username).Value;
                //•	CollectOrderID from DDL
                if (!int.TryParse(OpenOrderDDL.SelectedValue, out orderid))
                {
                    MessageUserControl.ShowInfo("Attention", "Current Order is invalid. Perform lookup again.");
                }
                else
                {
                    bool processableItemExists = false;
                    //•	Collect, validate, and parse input from 8,9,10,11,21,22 into ReceiveRejectUnitDetail and add to collection of List<ReceiveRejectUnitDetail>
                    List<ReceiveRejectUnitDetail> tableItems = new List<ReceiveRejectUnitDetail>();
                    foreach (GridViewRow item in OpenOrderDetailList.Rows)
                    {
                        ReceiveRejectUnitDetail lineItem = new ReceiveRejectUnitDetail();
                        //    public int ProductID { get; set; }
                        //    public int OrderDetailID { get; set; }  //ItemOrderDetailID
                        lineItem.OrderDetailID = int.Parse((item.FindControl("ItemOrderDetailID") as Label).Text);
                        //    public int? ReceivedUnits { get; set; } //ReceivedUnitsTextBox
                        lineItem.ReceivedUnits = (string.IsNullOrEmpty((item.FindControl("ReceivedUnitsTextBox") as TextBox).Text) ? 0 : int.Parse((item.FindControl("ReceivedUnitsTextBox") as TextBox).Text)) * int.Parse((item.FindControl("OrderedUnitSizeLabel1") as Label).Text);
                        if (lineItem.ReceivedUnits > 0)
                            processableItemExists = true;
                        //    public int? RejectedUnits { get; set; } //RejectedUnitsTextBox"
                        lineItem.RejectedUnits = (string.IsNullOrEmpty((item.FindControl("RejectedUnitsTextBox") as TextBox).Text) ? 0 : int.Parse((item.FindControl("RejectedUnitsTextBox") as TextBox).Text));
                        if (lineItem.RejectedUnits > 0)
                            processableItemExists = true;
                        //    public string RejectReason { get; set; } //"RejectedReasonTextBox"
                        lineItem.RejectReason = (item.FindControl("RejectedReasonTextBox") as TextBox).Text;
                        //    public int? SalvagedUnits { get; set; } ID="SalvagedUnitsTextBox"
                        lineItem.SalvagedUnits = (string.IsNullOrEmpty((item.FindControl("SalvagedUnitsTextBox") as TextBox).Text) ? 0 : int.Parse((item.FindControl("SalvagedUnitsTextBox") as TextBox).Text));
                        if (lineItem.SalvagedUnits > 0)
                            processableItemExists = true;
                        tableItems.Add(lineItem);
                    }
                    //•	Send to OrderController Receive_Order method
                    if (processableItemExists)
                    {
                        MessageUserControl.TryRun(() =>
                        {
                            OrderController osysmgr = new OrderController();
                            osysmgr.Receive_Order(orderid, employeeid, tableItems);
                        //•	Refresh display
                            OpenOrderDetailList.DataBind();
                            UnOrderedItemsList.DataBind();
                            OpenOrderDDL.Items.Clear();
                            OpenOrderDDL.DataBind();
                            OpenOrderDDL.Items.Insert(0, "Please Choose an Order");
                        }, "Success", "Shipment Has Been Received Successfully");
                    }
                    else
                    {
                        MessageUserControl.ShowInfo("Attention", "At least one item must be received or rejected to process.");
                    }
                }
            }
        }

        #region DataBoundExceptions
        protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }
        protected void InsertCheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Success", "UnOrderedItem added.");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }
        protected void DeleteCheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Success", "UnOrderedItem removed.");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }
        #endregion
    }
}
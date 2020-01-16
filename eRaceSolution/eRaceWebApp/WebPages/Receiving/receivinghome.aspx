<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="receivinghome.aspx.cs" Inherits="eRaceWebApp.WebPages.Receiving.receivinghome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>
    <h1>Receiving for <asp:Label ID="EmployeeName" runat="server" ></asp:Label></h1>

        <%-- Header --%>
            <blockquote style="font-style: italic">
                This page includes allows a user to receive shipments for open orders. 
            </blockquote>
    <%-- Message Label  --%>
    <uc1:MessageUserControl runat="server" id="MessageUserControl" />

    <%-- Validation summary --%>
    <asp:ValidationSummary ID="ValidationSummary" runat="server"
         HeaderText="Please correct the following Receive Table concerns before submission:"
         ValidationGroup="ReceiveValidationGroup"/>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server"
         HeaderText="Please correct the following Unordered Item concerns before submission:"
         ValidationGroup="UnorderedValidationGroup"/>
    <%-- Dropdownlist for selection --%>
    <asp:DropDownList ID="OpenOrderDDL" runat="server" AppendDataBoundItems="true" DataSourceID="OpenOrderDDLODS" DataTextField="VendorOrder" DataValueField="OrderId">
        <asp:ListItem Text="Please Choose an Order"></asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="SearchOrder" runat="server" Text="Search" CssClass="btn btn-primary" CausesValidation="false" OnClick="SearchOrder_Click"/><br/><br/>
    
    <%-- Vendor info labels --%>
    <asp:Label ID="VendorNameLabel" runat="server" Text=""></asp:Label>&nbsp&nbsp
    <asp:Label ID="VendorAddressLabel" runat="server" Text=""></asp:Label><br />
    <asp:Label ID="VendorContactLabel" runat="server" Text=""></asp:Label>&nbsp&nbsp
    <asp:Label ID="VendorPhoneLabel" runat="server" Text=""></asp:Label><br /><br />
    
    <%-- Gridview for Open Item Details --%>
    <asp:GridView ID="OpenOrderDetailList" runat="server" AutoGenerateColumns="False"
             GridLines="Both" BorderStyle="Solid" CssClass="table table-striped">
            <Columns>
                <%-- Hidden Key --%>
                <asp:TemplateField HeaderText="Keys" Visible="false">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="ItemOrderDetailID" Width="175px"
                            Text='<%# Eval("OrderDetailID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- Item --%>
                <asp:TemplateField HeaderText="Item">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="ItemNameLabel" Width="175px"
                            Text='<%# Eval("ItemName") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>

                <%-- QtyOrdered --%>
                <asp:TemplateField HeaderText="Qty Ordered">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="QtyOrderedLabel" Width="40px"
                            Text='<%# Eval("OrderSize")%>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>

                <%-- Ordered Units --%>
                <asp:TemplateField HeaderText="Ordered Units">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="OrderedUnitLabel"
                            Text='<%# Eval("UnitsOrdered") %>'></asp:Label>
                        <asp:Label runat="server" ID="OrderedUnitSizeLabel1"
                            Text='<%# Eval("OrderUnitSize") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                
                <%-- Qty Outstanding --%>
                <asp:TemplateField HeaderText="Qty Outstanding">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="QtyOutstandingLabel" Width="40px"
                            Text='<%# (Eval("QuantityOutstanding")==null ||int.Parse(Eval("QuantityOutstanding").ToString())<0)?0:Eval("QuantityOutstanding") %>'></asp:Label>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>

                <%-- Received Units (Entry) --%>
                <asp:TemplateField HeaderText="Received Units">
                    <ItemTemplate> 
                       <asp:RegularExpressionValidator ID="ReceiveExpressionValidator" 
                           ControlToValidate="ReceivedUnitsTextBox" runat="server" Display="None"
                           ErrorMessage="Receive unit entries must be blank or a number" 
                           ValidationExpression="^[0-9]+$|^$"
                           ValidationGroup="ReceiveValidationGroup"></asp:RegularExpressionValidator>
                       <asp:TextBox runat="server" ID="ReceivedUnitsTextBox" Width="40px"></asp:TextBox>
                          &nbsp;&nbsp;
                        <asp:Label runat="server" ID="ReceivedUnitLabel"
                            Text='<%# Eval("QuantityType") %>'></asp:Label>
                        <asp:Label runat="server" ID="OrderedUnitSizeLabel2" 
                            Text='<%# Eval("OrderUnitSize") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <%-- Rejected Units/Reason (Entry)(Entry) --%>
                <asp:TemplateField HeaderText="Rejected Units/Reason">
                    <ItemTemplate>
                        <%-- Number validation --%>  
                        <asp:RegularExpressionValidator ID="RejectExpressionValidator" Display="None"
                                ControlToValidate="RejectedUnitsTextBox" runat="server" 
                                ErrorMessage="Rejected unit entries must be blank or a number" 
                                ValidationExpression="(^[0-9]+$|^$)"
                                ValidationGroup="ReceiveValidationGroup">
                          </asp:RegularExpressionValidator>
                        <%-- Reason Validation --%>
                           <asp:RegularExpressionValidator ID="ReasonExpressionValidator" runat="server" 
                                ErrorMessage="Reason is limited to 50 characters" Display="None"
                                ControlToValidate="RejectedReasonTextBox" ValidationGroup="ReceiveValidationGroup"
                                ValidationExpression="^.{1,50}$">
                          </asp:RegularExpressionValidator>
                        <asp:TextBox runat="server" ID="RejectedUnitsTextBox" Width="40px"></asp:TextBox>
                          &nbsp;&nbsp;
                        <asp:TextBox runat="server" ID="RejectedReasonTextBox" Width="100px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <%-- Salvaged Units--%>
                <asp:TemplateField HeaderText="Salvaged Units">
                    <ItemTemplate>
                        <asp:RegularExpressionValidator ID="SalvagedExpressionValidator" 
                           ControlToValidate="SalvagedUnitsTextBox" runat="server" Display="None"
                           ErrorMessage="Salvaged unit entries must be blank or a number" 
                           ValidationExpression="(^[0-9]+$|^$)"
                           ValidationGroup="ReceiveValidationGroup"></asp:RegularExpressionValidator>
                        <asp:TextBox runat="server" ID="SalvagedUnitsTextBox" Width="40px"></asp:TextBox>
                          &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
            Please select an order to view outstanding items.
        </EmptyDataTemplate>
        </asp:GridView>

    
    <div class="row">
       <%-- Receive/ForceClose Buttons --%>
        <div class="col-md-4 col-md-offset-10">
            <asp:Button ID="ForceCloseButton" runat="server" Text="Force Close" CssClass="btn btn-secondary" CausesValidation="false" OnClick="ForceClose_Click" OnClientClick="return confirm('Are you sure you wish to force close order?')"/>
            <asp:Button ID="ReceiveItemsButton" runat="server" Text="Receive" CssClass="btn btn-primary" OnClick="ReceiveItems_Click" CausesValidation="true" ValidationGroup="ReceiveValidationGroup"/><br />
            <%-- Text area for Force Close reasoning --%>
            <asp:TextBox ID="ForceCloseTextBox" TextMode="MultiLine" width="180px" placeholder="If applicable, please enter reason for force closure." runat="server"></asp:TextBox>
        </div>

        <%-- UnoderedItems Scratchpad --%>
        <div class="col-med-8">
            <asp:ListView ID="UnOrderedItemsList" runat="server" DataSourceID="UnOrderedItemListODS"  DataKeyNames="ItemId" InsertItemPosition="LastItem">
                <%-- UnOrdered Items ListView - Layout Template --%>
                <LayoutTemplate>
                    <asp:Label ID="UnorderedItemsHeader" runat="server" Text="Enter UnOrdered Items" style="margin-left:20px;"></asp:Label>
                    <table runat="server" id="itemPlaceholderContainer" style="margin-left:20px; background-color: #cbe6f7; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                            <tr runat="server"> 
                                <th runat="server">Item ID</th>
                                <th runat="server">Item Name</th>
                                <th runat="server">Vendor Product ID</th>
                                <th runat="server">Quantity</th>
                                <th runat="server"></th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder"></tr>
                        </table>
                </LayoutTemplate>

                <%-- Empty data template --%>
                <EmptyDataTemplate>
                    <table runat="server">
                        <tr>
                            <td>No data was returned.</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>

                <%-- UnOrdered Items ListView - Item Template --%>
                <ItemTemplate>
                    <tr runat="server"> 
                        <td>
                            <asp:Label ID="ItemIDLabel" runat="server" Text='<%# Eval("ItemId") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="ItemNameLabel" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="ItemVendorIDLabel" runat="server" Text='<%# Eval("VendorProductID") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="ItemQuantityLabel" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                        </td>
                        <td>
                       <asp:LinkButton ID="DeleteUnOrderedItem" runat="server" CommandName="Delete"
                             CssClass="btn" CommandArgument='<%# Eval("ItemId") %>' CausesValidation="false" OnClientClick="return confirm('Are you sure you wish to remove?')">
                            <span aria-hidden="true" class="glyphicon glyphicon-remove">&nbsp;</span>
                        </asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>

                <%-- UnOrdered Items ListView - InsertItem Template --%>
                <InsertItemTemplate>
                    <tr runat="server"> 
                        <td></td>
                        <td>
                            <asp:TextBox ID="UnOrderedItemNameTextBoxI" runat="server" Text='<%# Bind("ItemName") %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="UnOrderedVendorProdIDTextBoxI" runat="server" Text='<%# Bind("VendorProductID") %>'></asp:TextBox>
                        </td>
                        <td>
                        <asp:RegularExpressionValidator ID="UnorderedExpressionValidator" 
                           ControlToValidate="UnOrderedQuantityTextBoxI" runat="server" Display="None"
                           ErrorMessage="Unordered Unit Quantity must be a number" 
                           ValidationExpression="(^[0-9]+$|^$)"
                           ValidationGroup="UnorderedValidationGroup"></asp:RegularExpressionValidator>
                            <asp:TextBox ID="UnOrderedQuantityTextBoxI" runat="server" Text='<%# Bind("Quantity") %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:LinkButton ID="AddUnOrderedItem" runat="server"
                             CssClass="btn" CommandName="Insert" CausesValidation="true" ValidationGroup="UnorderedValidationGroup">
                            <span aria-hidden="true" class="glyphicon glyphicon-ok" >&nbsp;</span>
                        </asp:LinkButton>
                        </td>
                    </tr>
                </InsertItemTemplate>
            </asp:ListView>
        </div>
    </div>



    <%-- ODS Locations --%>
    <asp:ObjectDataSource ID="OpenOrderDDLODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="List_OpenOrder" TypeName="eRaceSystem.BLL.Receiving.OrderController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="UnOrderedItemListODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="UnOrderedItems_List" 
        TypeName="eRaceSystem.BLL.Receiving.UnOrderedItemsController" 
        DataObjectTypeName="eRaceSystem.Data.Entities.UnOrderedItem" 
        DeleteMethod="Delete_UnOrderedItems" InsertMethod="Add_UnOrderedItems"        
        OnSelected="CheckForException"
        OnDeleted="DeleteCheckForException"
        OnInserted="InsertCheckForException">
        <DeleteParameters>
            <asp:Parameter Name="ItemId" Type="Int32"></asp:Parameter>
        </DeleteParameters>
    </asp:ObjectDataSource>

</asp:Content>

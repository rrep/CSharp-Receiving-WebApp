<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="eRaceWebApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>eRace Go-Karting</h1>
        <img src="images/logo.PNG" /> 
    </div>

    <div class="row">
     
        <div class="col-md-4">
            <h2>Team Member</h2>
            <ul>
                <li>Baihao Guan</li>
                 <li>Rosemary Hofstede</li>
                 <li>Paul Duerr</li>
                 <li>Ryan Hidson</li>

            </ul>
        </div>
        <div class="col-md-4">
            <h2>Responsibility</h2>
            <ul>
                <li>Baihao Guan: Purchasing</li>
                 <li>Rosemary Hofstede: Sales</li>
                 <li>Paul Duerr: Receiving and Project Setup</li>
                 <li>Ryan Hidson: Racing and Security</li>
            </ul>
        </div>

        <div class="col-md-4">
            <h2>Known bugs</h2>
            <p>Racing</p>
            <ul>
                <li>Car ID does not databind in editview</li>
                <li>Comments and Penalties do not databind in Race Results view</li>
            </ul>
            <p>Sales
                <ul>
                    <li>
                        Refunds does not verify the category of the product to be refunded
                    </li>
                    <li>
                        Checkbox for refund charge does not accurately display.
                    </li>
                </ul>
            </p>
        </div>


    </div>

</asp:Content>

<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="eRaceWebApp.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   <div class="jumbotron">
        <h1><%: Title %></h1>
   </div>
   <div class="alert alert-primary">
        <h3>Team Member Info:</h3>
   </div>
    
   <table class="table">
  <thead class="thead-dark">
    <tr>
      <th scope="col">Name</th>
      <th scope="col">Project Responsibility</th>
      <th scope="col">Security Role</th>
      <th scope="col">User Name</th>
      <th scope="col">Password</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>Paul Duerr</td>
      <td>Receving</td>
      <td>Clerk</td>
      <td>JCalder</td>
      <td>P@$$w0rd</td>
    </tr>
    <tr>
      <td>Rosemary Hofstede </td>
      <td>Sales</td>
      <td>Clerk</td>
      <td>MSerat</td>
      <td>P@$$w0rd</td>  
    </tr>
    <tr>
      <td>Baihao Guan</td>
      <td>Purchasing</td>
      <td>Director</td>
      <td>CGetz</td>
      <td>P@$$w0rd</td>
    </tr>
    <tr>
      <td>Ryan Hidson</td>
      <td>Racing</td>
      <td>Race_Coordinator</td>
      <td>BLong</td>
      <td>P@$$w0rd</td>
    </tr>
    <tr>
      <td>Webmaster</td>
      <td>---</td>
      <td>Administrators</td>
      <td>Webmaster</td>
      <td>Pa$$w0rd</td>
    </tr>
    <tr>
      <td>Employees</td>
      <td>---</td>
      <td>Various</td>
      <td>First Initial Last Name (NSmith)</td>
      <td>Pa$$w0rd</td>
    </tr>
  </tbody>
</table>
    <div class="alert alert-primary">
        <h3>Connection String</h3>
        <p>connectionString="Data Source=localhost\.;Initial Catalog=eRace2018;Integrated Security=True"</p>
    </div>
</asp:Content>

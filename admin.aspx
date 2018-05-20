<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="admin.aspx.cs" Inherits="admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
  <h3 class="h3 mb-3 font-weight-light d-block text-center">Manage Users</h3>

  <asp:SqlDataSource
    ID="SqlDataSource1"
    runat="server"
    ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=&quot;|DataDirectory|\UsersDatabase.mdf&quot;;Integrated Security=True;Connect Timeout=30"
    ProviderName="System.Data.SqlClient"
    DataSourceMode="DataSet"
    SelectCommand="SELECT * FROM Users"
    UpdateCommand="UPDATE Users SET Password=@Password, Status=@Status WHERE Username=@Username"
    DeleteCommand="DELETE FROM Users WHERE Username=@Username"></asp:SqlDataSource>

  <asp:GridView CssClass="mx-auto w-50 table-bordered table-dark table-hover"
    ID="GridView1"
    runat="server"
    AutoGenerateColumns="False"
    DataKeyNames="Username"
    AutoGenerateEditButton="True"
    DataSourceID="SqlDataSource1"
    >
    <Columns>
      <asp:BoundField HeaderText="Username" DataField="Username" />
      <asp:BoundField HeaderText="Password" DataField="Password" />
      <asp:BoundField HeaderText="Status" DataField="Status" />
    </Columns>
  </asp:GridView>
</asp:Content>

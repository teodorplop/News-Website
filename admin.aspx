<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="admin.aspx.cs" Inherits="admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
  <asp:sqldatasource
    ID="SqlDataSource1"
    runat="server" 
    ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=&quot;D:\Workspace\Visual Studio\Web\App_Data\UsersDatabase.mdf&quot;;Integrated Security=True;Connect Timeout=30"
    ProviderName="System.Data.SqlClient"
    DataSourceMode="DataSet"
    SelectCommand="SELECT * FROM Users"
    UpdateCommand="UPDATE Users SET Password=@Password, Status=@Status WHERE Username=@Username"></asp:sqldatasource>

  <asp:GridView
    ID="GridView1"
    runat="server"
    AutoGenerateColumns="False"
    DataKeyNames="Username"
    AutoGenerateEditButton="True"
    DataSourceID="SqlDataSource1">
    <columns>
      <asp:BoundField HeaderText="Username" DataField="Username" />
      <asp:BoundField HeaderText="Password" DataField="Password" />
      <asp:BoundField HeaderText="Status" DataField="Status" />
    </columns></asp:GridView>
</asp:Content>
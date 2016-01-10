<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
  <asp:Panel ID="LoginPanel" runat="server">
    <table>
      <tr>
        <td colspan="2" style="text-align: center"><asp:Label ID="Label1" runat="server" Text="Login"></asp:Label></td>
      </tr>
      <tr>
        <td><asp:Label ID="Label2" runat="server" Text="Username"></asp:Label></td>
        <td><asp:TextBox ID="LoginUsernameTextBox" runat="server" Width="205px"></asp:TextBox></td>
      </tr>
      <tr>
        <td><asp:Label ID="Label3" runat="server" Text="Password"></asp:Label></td>
        <td><asp:TextBox ID="LoginPasswordTextBox" runat="server" Width="205px" TextMode="Password"></asp:TextBox></td>
      </tr>
      <tr>
        <td colspan="2" style="text-align: right"><asp:Button ID="LoginButton" runat="server" Text="Login" OnClick="LoginButton_Click"/></td>
      </tr>
      <tr>
        <td><asp:Label ID="Label4" runat="server" Text="Don't have an account yet?"></asp:Label></td>
        <td><asp:Button ID="RegisterButton" runat="server" Text="Register" OnClick="ShouldRegister_Click"/></td>
      </tr>
    </table>
  </asp:Panel>

  <asp:Panel ID="RegisterPanel" runat="server" Visible="false">
    <table>
      <tr>
        <td colspan="2" style="text-align: center"><asp:Label ID="Label5" runat="server" Text="Register"></asp:Label></td>
      </tr>
      <tr>
        <td><asp:Label ID="Label6" runat="server" Text="Username"></asp:Label></td>
        <td><asp:TextBox ID="RegisterUsernameTextBox" runat="server" Width="205px"></asp:TextBox></td>
      </tr>
      <tr>
        <td><asp:Label ID="Label7" runat="server" Text="Password"></asp:Label></td>
        <td><asp:TextBox ID="RegisterPasswordTextBox" runat="server" Width="205px" TextMode="Password"></asp:TextBox></td>
      </tr>
      <tr>
        <td colspan="2" style="text-align: right"><asp:Button ID="Button1" runat="server" Text="Register" OnClick="RegisterButton_Click"/></td>
      </tr>
      <tr>
        <td><asp:Label ID="Label8" runat="server" Text="Already have an account?"></asp:Label></td>
        <td><asp:Button ID="Button2" runat="server" Text="Login" OnClick="ShouldLogin_Click"/></td>
      </tr>
    </table>
  </asp:Panel>
</asp:Content>
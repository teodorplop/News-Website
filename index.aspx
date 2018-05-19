<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">

  <asp:Panel ID="SortByPanel" runat="server">
    <asp:Label ID="SortByLabel" runat="server" Text="Sort By"></asp:Label>
    <asp:DropDownList ID="SortByDropdown" runat="server">
      <asp:ListItem runat="server" Text="Date" Value="Date"></asp:ListItem>
      <asp:ListItem runat="server" Text="Title" Value="Title"></asp:ListItem>
    </asp:DropDownList>
    <asp:CheckBox ID="SortByCheckBox" runat="server" Text="Ascending" />
    <asp:Button ID="SortButton" runat="server" Text="Sort" OnClick="SortButton_Click" />
  </asp:Panel>

  <asp:Panel ID="NewsPanel" runat="server" Width="100%"></asp:Panel>
</asp:Content>

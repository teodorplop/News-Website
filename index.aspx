<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">

  <asp:Panel ID="SortByPanel" CssClass="mx-auto w-50" runat="server">
    <div class="row">
      <div class="col-2 mb-5">
        <asp:DropDownList ID="SortByDropdown" CssClass="form-control" runat="server">
          <asp:ListItem runat="server" Text="Date" Value="Date"></asp:ListItem>
          <asp:ListItem runat="server" Text="Title" Value="Title"></asp:ListItem>
        </asp:DropDownList>
      </div>
      <div class="col-2 mb-5">
        <asp:CheckBox ID="SortByCheckBox" CssClass="form-check" runat="server" Text="Ascending" />
      </div>
      <div class="col-8 mb-5">
        <asp:Button ID="SortButton" CssClass="btn btn-secondary" runat="server" Text="Sort" OnClick="SortButton_Click" />
      </div>
    </div>
  </asp:Panel>

  <asp:Panel ID="NewsPanel" CssClass="mx-auto w-75" runat="server"></asp:Panel>
</asp:Content>

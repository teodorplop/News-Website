<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="propose.aspx.cs" Inherits="propose" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
  <asp:Panel ID="EditorPanel" runat="server" CssClass="mx-auto w-75" Visible="false"></asp:Panel>

  <asp:Panel ID="UsersPanel" CssClass="mx-auto w-50" runat="server" Visible="false">
    <asp:TextBox ID="TextBoxTitle" CssClass="form-control d-block mb-1" runat="server" placeholder="Title"></asp:TextBox>
    <asp:TextBox ID="TextBoxContent" CssClass="form-control d-block mb-1" runat="server" TextMode="Multiline" placeholder="Content"></asp:TextBox>

    <div class="row">
      <div class="col-4 mb-5">
        <asp:DropDownList ID="TextBoxDomain" CssClass="form-control" runat="server"></asp:DropDownList>
      </div>

      <div class="col-8 mb-5">
        <asp:FileUpload ID="FileUploadControl" CssClass="btn btn-outline-dark float-right" runat="server"></asp:FileUpload>
      </div>

      <div class="col-6 mx-auto">
        <asp:Button ID="SubmitButton" runat="server" CssClass="btn btn-block btn-outline-success" Text="Submit" OnClick="SubmitButton_Click" />
      </div>
    </div>
  </asp:Panel>
</asp:Content>

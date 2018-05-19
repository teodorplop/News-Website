<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="publish.aspx.cs" Inherits="publish" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
  <h3 class="h3 mb-3 font-weight-light d-block text-center">Add new content</h3>

  <asp:Panel ID="PublishPanel" CssClass="mx-auto w-50" runat="server">
      <asp:textbox ID="TextBoxTitle" CssClass="form-control d-block mb-1" runat="server" placeholder="Title"></asp:textbox>
      <asp:textbox ID="TextBoxContent" CssClass="form-control d-block mb-1" runat="server" placeholder="Content" TextMode="Multiline"></asp:textbox>
      <div class="row">
        <div class="col-4 mb-5">
          <asp:DropDownList ID="TextBoxDomain" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>

        <div class="col-8 mb-5">
          <asp:FileUpload ID="FileUploadControl" CssClass="btn btn-outline-dark float-right" runat="server"></asp:FileUpload>
        </div>

        <div class="col-6 mx-auto">
          <asp:button ID="SubmitButton" CssClass="btn btn-block btn-outline-success" runat="server" text="Submit" OnClick="SubmitButton_Click" />
        </div>
      </div>
  </asp:Panel>
</asp:Content>
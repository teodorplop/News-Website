<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="propose.aspx.cs" Inherits="propose" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
  <asp:Panel ID="EditorPanel" runat="server" Width="100%" Visible="false"></asp:Panel>

  <asp:Panel ID="UsersPanel" runat="server" Width="100%" Visible="false">
      <asp:label runat="server" text="Title"></asp:label>
      <asp:textbox ID="TextBoxTitle" runat="server" Width="500px"></asp:textbox>

      <asp:label runat="server" text="Content"></asp:label>
      <asp:textbox ID="TextBoxContent" runat="server" Width="500px" TextMode="Multiline"></asp:textbox>

      <asp:DropDownList ID="TextBoxDomain" runat="server"></asp:DropDownList>
      <asp:FileUpload ID="FileUploadControl" runat="server"></asp:FileUpload>

      <asp:button ID="SubmitButton" runat="server" text="Submit" OnClick="SubmitButton_Click" />
  </asp:Panel>
</asp:Content>
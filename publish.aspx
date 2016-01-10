<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="publish.aspx.cs" Inherits="publish" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
  <h1>Add new content</h1>

  <asp:Panel ID="PublishPanel" runat="server">
  <table>
    <tr>
      <td><asp:label runat="server" text="Title"></asp:label></td>
      <td><asp:textbox ID="TextBoxTitle" runat="server" Width="500px"></asp:textbox></td>
    </tr>
    <tr>
      <td><asp:label runat="server" text="Content"></asp:label></td>
      <td><asp:textbox ID="TextBoxContent" runat="server" Width="500px" TextMode="Multiline"></asp:textbox></td>
    </tr>
    <tr>
      <td colspan="2"><asp:DropDownList ID="TextBoxDomain" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td colspan="2"><asp:FileUpload ID="FileUploadControl" runat="server"></asp:FileUpload></td>
    </tr>
    <tr>
      <td colspan="2" style="text-align: right"><asp:button ID="SubmitButton" runat="server" text="Submit" OnClick="SubmitButton_Click" /></td>
    </tr>
  </table>
  </asp:Panel>
</asp:Content>
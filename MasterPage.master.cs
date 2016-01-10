using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage {
  protected void Page_Load(object sender, EventArgs e) {
    ResponseLabel.Text = string.Empty;
    ResponseLabel.Visible = false;

    LoggedIn(SessionManager.Instance.LoggedUser);
    ManageMenu();
  }
  protected void LoginButton_Clicked(object sender, EventArgs e) {
    if (SessionManager.Instance.LoggedUser != null) {
      SessionManager.Instance.Logout();
      Response.Redirect("index.aspx");
    } else {
      Response.Redirect("login.aspx");
    }
  }
  
  private void LoggedIn(string user) {
    if (user == null) {
      LoggedInLabel.Text = string.Empty;
      LoggedInLabel.Visible = false;
      LoginButton.Text = "Login";
    } else {
      LoggedInLabel.Text = "Logged in as " + user;
      LoggedInLabel.Visible = true;
      LoginButton.Text = "Logout";
    }
  }

  private void ManageMenu() {
    MasterMenu.Items.Clear();
    MasterMenu.Orientation = Orientation.Horizontal;
    MasterMenu.BorderStyle = BorderStyle.Ridge;
    MasterMenu.BorderWidth = new Unit(3f);

    AddMenuItem("Home", "index.aspx");

    List<string> domains;
    string error;
    bool success = DomainsTable.Instance.Select(out domains, out error);

    if (success) {
      foreach (var domain in domains) {
        AddMenuItem(domain, "index.aspx?Domain=" + domain);
      }
    }

    if (SessionManager.Instance.HasPublishRights) {
      AddMenuItem("Publish", "publish.aspx");
    }
    if (SessionManager.Instance.HasProposalRights) {
      AddMenuItem("Propose", "propose.aspx");
    }
    if (SessionManager.Instance.HasAdminRights) {
      AddMenuItem("Admin", "admin.aspx");
    }
  }
  private void AddMenuItem(string text, string url) {
    MenuItem item = new MenuItem();
    item.Text = text;
    item.NavigateUrl = url;

    MasterMenu.Items.Add(item);
  }

  public void ShowError(bool success, string message) {
    ResponseLabel.ForeColor = success ? System.Drawing.Color.Green : System.Drawing.Color.Red;
    ResponseLabel.Text = message;
    ResponseLabel.Visible = true;
  }

  protected void SearchButton_Click(object sender, EventArgs e) {
    if (string.IsNullOrWhiteSpace(SearchTextBox.Text)) {
      Response.Redirect("index.aspx");
    } else {
      Response.Redirect("index.aspx?" + "Search=" + SearchTextBox.Text);
    }
  }
}

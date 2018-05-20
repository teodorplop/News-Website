using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage {
	protected void Page_Load(object sender, EventArgs e) {
		ResponsePanel.Visible = false;

		HtmlGenericControl nav = new HtmlGenericControl();
		nav.TagName = "nav";
		nav.Attributes["class"] = "navbar navbar-expand-md navbar-dark fixed-top bg-dark";
		MenuPanel.Controls.Add(nav);

		string user = SessionManager.Instance.LoggedUser;
		LoggedIn(nav, user);
		ManageMenu(nav);
		ManageSearch(nav);

		HtmlButton bt = new HtmlButton();
		bt.InnerHtml = user == null ? "Login" : "Logout";
		bt.Attributes["class"] = "btn btn-primary float-right ml-5";
		bt.ServerClick += LoginButton_Clicked;
		nav.Controls.Add(bt);
	}

	protected void LoginButton_Clicked(object sender, EventArgs e) {
		if (SessionManager.Instance.LoggedUser != null) {
			SessionManager.Instance.Logout();
			Response.Redirect("index.aspx");
		} else {
			Response.Redirect("login.aspx");
		}
	}

	private void LoggedIn(HtmlGenericControl nav, string user) {
		HtmlGenericControl p = new HtmlGenericControl();
		p.TagName = "p";
		p.InnerText = user == null ? "" : user;
		p.Attributes["class"] = "text-primary mr-3 my-auto";
		nav.Controls.Add(p);
	}

	private void ManageMenu(HtmlGenericControl nav) {
		HtmlGenericControl ul = new HtmlGenericControl();
		ul.TagName = "ul";
		ul.Attributes["class"] = "navbar-nav mr-5";
		nav.Controls.Add(ul);

		AddMenuItem(ul, "Home", "index.aspx");

		List<string> domains;
		string error;
		bool success = DomainsTable.Instance.Select(out domains, out error);

		if (success) {
			foreach (var domain in domains) {
				AddMenuItem(ul, domain, "index.aspx?Domain=" + domain);
			}
		}

		if (SessionManager.Instance.HasPublishRights) {
			AddMenuItem(ul, "Publish", "publish.aspx");
		}
		if (SessionManager.Instance.HasProposalRights) {
			AddMenuItem(ul, "Propose", "propose.aspx");
		}
		if (SessionManager.Instance.HasAdminRights) {
			AddMenuItem(ul, "Admin", "admin.aspx");
		}
	}
	private void AddMenuItem(HtmlGenericControl ul, string text, string url) {
		HtmlGenericControl li = new HtmlGenericControl();
		li.TagName = "li";
		li.Attributes["class"] = "nav-item";
		ul.Controls.Add(li);

		HtmlGenericControl a = new HtmlGenericControl();
		a.TagName = "a";
		a.Attributes["class"] = "nav-link";
		a.Attributes["href"] = url;
		a.InnerHtml = text;
		li.Controls.Add(a);
	}

	private void ManageSearch(HtmlGenericControl nav) {
		HtmlGenericControl div0 = new HtmlGenericControl();
		div0.TagName = "div";
		div0.Attributes["class"] = "input-group mr-5 ml-5";
		nav.Controls.Add(div0);

		HtmlGenericControl div1 = new HtmlGenericControl();
		div1.TagName = "div";
		div1.Attributes["class"] = "input-group-prepend";
		div0.Controls.Add(div1);

		Button searchButton = new Button();
		searchButton.CssClass = "btn btn-outline-primary";
		searchButton.Click += SearchButton_Click;
		searchButton.Text = "Search";
		div1.Controls.Add(searchButton);

		TextBox searchInput = new TextBox();
		searchInput.CssClass = "form-control float-right";
		searchInput.ID = "searchInput";
		div0.Controls.Add(searchInput);
	}

	public void ShowError(bool success, string message) {
		ResponsePanel.CssClass = "alert alert-dismissible " + (success ? "alert-success" : "alert-danger");
		ResponsePanel.Controls.Clear();
		ResponsePanel.Visible = true;

		HtmlGenericControl a = new HtmlGenericControl();
		a.TagName = "a";
		a.Attributes["href"] = "#";
		a.Attributes["class"] = "close";
		a.Attributes["data-dismiss"] = "alert";
		a.Attributes["aria-label"] = "close";
		a.InnerHtml = "&times;";

		ResponsePanel.Controls.Add(a);

		HtmlGenericControl p = new HtmlGenericControl();
		p.TagName = "p";
		p.InnerHtml = message;

		ResponsePanel.Controls.Add(p);
	}

	protected void SearchButton_Click(object sender, EventArgs e) {
		TextBox searchInput = MenuPanel.FindControl("searchInput") as TextBox;
		if (string.IsNullOrWhiteSpace(searchInput.Text)) {
			Response.Redirect("index.aspx");
		} else {
			Response.Redirect("index.aspx?" + "Search=" + searchInput.Text);
		}
	}
}

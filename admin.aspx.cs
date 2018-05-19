using System;

public partial class admin : System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
		if (!SessionManager.Instance.HasAdminRights) {
			GridView1.Visible = false;
			(Master as MasterPage).ShowError(false, "You need permission to access this page.");
		}
	}
}
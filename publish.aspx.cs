using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;

public partial class publish : System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
		if (!SessionManager.Instance.HasPublishRights) {
			PublishPanel.Visible = false;
		}

		if (!IsPostBack) {
			string error;
			List<string> domains = new List<string>();
			bool success = DomainsTable.Instance.Select(out domains, out error);

			if (success) {
				foreach (string domain in domains) {
					TextBoxDomain.Items.Add(new ListItem(domain, domain));
				}
			} else {
				(Master as MasterPage).ShowError(false, error);
			}
		}
	}

	protected void SubmitButton_Click(object sender, EventArgs e) {
		string error;
		bool success;

		if (FileUploadControl.HasFile) {
			string fileName;
			success = ManageUpload(out error, out fileName);

			if (!success) {
				(Master as MasterPage).ShowError(success, error);
				return;
			}

			success = NewsTable.Instance.Insert(out error, TextBoxTitle.Text, TextBoxContent.Text, TextBoxDomain.SelectedValue, SessionManager.Instance.LoggedUser, fileName);
		} else {
			success = NewsTable.Instance.Insert(out error, TextBoxTitle.Text, TextBoxContent.Text, TextBoxDomain.SelectedValue, SessionManager.Instance.LoggedUser);
		}

		if (success) {
			TextBoxTitle.Text = string.Empty;
			TextBoxContent.Text = string.Empty;
		}

	  (Master as MasterPage).ShowError(success, error);
	}

	private bool ManageUpload(out string error, out string fileName) {
		string fileExtension = Path.GetExtension(FileUploadControl.FileName).ToLower();
		string[] allowedExtensions = { ".png", ".jpeg", ".jpg" };
		if (!allowedExtensions.Contains(fileExtension)) {
			error = "File extension not allowed.";
			fileName = "";
			return false;
		}

		string serverPath = Server.MapPath("uploads\\");
		fileName = FileUploadControl.FileName;

		string path = Path.Combine(serverPath, fileName);
		int index = 0;
		while (File.Exists(path)) {
			++index;
			fileName = Path.GetFileNameWithoutExtension(fileName) + " (" + index.ToString() + ")" + fileExtension;
			path = Path.Combine(serverPath, fileName);
		}

		try {
			FileUploadControl.SaveAs(path);
		} catch (Exception ex) {
			error = ex.Message;
		}

		error = "File uploaded successfully!";
		return true;
	}
}
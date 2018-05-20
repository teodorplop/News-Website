using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class propose : System.Web.UI.Page {
	private void ShowEditorPage() {
		EditorPanel.Controls.Clear();
		EditorPanel.Visible = true;
		UsersPanel.Visible = false;

		string error;
		List<News> news = new List<News>();
		bool success = UsersNewsTable.Instance.Select(out news, out error, new Arguments());

		if (success) {
			HtmlGenericControl divRow = new HtmlGenericControl();
			divRow.TagName = "div";
			divRow.Attributes["class"] = "row";
			EditorPanel.Controls.Add(divRow);

			for (int i = 0; i < news.Count; ++i) {
				HtmlGenericControl divCol6 = new HtmlGenericControl();
				divCol6.TagName = "div";
				divCol6.Attributes["class"] = "col-6";
				divRow.Controls.Add(divCol6);

				Panel panel = new Panel();
				panel.CssClass = "card mx-auto mb-5 text-white bg-dark";

				if (SessionManager.Instance.HasPublishRights) {
					HtmlGenericControl headerDiv = new HtmlGenericControl();
					headerDiv.TagName = "div";
					headerDiv.Attributes["class"] = "card-header";
					panel.Controls.Add(headerDiv);

					HtmlGenericControl btnDiv = new HtmlGenericControl();
					btnDiv.TagName = "div";
					btnDiv.Attributes["class"] = "btn-group";
					headerDiv.Controls.Add(btnDiv);

					Button deleteButton = new Button();
					deleteButton.ID = news[i].ID;
					deleteButton.CssClass = "btn btn-outline-danger";
					deleteButton.Text = "Delete";
					deleteButton.Click += DeleteButton_Clicked;
					btnDiv.Controls.Add(deleteButton);

					Button publishButton = new Button();
					publishButton.ID = news[i].ID + "()";
					publishButton.CssClass = "btn btn-outline-success";
					publishButton.Text = "Publish";
					publishButton.Click += PublishButton_Clicked;
					btnDiv.Controls.Add(publishButton);
				}

				if (news[i].imageFile != null) {
					Image image = new Image();
					image.CssClass = "card-img-top mx-auto";
					image.ImageUrl = "uploads/" + news[i].imageFile;
					panel.Controls.Add(image);
				}

				HtmlGenericControl div = new HtmlGenericControl();
				div.TagName = "div";
				div.Attributes["class"] = "card-body";
				panel.Controls.Add(div);

				HtmlGenericControl title = new HtmlGenericControl("h4");
				title.Attributes["class"] = "card-title text-center";
				title.InnerHtml = news[i].title;
				div.Controls.Add(title);

				HtmlGenericControl subtitle = new HtmlGenericControl("h6");
				subtitle.Attributes["class"] = "card-subtitle mb-2 text-muted text-center";
				subtitle.InnerHtml = "by " + news[i].publisher + " at " + news[i].date;
				div.Controls.Add(subtitle);

				HtmlGenericControl content = new HtmlGenericControl("p");
				content.Attributes["class"] = "card-text";
				content.InnerHtml = news[i].content;
				div.Controls.Add(content);

				divCol6.Controls.Add(panel);
			}
		} else {
			(Master as MasterPage).ShowError(false, error);
		}
	}
	private void ShowUserPage() {
		EditorPanel.Visible = false;
		UsersPanel.Visible = true;

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

	protected void Page_Load(object sender, EventArgs e) {
		if (SessionManager.Instance.LoggedStatus == "Editor" ||
			SessionManager.Instance.LoggedStatus == "Administrator") {
			ShowEditorPage();
		} else if (SessionManager.Instance.LoggedStatus == "Normal") {
			ShowUserPage();
		}
	}

	protected void DeleteButton_Clicked(object sender, EventArgs e) {
		string error;
		bool success = UsersNewsTable.Instance.Delete(out error, (sender as Button).ID);

		(Master as MasterPage).ShowError(success, error);

		if (success) {
			Response.Redirect(Request.RawUrl);
		}
	}
	protected void PublishButton_Clicked(object sender, EventArgs e) {
		string error;
		News news;
		string buttonID = (sender as Button).ID;
		buttonID = buttonID.Remove(buttonID.Length - 2);
		bool success = UsersNewsTable.Instance.SelectByID(out error, out news, buttonID);

		if (!success) {
			(Master as MasterPage).ShowError(false, error);
			return;
		}

		success = NewsTable.Instance.Insert(out error, news.title, news.content, news.domain, news.publisher, news.imageFile);
		if (!success) {
			(Master as MasterPage).ShowError(false, error);
			return;
		}

		string error2;
		success = UsersNewsTable.Instance.Delete(out error2, buttonID);

		if (!success) {
			(Master as MasterPage).ShowError(false, error2);
			return;
		}

	  (Master as MasterPage).ShowError(true, error);
		Response.Redirect(Request.RawUrl);
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

			success = UsersNewsTable.Instance.Insert(out error, @TextBoxTitle.Text, @TextBoxContent.Text, TextBoxDomain.SelectedValue, fileName);
		} else {
			success = UsersNewsTable.Instance.Insert(out error, @TextBoxTitle.Text, @TextBoxContent.Text, TextBoxDomain.SelectedValue);
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
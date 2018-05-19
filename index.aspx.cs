using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
		Arguments arguments = new Arguments(Request);

		if (arguments.domain == null) {
			SortByPanel.Visible = false;
		}

		string error;
		List<News> news = new List<News>();
		bool success = NewsTable.Instance.Select(out news, out error, arguments);

		if (success) {
			for (int i = 0; i < news.Count; ++i) {
				Panel panel = new Panel();
				panel.BackColor = System.Drawing.Color.AntiqueWhite;
				panel.BorderStyle = BorderStyle.Groove;
				panel.BorderWidth = new Unit(5f);

				if (SessionManager.Instance.HasAdminRights) {
					Button button = new Button();
					button.ID = news[i].ID;
					button.Text = "Delete";
					button.Click += DeleteButton_Clicked;
					panel.Controls.Add(button);
				}

				HtmlGenericControl title = new HtmlGenericControl("h1");
				title.InnerHtml = news[i].title;
				panel.Controls.Add(title);

				HtmlGenericControl subtitle = new HtmlGenericControl("h5");
				subtitle.InnerHtml = "by " + news[i].publisher + " at " + news[i].date;
				panel.Controls.Add(subtitle);

				if (news[i].imageFile != null) {
					Image image = new Image();
					image.ImageUrl = "uploads/" + news[i].imageFile;
					panel.Controls.Add(image);
				}

				HtmlGenericControl content = new HtmlGenericControl("p");
				content.InnerHtml = news[i].content;
				panel.Controls.Add(content);

				if (SessionManager.Instance.HasCommentRights) {
					TextBox textBox = new TextBox();
					textBox.ID = news[i].ID + "_textBox";
					textBox.Width = Unit.Percentage(75);
					textBox.Height = Unit.Pixel(100);
					textBox.TextMode = TextBoxMode.MultiLine;
					panel.Controls.Add(textBox);
				}

				Button button2 = new Button();
				button2.ID = news[i].ID + "_comment";
				button2.Text = "Comment";
				button2.Click += CommentButton_Clicked;
				panel.Controls.Add(button2);

				HtmlGenericControl commentsTitle = new HtmlGenericControl("h3");
				commentsTitle.InnerHtml = "Comments";
				panel.Controls.Add(commentsTitle);

				List<Comment> comments;
				success = CommentsTable.Instance.Select(out comments, out error, news[i].ID);
				if (success && comments.Count > 0) {
					Panel commentsPanel = new Panel();

					foreach (var comment in comments) {
						HtmlGenericControl commentText = new HtmlGenericControl("p");
						commentText.InnerHtml = comment.text;
						commentsPanel.Controls.Add(commentText);

						HtmlGenericControl commentUser = new HtmlGenericControl("p");
						commentUser.InnerHtml = "by " + comment.username;
						commentsPanel.Controls.Add(commentUser);
					}

					panel.Controls.Add(commentsPanel);
				} else {
					(Master as MasterPage).ShowError(false, error);
				}

				NewsPanel.Controls.Add(panel);
			}
		} else {
			(Master as MasterPage).ShowError(false, error);
		}
	}

	protected void SortButton_Click(object sender, EventArgs e) {
		string domain = Request.QueryString["Domain"];
		string orderBy = SortByDropdown.SelectedValue;
		string sortOrder = SortByCheckBox.Checked ? "Asc" : "Desc";

		Response.Redirect("index.aspx?Domain=" + domain + "&OrderBy=" + orderBy + "&SortOrder=" + sortOrder);
	}
	protected void DeleteButton_Clicked(object sender, EventArgs e) {
		string newsID = (sender as Button).ID;

		string error;
		bool success = CommentsTable.Instance.Delete(out error, newsID);

		if (!success) {
			(Master as MasterPage).ShowError(false, error);
			return;
		}

		success = NewsTable.Instance.Delete(out error, newsID);
		(Master as MasterPage).ShowError(success, error);

		if (success) {
			Response.Redirect(Request.RawUrl);
		}
	}
	protected void CommentButton_Clicked(object sender, EventArgs e) {
		if (!SessionManager.Instance.HasCommentRights) {
			Response.Redirect("login.aspx");
			return;
		}


		string buttonID = (sender as Button).ID;
		string newsID = buttonID.Remove(buttonID.Length - 8);
		string textBoxID = newsID + "_textBox";

		ContentPlaceHolder placeHolder = (ContentPlaceHolder)this.Master.FindControl("body");
		TextBox textBox = placeHolder.FindControl(textBoxID) as TextBox;

		string error;
		string user = SessionManager.Instance.LoggedUser;
		string text = textBox.Text;

		bool success = CommentsTable.Instance.Insert(out error, text, user, newsID);
		(Master as MasterPage).ShowError(success, error);
		if (success) {
			Response.Redirect(Request.RawUrl);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Web.UI;
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
			HtmlGenericControl divRow = new HtmlGenericControl();
			divRow.TagName = "div";
			divRow.Attributes["class"] = "row";
			NewsPanel.Controls.Add(divRow);

			for (int i = 0; i < news.Count; ++i) {
				HtmlGenericControl divCol6 = new HtmlGenericControl();
				divCol6.TagName = "div";
				divCol6.Attributes["class"] = "col-6";
				divRow.Controls.Add(divCol6);

				Panel panel = new Panel();
				panel.CssClass = "card mx-auto mb-5 text-white bg-dark";

				if (SessionManager.Instance.HasAdminRights) {
					HtmlGenericControl headerDiv = new HtmlGenericControl();
					headerDiv.TagName = "div";
					headerDiv.Attributes["class"] = "card-header";
					panel.Controls.Add(headerDiv);

					Button button = new Button();
					button.ID = news[i].ID;
					button.CssClass = "btn btn-outline-danger";
					button.Text = "Delete";
					button.Click += DeleteButton_Clicked;
					headerDiv.Controls.Add(button);
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
				content.Attributes["class"] = "card-text mb-5";
				content.InnerHtml = news[i].content;
				div.Controls.Add(content);

				HtmlGenericControl showCommentBtn = new HtmlGenericControl();
				showCommentBtn.TagName = "a";
				showCommentBtn.Attributes["class"] = "btn btn-outline-light mb-2 btn-block";
				showCommentBtn.Attributes["data-toggle"] = "collapse";
				showCommentBtn.Attributes["href"] = "#body_Comments" + i.ToString();
				showCommentBtn.Attributes["role"] = "button";
				showCommentBtn.Attributes["aria-expanded"] = "false";
				showCommentBtn.Attributes["aria-controls"] = "body_Comments" + i.ToString();
				showCommentBtn.InnerHtml = "Comments";
				div.Controls.Add(showCommentBtn);

				HtmlGenericControl commentDiv = new HtmlGenericControl();
				commentDiv.TagName = "div";
				commentDiv.Attributes["class"] = "collapse";
				commentDiv.ID = "Comments" + i.ToString();
				div.Controls.Add(commentDiv);

				if (SessionManager.Instance.HasCommentRights) {
					HtmlGenericControl inputDiv = new HtmlGenericControl();
					inputDiv.TagName = "div";
					inputDiv.Attributes["class"] = "input-group mb-2";
					commentDiv.Controls.Add(inputDiv);

					TextBox textBox = new TextBox();
					textBox.ID = news[i].ID + "_textBox";
					textBox.CssClass = "form-control";
					textBox.TextMode = TextBoxMode.MultiLine;
					inputDiv.Controls.Add(textBox);

					HtmlGenericControl btnDiv = new HtmlGenericControl();
					btnDiv.TagName = "div";
					btnDiv.Attributes["class"] = "input-group-append";
					inputDiv.Controls.Add(btnDiv);

					Button commentButton = new Button();
					commentButton.ID = news[i].ID + "_comment";
					commentButton.CssClass = "btn btn-info";
					commentButton.Text = "Post";
					commentButton.Click += CommentButton_Clicked;
					btnDiv.Controls.Add(commentButton);
				}

				HtmlGenericControl commentsTitle = new HtmlGenericControl("h5");
				commentsTitle.Attributes["class"] = "card-title";
				commentsTitle.InnerHtml = "Comments";
				commentDiv.Controls.Add(commentsTitle);

				List<Comment> comments;
				success = CommentsTable.Instance.Select(out comments, out error, news[i].ID);
				if (success && comments.Count > 0) {
					Panel commentsPanel = new Panel();

					foreach (var comment in comments) {
						HtmlGenericControl commentText = new HtmlGenericControl("p");
						commentText.InnerHtml = "<b>" + comment.username + "</b>" + " " + comment.text;
						commentsPanel.Controls.Add(commentText);
					}

					commentDiv.Controls.Add(commentsPanel);
				} else {
					(Master as MasterPage).ShowError(false, error);
				}

				divCol6.Controls.Add(panel);
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
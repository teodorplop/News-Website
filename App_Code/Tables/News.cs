using System;

/// <summary>
/// News description class
/// </summary>
public class News {
	public string ID;
	public string title;
	public string content;
	public string date;
	public DateTime dateTime;
	public string publisher;
	public string domain;
	public string imageFile;

	public News(int ID, string title, string content, DateTime date, string publisher, string domain, string imageFile) {
		this.ID = ID.ToString();
		this.title = title;
		this.content = content;
		this.date = date.ToLocalTime().ToString();
		this.dateTime = date;
		this.publisher = publisher;
		this.domain = domain;
		this.imageFile = imageFile;
	}
}
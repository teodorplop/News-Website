using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// UsersNewsTable functions
/// </summary>
public class UsersNewsTable : TableBase<UsersNewsTable> {
	private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["NewsDatabaseConnectionString"].ConnectionString;
	private UsersNewsTable() : base(_connectionString) {
	}

	public bool Insert(out string error, string title, string content, string domain, string imageFile = null) {
		if (string.IsNullOrWhiteSpace(title)) {
			error = "Please insert a title.";
			return false;
		}
		if (string.IsNullOrWhiteSpace(content)) {
			error = "Please insert some content.";
			return false;
		}

		string publisher = SessionManager.Instance.LoggedUser;
		string commandString;
		if (imageFile != null) {
			commandString = string.Format("INSERT INTO USERSNEWS (Title, Content, Domain, Publisher, Image) VALUES('{0}', '{1}', '{2}', '{3}', '{4}')", title, content, domain, publisher, imageFile);
		} else {
			commandString = string.Format("INSERT INTO USERSNEWS (Title, Content, Domain, Publisher) VALUES('{0}', '{1}', '{2}', '{3}')", title, content, domain, publisher);
		}

		SqlCommand command = new SqlCommand(commandString, connection);

		try {
			connection.Open();
			command.ExecuteNonQuery();
		} catch (Exception ex) {
			error = ex.Message;
			return false;
		} finally {
			connection.Close();
		}

		error = "News added successfully!";
		return true;
	}

	public bool Select(out List<News> news, out string error, Arguments arguments) {
		string commandString = "SELECT ID, Title, Content, Date, Publisher, Domain, Image FROM USERSNEWS";
		if (arguments.domain != null) {
			commandString += " WHERE Domain=\'" + arguments.domain + "\'";
		}
		if (arguments.search != null) {
			commandString += string.Format(" WHERE Title LIKE \'{0}%\' or Content LIKE \'{0}%\' or Date LIKE \'{0}%\' or Publisher LIKE \'{0}%\' or Image LIKE \'{0}%\'", arguments.search);
		}
		commandString += " ORDER BY";
		if (arguments.orderBy == null) {
			commandString += " Date DESC";
		} else {
			commandString += " " + arguments.orderBy + " " + arguments.sortOrder;
		}
		SqlCommand command = new SqlCommand(commandString, connection);

		SqlDataReader reader = null;
		news = new List<News>();
		try {
			connection.Open();
			reader = command.ExecuteReader();

			if (reader.HasRows) {
				while (reader.Read()) {
					news.Add(new News(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3), reader.GetString(4), reader.GetString(5), reader.IsDBNull(6) ? null : reader.GetString(6)));
				}
			}
		} catch (Exception ex) {
			error = ex.Message;
			return false;
		} finally {
			reader.Close();
			connection.Close();
		}

		error = "News successfully selected!";
		return true;
	}

	public bool Delete(out string error, string ID) {
		string commandString = "DELETE FROM UsersNews WHERE ID=" + ID;
		SqlCommand command = new SqlCommand(commandString, connection);

		try {
			connection.Open();
			command.ExecuteNonQuery();
		} catch (Exception ex) {
			error = ex.Message;
			return false;
		} finally {
			connection.Close();
		}

		error = "News deleted successfully!";
		return true;
	}

	public bool SelectByID(out string error, out News news, string ID) {
		string commandString = "SELECT Title, Content, Publisher, Domain, Image FROM UsersNews WHERE ID=" + ID;
		SqlCommand command = new SqlCommand(commandString, connection);

		SqlDataReader reader = null;
		News fetchedNews = null;
		try {
			connection.Open();
			reader = command.ExecuteReader();

			if (reader.HasRows) {
				while (reader.Read()) {
					fetchedNews = new News(0, reader.GetString(0), reader.GetString(1), DateTime.Now, reader.GetString(2), reader.GetString(3), reader.IsDBNull(4) ? null : reader.GetString(4));
				}
			}
		} catch (Exception ex) {
			error = ex.Message;
			news = fetchedNews;
			return false;
		} finally {
			reader.Close();
			connection.Close();
		}

		error = "News fetched successfully!";
		news = fetchedNews;
		return true;
	}
}
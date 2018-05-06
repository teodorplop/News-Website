using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Responsible for interactions with the news table
/// </summary>
public class NewsTable : TableBase<NewsTable> {
  private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["NewsDatabaseConnectionString"].ConnectionString;
  private NewsTable() : base(_connectionString) {
  }

  public bool Insert(out string error, string title, string content, string domain, string publisher, string imageFile = null) {
    if (string.IsNullOrWhiteSpace(title)) {
      error = "Please insert a title.";
      return false;
    }
    if (string.IsNullOrWhiteSpace(content)) {
      error = "Please insert some content.";
      return false;
    }
    
    string commandString;
    if (imageFile != null) {
      commandString = string.Format("INSERT INTO NEWS (Title, Content, Domain, Publisher, Image) VALUES('{0}', '{1}', '{2}', '{3}', '{4}')", title, content, domain, publisher, imageFile);
    } else {
      commandString = string.Format("INSERT INTO NEWS (Title, Content, Domain, Publisher) VALUES('{0}', '{1}', '{2}', '{3}')", title, content, domain, publisher);
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
    string commandString = "SELECT ID, Title, Content, Date, Publisher, Domain, Image FROM NEWS";
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
	  if (reader != null) reader.Close();
      connection.Close();
    }

    error = "News successfully selected!";
    return true;
  }

  public bool Delete(out string error, string ID) {
    string commandString = "DELETE FROM News WHERE ID=" + ID;
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
}
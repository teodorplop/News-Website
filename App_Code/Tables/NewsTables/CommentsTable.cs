using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CommentsTable
/// </summary>
public class CommentsTable : TableBase<CommentsTable> {
  private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["NewsDatabaseConnectionString"].ConnectionString;
  private CommentsTable() : base(_connectionString) {
  }

  public bool Insert(out string error, string text, string user, string newsID) {
    if (string.IsNullOrWhiteSpace(text)) {
      error = "Please insert a content.";
      return false;
    }

    string commandString = string.Format("INSERT INTO COMMENTS (NewsID, Username, Text) VALUES('{0}', '{1}', '{2}')", newsID, user, text);

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

    error = "Comment added successfully!";
    return true;
  }

  public bool Select(out List<Comment> comments, out string error, string newsID) {
    string commandString = "SELECT Username, Text FROM COMMENTS WHERE NewsID=" + newsID;
    SqlCommand command = new SqlCommand(commandString, connection);

    SqlDataReader reader = null;
    comments = new List<Comment>();
    try {
      connection.Open();
      reader = command.ExecuteReader();

      if (reader.HasRows) {
        while (reader.Read()) {
          comments.Add(new Comment(reader.GetString(0), reader.GetString(1)));
        }
      }
    } catch (Exception ex) {
      error = ex.Message;
      return false;
    } finally {
      reader.Close();
      connection.Close();
    }

    error = "Comment successfully selected!";
    return true;
  }

  public bool Delete(out string error, string newsID) {
    string commandString = "DELETE FROM Comments WHERE NewsID=" + newsID;
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

    error = "Comments deleted successfully!";
    return true;
  }
}
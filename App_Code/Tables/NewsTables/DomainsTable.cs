using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Responsible for interactions with the domains table
/// </summary>
public class DomainsTable : TableBase<DomainsTable> {
  private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["NewsDatabaseConnectionString"].ConnectionString;
  private DomainsTable() : base(_connectionString) {
  }

  public bool Select(out List<string> news, out string error) {
    string commandString = "SELECT Name FROM DOMAINS";
    SqlCommand command = new SqlCommand(commandString, connection);

    SqlDataReader reader = null;
    news = new List<string>();
    try {
      connection.Open();
      reader = command.ExecuteReader();

      if (reader.HasRows) {
        while (reader.Read()) {
          news.Add(reader.GetString(0));
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
}
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Abstract class for tables
/// </summary>
public class TableBase<T> : Singleton<T> where T : class {
  protected SqlConnection connection;
  protected TableBase(string connectionString) {
    connection = new SqlConnection(connectionString);
  }
  ~TableBase() {
    connection = null;
  }
}
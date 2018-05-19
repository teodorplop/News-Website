using System;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Responsible for interactions with the news table
/// </summary>
public class UsersTable : TableBase<UsersTable> {
	private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["UsersDatabaseConnectionString"].ConnectionString;
	private UsersTable() : base(_connectionString) {
	}

	public bool Insert(out string error, string username, string password) {
		if (string.IsNullOrWhiteSpace(username)) {
			error = "Please insert a username.";
			return false;
		}
		if (string.IsNullOrEmpty(password)) {
			error = "Please insert a password";
			return false;
		}

		User user = null;
		bool success = SelectUser(out error, out user, username);

		if (!success) {
			return false;
		}
		if (user != null) {
			error = "User already exists.";
			return false;
		}

		string commandString = string.Format("INSERT INTO USERS (Username, Password) VALUES('{0}', '{1}')", username, password);
		SqlCommand command = new SqlCommand(commandString, connection);

		try {
			connection.Open();
			command.ExecuteNonQuery();
		} catch (Exception ex) {
			error = ex.Message;
		} finally {
			connection.Close();
		}

		error = "User created successfully!";
		return true;
	}

	public bool SelectUser(out string error, out User user, string username) {
		string commandString = "SELECT Username, Password, Status FROM USERS WHERE Username=\'" + username + "\'";
		SqlCommand command = new SqlCommand(commandString, connection);

		SqlDataReader reader = null;
		user = null;
		try {
			connection.Open();
			reader = command.ExecuteReader();

			if (reader.HasRows) {
				while (reader.Read()) {
					user = new User(reader.GetString(0), reader.GetString(1), reader.GetString(2));
				}
			}
		} catch (Exception ex) {
			error = ex.Message;
			System.Diagnostics.Debug.WriteLine(error);

		} finally {
			if (reader != null) reader.Close();
			connection.Close();
		}

		error = "User selected successfully!";
		return true;
	}
}
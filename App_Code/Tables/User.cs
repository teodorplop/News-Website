using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for User
/// </summary>
public class User {
	public string username;
	public string password;
	public string status;
	public User(string username, string password, string status) {
		this.username = username;
		this.password = password;
		this.status = status;
	}
}
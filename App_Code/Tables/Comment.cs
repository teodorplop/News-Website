using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Comment
/// </summary>
public class Comment {
  public string username;
  public string text;
  public Comment(string username, string text) {
    this.username = username;
    this.text = text;
  }
}
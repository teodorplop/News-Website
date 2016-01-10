using System.Web.SessionState;
using System.Linq;

/// <summary>
/// Summary description for SessionManager
/// </summary>
public class SessionManager : Singleton<SessionManager> {
  private HttpSessionState session {
    get {
      return System.Web.HttpContext.Current.Session;
    }
  }

  public string LoggedUser {
    get {
      object userObj = session["user"];
      if (userObj == null) {
        return null;
      }
      return userObj as string;
    }
  }
  public string LoggedStatus {
    get {
      object statusObj = session["status"];
      if (statusObj == null) {
        return null;
      }
      return statusObj as string;
    }
  }

  private string[] _proposalRights = {"Normal", "Editor", "Administrator"};
  public bool HasProposalRights {
    get {
      string status = LoggedStatus;
      return status != null && _proposalRights.Contains(status);
    }
  }
  private string[] _publishRights = {"Editor", "Administrator"};
  public bool HasPublishRights {
    get {
      string status = LoggedStatus;
      return status != null && _publishRights.Contains(status);
    }
  }
  private string[] _adminRights = {"Administrator"};
  public bool HasAdminRights {
    get {
      string status = LoggedStatus;
      return status != null && _adminRights.Contains(status);
    }
  }
  private string[] _commentRights = { "Normal", "Editor", "Administrator" };
  public bool HasCommentRights {
    get {
      string status = LoggedStatus;
      return status != null && _commentRights.Contains(status);
    }
  }

  public void Login(User user) {
    session["user"] = user.username;
    session["status"] = user.status;
  }
  public void Logout() {
    session.Clear();
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using Jenzabar.Portal.Framework;
using Jenzabar.Portal.Framework.Web.UI;
//using ActiveDs;

namespace CUS.ICS.LoginReset
{
  public partial class ChangePassword_View : PortletViewBase
  {
    protected void Page_Load(object sender, EventArgs e)
    {
        ParentPortlet.State = PortletState.Maximized;

        if (this.IsFirstLoad && ((String)(this.ParentPortlet.Session["loginUsername"])) != String.Empty)
        {
            CurrentUsername.Text = (String)(this.ParentPortlet.Session["loginUsername"]);
        }

        if (PortalUser.Current.IsMemberOf(PortalGroup.Administrators) && PortalUser.Current.Username.Equals("Administrator", StringComparison.OrdinalIgnoreCase)) //Dual check to prevent session hacking)
        {
            CurrentUsername.Enabled = true;
        }

        if (!IsPostBack)
        {
            CurrentUsername.Attributes.Add("onkeypress", "return clickButton(event,'" + ChangePasswordPushButton.ClientID + "')");
            SecurityQuestionAnswer1.Attributes.Add("onkeypress", "return clickButton(event,'" + ChangePasswordPushButton.ClientID + "')");
            SecurityQuestionAnswer2.Attributes.Add("onkeypress", "return clickButton(event,'" + ChangePasswordPushButton.ClientID + "')");
            NewPassword.Attributes.Add("onkeypress", "return clickButton(event,'" + ChangePasswordPushButton.ClientID + "')");
            ConfirmNewPassword.Attributes.Add("onkeypress", "return clickButton(event,'" + ChangePasswordPushButton.ClientID + "')");
        }

        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JenzabarConnectionString"].ConnectionString))
        {
            conn.Open();
            SqlCommand sqlcmdSelect = conn.CreateCommand();

            sqlcmdSelect.CommandText = "SELECT"
                + " QUESTION1, QUESTION2, ANSWER1, ANSWER2"
                + " FROM TLU_ACCOUNT_INFO"
                + " WHERE ID_NUM = (SELECT ID_NUM FROM LOGIN_ID_XREF WHERE LOGIN = '" + CurrentUsername.Text.Trim().Replace("'","''") + "')"
                + " AND ANSWER1 IS NOT NULL"
                + " AND	LEN(RTRIM(LTRIM(CONVERT(varchar(MAX),ANSWER1)))) > 0"
                + " AND	ANSWER2 IS NOT NULL"
                + " AND	LEN(RTRIM(LTRIM(CONVERT(varchar(MAX),ANSWER2)))) > 0";

            using (SqlDataReader sqlReader = sqlcmdSelect.ExecuteReader())
            {
                // **** Check for Security Questions ****
                if (sqlReader.HasRows)
                {
                    AnswerSecurityQuestionMessageFailure.Visible = false;
                    sqlReader.Read();
                    SecurityQuestionText1.Text = sqlReader["QUESTION1"].ToString();
                    SecurityQuestionText2.Text = sqlReader["QUESTION2"].ToString();
                }
                else
                {
                    SecurityQuestionLabel1.Visible = false;
                    SecurityQuestionText1.Visible = false;
                    SecurityQuestionAnswer1.Visible = false;
                    SecurityQuestionAnswerLabel1.Visible = false;
                    SecurityQuestionLabel2.Visible = false;
                    SecurityQuestionText2.Visible = false;
                    SecurityQuestionAnswer2.Visible = false;
                    SecurityQuestionAnswerLabel2.Visible = false;
                }
            }
        }
        if (CurrentUsername.Text != String.Empty && IslockedOut(CurrentUsername.Text))
        {
            this.ChangePasswordPushButton.Enabled = false;
            this.NewPassword.Enabled = false;
            this.ConfirmNewPassword.Enabled = false;
            this.CurrentPassword.Enabled = false;
            this.SecurityQuestionAnswer1.Enabled = false;
            this.SecurityQuestionAnswer2.Enabled = false;
            this.ParentPortlet.ShowFeedback(FeedbackType.Error, "YOUR ACCOUNT IS CURRENTLY LOCKED OUT DUE TO TOO MANY FAILED LOGIN ATTEMPTS. IT WILL UNLOCK AUTOMATICALLY IN ONE HOUR");
        }
    }

    protected Boolean IslockedOut(String userAccount)
    {
        Boolean accountIsLockedOut = false;
        DirectoryEntry uEntry;
        String userDN;
        String usr = Jenzabar.Common.Configuration.ConfigSettings.GetConfigValue("CUS_LOGINRESET", "SystemAccountName");
        String pwd = Jenzabar.Common.Configuration.ConfigSettings.GetConfigValue("CUS_LOGINRESET", "SystemAccountPassword");
        userDN = GetObjectDistinguishedName(returnType.distinguishedName, CurrentUsername.Text, "tlu.edu");
        uEntry = new DirectoryEntry(userDN, usr, pwd, AuthenticationTypes.Secure);

        if(uEntry != null)
        {
            //user is a DirectoryEntry for our user account
            string attrib = "msDS-User-Account-Control-Computed";

            //this is a constructed attrib
            uEntry.RefreshCache(new string[] { attrib });

            const int UF_LOCKOUT = 0x0010;
            int flags = (int)uEntry.Properties[attrib].Value;
            accountIsLockedOut = Convert.ToBoolean(flags & UF_LOCKOUT);
        }

        return accountIsLockedOut;
    }

    protected void ChangePasswordPushButton_Click(Object Sender, EventArgs e)
    {
        String loginUsername = String.Empty;
        String loginPassword = CurrentPassword.Text;
        String loginPasswordNew = NewPassword.Text;
        String userDN;
        String usr = Jenzabar.Common.Configuration.ConfigSettings.GetConfigValue("CUS_LOGINRESET", "SystemAccountName");
        String pwd = Jenzabar.Common.Configuration.ConfigSettings.GetConfigValue("CUS_LOGINRESET", "SystemAccountPassword");
        Boolean authenticated = false;
        Boolean passwordChanged = false;
        DirectoryEntry uEntry;

        if (PortalUser.Current.Username.Equals("guest", StringComparison.OrdinalIgnoreCase))
        {
            // User not logged in
            loginUsername = CurrentUsername.Text;
        }
        else
        {
            // We are logged in, reset the login case someone is session hacking
            loginUsername = Jenzabar.Portal.Framework.PortalUser.Current.Username;

            // If we really are the Administrator, lets use what is in the CurrentUsername Box
            if (PortalUser.Current.Username.Equals("Administrator", StringComparison.OrdinalIgnoreCase)
                && PortalUser.Current.IsMemberOf(PortalGroup.Administrators)
                && PortalUser.Current.IsSiteAdmin)
            {
                loginUsername = CurrentUsername.Text;
            }

        }

        userDN = GetObjectDistinguishedName(returnType.distinguishedName, loginUsername, "tlu.edu");
        uEntry = new DirectoryEntry(userDN, usr, pwd, AuthenticationTypes.Secure);


        authenticated = AuthenticateWithCurrentPassword(userDN, loginUsername, loginPassword)
                     || AuthenticateWithSecurityQuestions(userDN, loginUsername, loginPassword)
                     || ( PortalUser.Current.Username.Equals("Administrator", StringComparison.OrdinalIgnoreCase)
                          && PortalUser.Current.IsMemberOf(PortalGroup.Administrators)
                          && PortalUser.Current.IsSiteAdmin);

        Boolean accountIsLockedOut = IslockedOut(loginUsername);

        if (accountIsLockedOut)
        {
            this.ParentPortlet.NextScreen("Default_View");
            this.ParentPortlet.ShowFeedback(FeedbackType.Error, "YOUR ACCOUNT IS CURRENTLY LOCKED OUT DUE TO TOO MANY FAILED LOGIN ATTEMPTS. IT WILL UNLOCK AUTOMATICALLY IN ONE HOUR");
            return;
        }

        if (authenticated)
        {
            try
            {
                uEntry.Invoke("SetPassword", new object[] { NewPassword.Text });
                uEntry.CommitChanges();
                passwordChanged = AuthenticateWithCurrentPassword(userDN, loginUsername, loginPasswordNew);
            }
            catch (Exception)
            { }
        }

        if (!passwordChanged)
        {
            if (!authenticated)
            {
                this.ParentPortlet.ShowFeedbackGlobalized(FeedbackType.Error, "CUS_LOGINRESET_RESETFAILURE_AUTH");
            }
            else
            {
                this.ParentPortlet.ShowFeedbackGlobalized(FeedbackType.Error, "CUS_LOGINRESET_RESETFAILURE_COMPLEXITY");
            }
        }
        else
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText("E:\\Program Files\\Jenzabar\\ICS.NET\\Portal\\ChangePasswordPortletUsage.log");
            try
            {
                string logLine = System.String.Format("{0:G}: User [{1}] password was changed.", System.DateTime.Now, loginUsername);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }

            uEntry.Close();
            this.ParentPortlet.Session["passwordChanged"] = "True";
            this.ParentPortlet.NextScreen("Default_View", true);
        }
    }

    private enum returnType
    {
        distinguishedName, ObjectGUID
    }

    public static String sayHello(String loginUsername, String loginPassword)
    {
        const int ERROR_LOGON_FAILURE = 1326;
        const int ERROR_ACCOUNT_DISABLED = 1331;
        const int ERROR_ACCOUNT_LOCKED_OUT = 1909;  //It gives this error if the account is locked, REGARDLESS OF WHETHER VALID CREDENTIALS WERE PROVIDED!!!
        const int ERROR_ACCOUNT_EXPIRED = 1793;
        const int ERROR_PASSWORD_EXPIRED = 1330;

        int errorCode = AuthenticateWithCurrentPassword(loginUsername, loginPassword);
        string message = "";

        switch (errorCode)
        {
            case ERROR_LOGON_FAILURE:
                message = "Invalid password or username";
                break;
            case ERROR_ACCOUNT_DISABLED:
                message = "Account is disabled. Please contact the <a href='mailto:helpdesk@tlu.edu' />";
                break;
            case ERROR_ACCOUNT_LOCKED_OUT:
                message = "Invalid password or username";
                break;
            case ERROR_ACCOUNT_EXPIRED:
                message = "Your account has expired. Please conact the <a href='mailto:helpdesk@tlu.edu' />";
                break;
            case ERROR_PASSWORD_EXPIRED:
                message = "Password expired";
                break;
            default:
                break;
        }

        return message;
    }


    // Gets Error Code for failed login attempt
    public static int AuthenticateWithCurrentPassword(String loginUsername, String loginPassword)
    {
        int errorCode = 0;
        IntPtr token = new IntPtr();

        try
        {
            if (!LogonUser(loginUsername, "tlu.edu", loginPassword, LogonTypes.Network, LogonProviders.Default, out token))
            {
                errorCode = Marshal.GetLastWin32Error();
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            CloseHandle(token);
        }

        return errorCode;
    }

    private bool AuthenticateWithCurrentPassword(String userDN, String loginUsername, String loginPassword)
    {
        bool authentic = false;
        int errorCode;
        IntPtr token = new IntPtr();

        try
        {
            if (!LogonUser(loginUsername, "tlu.edu", loginPassword, LogonTypes.Network, LogonProviders.Default, out token))
            {
                errorCode = Marshal.GetLastWin32Error();

                if (errorCode == 1330)
                {
                    authentic = true;
                }
            }
            else
            {
                authentic = true;
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            CloseHandle(token);
        }

        return authentic;
    }

    #region old code
    //private bool AuthenticateWithCurrentPassword(String userDN, String loginUsername, String loginPassword)
    //{

    //    bool authentic = false;
    //    try
    //    {
    //        DirectoryEntry entry = new DirectoryEntry(userDN, loginUsername, loginPassword);
    //        object nativeObject = entry.NativeObject;
    //        authentic = true;
    //    }
    //    catch (DirectoryServicesCOMException)
    //    {
    //        //System.IO.StreamWriter sw = System.IO.File.AppendText("E:\\Program Files\\Jenzabar\\ICS.NET\\Portal\\ChangePasswordPortletUsage.log");
    //        //try
    //        //{
    //        //    string logLine = System.String.Format("{0:G}: User [{1}] FAILED TO AUTHENTICATE WITH CURRENT PASSWORD.", System.DateTime.Now, loginUsername);
    //        //    sw.WriteLine(logLine);
    //        //}
    //        //finally
    //        //{
    //        //    sw.Close();
    //        //}
    //    }

    //    return authentic;
    //}
#endregion

    private bool AuthenticateWithSecurityQuestions(String userDN, String loginUsername, String loginPassword)
    {
        bool authentic = false;
        
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JenzabarConnectionString"].ConnectionString))
        {
            conn.Open();
            SqlCommand sqlcmdSelect = conn.CreateCommand();

            sqlcmdSelect.CommandText = "SELECT"
                + " QUESTION1, QUESTION2, ANSWER1, ANSWER2"
                + " FROM TLU_ACCOUNT_INFO"
                + " WHERE ID_NUM = (SELECT ID_NUM FROM LOGIN_ID_XREF WHERE LOGIN = '" + CurrentUsername.Text.Trim().Replace("'","''") + "')"
                + " AND ANSWER1 IS NOT NULL"
                + " AND	LEN(RTRIM(LTRIM(CONVERT(varchar(MAX),ANSWER1)))) > 0"
                + " AND	ANSWER2 IS NOT NULL"
                + " AND	LEN(RTRIM(LTRIM(CONVERT(varchar(MAX),ANSWER2)))) > 0";

            using (SqlDataReader sqlReader = sqlcmdSelect.ExecuteReader())
            {
                // **** Check for Security Questions ****
                if (sqlReader.HasRows)
                {
                    AnswerSecurityQuestionMessageFailure.Visible = false;
                    sqlReader.Read();
                    authentic = SecurityQuestionAnswer1.Text.Trim().Equals(sqlReader["ANSWER1"].ToString().Trim(), StringComparison.OrdinalIgnoreCase)
                             && SecurityQuestionAnswer2.Text.Trim().Equals(sqlReader["ANSWER2"].ToString().Trim(), StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        return authentic;
    }

    protected void CancelPushButton_Click(object sender, EventArgs e)
    {
        this.ParentPortlet.PreviousScreen("Default_View");
    }


    private String GetObjectDistinguishedName(returnType returnValue, string objectName, string LdapDomain)
    {
        string distinguishedName = string.Empty;
        string connectionPrefix = "LDAP://" + LdapDomain;
        DirectoryEntry entry = new DirectoryEntry(connectionPrefix);
        DirectorySearcher mySearcher = new DirectorySearcher(entry);
        SearchResult result = null;

        mySearcher.Filter = "(&(objectClass=user)(|(cn=" + objectName + ")(sAMAccountName=" + objectName + ")))";

        if (objectName != null || objectName != String.Empty)
        {
            result = mySearcher.FindOne();
        }

        if (result != null)
        {
            DirectoryEntry directoryObject = result.GetDirectoryEntry();

            if (returnValue.Equals(returnType.distinguishedName))
            {
                distinguishedName = "LDAP://" + directoryObject.Properties
                    ["distinguishedName"].Value;
            }
            if (returnValue.Equals(returnType.ObjectGUID))
            {
                distinguishedName = directoryObject.Guid.ToString();
            }
        }
        entry.Close();
        entry.Dispose();
        mySearcher.Dispose();
        return distinguishedName;
    }

    enum LogonTypes : uint
    {
        Interactive = 2,
        Network = 3,
        Batch = 4,
        Service = 5,
        Unlock = 7,
        NetworkCleartext = 8,
        NewCredentials = 9
    }

    enum LogonProviders : uint
    {
        Default = 0, // default for platform (use this!)
        WinNT35,     // sends smoke signals to authority
        WinNT40,     // uses NTLM
        WinNT50      // negotiates Kerb or NTLM
    }
      
    [DllImport("advapi32.dll", SetLastError = true)]
    static extern bool LogonUser(
        string principal,
        string authority,
        string password,
        LogonTypes logonType,
        LogonProviders logonProvider,
        out IntPtr token);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool CloseHandle(IntPtr handle);
  }

}

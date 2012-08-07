using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CUS.ICS.LoginReset
{
  public partial class EditQuestions_View : Jenzabar.Portal.Framework.Web.UI.PortletViewBase
  {
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsFirstLoad)
        {
            return;
        }

        if (Jenzabar.Portal.Framework.PortalUser.Current.HostID != null && Jenzabar.Portal.Framework.PortalUser.Current.HostID != String.Empty)
        {
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JenzabarConnectionString"].ConnectionString))
            {
                conn.Open();
                System.Data.SqlClient.SqlCommand sqlcmdSelect = conn.CreateCommand();

                sqlcmdSelect.CommandText = "SELECT"
                    + " QUESTION1, QUESTION2, ANSWER1, ANSWER2"
                    + " FROM TLU_ACCOUNT_INFO"
                    + " WHERE ID_NUM = " + Jenzabar.Portal.Framework.PortalUser.Current.HostID.Trim().Replace("'","''");

                using (System.Data.SqlClient.SqlDataReader sqlReader = sqlcmdSelect.ExecuteReader())
                {
                    // **** Check for Security Questions ****
                    if (sqlReader.HasRows)
                    {
                        sqlReader.Read();
                        SecurityQuestionText1.Text = sqlReader["QUESTION1"].ToString();
                        SecurityQuestionText2.Text = sqlReader["QUESTION2"].ToString();
                    }
                }
            }
        }
        else
        {
            this.ParentPortlet.ShowFeedback(Jenzabar.Portal.Framework.Web.UI.FeedbackType.Error, "User Must have a valid ID#");
            SavePushButton.Enabled = false;
        }
    }

        protected void CancelPushButton_Click(object sender, EventArgs e)
        {
            this.ParentPortlet.PreviousScreen();
        }

        protected void SavePushButton_Click(object sender, EventArgs e)
        {
            // don't do anything if a question is filled out and an answers isn't
            if (  ( SecurityQuestionAnswer1.Text.Trim().Equals(String.Empty) || SecurityQuestionAnswer2.Text.Trim().Equals(String.Empty)
                || SecurityQuestionText1.Text.Trim().Equals(String.Empty) || SecurityQuestionText2.Text.Trim().Equals(String.Empty) )
                &&
                ( !SecurityQuestionAnswer1.Text.Trim().Equals(String.Empty) || !SecurityQuestionAnswer2.Text.Trim().Equals(String.Empty)
                || !SecurityQuestionText1.Text.Trim().Equals(String.Empty) || !SecurityQuestionText2.Text.Trim().Equals(String.Empty) )  )
            {
                this.ParentPortlet.ShowFeedback(Jenzabar.Portal.Framework.Web.UI.FeedbackType.Error, "You must provide two questions and answers");
                return;
            }

            Boolean tluAccountInfoRecordExists = false;
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["JenzabarConnectionString"].ConnectionString))
            {
                conn.Open();
                System.Data.SqlClient.SqlCommand sqlcmdInsertUpdate = conn.CreateCommand();
                System.Data.SqlClient.SqlCommand sqlcmdSelect = conn.CreateCommand();

                // **** Check for Security Questions ****
                sqlcmdSelect.CommandText = "SELECT *"
                    + " FROM TLU_ACCOUNT_INFO"
                    + " WHERE ID_NUM = " + Jenzabar.Portal.Framework.PortalUser.Current.HostID.Trim().Replace("'","''");

                using (System.Data.SqlClient.SqlDataReader sqlReader = sqlcmdSelect.ExecuteReader())
                {
                    tluAccountInfoRecordExists = sqlReader.HasRows;
                }
                /// *************************************
 
                if (tluAccountInfoRecordExists)
                {
                    sqlcmdInsertUpdate.CommandText = "UPDATE TLU_ACCOUNT_INFO SET"
                    + " QUESTION1 = " + "'" + SecurityQuestionText1.Text.Trim().Replace("'", "''") + "'"
                    + ", QUESTION2 = " + "'" + SecurityQuestionText2.Text.Trim().Replace("'", "''") + "'"
                    + ", ANSWER1 = " + "'" + SecurityQuestionAnswer1.Text.Trim().Replace("'", "''") + "'"
                    + ", ANSWER2 = " + "'" + SecurityQuestionAnswer2.Text.Trim().Replace("'", "''") + "'"
                    + ", USER_NAME = 'jaramirez',JOB_NAME = 'JICS_Portlet_LoginReset', JOB_TIME = GETDATE() WHERE ID_NUM = " + Jenzabar.Portal.Framework.PortalUser.Current.HostID.Trim().Replace("'","''");
                }
                else
                {
                    sqlcmdInsertUpdate.CommandText = "INSERT INTO TLU_ACCOUNT_INFO (ID_NUM, username, QUESTION1, QUESTION2, ANSWER1, ANSWER2, CREATION_DTE, USER_NAME,JOB_NAME,JOB_TIME)"
                        + " VALUES (" + Jenzabar.Portal.Framework.PortalUser.Current.HostID.Trim().Replace("'","''")
                        + ", " + "'" + Jenzabar.Portal.Framework.PortalUser.Current.Username.Replace("'", "''") + "'"
                        + ", " + "'" + SecurityQuestionText1.Text.Trim().Replace("'", "''") + "'"
                        + ", " + "'" + SecurityQuestionText2.Text.Trim().Replace("'", "''") + "'"
                        + ", " + "'" + SecurityQuestionAnswer1.Text.Trim().Replace("'", "''") + "'"
                        + ", " + "'" + SecurityQuestionAnswer2.Text.Trim().Replace("'", "''") + "'"
                        + ", GETDATE(),'jaramirez','JICS_Portlet_LoginReset',GETDATE())";
                }

                Boolean securityQuestionsUpdated = !(sqlcmdInsertUpdate.ExecuteNonQuery().Equals(0));

                if (securityQuestionsUpdated)
                {
                    this.ParentPortlet.Session["securityQuestionsUpdated"] = securityQuestionsUpdated.ToString();

                    System.IO.StreamWriter sw = System.IO.File.AppendText("E:\\Program Files\\Jenzabar\\ICS.NET\\Portal\\ChangePasswordPortletUsage.log");
                    try
                    {
                        string logLine = System.String.Format("{0:G}: User [{1}] Questions updated.", System.DateTime.Now, Jenzabar.Portal.Framework.PortalUser.Current.Username);
                        sw.WriteLine(logLine);
                    }
                    finally
                    {
                        sw.Close();
                    }

                    this.ParentPortlet.NextScreen("Default_View", true);
                }
                else
                {
                    this.ParentPortlet.ShowFeedback(Jenzabar.Portal.Framework.Web.UI.FeedbackType.Error, "Failed to update security questions");
                }
            }
        }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CUS.ICS.LoginReset
{
    public partial class Default_View : Jenzabar.Portal.Framework.Web.UI.PortletViewBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (((String)this.ParentPortlet.Session["passwordChanged"]) != null && ((String)this.ParentPortlet.Session["passwordChanged"]).Equals(Boolean.TrueString))
            {
                    this.ParentPortlet.ShowFeedbackGlobalized(Jenzabar.Portal.Framework.Web.UI.FeedbackType.Message, "CUS_LOGINRESET_PASSWORDCHANGED_MESSAGE");
                    this.ParentPortlet.Session.Remove("passwordChanged");
            }

            if (((String)this.ParentPortlet.Session["securityQuestionsUpdated"]) != null && ((String)this.ParentPortlet.Session["securityQuestionsUpdated"]).Equals(Boolean.TrueString))
            {
                this.ParentPortlet.ShowFeedback(Jenzabar.Portal.Framework.Web.UI.FeedbackType.Message, "Questions Updated");
                this.ParentPortlet.Session.Remove("securityQuestionsUpdated");
            }

            if (ParentPortlet.AccessCheck("CANADMIN"))
            {
                divAdminLink.Visible = true;
            }

            if (!Jenzabar.Portal.Framework.PortalUser.Current.Username.Equals("guest", StringComparison.OrdinalIgnoreCase))
            {
                CurrentUsername.Text = Jenzabar.Portal.Framework.PortalUser.Current.Username; // Really just for Admins use on next screen
                CurrentUsername.Visible = false;
                CurrentUsernameLabel.Visible = false;
                CurrentUsernameRequired.Enabled = false;
            }
        }

        protected void glnkAdmin_Click(object sender, EventArgs e)
        {
            this.ParentPortlet.NextScreen("Admin_View");
        }

        protected void ContinuePushButton_Click(object sender, EventArgs e)
        {
            this.ParentPortlet.Session["loginUsername"] = CurrentUsername.Text;
            this.ParentPortlet.NextScreen("ChangePassword_View");
        }

        protected void EditSecurityQuestions_Click(object sender, EventArgs e)
        {
            // Recheck login status
            if (Jenzabar.Portal.Framework.PortalUser.Current.Username.Equals("guest", StringComparison.OrdinalIgnoreCase))
            {
                this.ParentPortlet.ShowFeedback(Jenzabar.Portal.Framework.Web.UI.FeedbackType.Error, "You must login to edit your security questions");
            }
            else
            {
                this.ParentPortlet.NextScreen("EditQuestions_View");
            }
        }
    }
}
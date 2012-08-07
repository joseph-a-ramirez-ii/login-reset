using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jenzabar.Portal.Framework.Security.Authorization;

namespace CUS.ICS.LoginReset
{
  [PortletOperation(
    "CANACCESS",
    "Can Access Portlet",
    "Whether a user can access this portlet or not",
    Jenzabar.Portal.Framework.Security.Authorization.PortletOperationScope.Global)]

  [PortletOperation(
    "CANADMIN",
    "Can Admin Portlet",
    "Whether a user can admin this portlet or not",
    Jenzabar.Portal.Framework.Security.Authorization.PortletOperationScope.Global)]

  public class LoginReset : Jenzabar.Portal.Framework.Web.UI.SecuredPortletBase
  {
    public LoginReset()
    { }

    protected override Jenzabar.Portal.Framework.Web.UI.PortletViewBase GetCurrentScreen()
    {
        Jenzabar.Portal.Framework.Web.UI.PortletViewBase screen = null;

        switch (this.CurrentPortletScreenName)
        {
            case "ChangePassword_View":
                screen = this.LoadPortletView("ICS/LoginResetPortlet/" + this.CurrentPortletScreenName + ".ascx");
                break;
            case "EditQuestions_View":
                screen = this.LoadPortletView("ICS/LoginResetPortlet/" + this.CurrentPortletScreenName + ".ascx");
                break;
            case "Admin_View":
                screen = this.LoadPortletView("ICS/LoginResetPortlet/" + this.CurrentPortletScreenName + ".ascx");
                break;
            case "Default_View":
            default:
                screen = this.LoadPortletView("ICS/LoginResetPortlet/Default_View.ascx");
                break;

            //try
            //{
            //    screen = this.LoadPortletView("ICS/LoginResetPortlet/" + this.CurrentPortletScreenName + ".ascx");
            //}
            //catch (Exception)
            //{
            //    screen = this.LoadPortletView("ICS/LoginResetPortlet/Default_View.ascx");
            //}
        }

        return screen;
    }
  }
}

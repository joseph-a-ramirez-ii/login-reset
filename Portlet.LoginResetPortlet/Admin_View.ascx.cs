using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jenzabar.Portal.Framework.Web.UI;

namespace CUS.ICS.LoginReset
{
    public partial class Admin_View : PortletViewBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsFirstLoad)
            {
                SqlDataSource sqlDataSource = new SqlDataSource(System.Configuration.ConfigurationManager
                            .ConnectionStrings["ICSConnectionString"]
                            .ConnectionString, "SELECT * FROM FWK_GLOBALIZATION WHERE TEXT_KEY like 'CUS_LOGINRESET_%'");
                rpt1.DataSource = sqlDataSource;
                rpt1.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rpt1.Items)
            {
                Jenzabar.Common.Globalization.Globalizer.UpdateCustomValue(
                    ((Jenzabar.Common.Web.UI.Controls.Subheader)item.FindControl("lblText")).Text,
                    ((Jenzabar.Common.Web.UI.Controls.TextBoxEditor)item.FindControl("txtText")).InnerHtml.Replace("'","''"));
                this.ParentPortlet.PreviousScreen();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            this.ParentPortlet.PreviousScreen();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_footer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            svn.Controls.Add(LoadControl("svn.ascx"));
        }
    }
}
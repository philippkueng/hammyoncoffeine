using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_login : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            loginUser.Click += new EventHandler(loginUser_Click);
            error.Visible = false;
        }

        void loginUser_Click(object sender, EventArgs e)
        {
            //error.InnerText = checkBox.Checked.ToString();

            if (Membership.ValidateUser(userName.Value, passWord.Value))
            {
                FormsAuthentication.SetAuthCookie(userName.Value, checkBox.Checked);
                Response.Redirect("Default.aspx?c=content");
            }
            error.Visible = true;
            error.InnerText = "Leider sind Ihre Anmeldedaten ungültig. Versuchen Sie es doch noch einmal.";
        }
    }
}
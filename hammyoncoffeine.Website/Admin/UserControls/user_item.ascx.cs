using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_user_item : System.Web.UI.UserControl
    {
        public string[] Data = { "Username", "username@domain.com", "079 / 385 53 78", "http://pkstudio.ch/cms/admin/gravatar_default.jpg" };
        //public string UserName = "Username";
        //public string EmailAddress = "username@domain.com";
        //public string PhoneNumber = "079 / 385 53 78";
        //public string defaultImagePath = "http://pkstudio.ch/cms/admin/gravatar_default.jpg";
        protected void Page_Load(object sender, EventArgs e)
        {
            delete.Click += new EventHandler(delete_Click);
            message.Visible = false;
            Gravatar1.Email = Data[1];
            Gravatar1.DefaultImage = Data[3];
        }

        void delete_Click(object sender, EventArgs e)
        {
            if (Membership.DeleteUser(Data[0]))
            {
                box.Visible = false;
                message.Visible = true;
                message.InnerHtml = "<span class='success'>Benutzer <strong>" + Data[0] + "</strong> wurde gelöscht.</span>";
            }
            //throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_navigation : System.Web.UI.UserControl
    {
        //public string Test = "";
        public string[] active = { "<li", "<li", "<li", "<li", "<li", "<li" };
        public string userLink = "<a href='Default.aspx'>Anmelden</a>";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.User.Identity.IsAuthenticated)
            {
                userLink = "<a href='Default.aspx?c=logout'>Abmelden</a>";
            }

            string categoryName = Page.Request.QueryString["c"];
            switch (categoryName)
            {
                case "content":
                    active[0] = @"<li id=""active""";
                    break;
                case "user":
                    active[1] = @"<li id=""active""";
                    break;
                case "files":
                    active[2] = @"<li id=""active""";
                    break;
                case "properties":
                    active[3] = @"<li id=""active""";
                    break;
                case "myaccount":
                    active[4] = @"<li id=""active""";
                    break;
                default:
                    goto case "content";
            }
        }
        //private void makeActive(string elementContent)
        //{
        //    foreach (Control c in navigation.Controls)
        //    {
        //        Test += c.GetType().ToString() + "<hr/>";
        //        if (c.GetType().Equals("System.Web.UI.HtmlControls.HtmlGenericControl"))
        //        {
        //            if (((HtmlGenericControl)c).InnerText == elementContent)
        //            {
        //                ((HtmlGenericControl)c).Attributes.Add("id", "active");
        //            }
        //        }

        //    }
        //}
    }

}
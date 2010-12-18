using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{
    public partial class email_template : System.Web.UI.UserControl
    {
        public string[] User = { "Titel", "Content" };
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                XDocument data = DataIO.LoadData;
                var myArray = from item in User.Take(1)
                              select new
                              {
                                  Title = User[0],
                                  Content = User[1]
                              };
                ListView1.DataSource = myArray;
                ListView1.DataBind();

            }
            catch
            {
                Website_Helpers.sendError(e.ToString());
            }

        }
    }
}

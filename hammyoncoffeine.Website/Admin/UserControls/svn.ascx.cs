using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_svn : System.Web.UI.UserControl
    {
        public string Revision = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Revision = (string)HttpContext.Current.Cache["svn"];
                if (Revision == null || Revision == "")
                {
                    string svnRev;
                    using (StreamReader sr = new StreamReader(Server.MapPath("~/svn_rev.txt")))
                    {
                        svnRev = sr.ReadLine();
                    }
                    Revision = "Revision: " + svnRev.Replace(@"""", "");
                    CacheDependency cd = new CacheDependency(Server.MapPath("~/svn_rev.txt"));
                    HttpContext.Current.Cache.Insert("svn", Revision, cd);
                }
            }
            catch
            {
                Revision = "Die SVN Datei wurde nicht gefunden / konnte nicht gelesen werden";
            }

        }


    }
}
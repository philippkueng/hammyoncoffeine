using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using hammyoncoffeine.Core;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_folders_add : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            addFolder.Click += new EventHandler(addFolder_Click);
        }

        void addFolder_Click(object sender, EventArgs e)
        {
            string converted_folder_name = convert_to_filename(folder_name.Text.ToLower());
            string folder_path = Page.Request.QueryString["f"];

            string path = "";

            if (string.IsNullOrEmpty(folder_path))
                path = DataIO.PagesDirectory + converted_folder_name;
            else
                path = DataIO.PagesDirectory + folder_path + "/" + converted_folder_name;


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                message.InnerText = "Leider existiert schon ein Ordner mit diesem Namen.";
            }
        }

        public bool doesPageExists(string folderPath, string pageName)
        {
            if(string.IsNullOrEmpty(folderPath))
                return File.Exists(DataIO.PagesDirectory + pageName + ".htm");
            else
                return File.Exists(DataIO.PagesDirectory + folderPath + "/" + pageName + ".htm");
        }
        private string convert_to_filename(string pt)
        {
            pt = Regex.Replace(pt, "ä", "ae");
            pt = Regex.Replace(pt, "ö", "oe");
            pt = Regex.Replace(pt, "ü", "ue");
            pt = Regex.Replace(pt, "\\+", "und");
            pt = Regex.Replace(pt, "\\W", "-");
            return pt;
        }
    }
}
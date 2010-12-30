using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.IO;

namespace hammyoncoffeine.Website.Modules
{

    public partial class navigation : System.Web.UI.UserControl
    {
        public object[] Data;
        public string content;
        private string folder;
        private string page;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Data[0] => pageName
            // Data[1] => XElement (elementContent)
            //content = ((XElement)Data[1]).Value.ToString();

            XElement module_data = ((XElement)Data[1]);
            folder = Page.Request.QueryString["f"];
            page = Page.Request.QueryString["p"];

            if (string.IsNullOrEmpty(folder))
                folder = "/";

            content += nav_for_folder(folder.Split('/'), int.Parse(module_data.Element("nav_depth").Value.ToString()));
        }
        public string nav_for_folder(string[] folder, int folder_number)
        {
            DirectoryInfo dir;
            #region generate DirectoryInfo for current iteration
            string path = generate_path_from_array(folder, folder.Length - folder_number - 1);
            if (folder.Length > 1)
                dir = new DirectoryInfo(hammyoncoffeine.Core.DataIO.VirtualLocation + "pages/" + path);
            else
            {
                dir = new DirectoryInfo(hammyoncoffeine.Core.DataIO.VirtualLocation + "pages");
            }
            #endregion


            List<string> files_and_folders = new List<string>();
            #region add files and folders from the current directory to list
            foreach (FileInfo file in dir.GetFiles("*.htm"))
            {
                files_and_folders.Add(file.Name);
            }
            foreach (DirectoryInfo directory in dir.GetDirectories())
            {
                files_and_folders.Add(directory.Name);
            }
            #endregion

            string output = "<ul class='" + dir.Name + " folder'>";
            foreach (string item in files_and_folders.OrderBy(a=>a))
            {
                #region item is a page
                if (item.ToLower().Contains(".htm"))
                {
                    // item is not default.htm
                    if (!item.ToLower().Equals("default.htm"))
                    {
                        if(item.ToLower().Equals(page.ToLower() + ".htm"))
                            output += "<li class='active " + item.Replace(".htm", "") + " page'><a href='/" + path + item.Replace(".htm", ".aspx") + "'>" + item.Replace(".htm", "") + "</a></li>";
                        else
                            output += "<li class='" + item.Replace(".htm", "") + " page'><a href='/" + path + item.Replace(".htm", ".aspx") + "'>" + item.Replace(".htm", "") + "</a></li>";
                    }
                }
                #endregion
                #region item is a directory
                else
                {
                    // check if folder has a default file, if not do not list it
                    if (dir.GetDirectories(item, SearchOption.TopDirectoryOnly).Single().GetFiles("default.htm", SearchOption.TopDirectoryOnly).Any())
                    {
                        // the folder item is the active one
                        if (folder_number > 0 && item.ToLower().Equals(folder[folder.Length - folder_number].ToLower()) && (folder_number <= 1))
                            output += "<li class='active " + item + " folder'><a href='/" + path + item + "/default.aspx'>" + item + "</a></li>";
                        // the folder item is a non active one
                        else
                            output += "<li class='" + item + " folder'><a href='/" + path + item + "/default.aspx'>" + item + "</a></li>";

                        // do this recursively for subfolders
                        if (folder_number > 0 && item.ToLower().Equals(folder[folder.Length - folder_number].ToLower()))
                            output += nav_for_folder(folder, folder_number - 1);
                    }
                }
                #endregion
            }
            output += "</ul>";
            return output;
        }

        private string generate_path_from_array(string[] folder, int folder_number)
        {
            string result = "";
            for (int i = 1; (i < folder_number+1) && (i < folder.Length+1); i++)
            {
                result += folder[i] + "/";
            }
            return result;
        }
        

    }
}
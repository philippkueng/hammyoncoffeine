using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.IO;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_files_directory : System.Web.UI.UserControl
    {
        public string DirectoryHeader = "";
        public string DirectoryHeaderHelperString = "";
        public bool DirectoryHeaderIsNotCreated = true;
        public string Folders = "";
        public string Files = "";
        public string Message = "";
        public XElement Data = new XElement("test", "bla");


        protected void Page_Load(object sender, EventArgs e)
        {
            messageUL.Visible = false;
            string currentPath = Page.Request.QueryString["p"];

            if (!string.IsNullOrEmpty(currentPath))
            {
                deleteFolder.Click += new EventHandler(deleteFolder_Click);
                string[] foldersArray = currentPath.Split('/');
                deleteFolder.Text = "Ordner " + foldersArray[foldersArray.Length - 1] + " löschen";

                DirectoryHeader = "<a href='" + Helpers.AbsoluteWebsiteRoot + "admin/default.aspx?c=files'>Dateien</a> > ";
                foreach (var folder in getCurrentFolder(Data, foldersArray, 0).Elements("folder").OrderBy(t => t.Attribute("name").Value))
                {
                    Folders += "<li><a href='" + generateAnchorRef(folder.Attribute("name").Value, false) + "'>" + folder.Attribute("name").Value.ToString() + "</a></li>";
                }
                DirectoryHeaderIsNotCreated = false;
                foreach (var file in getCurrentFolder(Data, foldersArray, 0).Elements("file").OrderBy(t => t.Attribute("name").Value))
                {
                    Files += "<li><a href='" + generateAnchorRef(file.Attribute("name").Value, true) + "'>" + file.Attribute("name").Value.ToString() + "</a></li>";
                }
                //generateHeaderLinks();
            }
            else // Es wird der root Ordner Inhalt angezeigt
            {
                controlsPlaceHolder.Visible = false;
                foreach (var folder in Data.Elements("folder").OrderBy(t => t.Attribute("name").Value))
                {
                    Folders += "<li><a href='" + generateAnchorRef(folder.Attribute("name").Value, false) + "'>" + folder.Attribute("name").Value.ToString() + "</a></li>";
                }
                foreach (var file in Data.Elements("file").OrderBy(t => t.Attribute("name").Value))
                {
                    Files += "<li><a href='" + generateAnchorRef(file.Attribute("name").Value, true) + "'>" + file.Attribute("name").Value.ToString() + "</a></li>";
                }
                DirectoryHeader = "Dateien";
            }
            if (Folders == null || Folders == "")
                foldersUL.Visible = false;
            if (Files == null || Files == "")
                filesUL.Visible = false;
            if ((Folders == null || Folders == "") && (Files == null || Files == ""))
            {
                filesUL.Visible = false;
                foldersUL.Visible = false;
                messageUL.Visible = true;
                Message = "<li><strong>Dieser Ordner ist leer.</strong> <br/>Benutzen Sie die Boxen rechts, um Dateien hochzuladen oder Ordner anzulegen.</li>";
            }
        }

        void deleteFolder_Click(object sender, EventArgs e)
        {
            string path = Page.Request.QueryString["p"];
            if (!string.IsNullOrEmpty(path)) // Der Pfad schein gültig zu sein...
            {
                if (!(path.Substring(0, 1).Equals("/")))
                    path = "/" + path;
                if (!(path.Substring(path.Length - 1).Equals("/")))
                    path += "/";
                //Directory.CreateDirectory(DataIO.DataIO.VirtualLocation + "/files" + path + folderName.Value);
                Directory.Delete(DataIO.VirtualLocation + "/files" + path, true);
                Response.Redirect("~/admin/Default.aspx?c=files" + newPath(path));
            }
            //Directory.Delete(HttpContext.Current.Server.MapPath("~/App_Data/files/" + Page.Request.QueryString["p"]));
            //Response.Redirect(Request.Url.AbsoluteUri);
        }
        public XElement getCurrentFolder(XElement element, string[] foldersArray, int counter)
        {
            if (foldersArray[counter] != null && foldersArray[counter] != "")
            {
                foreach (var item in element.Elements("folder"))
                {
                    if (item.Attribute("name").Value.ToLower().Equals(foldersArray[counter].ToLower()))
                    {
                        if ((foldersArray.Length - counter - 1) > 0)
                        {
                            if (DirectoryHeaderIsNotCreated)
                            {
                                DirectoryHeader += "<a href='" + Helpers.AbsoluteWebsiteRoot + "admin/default.aspx?c=files&p=" + DirectoryHeaderHelperString + item.Attribute("name").Value + "&f=false'>" + item.Attribute("name").Value + "</a> > ";
                                DirectoryHeaderHelperString += item.Attribute("name").Value.ToString() + "/";
                            }
                            return getCurrentFolder(item, foldersArray, (counter + 1));
                        }
                        else
                        {
                            if (DirectoryHeaderIsNotCreated)
                            {
                                DirectoryHeader += "<a href='" + Helpers.AbsoluteWebsiteRoot + "admin/default.aspx?c=files&p=" + DirectoryHeaderHelperString + item.Attribute("name").Value + "&f=false'>" + item.Attribute("name").Value + "</a>";
                                DirectoryHeaderHelperString += item.Attribute("name").Value.ToString() + "/";
                            }
                            return item;
                        }
                    }
                }
            }
            return element;
        }
        public string generateAnchorRef(string itemToLink, bool isFile)
        {
            string currentPath = Page.Request.QueryString["p"];
            if (currentPath != null && currentPath != "")
            {
                if (!(currentPath.Substring(currentPath.Length - 1).Equals("/")))
                {
                    currentPath += "/";
                }
            }
            if (isFile)
            {
                return Helpers.AbsoluteWebsiteRoot + "admin/Default.aspx?c=files&p=" + currentPath + itemToLink + "&f=true";
            }
            return Helpers.AbsoluteWebsiteRoot + "admin/Default.aspx?c=files&p=" + currentPath + itemToLink + "&f=false";
        }
        public string newPath(string currentPath)
        {
            string path = currentPath;
            if (path[path.Length - 1] == '/') // Last / will be removed
            {
                path = path.Remove(path.Length - 1);
            }
            if (path[0] == '/') // First / will be removed
            {
                path = path.Remove(0, 1);
            }
            if (path.Contains("/"))
            {
                return "&p=" + path.Substring(0, path.LastIndexOf("/")) + "&f=false";
            }
            else
            {
                return "";
            }
        }
        //public void generateHeaderLinks()
        //{
        //    string currentPath = Page.Request.QueryString["p"];
        //    if (currentPath != null && currentPath != "")
        //    {
        //        DirectoryHeader = "<a href=>Alle Dateien</a> > ";
        //        string[] foldersArray = currentPath.Split('/');

        //    }
        //    else
        //        DirectoryHeader = "Dateien";
        //}
        //public XElement generateHeaderLinksIterator(XElement element, string[] foldersArray, int counter)
        //{
        //    if (foldersArray[counter] != null && foldersArray[counter] != "")
        //    {
        //    }
        //    else
        //    {

        //        return element;
        //    }
        //}
    }
}
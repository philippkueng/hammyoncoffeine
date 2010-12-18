using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_files_detail : System.Web.UI.UserControl
    {
        public string DirectoryHeader = "";
        public string DirectoryHeaderHelperString = "";
        public bool DirectoryHeaderIsNotCreated = true;
        public XElement Data = new XElement("test", "bla");
        public string Message = "Error";
        public string DownloadFile = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentPath = Page.Request.QueryString["p"];
            if (currentPath != null && currentPath != "")
            {
                string[] foldersArray = currentPath.Split('/');
                // Der letzte muss leer sein... damit getCurrentFile nicht crasht...
                string fileName = foldersArray[foldersArray.Length - 1];
                foldersArray[foldersArray.Length - 1] = "";
                DirectoryHeader = "<a href='" + Helpers.AbsoluteWebsiteRoot + "admin/default.aspx?c=files'>Dateien</a> > ";
                foreach (var file in getCurrentFile(Data, foldersArray, 0).Elements("file").OrderBy(t => t.Attribute("name").Value))
                {
                    if (file.Attribute("name").Value.ToLower().Equals(fileName.ToLower()))
                    {
                        // Link im DirectoryHeader hinzufügen
                        DirectoryHeader += "<a href='" + Helpers.AbsoluteWebsiteRoot + "admin/default.aspx?c=files&p=" + DirectoryHeaderHelperString + file.Attribute("name").Value + "&f=true'>" + file.Attribute("name").Value + "</a>";
                        DownloadFile = "<a href='" + Helpers.AbsoluteWebsiteRoot + "file.axd?file=" + DirectoryHeaderHelperString + file.Attribute("name").Value + "'>" + file.Attribute("name").Value.ToString() + "</a>";

                        fileLink.Value = DownloadFile;
                        DownloadFile = "Herunterladen " + DownloadFile;

                        if (Website_Helpers.isImage(file.Attribute("name").Value.ToLower()))
                            fileEmbed.InnerText = "<img src='" + Helpers.AbsoluteWebsiteRoot + "image.axd?image=" + DirectoryHeaderHelperString + file.Attribute("name").Value + "' alt='" + file.Attribute("name").Value.ToString() + "'/>";
                        else
                            fileEmbed.InnerText = "Leider steht für diesen Dateityp die Einbetten Option noch nicht zur verfügung";
                    }
                }
            }
            else
            {
                // Hier muss die Seite weitergeleitet werden...
            }
        }
        public XElement getCurrentFile(XElement element, string[] foldersArray, int counter)
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
                            return getCurrentFile(item, foldersArray, (counter + 1));
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
    }
}
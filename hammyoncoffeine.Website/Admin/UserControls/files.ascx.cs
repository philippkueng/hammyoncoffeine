using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using System.Web.Security;
using System.Reflection;
using System.IO;
using System.Web.Caching;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_files : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            file_message.Visible = false;
            folder_message.Visible = false;
            upload.Click += new EventHandler(upload_Click);
            createFolder.Click += new EventHandler(createFolder_Click);

            // Der Pfad wird im QueryString angegeben Default.aspx?c=files&p=test/bla/root/info.txt
            string path = Page.Request.QueryString["p"];
            if (!string.IsNullOrEmpty(path))
            {
                if (Page.Request.QueryString["f"].ToLower().Equals("true"))
                {
                    // TODO

                    Page pageHolder = new Page();
                    UserControl viewControl = (UserControl)pageHolder.LoadControl("~/Admin/UserControls/files_detail.ascx");

                    Type viewControlType = viewControl.GetType();
                    FieldInfo field = viewControlType.GetField("Data");
                    field.SetValue(viewControl, DataIO.loadFilesandFolders().Element("root"));

                    filesPlaceHolder.Controls.Add(viewControl);
                }
                else
                {
                    Page pageHolder = new Page();
                    UserControl viewControl = (UserControl)pageHolder.LoadControl("~/Admin/UserControls/files_directory.ascx");

                    Type viewControlType = viewControl.GetType();
                    FieldInfo field = viewControlType.GetField("Data");
                    field.SetValue(viewControl, DataIO.loadFilesandFolders().Element("root"));

                    filesPlaceHolder.Controls.Add(viewControl);
                }
            }
            else // Hier wird das Root Verzeichnis angezeigt
            {
                Page pageHolder = new Page();
                UserControl viewControl = (UserControl)pageHolder.LoadControl("~/Admin/UserControls/files_directory.ascx");

                Type viewControlType = viewControl.GetType();
                FieldInfo field = viewControlType.GetField("Data");
                field.SetValue(viewControl, DataIO.loadFilesandFolders().Element("root"));

                filesPlaceHolder.Controls.Add(viewControl);
            }
        }



        void upload_Click(object sender, EventArgs e)
        {
            file_message.Visible = true;

            string path = Page.Request.QueryString["p"];
            if (!string.IsNullOrEmpty(path)) // Der Pfad schein gültig zu sein...
            {
                if (!(path.Substring(0, 1).Equals("/"))) // Slash am Anfang
                    path = "/" + path;
                if (!(path.Substring(path.Length - 1).Equals("/"))) // Slash am Ende
                    path += "/";
                // Prüfen ob der Ordner überhaupt existiert, falls nicht, wird die Datei gar nicht hochgeladen...


            }
            else
                path = "";

            filePath.SaveAs(DataIO.VirtualLocation + "files/" + path + filePath.FileName.ToString());
            HttpContext.Current.Cache.Remove("files");
            //file_message.InnerText = "Die Datei " + filePath.FileName.ToString() + " wurde raufgeladen.";
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        void createFolder_Click(object sender, EventArgs e)
        {
            // aktuelles Verzeichnis laden, Ordner erstellen, Seite neu laden...
            string path = Page.Request.QueryString["p"];
            if (!string.IsNullOrEmpty(path)) // Der Pfad schein gültig zu sein...
            {
                if (!(path.Substring(0, 1).Equals("/")))
                    path = "/" + path;
                if (!(path.Substring(path.Length - 1).Equals("/")))
                    path += "/";
                Directory.CreateDirectory(DataIO.VirtualLocation + "/files" + path + folderName.Value);

                //folder_message.Visible = true;
                //folder_message.InnerText = "Ordner erstellt.";    
            }
            else
            {
                Directory.CreateDirectory(DataIO.VirtualLocation + "/files/" + folderName.Value);
            }
            HttpContext.Current.Cache.Remove("files");
            Response.Redirect(Request.Url.AbsoluteUri);
        }

    }
}
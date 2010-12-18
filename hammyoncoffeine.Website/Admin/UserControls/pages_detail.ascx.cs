using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_pages_detail : System.Web.UI.UserControl
    {
        public string pageTitle = "";
        public string pageDetailsTitle = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            save.Click += new EventHandler(save_Click);
            pageTitle = Page.Request.QueryString["p"];
            if (!string.IsNullOrEmpty(pageTitle))
            {
                pageDetailsTitle = "Details der Seite " + pageTitle;
                deletePage.Text = "Seite " + pageTitle + " löschen";
                deletePage.Click += new EventHandler(deletePage_Click);
                try
                {
                    using (StreamReader mySR = new StreamReader(DataIO.PagesDirectory + pageTitle.ToLower() + ".htm"))
                    {
                        pageSourceCode.Text = mySR.ReadToEnd();
                    }
                    try
                    {
                        pageTitleElement.Value = pageSourceCode.Text.Substring(pageSourceCode.Text.IndexOf("<title>") + 7, pageSourceCode.Text.IndexOf("</title>") - pageSourceCode.Text.IndexOf("<title>") - 7);
                    }
                    catch
                    {
                        pageTitleElement.Value = "Bitte definieren Sie im Quellcode ein <title> Element";
                    }
                }
                catch
                {
                    pageSourceCode.Text = "Es ist ein Fehler aufgetreten beim auslesen des Quellcodes, bitte aktualisieren Sie die Seite";
                }
            }
            else
            {
                pageDetailsTitle = "Leider trat ein Fehler beim lesen des Seitetitels auf. Bitte aktualisieren Sie.";
                pageSourceCode.Text = "Leider trat ein Fehler beim lesen des Seitetitels auf. Bitte aktualisieren Sie.";
            }

        }

        void deletePage_Click(object sender, EventArgs e)
        {
            File.Delete(DataIO.PagesDirectory + Page.Request.QueryString["p"] + ".htm");
            Response.Redirect("~/Admin/Default.aspx?c=content");
        }

        void save_Click(object sender, EventArgs e)
        {
            // Hier wird alles gespeichert...
            try
            {
                string siteContent = pageSourceCode.Text;
                if (siteContent.Contains("<title>"))
                {
                    siteContent = siteContent.Substring(0, siteContent.IndexOf("<title>") + 7) + pageTitleElement.Value.ToString() + siteContent.Substring(siteContent.IndexOf("</title>"), siteContent.Length - siteContent.IndexOf("</title>"));
                }
                else
                {
                    siteContent = siteContent.Substring(0, siteContent.IndexOf("</head>") - 1) + "<title>" + pageTitleElement.Value.ToString() + "</title>" + siteContent.Substring(siteContent.IndexOf("</head>"));
                }
                using (StreamWriter mySW = new StreamWriter(DataIO.PagesDirectory + pageTitle.ToString() + ".htm"))
                {
                    mySW.Write(siteContent);
                }
                message.InnerText = "Der neue Quellcode wurde erfolgreich gespeichert.";
            }
            catch (Exception ex)
            {
                message.InnerText = "Beim schreiben des Quellcodes in die Datei ist ein Fehler aufgetreten. " + ex.Message.ToString();
            }

            //message.InnerText = "Momentan können leider noch keine Änderungen gespeichert werden.";
        }
    }
}
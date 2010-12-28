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
                    if (string.IsNullOrEmpty(Page.Request.QueryString["f"]))
                    {
                        using (StreamReader mySR = new StreamReader(DataIO.PagesDirectory + pageTitle.ToLower() + ".htm", System.Text.Encoding.UTF8))
                        {
                            pageSourceCode.Text = mySR.ReadToEnd();
                        }
                    }
                    else
                    {
                        using (StreamReader mySR = new StreamReader(DataIO.PagesDirectory + Page.Request.QueryString["f"] + "/" + pageTitle.ToLower() + ".htm", System.Text.Encoding.UTF8))
                        {
                            pageSourceCode.Text = mySR.ReadToEnd();
                        }
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
            // save the changed page to disk
            try
            {
                string siteContent = pageSourceCode.Text;
                if (string.IsNullOrEmpty(Page.Request.QueryString["f"]))
                {
                    using (StreamWriter mySW = new StreamWriter(DataIO.PagesDirectory + pageTitle.ToString() + ".htm", false, System.Text.Encoding.UTF8))
                    {
                        mySW.Write(siteContent);
                    }
                }
                else
                {
                    using (StreamWriter mySW = new StreamWriter(DataIO.PagesDirectory + Page.Request.QueryString["f"] + "/" + pageTitle.ToString() + ".htm" ,false, System.Text.Encoding.UTF8))
                    {
                        mySW.Write(siteContent);
                    }
                }
                message.InnerText = "Der neue Quellcode wurde erfolgreich gespeichert.";
            }
            catch (Exception ex)
            {
                message.InnerText = "Beim schreiben des Quellcodes in die Datei ist ein Fehler aufgetreten. " + ex.Message.ToString();
            }
        }
    }
}
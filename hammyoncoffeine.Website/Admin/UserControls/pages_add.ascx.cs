using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_pages_add : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            addPage.Click += new EventHandler(addPage_Click);
            scPlaceHolder.Visible = false;
            fuPlaceHolder.Visible = false;
            makeHidden();
            pageAddOptions.Items.Clear();
            pageAddOptions.Items.Add("Bitte Option wählen");
            pageAddOptions.Items.Add("Quellcode einfügen");
            pageAddOptions.Items.Add("HTML Datei hochladen");
            
            pageAddOptions.SelectedIndexChanged += new EventHandler(pageAddOptions_SelectedIndexChanged);
        }
        void pageAddOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PlaceHolder1.Controls.Clear();
            if (pageAddOptions.SelectedIndex == 1)
            {
                // Es wird Quellcode eingefügt...
                scPlaceHolder.Visible = true;
                makeVisible();
            }
            else if (pageAddOptions.SelectedIndex == 2)
            {
                // Es wird eine HTML Datei hochgeladen...
                fuPlaceHolder.Visible = true;
                makeVisible();
            }
            else if (pageAddOptions.SelectedIndex == 0)
            {
                makeHidden();
            }
        }

        public void makeVisible()
        {
            addPage.Visible = true;
        }
        public void makeHidden()
        {
            addPage.Visible = false;
            message.Visible = false;
        }

        void addPage_Click(object sender, EventArgs e)
        {
            if (pageAddOptions.SelectedIndex == 1) // Quellcode
            {
                try
                {
                    if (!doesPageExists(pageName.Value.ToString()))
                    {
                        using (StreamWriter mySW = new StreamWriter(DataIO.PagesDirectory + pageName.Value))
                        {
                            mySW.Write(pageSourceCode.Text);
                        }
                        Response.Redirect(Request.Url.AbsoluteUri);
                    }
                    else
                    {
                        message.Visible = true;
                        message.InnerText = "Die Datei existiert schon.";
                    }
                }
                catch
                {
                    message.Visible = true;
                    message.InnerText = "Leider ist ein Fehler beim Speichern aufgetreten.";
                }
            }
            else if (pageAddOptions.SelectedIndex == 2) // file upload
            {
                try
                {
                    if (!doesPageExists(fileUpload.FileName.ToString()))
                    {
                        fileUpload.SaveAs(DataIO.PagesDirectory + fileUpload.FileName.ToString());
                        Response.Redirect(Request.Url.AbsoluteUri);
                    }
                    else
                    {
                        message.Visible = true;
                        message.InnerText = "Die Datei existiert schon.";
                    }
                }
                catch
                {
                    message.Visible = true;
                    message.InnerText = "Es ist ein Fehler beim speichern/hochladen der Datei aufgetreten.";
                }
            }
        }
        public bool doesPageExists(string pageName)
        {
            return File.Exists(DataIO.PagesDirectory + pageName);
        }
    }
}
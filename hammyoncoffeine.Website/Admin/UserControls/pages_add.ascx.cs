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

    public partial class Admin_UserControls_pages_add : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            addPage.Click += new EventHandler(addPage_Click);
            makeHidden();
            pageAddOptions.Items.Clear();
            pageAddOptions.Items.Add("Bitte Option wählen");
            pageAddOptions.Items.Add("Seiten Template benutzen");
            pageAddOptions.Items.Add("Quellcode einfügen");
            pageAddOptions.Items.Add("HTML Datei hochladen");
            
            pageAddOptions.SelectedIndexChanged += new EventHandler(pageAddOptions_SelectedIndexChanged);
            available_page_templates.SelectedIndexChanged += new EventHandler(available_page_templates_SelectedIndexChanged);
                
        }
        void pageAddOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pageAddOptions.SelectedIndex == 1)
            {
                ptPlaceHolder.Visible = true;

                // generate get all the page templates available
                DirectoryInfo dir = new DirectoryInfo(hammyoncoffeine.Core.DataIO.VirtualLocation + "/files/page_templates");
                if (IsPostBack && available_page_templates.Items.Count <= 1)
                {
                    if (dir.Exists)
                    {
                        available_page_templates.Items.Clear();
                        available_page_templates.Items.Add("Bitte wählen Sie ein Template");
                        foreach (FileInfo pt in dir.GetFiles("*.htm"))
                        {
                            available_page_templates.Items.Add(pt.Name);
                        }   
                    }
                }

            }
            else if (pageAddOptions.SelectedIndex == 2)
            {
                // Es wird Quellcode eingefügt...
                scPlaceHolder.Visible = true;
                makeVisible();
            }
            else if (pageAddOptions.SelectedIndex == 3)
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

        void available_page_templates_SelectedIndexChanged(object sender, EventArgs e)
        {
            pt_sub_PlaceHolder.Visible = true;
            makeVisible();
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
            if (pageAddOptions.SelectedIndex == 1)
            {
                try
                {
                    // read the file
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(hammyoncoffeine.Core.DataIO.VirtualLocation + "/files/page_templates/" + available_page_templates.SelectedValue);
                    string page_content = doc.DocumentNode.WriteTo();

                    string regex_pattern = "__(title)\\,([\\w0-9]+)__";

                    // replace the keywords with the actual title value
                    page_content = Regex.Replace(page_content, regex_pattern, new MatchEvaluator(template_title_replacement));

                    // save the new page to disk into the correct folder
                    if (!doesPageExists(Page.Request.QueryString["f"], convert_to_filename(page_title.Value.ToLower())))
                    {
                        if (string.IsNullOrEmpty(Page.Request.QueryString["f"]))
                        {
                            using (StreamWriter mySW = new StreamWriter(DataIO.PagesDirectory + HttpUtility.UrlEncode(convert_to_filename(page_title.Value.ToLower())) + ".htm"))
                            {
                                mySW.Write(page_content);
                                mySW.Close();
                            }
                        }
                        else
                        {
                            using (StreamWriter mySW = new StreamWriter(DataIO.PagesDirectory + Page.Request.QueryString["f"] + "/" + HttpUtility.UrlEncode(convert_to_filename(page_title.Value.ToLower())) + ".htm"))
                            {
                                mySW.Write(page_content);
                                mySW.Close();
                            }
                        }
                        Response.Redirect(Request.Url.AbsoluteUri);
                    }
                    else
                    {
                        message.Visible = true;
                        message.InnerText = "Die Datei existiert schon.";
                    }

                }
                catch (Exception ex)
                {
                    message.Visible = true;
                    message.InnerText = "Leider ist ein Fehler beim erstellen der Seite aufgetreten.";
                    hammyoncoffeine.Core.Website_Helpers.sendError(ex);
                }
            }
            else if (pageAddOptions.SelectedIndex == 2) // Quellcode
            {
                try
                {
                    if (!doesPageExists(Page.Request.QueryString["f"], pageName.Value.ToString()))
                    {
                        if (string.IsNullOrEmpty(Page.Request.QueryString["f"]))
                        {
                            using (StreamWriter mySW = new StreamWriter(DataIO.PagesDirectory + HttpUtility.UrlEncode(convert_to_filename(pageName.Value.ToLower())) + ".htm"))
                            {
                                mySW.Write(pageSourceCode.Text);
                                mySW.Close();
                            }
                        }
                        else
                        {
                            using (StreamWriter mySW = new StreamWriter(DataIO.PagesDirectory + Page.Request.QueryString["f"] + "/" + HttpUtility.UrlEncode(convert_to_filename(pageName.Value.ToLower())) + ".htm"))
                            {
                                mySW.Write(pageSourceCode.Text);
                                mySW.Close();
                            }
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
            else if (pageAddOptions.SelectedIndex == 3) // file upload
            {
                try
                {
                    if (!doesPageExists(Page.Request.QueryString["f"], fileUpload.FileName.ToString()))
                    {
                        if(string.IsNullOrEmpty(Page.Request.QueryString["f"]))
                            fileUpload.SaveAs(DataIO.PagesDirectory + fileUpload.FileName.ToString());
                        else
                            fileUpload.SaveAs(DataIO.PagesDirectory + Page.Request.QueryString["f"] + "/" + fileUpload.FileName.ToString());
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
        public bool doesPageExists(string folderPath, string pageName)
        {
            if(string.IsNullOrEmpty(folderPath))
                return File.Exists(DataIO.PagesDirectory + pageName);
            else
                return File.Exists(DataIO.PagesDirectory + folderPath + "/" + pageName);
        }
        private string template_title_replacement(Match m)
        {
            string keyword = m.Groups[2].Value;
            if (keyword == "url")
            {
                return HttpUtility.UrlEncode(convert_to_filename(page_title.Value.ToLower()));
            }
            else
            {
                return page_title.Value;
            }
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
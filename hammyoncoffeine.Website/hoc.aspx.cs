#region using
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using hammyoncoffeine.Core;
using HtmlAgilityPack;
#endregion

namespace hammyoncoffeine.Website
{
    public partial class hoc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageName = Page.Request.QueryString["p"].ToLower();
            string folderPath = null;
            if (!string.IsNullOrEmpty(Page.Request.QueryString["f"]))
            {
                folderPath = Page.Request.QueryString["f"].ToLower();
            }

            // Load page (without module items) from cache or generate it from scratch
            HtmlDocument doc = HTMLWorker.GetPageByName(pageName, folderPath);
            if (doc == null)
                Response.Redirect("Error.aspx");

            // convert the htmldocument to a string
            string doc_html = doc.DocumentNode.WriteTo();


            // find all elements with cms attribute and fill in the correct content
            string cms_pattern = "(<cms>)([\\w\\s.-]+)(</cms>)";
            Regex re = new Regex(cms_pattern, RegexOptions.IgnoreCase);

            int currentIndex = 0;
            foreach (Match match in re.Matches(doc_html))
            {
                // get html between last cms element (or document start) and the current cms element
                ph.Controls.Add(HTMLWorker.getStandardControl(doc_html.Substring(currentIndex, match.Index - currentIndex)));
                currentIndex = match.Index + match.Length;

                try
                {
                    // add the usercontrol with the name matched
                    ph.Controls.Add(HTMLWorker.GetControl(pageName, folderPath, DataIO.loadXItem( folderPath, pageName, match.Groups[2].Value.ToLower())));
                }
                catch (Exception ex)
                {
                    ph.Controls.Add(HTMLWorker.getStandardControl(ex.Message.ToString() + "<br/>"));
                    // ERROR :: if the element is not yet in the data.xml the page will fail.
                    // Write a note to a log file, so that the admin know's this element has to be defined
                }
            }
            // add the last html chunk from the last cms element to the document end
            ph.Controls.Add(HTMLWorker.getStandardControl(doc_html.Substring(currentIndex, doc_html.Length - currentIndex)));

        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using hammyoncoffeine.Core;

/// <summary>
/// TODO::
/// generate a sitemap to be indexed faster by search engines
/// </summary>
public class SiteMap: IHttpHandler 
{
    #region IHttpHandler Members

    public bool IsReusable
    {
        get { return false; }
    }

    public void ProcessRequest(HttpContext context)
    {
        using (XmlWriter writer = XmlWriter.Create(context.Response.OutputStream))
        {
            writer.WriteStartElement("urlset", "http://www.google.com/schemas/sitemap/0.84");
            DirectoryInfo directoy = new DirectoryInfo(DataIO.PagesDirectory);
            foreach (FileInfo file in directoy.GetFiles("*.htm"))
            {                
                writer.WriteStartElement("url");
                //writer.WriteElementString("loc", page.AbsoluteLink.ToString());             
                writer.WriteElementString("loc", Helpers.AbsoluteWebsiteRoot + file.Name.Substring(0, file.Name.IndexOf(".htm")) + ".aspx");
                  
                //writer.WriteElementString("lastmod", page.DateModified.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteElementString("changefreq", "monthly");
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        context.Response.ContentType = "text/xml";
    }

    #endregion
}

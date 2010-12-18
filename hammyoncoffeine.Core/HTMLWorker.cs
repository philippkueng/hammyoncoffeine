#region using
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Xml.Linq;
using HtmlAgilityPack;
#endregion

namespace hammyoncoffeine.Core
{
    public class HTMLWorker
    {
        /// <summary>
        /// load the html from a page, parse it, cache it and return the htmldocument
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static HtmlDocument GetPageByName(string p, string f)
        {
            HtmlDocument doc;
            if(string.IsNullOrEmpty(f))
                doc = (HtmlDocument)HttpContext.Current.Cache["page_htmldocument_" + p];
            else
                doc = (HtmlDocument)HttpContext.Current.Cache["page_htmldocument_" + p + "_" + f];

            if (doc == null)
            {
                try
                {
                    doc = new HtmlDocument();

                    //hammyoncoffeine.Core.Website_Helpers.sendError("p - " + p + ":: f - " + f); 

                    if(string.IsNullOrEmpty(f))
                        doc.Load(DataIO.PagesDirectory + p + ".htm");
                    else
                        doc.Load(DataIO.PagesDirectory + f + "/" + p + ".htm");
                    
                    // format all cms correctly so that they can be extracted via regex
                    doc = CleanAndFormatCMSElements(doc);

                    // fix all relative anchor tags to work inside the site
                    doc = FixAnchorElements(doc);

                    // at this point no module items have been added, only the flat html with corrected cms tags gets cached
                    addToDiskCache(doc, p);

                    // add the corrected page to the RAMcache to decrease load time next time.
                    if (string.IsNullOrEmpty(f))
                    {
                        CacheDependency cd = new CacheDependency(DataIO.PagesDirectory + p + ".htm");
                        HttpContext.Current.Cache.Insert("page_htmldocument_" + p, doc, cd);
                    }
                    else
                    {
                        CacheDependency cd = new CacheDependency(DataIO.PagesDirectory + f + "/" + p + ".htm");
                        HttpContext.Current.Cache.Insert("page_htmldocument_" + p + "_" + f, doc, cd);
                    }
                    
                }
                catch (Exception ex)
                {
                    hammyoncoffeine.Core.Website_Helpers.sendError(ex);
                    return null;
                }
            }
            return doc;
        }

        /// <summary>
        /// make <cms name="tag" /> and <cms name="tag"></cms> look the same for easier regex parsing
        /// and remove other attributes and cms element innerhtml
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static HtmlDocument CleanAndFormatCMSElements(HtmlDocument doc)
        {
            try
            {
                foreach (HtmlNode cms in doc.DocumentNode.SelectNodes("//cms[@name]"))
                {
                    cms.InnerHtml = cms.Attributes["name"].Value.ToLower();
                    cms.Attributes.RemoveAll();
                }
            }
            catch // if document has no elements with cms attributes the foreach loop will fail
            {
            }
            return doc;
        }

        public static HtmlDocument FixAnchorElements(HtmlDocument doc)
        {
            // TODO :: implement anchor fixing
            List<string> pagesAvailable = DataIO.PagesList;
            try
            {
                foreach (HtmlNode anchor in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    // NOTE :: should only change page names from the current address at not some of other domains aswell
                    // get name of the file without the ending
                    // does this method handle http://example.com without any filename?
                    string pageName = URLRewrite.get_file_name_out_of_url(anchor.Attributes["href"].Value.ToLower())[0];

                    // check if domain linked to is the currently used domain
                    // changes anchors with url <a href="sample.htm">link</a> to <a href="sample.aspx">link</a>
                    foreach(string page in pagesAvailable)
                    {
                        if (page.ToLower() == pageName)
                        {
                            anchor.Attributes["href"].Value = anchor.Attributes["href"].Value.ToLower().Replace(".htm", ".aspx");
                        }
                    }


                    // if both are true, change the ending from .htm to .aspx
                }
            }
            catch // if the document has no anchor tags the foreach loop will fail
            {

            }
            return doc;
        }

        public static UserControl GetControl(string pageName, string folderPath, XElement element)
        {
            // TODO :: check if Control supports caching, if true, cache the element
            if (element == null)
                element = new XElement("item",
                    new XAttribute("type", "html"),
                    new XElement("content",""));

            Page pageHolder = new Page();
            UserControl viewControl = (UserControl)pageHolder.LoadControl("~/modules/" + element.Attribute("type").Value.ToString() + ".ascx");
            pageHolder.Controls.Add(viewControl);

            Type viewControlType = viewControl.GetType();
            FieldInfo field = viewControlType.GetField("Data");
            try
            {
                object[] Data = { pageName, element.Element("content"), folderPath };
                field.SetValue(viewControl, Data);
                return viewControl;
            }
            catch
            {
                object[] Data = { pageName, new XElement("content", ""), folderPath };
                field.SetValue(viewControl, Data);
                return viewControl;
            }
        }

        /// <summary>
        /// generates a UserControl with the html given
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static UserControl getStandardControl(object Data)
        {
            Page pageHolder = new Page();
            UserControl viewControl = (UserControl)pageHolder.LoadControl("~/modules/standard_html.ascx");

            Type viewControlType = viewControl.GetType();
            FieldInfo field = viewControlType.GetField("Data");
            field.SetValue(viewControl, Data);
            return viewControl;
        }

        /// <summary>
        /// gets the XElement from the data.xml file by page name and item name
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        //public static XElement GetAcordingXElementToNameTag(string pageName, string itemName)
        //{
        //    try
        //    {
        //        return DataIO.LoadData.Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(pageName)).Single().Elements("item").Where(s => s.Attribute("id").Value.ToLower().Equals(itemName.ToLower())).Single();
        //    }
        //    catch // if the element doesn't exist, return null
        //    {
        //        return null;
        //    }
        //}

        public static void addToDiskCache(HtmlDocument doc, string pageName)
        {
            // check if the cache directory exists and create it if necessary
            DirectoryInfo pageCacheDir = new DirectoryInfo(DataIO.CacheDirectory);
            if (!pageCacheDir.Exists)
                pageCacheDir.Create();

            // write the new file to disk and add it to the cache
            doc.Save(DataIO.CacheDirectory + "html_document_" + pageName + ".htm");
        }
    }
}

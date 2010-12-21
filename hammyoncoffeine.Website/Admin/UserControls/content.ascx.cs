using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Linq;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{
    public partial class Admin_UserControls_content : System.Web.UI.UserControl
    {
        public static XElement getFolder(XElement element, string[] path, int current_folder)
        {
            if (element.Name == "root" && string.IsNullOrEmpty(path[0]) && path.Length == 1)
                return element;
            else
            {
                if ((element.Name == "folder") && (element.Attribute("name").Value.ToLower() == path[current_folder].ToLower()) && (path.Length == (current_folder + 1)))
                {
                    return element;
                }
                else
                {
                    if (element.Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(path[current_folder + 1].ToLower())).Any())
                        return getFolder(element.Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(path[current_folder + 1].ToLower())).Single(), path, current_folder + 1);
                    else
                        return null;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            XDocument doc = Website_Helpers.newDoc();

            string pageName = Page.Request.QueryString["p"];

            #region folderStuff
            string folderName = Page.Request.QueryString["f"];
            XElement folders;
            if (!string.IsNullOrEmpty(folderName))
                folders = getFolder(doc.Element("root"), folderName.Split('/'), 0);
            else
                folders = doc.Element("root");
            #endregion

            #region page view
            if (!string.IsNullOrEmpty(pageName))
            {
                #region show item
                if (!string.IsNullOrEmpty(Page.Request.QueryString["i"]))
                {
                    // Das Element aus der Data.xml Datei holen
                    string itemType = null;
                    if(DataIO.loadItem(Page.Request.QueryString["f"], Page.Request.QueryString["p"], Page.Request.QueryString["i"]) != null)
                        itemType = DataIO.loadItem(Page.Request.QueryString["f"], Page.Request.QueryString["p"], Page.Request.QueryString["i"]).type;

                    if (string.IsNullOrEmpty(itemType))
                        itemType = "new";
                    Control typeControl = LoadControl("type_" + itemType + ".ascx");
                    content.Controls.Add(typeControl);

                    // load type_shared every time...
                    Control sharedControl = LoadControl("type_shared.ascx");
                    content.Controls.Add(sharedControl);
                }
                #endregion

                #region show page
                else // Es wird eine Seitenansicht angezeigt... --> Zusätlich werden hier die zusätzlichen Informationen über die Seite angezeigt...
                {
                    // contentHeader
                    HtmlGenericControl contentHeader = new HtmlGenericControl("ul");
                    contentHeader.Attributes.Add("class", "content_header");
                    contentHeader.InnerHtml = "<li class='page'>Seite</li><li class='element'>Element</li><li class='type'>Typ</li>";
                    content.Controls.Add(contentHeader);

                    //foreach (var page in doc.Element("root").Elements("page"))
                    var page = folders.Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(pageName.ToLower())).Single();
                    HtmlGenericControl content_content = new HtmlGenericControl("ul");
                    content_content.Attributes.Add("class", "content_content");

                    // pageTitle
                    HtmlGenericControl pageTitle = new HtmlGenericControl("li");
                    pageTitle.Attributes.Add("class", "page");
                    pageTitle.InnerHtml = "<a href='Default.aspx?c=content&p=" + page.Attribute("name").Value.ToString() + "&f="+folderName+"'>" + page.Attribute("name").Value.ToString() + "</a>";
                    content_content.Controls.Add(pageTitle);

                    // pageItems && typeItems
                    HtmlGenericControl pageItems_li = new HtmlGenericControl("li");
                    HtmlGenericControl pageItems_ul = new HtmlGenericControl("ul");

                    HtmlGenericControl typeItems_li = new HtmlGenericControl("li");
                    HtmlGenericControl typeItems_ul = new HtmlGenericControl("ul");

                    foreach (var item in page.Elements("item"))
                    {
                        HtmlGenericControl itemTitle = new HtmlGenericControl("li");
                        itemTitle.Attributes.Add("class", "element");
                        itemTitle.InnerHtml = "<a href='Default.aspx?c=content&p=" + page.Attribute("name").Value.ToString() + "&i=" + item.Attribute("id").Value.ToString() + "&f="+folderName+"'>" + item.Attribute("id").Value.ToString() + "</a>";
                        pageItems_ul.Controls.Add(itemTitle);
                        HtmlGenericControl typeTitle = new HtmlGenericControl("li");

                        if (DataIO.loadItem(folderName, page.Attribute("name").Value, item.Attribute("id").Value) != null)
                            typeTitle.InnerText = DataIO.loadItem(folderName, page.Attribute("name").Value, item.Attribute("id").Value).type;

                        if(string.IsNullOrEmpty(typeTitle.InnerText))
                        {
                            typeTitle.InnerText = "Noch nicht zugewiesen";
                        }
                        typeItems_ul.Controls.Add(typeTitle);
                    }

                    pageItems_li.Controls.Add(pageItems_ul);
                    typeItems_li.Controls.Add(typeItems_ul);

                    content_content.Controls.Add(pageItems_li);
                    content_content.Controls.Add(typeItems_li);

                    content.Controls.Add(content_content);

                    // Hier werden die zusätzlichen Informationen angezeigt...
                    content.Controls.Add(LoadControl("pages_detail.ascx"));
                }
                #endregion

            }
            #endregion

            #region global view
            else // Es wird eine Gesamtansicht gezeigt...
            {
                #region folderStuff
                

                if (doc.Element("root").Elements("folder").Any())
                {                  
                    #region folder title
                    HtmlGenericControl folder_title_container = new HtmlGenericControl("ul");
                    folder_title_container.Attributes.Add("class", "content_header");
                    HtmlGenericControl folder_title = new HtmlGenericControl("li");
                    folder_title.Attributes.Add("class", "folders_title");
                    folder_title.InnerText = "Ordner";
                    folder_title_container.Controls.Add(folder_title);
                    content.Controls.Add(folder_title_container);
                    #endregion

                    HtmlGenericControl folderItems_ul = new HtmlGenericControl("ul");
                    folderItems_ul.Attributes.Add("class", "content_content");
                    
                    //var folders = getFolder(doc.Element("root"), folderName.Split('/'), 0);

                    foreach (var folder in folders.Elements("folder"))
                    {
                        HtmlGenericControl folderItems_li = new HtmlGenericControl("li");
                        folderItems_li.Attributes.Add("class", "folder_icon");
                        HtmlAnchor folder_anchor = new HtmlAnchor();

                        folder_anchor.HRef = "../Default.aspx?c=content&f=" + folder.Attribute("path").Value.ToString();
                        folder_anchor.InnerText = folder.Attribute("name").Value.ToString();

                        folderItems_li.Controls.Add(folder_anchor);
                        folderItems_ul.Controls.Add(folderItems_li);
                    }
                    content.Controls.Add(folderItems_ul);


                }
                else // display something to create new folders
                {

                }
                #endregion

                #region contentHeader
                HtmlGenericControl contentHeader = new HtmlGenericControl("ul");
                contentHeader.Attributes.Add("class", "content_header");
                contentHeader.InnerHtml = "<li class='page'>Seite</li><li class='element'>Element</li><li class='type'>Typ</li>";
                content.Controls.Add(contentHeader);
                #endregion  

                content.Controls.Add(LoadControl("pages_add.ascx"));

                foreach (var page in folders.Elements("page"))
                {
                    HtmlGenericControl content_content = new HtmlGenericControl("ul");
                    content_content.Attributes.Add("class", "content_content");

                    #region pageTitle
                    HtmlGenericControl pageTitle = new HtmlGenericControl("li");
                    pageTitle.Attributes.Add("class", "page");
                    pageTitle.InnerHtml = "<a href='Default.aspx?c=content&p=" + page.Attribute("name").Value.ToString() + "&f=" + folderName + "'>" + page.Attribute("name").Value.ToString() + "</a>";
                    content_content.Controls.Add(pageTitle);
                    #endregion


                    #region pageItems && typeItems
                    HtmlGenericControl pageItems_li = new HtmlGenericControl("li");
                    HtmlGenericControl pageItems_ul = new HtmlGenericControl("ul");

                    HtmlGenericControl typeItems_li = new HtmlGenericControl("li");
                    HtmlGenericControl typeItems_ul = new HtmlGenericControl("ul");

                    foreach (var item in page.Elements("item"))
                    {
                        HtmlGenericControl itemTitle = new HtmlGenericControl("li");
                        itemTitle.Attributes.Add("class", "element");
                        itemTitle.InnerHtml = "<a href='Default.aspx?c=content&p=" + page.Attribute("name").Value.ToString() + "&i=" + item.Attribute("id").Value.ToString() + "&f=" + folderName +  "'>" + item.Attribute("id").Value.ToString() + "</a>";
                        pageItems_ul.Controls.Add(itemTitle);

                        HtmlGenericControl typeTitle = new HtmlGenericControl("li");

                        if (DataIO.loadItem(folderName, page.Attribute("name").Value, item.Attribute("id").Value) != null)
                            typeTitle.InnerText = DataIO.loadItem(folderName, page.Attribute("name").Value, item.Attribute("id").Value).type;

                        if (string.IsNullOrEmpty(typeTitle.InnerText))
                        {
                            typeTitle.InnerText = "Noch nicht zugewiesen";
                        }
                        typeItems_ul.Controls.Add(typeTitle);
                    }

                    pageItems_li.Controls.Add(pageItems_ul);
                    typeItems_li.Controls.Add(typeItems_ul);

                    content_content.Controls.Add(pageItems_li);
                    content_content.Controls.Add(typeItems_li);
                    #endregion

                    content.Controls.Add(content_content);
                }
            }
            #endregion

        }
    }
}
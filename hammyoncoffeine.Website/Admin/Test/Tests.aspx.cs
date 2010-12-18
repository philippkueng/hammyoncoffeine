using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website.Admin.Test
{
    public partial class Tests : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region check if log files are getting written to the disk
            //DataIO.addToLog("this is a sample message");
            //message.InnerHtml = "message was added to log file";
            #endregion

            #region get shared element by type, page and item name
            //string type = "html";
            //string page = "sample";
            //string item = "bla";
            //// item element mit richtigem type
            //var list1 = DataIO.LoadData.Element("root").Element("shared").Elements("item").Where(t => t.Attribute("type").Value.ToLower().Equals(type.ToLower()));

            //// item element mit richtigem page name und item element mit dem richtigen value
            //var list2 = list1.Where(s => s.Element("pages").Elements("page").Any(p => p.Attribute("name").Value.ToLower().Equals(page.ToLower()) && p.Elements("item").Any(r => r.Value.ToLower().Equals(item.ToLower()))));

            //foreach (var xitem in list2)
            //{
            //    message.InnerHtml += xitem.Attribute("id").Value.ToString() + "<BR/>";
            //}
            #endregion

            #region saveItem Test
            //string type = "html";
            //string page = "bla";
            //string item = "sample";
            //string shared_item = "testmodul2";
            //XElement content = new XElement("content", DateTime.Now.ToString());

            //if (DataIO.saveItemContent(content, page, item, type, shared_item))
            //{
            //    message.InnerHtml = "new item was saved successfully!";
            //}
            //else
            //{
            //    message.InnerHtml = "there was an error somewhere!";
            //}
            #endregion

            #region convertSingleToSharedItem Test
            //string type = "html";
            //string page = "index";
            //string item = "testmodul";
            //string shared_item = "somesample";
            //XElement content = new XElement("content", DateTime.Now.ToString());

            //if (DataIO.convertSingleToSharedItem(content, page, item, type, shared_item))
            //    message.InnerHtml = "new item was converted successfully!";
            //else
            //    message.InnerHtml = "there was an error somewhere!";
            #endregion

            #region convertSharedToSingleItem Test
            //string page = "foo";
            //string item = "sample";
            //string shared_item = "testmodul2";

            //if (DataIO.convertSharedToSingleItem(page, item, shared_item))
            //    message.InnerHtml = "the item element was converted successfully!";
            //else
            //    message.InnerHtml = "there was an error somewhere!";

            #endregion
        }
    }
}
using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{
    public partial class Admin_UserControls_type_text : System.Web.UI.UserControl
    {
        public string page = "";
        public string r_page = "";
        public string item = "";
        public string r_item = "";
        public string r_folder = "";
        public string shared_item = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            r_folder = Page.Request.QueryString["f"];
            r_page = Page.Request.QueryString["p"];
            r_item = Page.Request.QueryString["i"];

            page = "<a href='Default.aspx?c=content&p=" + r_page + "'>" + r_page + "</a>";
            item = "<a href='Default.aspx?c=content&p=" + r_page + "&i=" + r_item + "'>" + r_item + "</a>";

            shared_item = DataIO.loadItem(r_folder, r_page, r_item).shared_item;
            txtContent.Text = DataIO.loadItem(r_folder, r_page, r_item).elementContent.Value.ToString();


            //txtContent.Text = Website_Helpers.newDoc().Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(Page.Request.QueryString["p"].ToLower())).Single().Elements("item").Where(s => s.Attribute("id").Value.ToLower().Equals(Page.Request.QueryString["i"].ToLower())).Single().Value.ToString();
        }
        protected void save_OnClick(object sender, EventArgs e)
        {
            message.Visible = true;
            if (DataIO.saveItemContent(new XElement("content", txtContent.Text),r_folder, Page.Request.QueryString["p"], Page.Request.QueryString["i"], "text", shared_item))
            {
                message.InnerText = "Der Eintrag wurde erfolgreich gespeichert.";
            }
            else
            {
                message.InnerText = "Es ist ein Fehler beim speichern aufgetreten. Bitte kopieren Sie den Text per Ctrl + C und versuchen Sie es nocheinmal.";
            }
        }
    }
}
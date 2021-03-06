﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml.Linq;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{

    public partial class Admin_type_new : System.Web.UI.UserControl
    {
        public string page = "";
        public string item = "";
        public string r_page = "";
        public string r_item = "";
        public string r_folder = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            r_page = Page.Request.QueryString["p"];
            r_item = Page.Request.QueryString["i"];
            r_folder = Page.Request.QueryString["f"];


            page = "<a href='Default.aspx?c=content&f=" + r_folder + "&p=" + r_page + "'>" + r_page + "</a>";
            item = "<a href='Default.aspx?c=content&f=" + r_folder + "&p=" + r_page + "&i=" + r_item + "'>" + r_item + "</a>"; availableTypes.DataSource = getAvailableTypes();
            availableTypes.DataBind();
        }
        protected void save_OnClick(object sender, EventArgs e)
        {
            message.Visible = true;
            // evt. sollte noch geprüft werden ob noch kein Element existiert, oder dieses keinen Typ hat.
            XElement element = new XElement("item",
                new XAttribute("id", Page.Request.QueryString["i"]),
                new XAttribute("type", availableTypes.SelectedValue.ToString()));

            // Prüfen ob die Seite in data.xml schon existiert
            //if (!DataIO.LoadData.Element("root").Elements("page").Where(pageElement => pageElement.Attribute("name").Value.ToLower().Equals(Page.Request.QueryString["p"].ToLower())).Any())
            //{
            //    XElement x_page = new XElement("page",
            //        new XAttribute("name", Page.Request.QueryString["p"]));
            //    DataIO.LoadData.Element("root").Add(x_page);
            //}
            //DataIO.LoadData.Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(Page.Request.QueryString["p"].ToLower())).Single().Add(element);
            //DataIO.LoadData.Save(DataIO.StorageLocation);

            ItemContent ic = new ItemContent();
            ic.elementContent = new XElement("content", "");
            ic.folder = Page.Request.QueryString["f"];
            ic.page = r_page;
            ic.item = r_item;
            ic.type = availableTypes.SelectedValue.ToString();
            ic.shared_item = null;

            if(DataIO.saveItemContent(ic))
                message.InnerText = "Der Type wurde erfolgreich gespeichert";
            else
                message.InnerText = "Leider trat beim speichern des Typs ein Fehler auf.";
        }
        public List<string> getAvailableTypes()
        {
            List<string> types = new List<string>();
            DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/admin/usercontrols/"));
            foreach (FileInfo file in directory.GetFiles("*.ascx"))
            {
                if (file.Name.Contains("type_"))
                    if (!file.Name.Contains("type_new.ascx"))
                        types.Add(file.Name.Substring((file.Name.IndexOf("type_") + 5), (file.Name.LastIndexOf(".ascx") - (file.Name.IndexOf("type_") + 5))));
            }
            return types;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_properties : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            save.Click += new EventHandler(save_Click);
            try
            {
                websiteroot.Value = DataIO.LoadSettings.Element("root").Element("websiteroot").Value.ToString();
                defaultpage.Value = DataIO.LoadSettings.Element("root").Element("defaultpage").Value.ToString();
                foreach (var subdomain in DataIO.LoadSettings.Element("root").Element("subdomains").Elements("subdomain"))
                {
                    subdomains.Value += subdomain.Value.ToString() + " ";
                }
            }
            catch
            {
            }
            //test.InnerText = "Hi " + Page.User.Identity.Name.ToString();
        }

        void save_Click(object sender, EventArgs e)
        {
            // Hier sollten die Daten gespeichert werden.
            //XDocument doc = DataIO.DataIO.LoadSettings;

            #region websiteroot speichern
            try
            {
                DataIO.LoadSettings.Element("root").Element("websiteroot").SetValue(websiteroot.Value.ToString());
            }
            catch // Der XML Knoten kann nicht gefunden werden, oder existiert noch gar nicht, daher wird dieser neu erstellt und hinzugefügt.
            {
                XElement element = new XElement("websiteroot", websiteroot.Value.ToString());
                DataIO.LoadSettings.Element("root").Add(element);
            }
            #endregion

            #region defaultpage speichern
            try
            {
                DataIO.LoadSettings.Element("root").Element("defaultpage").SetValue(defaultpage.Value.ToString());
            }
            catch // Der XML Knoten kann nicht gefunden werden, oder existiert noch gar nicht, daher wird dieser neu erstellt und hinzugefügt.
            {
                XElement element = new XElement("defaultpage", defaultpage.Value.ToString());
                DataIO.LoadSettings.Element("root").Add(element);
            }
            #endregion

            #region subdomains speichen
            // Die verschiedenen Subdomains müssen zuerst wieder getrennt werden (ohne Regex)
            string[] subdomainsArray = subdomains.Value.ToString().Split(' ');

            // Zuerst wird die Kategorie subdomains gelöscht, um nachher von dem Textfeld wieder neu hinzugefügt zu werden.
            DataIO.LoadSettings.Element("root").Element("subdomains").Remove();

            XElement subdomainsElement = new XElement("subdomains", "");
            foreach (string subdomain in subdomainsArray)
            {
                XElement subdomainElement = new XElement("subdomain", subdomain);
                if (subdomain != "" && subdomain != null)
                    subdomainsElement.Add(subdomainElement);
            }
            DataIO.LoadSettings.Element("root").Add(subdomainsElement);
            #endregion


            DataIO.LoadSettings.Save(DataIO.SettingsLocation);

        }
    }
}
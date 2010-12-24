using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{
    public partial class Admin_type_shared : System.Web.UI.UserControl
    {
        public string folder = "";
        public string page = "";
        public string item = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            folder = Page.Request.QueryString["f"];
            page = Page.Request.QueryString["p"];
            item = Page.Request.QueryString["i"];

            if(DataIO.loadItem(folder, page, item) == null || string.IsNullOrEmpty(DataIO.loadItem(folder, page, item).shared_item))
            {
                // if element is single
                convertSharedToSingle.Visible = false;
                convertSingleToShared.Visible = true;
                linkSingleToShared.Visible = true;

                cSiTSh_Button.Click += new EventHandler(cSiTSh_Button_Click);
                lSiTSh_Button.Click += new EventHandler(lSiTSh_Button_Click);


                #region list linkable shared items
                lSiTSh_items.Items.Clear();
                if (DataIO.LoadData.Elements("root").Any() && DataIO.LoadData.Element("root").Elements("shared").Any() && DataIO.LoadData.Element("root").Element("shared").Elements("item").Any())
                {
                    foreach (var sh_item in DataIO.LoadData.Element("root").Element("shared").Elements("item"))
                    {
                        lSiTSh_items.Items.Add(sh_item.Attribute("id").Value);
                    }
                }
                else
                {
                    linkSingleToShared.Visible = false;
                    //convertSingleToShared.Visible = false;
                }
                #endregion

            }
            else
            {
                // if element is shared
                convertSingleToShared.Visible = false;
                linkSingleToShared.Visible = false;
                convertSharedToSingle.Visible = true;

                cShTSi_Button.Click += new EventHandler(cShTSi_Button_Click);
            }
       }

        void cShTSi_Button_Click(object sender, EventArgs e)
        {
            if (DataIO.convertSharedToSingleItem(folder, page, item, DataIO.loadItem(null, page, item).shared_item))
                cShTSi_Label.Text = "Das Element wurde erfolgreich entknüpft.";
            else
                cShTSi_Label.Text = "Beim entknüpfen ist ein Fehler aufgetreten.";
        }

        void lSiTSh_Button_Click(object sender, EventArgs e)
        {
            if (DataIO.linkSingleToSharedItem(folder, page, item, lSiTSh_items.SelectedItem.Value))
                Response.Redirect(Request.Url.AbsoluteUri);
            //lSiTSh_Label.Text = "Das Element wurde erfolgreich verknüpft.";
            else
                lSiTSh_Label.Text = "Leider trat beim verknüpfen ein Fehler auf.";
        }

        void cSiTSh_Button_Click(object sender, EventArgs e)
        {
            string shared_item_name = cSiTSh_Textbox.Text;
            if (string.IsNullOrEmpty(shared_item_name)) // user entered not name for the shared item
                cSiTSh_Label.Text = "Bitte Namen für Modul eingeben";
            else if (DataIO.LoadData.Element("root").Element("shared").Elements("item").Where(t => t.Attribute("id").Value.ToLower().Equals(shared_item_name.ToLower())).Any())
                cSiTSh_Label.Text = "Dieser Name ist bereits vergeben, bitte wählen Sie einen anderen.";
            else
            {
                if (DataIO.convertSingleToSharedItem(DataIO.loadItem(folder, page, item).elementContent, folder, page, item, DataIO.loadItem(folder, page, item).type, shared_item_name))
                    cSiTSh_Label.Text = "Element wurde erfolgreich umgewandelt";
                else
                    cSiTSh_Label.Text = "Leider trat beim konvertieren ein Fehler auf.";
            }
        }
    }
}
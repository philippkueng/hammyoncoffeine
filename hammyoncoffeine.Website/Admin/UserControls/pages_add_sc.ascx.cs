using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_pages_add_sc : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            message.InnerText = "Seite geladen...";
            addPage.Click += new EventHandler(addPage_Click);
        }

        void addPage_Click(object sender, EventArgs e)
        {
            // Hier werden die Daten gespeichert, und die Seite angelegt...
            message.InnerText = "Button geklickt...";
            //try
            //{
            //    using (StreamWriter mySW = new StreamWriter(Helpers.DataIO.PagesDirectory + pageName.Value))
            //    {
            //        try
            //        {
            //            mySW.Write(pageSourceCode.Text);
            //        }
            //        catch
            //        {
            //            message.InnerText = "Leider hat es nicht funktioniert";
            //        }
            //    }
            //    //Response.Redirect(Request.Url.AbsoluteUri);
            //    message.InnerText = "Die neue Seite wurde erstellt";
            //}
            //catch
            //{
            //    message.InnerText = "Leider ist ein Fehler beim Speichern aufgetreten.";
            //}
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Default : System.Web.UI.Page
{
    public string WebSite = "Falls Sie das sehen, ist leider ein Fehler aufgetreten";
    protected void Page_Load(object sender, EventArgs e)
    {
        string categoryName = Page.Request.QueryString["c"];
        container.Controls.Add(LoadControl("UserControls/logo.ascx"));
        container.Controls.Add(LoadControl("UserControls/navigation.ascx"));
        
        if (User.Identity.IsAuthenticated && categoryName != "activate")
        {
            switch (categoryName)
            {
                case "content":
                    container.Controls.Add(LoadControl("UserControls/content.ascx"));
                    break;
                case "user":
                    container.Controls.Add(LoadControl("UserControls/users.ascx"));
                    //WebSite = "Die Funktion <strong>Benutzer</strong> ist leider noch nicht implementiert";
                    break;
                case "files":
                    container.Controls.Add(LoadControl("UserControls/files.ascx"));
                    //WebSite = "Die Funktion <strong>Dateien</strong> ist leider noch nicht implementiert";
                    break;
                case "properties":
                    container.Controls.Add(LoadControl("UserControls/properties.ascx"));
                    //WebSite = "Die Funktion <strong>Einstellungen</strong> ist leider noch nicht implementiert";
                    break;
                case "myaccount":
                    container.Controls.Add(LoadControl("UserControls/myaccount.ascx"));                
                    //WebSite = "Die Funktion <strong>Mein Konto</strong> ist leider noch nicht implementiert";
                    break;
                case "logout":
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                    Response.Redirect("Default.aspx");
                    break;
                default:
                    goto case "content";
            }
        }
        else
        {
            if(categoryName == "activate")
                container.Controls.Add(LoadControl("UserControls/activate.ascx"));
            else
                container.Controls.Add(LoadControl("UserControls/login.ascx"));
        }
        if (WebSite != "Falls Sie das sehen, ist leider ein Fehler aufgetreten")
        {
            container.Controls.Clear();
            HtmlGenericControl websiteControl = new HtmlGenericControl("p");
            websiteControl.InnerHtml = WebSite;
            container.Controls.Add(websiteControl);
        }
        else
        {
            container.Controls.Add(LoadControl("UserControls/footer.ascx"));
        }
        // Neue Seite hinzufügen -> wird danach überprüft auf deren Felder -> Falls die Felder schon was drin haben wird dies gerade übernommen.

        // 1. Die Daten werden aus dem XML File geladen

        // 2. Die Daten werden visuell aufbereitet, und nachher dargestellt


    }
}

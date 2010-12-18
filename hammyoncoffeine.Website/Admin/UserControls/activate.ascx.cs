using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Xml.Linq;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_activate : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("Default.aspx?c=content");
            }
            else
            {
                save.Click += new EventHandler(save_Click);
                message.Visible = false;
            }
        }

        void save_Click(object sender, EventArgs e)
        {
            // Passwort ändern
            #region save password
            try
            {
                string tempPassword = Page.Request.QueryString["p"].ToString();
                //string oldPassword = DataIO.loadUsers().Element("Users").Elements("User").Where(t => t.Element("UserName").Value.ToString().Equals(userName.Value.ToString())).Single().Attribute("tempPassword").Value.ToString();
                if (passWord1.Value == passWord2.Value)
                {
                    MembershipUser currentUser = Membership.GetUser(userName.Value);
                    if (currentUser.ChangePassword(tempPassword, passWord1.Value.ToString()))
                    {
                        // Temporäres Passwort wieder löschen...
                        //DataIO.loadUsers().Element("Users").Elements("User").Where(t
                    }
                }
            }
            catch (Exception ex)
            {
                message.Visible = true;
                message.InnerText = "Leider ist beim ändern ihres Passworts ein Fehler aufgetreten. " + ex.Message.ToString();
            }
            #endregion
            #region save userdata (surname, name, phonenumber, etc.)
            try
            {
                XElement surnameElement = new XElement("Surname", surname.Value.ToString());
                XElement nameElement = new XElement("Name", name.Value.ToString());
                XElement phoneNumberElement = new XElement("PhoneNumber", phoneNumber.Value.ToString());
                DataIO.LoadUsers.Element("Users").Elements("User").Where(t => t.Element("UserName").Value.ToLower().Equals(userName.Value.ToLower())).Single().Add(surnameElement);
                DataIO.LoadUsers.Element("Users").Elements("User").Where(t => t.Element("UserName").Value.ToLower().Equals(userName.Value.ToLower())).Single().Add(nameElement);
                DataIO.LoadUsers.Element("Users").Elements("User").Where(t => t.Element("UserName").Value.ToLower().Equals(userName.Value.ToLower())).Single().Add(phoneNumberElement);
                DataIO.LoadUsers.Save(DataIO.UsersLocation);
                if (Membership.ValidateUser(userName.Value, passWord1.Value))
                {
                    FormsAuthentication.SetAuthCookie(userName.Value, false);
                    Response.Redirect("Default.aspx?c=content");
                }
                message.Visible = true;
                message.InnerText = "Leider ist beim Einloggen ein Fehler aufgetreten, versuchen Sie die Seite zu aktualisieren.";

            }
            catch (Exception ex)
            {
                message.Visible = true;
                message.InnerText = "Leider ist beim speichern ihrer Daten ein Fehler aufgetreten.<br/>Genauere Fehlermeldung: " + ex.Message.ToString();
            }
            #endregion
            // Überprüfen ob schon Elemente existieren (damit keine doppelten Einträge entstehen)
            // Andere Daten speichern
        }
    }
}
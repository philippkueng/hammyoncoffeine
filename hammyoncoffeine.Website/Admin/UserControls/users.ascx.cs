using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using System.Web.Security;
using System.Reflection;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{

    public partial class Admin_UserControls_users : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            message.Visible = false;
            save.Click += new EventHandler(save_Click);
            foreach (var user in DataIO.LoadUsers.Element("Users").Elements("User").OrderBy(user => user.Element("UserName").Value))
            {
                if (!(user.Element("UserName").Value.ToLower().Equals(Page.User.Identity.Name.ToLower())))
                {
                    string[] Data = { user.Element("UserName").Value, user.Element("Email").Value, "079 / 465 54 63", "http://pkstudio.ch/cms/admin/gravatar_default.jpg" };

                    Page pageHolder = new Page();
                    UserControl viewControl = (UserControl)pageHolder.LoadControl("~/Admin/UserControls/user_item.ascx");

                    Type viewControlType = viewControl.GetType();
                    FieldInfo field = viewControlType.GetField("Data");
                    field.SetValue(viewControl, Data);

                    usersPlaceHolder.Controls.Add(viewControl);
                }
            }
        }

        void save_Click(object sender, EventArgs e)
        {
            // Prüfen ob der Benutzername nicht schon existiert...
            try
            {
                // Benutzer erstellen
                string tempPassword = Guid.NewGuid().ToString();
                MembershipUser currentUser = Membership.CreateUser(userName.Value, tempPassword, emailAddress.Value);
                while (!(DataIO.LoadUsers.Element("Users").Descendants("User").Any(user => user.Element("UserName").Value.ToString().Equals(userName.Value.ToString()))))
                {
                }
                // Falls keine Elemente vorhanden sind, soll einfach nochmal gefragt werden
                DataIO.LoadUsers.Element("Users").Elements("User").Where(t => t.Element("UserName").Value.ToLower().Equals(userName.Value.ToLower())).Single().Add(new XAttribute("tempPassword", tempPassword));

                // Die Benutzerdaten speichern...
                DataIO.LoadUsers.Save(DataIO.UsersLocation);

                // Mail an denjenigen schicken, für welchen gerade ein Konto errichtet wurde
                Website_Helpers.sendMail("Konto für Sie erstellt", generateCreateUserMailContent(tempPassword), false, "admin@pkstudio.ch", emailAddress.Value);

                // Nachricht an den Admin ausgeben, dass ein Konto errichtet wurde
                message.Visible = true;
                message.InnerText = "Der Benutzer " + userName.Value + " wurde erfolgreich erstellt";
            }
            catch (Exception ex)
            {
                message.Visible = true;
                message.InnerText = ex.Message.ToString();
            }


            // Link an Benutzer senden
        }

        public string generateCreateUserMailContent(string tempPassword)
        {
            return Page.User.Identity.Name + " hat für Sie ein Konto auf " + DataIO.LoadSettings.Element("root").Element("sitename").Value.ToString() + " eingerichtet.<br/>Ihr Benutzername ist: <strong>" + userName.Value + "</strong><br/>Klicken Sie <a href='http://pkstudio.ch/cms/admin?c=activate&p=" + tempPassword + "'>hier<a> um Ihr Konto zu aktivieren.";
        }

    }

}
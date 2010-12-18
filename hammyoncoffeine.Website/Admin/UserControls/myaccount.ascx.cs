using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using System.Web.Security;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{
    public partial class Admin_UserControls_myaccount : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            save.Click += new EventHandler(save_Click);
            try
            {
                XElement user = DataIO.LoadUsers.Element("Users").Elements("User").Where(t => t.Element("UserName").Value.ToLower().Equals(Page.User.Identity.Name.ToLower())).Single();
                emailAddress.Value = getMyAccountInputValue(user, "Email");
                surname.Value = getMyAccountInputValue(user, "Surname");
                lastname.Value = getMyAccountInputValue(user, "Lastname");
                address.Value = getMyAccountInputValue(user, "Address");
                postcode.Value = getMyAccountInputValue(user, "Postcode");
                location.Value = getMyAccountInputValue(user, "Location");
                phoneNumber.Value = getMyAccountInputValue(user, "PhoneNumber");
                mobileNumber.Value = getMyAccountInputValue(user, "MobileNumber");
                website.Value = getMyAccountInputValue(user, "Website");
            }
            catch
            {
            }
        }

        void save_Click(object sender, EventArgs e)
        {
            string infoMessage = "";
            int eventCounter = 0;
            // Email Adresse validieren...
            try
            {
                if (passWord1.Value == passWord2.Value && passWord1.Value != "" && passWord1.Value != null)
                {
                    // Überprüfen ob es ein zulässiges Passwort ist
                    MembershipUser currentUser = Membership.GetUser(Page.User.Identity.Name);

                    if (currentUser.ChangePassword(oldPassWord.Value, passWord1.Value))
                    {
                        if (!string.IsNullOrEmpty(emailAddress.Value)) // Falls die Email Adresse beim speichern gerade geändert wurde, wir die Änderungsbestätigung direkt an die neue Adresse geschickt
                            Website_Helpers.sendMail("Mein Konto", "Sie haben Ihr Passwort erfolgreich geändert.", false, "admin@pkstudio.ch", emailAddress.Value.ToLower());
                        else // Falls aus irgend einem Grund die Email Adresse bei Mein Konto leer sein sollte, wir diejenige aus der Users.xml Datei genommen.
                            Website_Helpers.sendMail("Mein Konto", "Sie haben Ihr Passwort erfolgreich geändert.", false, "admin@pkstudio.ch", DataIO.LoadUsers.Element("Users").Elements("User").Where(t => t.Element("UserName").Value.ToLower().Equals(Page.User.Identity.Name.ToLower())).Single().Element("Email").Value);
                        infoMessage = "Ihr Passwort wurde erfolgreich geändert";
                        eventCounter++;
                    }
                }

                #region Hier werden die persönlichen Daten wie Vorname, Nachname, etc. gespeichert.
                XDocument doc = DataIO.LoadUsers;
                XElement user = doc.Element("Users").Elements("User").Where(t => t.Element("UserName").Value.ToLower().Equals(Page.User.Identity.Name.ToLower())).Single();
                user = setMyAccountInputValue(user, "Email", emailAddress.Value);
                user = setMyAccountInputValue(user, "Surname", surname.Value);
                user = setMyAccountInputValue(user, "Lastname", lastname.Value);
                user = setMyAccountInputValue(user, "Address", address.Value);
                user = setMyAccountInputValue(user, "Postcode", postcode.Value);
                user = setMyAccountInputValue(user, "Location", location.Value);
                user = setMyAccountInputValue(user, "PhoneNumber", phoneNumber.Value);
                user = setMyAccountInputValue(user, "MobileNumber", mobileNumber.Value);
                user = setMyAccountInputValue(user, "Website", website.Value);
                doc.Element("Users").Elements("User").Where(t => t.Element("UserName").Value.ToLower().Equals(Page.User.Identity.Name.ToLower())).Single().ReplaceAll(user.Descendants());
                doc.Save(DataIO.UsersLocation);
                eventCounter = 2; // kleiner fix, damit die Nachricht 'Änderungen übernommen' angezeigt wird.
                #endregion

            }
            catch (Exception ex)
            {
                infoMessage = ex.Message.ToString();
                try
                {
                    Website_Helpers.sendError(ex.Message.ToString());
                    infoMessage = "Leider trat beim speichern ein Fehler auf. Aktualisieren Sie die Seite, und versuchen Sie es noch einmal";
                }
                catch (Exception mex)
                {
                    infoMessage = mex.Message.ToString();
                }

            }
            DataIO.LoadUsers.Save(DataIO.UsersLocation);

            // Damit die Nachrichten an den Nutzer einigermassen sinvoll sind, werden diese hier angepasst, je nachdem wieviel gepspeichert wurde.
            if (eventCounter >= 2)
                message.InnerText = "Änderungen übernommen";
            else
                message.InnerText = infoMessage;

        }

        public string getMyAccountInputValue(XElement user, string ElementName)
        {
            // Prüfen ob das Element existiert
            var element = user.Elements().Where(x => x.Name.ToString().ToLower().Equals(ElementName.ToLower())).Single();
            if (element != null)
            {
                return element.Value.ToString();
            }
            return "";
        }
        public XElement setMyAccountInputValue(XElement user, string ElementName, string inputValue)
        {
            if (user.Elements().Where(x => x.Name.ToString().ToLower().Equals(ElementName.ToLower())).Any())
            {
                if (!string.IsNullOrEmpty(inputValue))
                    user.Elements().Where(x => x.Name.ToString().ToLower().Equals(ElementName.ToLower())).Single().SetValue(inputValue);
                else
                    user.Elements().Where(x => x.Name.ToString().ToLower().Equals(ElementName.ToLower())).Single().Remove();
            }
            else
            {
                if (!string.IsNullOrEmpty(inputValue))
                    user.Add(new XElement(ElementName, inputValue));
            }
            return user;
        }
    }
}
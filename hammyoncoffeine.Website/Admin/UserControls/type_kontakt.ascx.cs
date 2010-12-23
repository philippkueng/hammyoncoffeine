using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Net.Mail;
using hammyoncoffeine.Core;

namespace hammyoncoffeine.Website
{

    public partial class Admin_type_kontakt : System.Web.UI.UserControl
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
            item = "<a href='Default.aspx?c=content&f=" + r_folder + "&p=" + r_page + "&i=" + r_item + "'>" + r_item + "</a>";
            
            //txtContent.Text = Helpers.newDoc().Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(Page.Request.QueryString["p"].ToLower())).Single().Elements("item").Where(s => s.Attribute("id").Value.ToLower().Equals(Page.Request.QueryString["i"].ToLower())).Single().Value.ToString();
            var accordingData = Website_Helpers.newDoc().Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(Page.Request.QueryString["p"].ToLower())).Single().Elements("item").Where(s => s.Attribute("id").Value.ToLower().Equals(Page.Request.QueryString["i"].ToLower())).Single();

            //testen ob dieses Element existiert, und falls dieses laden...
            if (accordingData.Elements("emailAddress").Any())
                emailAddress.Text = accordingData.Element("emailAddress").Value.ToString();
            if (accordingData.Elements("smtpServer").Any())
                smtpServer.Text = accordingData.Element("smtpServer").Value.ToString();
            if (accordingData.Elements("portNumber").Any())
                smtpServer.Text = accordingData.Element("portNumber").Value.ToString();
            if (accordingData.Elements("username").Any())
                smtpServer.Text = accordingData.Element("username").Value.ToString();
            if (accordingData.Elements("password").Any())
                smtpServer.Text = accordingData.Element("password").Value.ToString();
            if (accordingData.Elements("subjectPrefix").Any())
                smtpServer.Text = accordingData.Element("subjectPrefix").Value.ToString();
        }
        protected void save_OnClick(object sender, EventArgs e)
        {
            message.Visible = true;
            string shared_item = null;

            // neues content element zusammensetzen, welches danach gespeichert werden kann...
            XElement element = new XElement("content",
                new XElement("emailAddress", emailAddress.Text),
                new XElement("smtpServer", smtpServer.Text),
                new XElement("portNumber", portNumber.Text),
                new XElement("username", username.Text),
                new XElement("password", password.Text),
                new XElement("subjectPrefix", subjectPrefix.Text));
            if (DataIO.saveItemContent(element,Page.Request.QueryString["f"], Page.Request.QueryString["p"], Page.Request.QueryString["i"], "kontakt", shared_item))
            {
                // Testemail senden...
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.IsBodyHtml = true;
                    mail.From = new MailAddress(emailAddress.Text);
                    mail.To.Add(new MailAddress(emailAddress.Text));
                    mail.Subject = subjectPrefix.Text + "Testmail";
                    mail.Body = "Success";
                    SmtpClient smtp = new SmtpClient(smtpServer.Text);
                    smtp.Send(mail);
                    message.InnerText = "Der Eintrag wurde erfolgreich gespeichert und das Testmail gesendet.";

                }
                catch
                {
                    message.InnerText = "Der Eintrag wurde gespeichert, allerdings trat beim senden der Nachricht ein Fehler auf.";
                }
            }
            else
            {
                message.InnerText = "Es ist ein Fehler beim speichern aufgetreten. Bitte kopieren Sie den Text per Ctrl + C und versuchen Sie es nocheinmal.";
            }
        }
    }
}
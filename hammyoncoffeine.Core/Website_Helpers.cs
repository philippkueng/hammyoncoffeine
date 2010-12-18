using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace hammyoncoffeine.Core
{
    /// <summary>
    /// Summary description for Helpers
    /// </summary>
    public class Website_Helpers
    {
        public static XElement newFolder(string path, string name)
        {
            XElement tempElement;
            if (string.IsNullOrEmpty(path))
                tempElement = new XElement("root");
            else
            {
                tempElement = new XElement("folder",
                    new XAttribute("path", path),
                    new XAttribute("name", name));
            }
            DirectoryInfo directoy = new DirectoryInfo(DataIO.PagesDirectory + path);
            foreach (FileInfo file in directoy.GetFiles("*.htm"))
            {
                string websiteContent;
                using (StreamReader mySR = new StreamReader(DataIO.PagesDirectory + file.Name))
                {
                    websiteContent = mySR.ReadToEnd();
                }
                Regex extractCMSTag = new Regex("(\\<cms[^\\>]+\\>)", RegexOptions.None);

                XElement page = new XElement("page",
                    new XAttribute("name", removeFileEnding(file.Name.ToString())));

                foreach (Match myMatch in extractCMSTag.Matches(websiteContent))
                {
                    string item_id = getNameTag(myMatch.ToString());

                    var item_from_dataXML = DataIO.loadXItem(path, page.Attribute("name").Value.ToString(), item_id);
                    if (item_from_dataXML != null)
                    {
                        page.Add(
                        new XElement("item",
                            new XAttribute("id", item_id),
                            new XAttribute("type", item_from_dataXML.Attribute("type").Value.ToString())));
                    }
                    else
                    {
                        page.Add(
                        new XElement("item",
                            new XAttribute("id", item_id),
                            new XAttribute("type", "")));
                    }
                }

                tempElement.Add(page);
                
                
                //var test = DataIO.loadXItem(path, page.Attribute("name").Value.ToString(), page.Elements("
                //tempDoc.Element("root").Add(page);
            }
            foreach (DirectoryInfo dir in directoy.GetDirectories())
            {
                tempElement.Add(newFolder(path + "/" + dir.Name, dir.Name));
            }
            return tempElement;
        }

        public static XDocument newDoc()
        {
            //List<PageItem> pageItems = new List<PageItem>();
            XDocument tempDoc = new XDocument();
            //tempDoc.Add(new XElement("root"));

            tempDoc.Add(newFolder(null, "root"));

            #region
            //DirectoryInfo directoy = new DirectoryInfo(DataIO.PagesDirectory);
            //foreach (FileInfo file in directoy.GetFiles("*.htm"))
            //{
            //    string websiteContent;
            //    using (StreamReader mySR = new StreamReader(DataIO.PagesDirectory + file.Name))
            //    {
            //        websiteContent = mySR.ReadToEnd();
            //    }
            //    Regex extractCMSTag = new Regex("(\\<cms[^\\>]+\\>)", RegexOptions.None);

            //    XElement page = new XElement("page",
            //        new XAttribute("name", removeFileEnding(file.Name.ToString())));

            //    foreach (Match myMatch in extractCMSTag.Matches(websiteContent))
            //    {
            //        page.Add(
            //            new XElement("item",
            //                new XAttribute("id", getNameTag(myMatch.ToString())),
            //                new XAttribute("type", "")));
            //    }

            //    tempDoc.Element("root").Add(page);
            //}
            #endregion
            #region
            //// Nun muss noch geprüft werden, was schon vorhanden ist im data.xml...
            //XDocument doc = DataIO.LoadData;
            //foreach (var tempPage in tempDoc.Element("root").Elements("page"))
            //{
            //    foreach (var page in doc.Element("root").Elements("page"))
            //    {
            //        if (page.Attribute("name").Value.ToLower().Equals(tempPage.Attribute("name").Value.ToLower()))
            //        {
            //            // Die Seiten sind gleich...
            //            foreach (var tempItem in tempPage.Elements("item"))
            //            {
            //                foreach (var item in page.Elements("item"))
            //                {
            //                    if (tempItem.Attribute("id").Value.ToLower().Equals(item.Attribute("id").Value.ToLower()))
            //                    {
            //                        // Die Items sind gleich...
            //                        tempItem.SetAttributeValue("type", item.Attribute("type").Value);
            //                        tempItem.SetValue(item.Value);
            //                        // (TODO) in eine Sicherheitsdatei schreiben, die dann nach dem editieren gelöscht werden kann.
            //                        //item.Remove();
            //                        break;
            //                    }
            //                }
            //            }
            //            break;
            //        }
            //    }
            //}
            #endregion

            // (TODO) Prüfen welche Elemente noch in doc sind, und diese in eine Sicherungsdatei schreiben...

            // (TODO) tempDoc wird als neues data.xml gespeichert.

            return tempDoc;
        }
        public static string getNameTag(string RegexContent)
        {
            // liefert von <cms name="test" /> ---> test zurück
            return RegexContent.Substring((RegexContent.IndexOf('\"') + 1), (RegexContent.LastIndexOf('\"') - RegexContent.IndexOf('\"') - 1));
        }
        public static string removeFileEnding(string fileName)
        {
            return fileName.Substring(0, fileName.LastIndexOf("."));
        }
        public static List<string> getPages()
        {
            List<string> pages;
            pages = (List<string>)HttpContext.Current.Cache["pages"];
            if (pages == null)
            {
                pages = new List<string>();
                foreach (var page in DataIO.LoadData.Element("root").Elements("page"))
                {
                    pages.Add(page.Attribute("name").Value.ToLower().ToString());
                }
                CacheDependency cd = new CacheDependency(DataIO.StorageLocation);
                HttpContext.Current.Cache.Insert("pages", pages, cd);
            }
            return pages;
        }
        public static bool doesPageExists(string pageName)
        {
            foreach (string page in getPages())
            {
                if (page == pageName.ToLower())
                    return true;
            }
            return false;
        }

        public static void sendMail(string title, string content, bool toAll, string sender, string receiver)
        {
            #region Mail to a single user
            if (!toAll)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.IsBodyHtml = true;
                    mail.From = new MailAddress(sender);
                    mail.To.Add(receiver);
                    mail.Subject = title;
                    string[] Content = { title, content };
                    mail.Body = ViewManager.RenderView("~/email_template.ascx", Content, "User");
                    SmtpClient smtp = new SmtpClient(DataIO.smtpServer);
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    sendError(ex);
                }
            }
            #endregion
            #region Mail to all registered users in users.xml
            else
            {
                try
                {
                    foreach (var user in DataIO.LoadUsers.Element("Users").Elements("User"))
                    {
                        try
                        {
                            MailMessage mail = new MailMessage();
                            mail.IsBodyHtml = true;
                            mail.From = new MailAddress(sender);
                            mail.To.Add(new MailAddress(user.Element("Email").Value.ToString(), user.Element("UserName").Value.ToString()));
                            mail.Subject = title;
                            string[] Content = { title, content };
                            mail.Body = ViewManager.RenderView("~/email_template.ascx", Content, "User");
                            SmtpClient smtp = new SmtpClient(DataIO.smtpServer);
                            smtp.Send(mail);
                        }
                        catch (Exception ex)
                        {
                            sendError(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    sendError(ex);
                }
            }
            #endregion
        }

        public static void sendError(Exception exception)
        {
            sendError("Message: " + exception.Message + "<br/><br/>Source: " + exception.Source + "<br/><br/>StackTrace: " + exception.StackTrace);
        }
        public static void sendError(string ErrorMessage)
        {
            if (!string.IsNullOrEmpty(DataIO.errorEmailAddress))
            {
                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(DataIO.emailAddress);
                mail.To.Add(DataIO.errorEmailAddress);
                mail.Subject = "hammyoncoffeine - Error";
                mail.Body = ErrorMessage;
                SmtpClient smtp = new SmtpClient(DataIO.smtpServer);
                smtp.Send(mail);
            }
            else
            {
                DataIO.addToLog(ErrorMessage);
            }
        }

        /// <summary>
        /// Writes ETag and Last-Modified headers and sets the conditional get headers.
        /// </summary>
        /// <param name="date">The date.</param>
        public static bool SetConditionalGetHeaders(DateTime date)
        {

            // SetLastModified() below will throw an error if the 'date' is a future date.
            if (date > DateTime.Now)
                date = DateTime.Now;

            HttpResponse response = HttpContext.Current.Response;
            HttpRequest request = HttpContext.Current.Request;

            string etag = "\"" + date.Ticks + "\"";
            string incomingEtag = request.Headers["If-None-Match"];

            response.AppendHeader("ETag", etag);
            response.Cache.SetLastModified(date);

            if (String.Compare(incomingEtag, etag) == 0)
            {
                response.Clear();
                response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                return true;
            }
            return false;
        }
        public static string getFileNameFromQueryString(string queryString)
        {
            if (queryString.Contains("/"))
                return queryString.Substring(queryString.LastIndexOf("/") + 1, queryString.Length - queryString.LastIndexOf("/") - 1);
            return queryString;
        }
        public static string getFolderPathFromQueryString(string queryString)
        {
            if (queryString.Contains("/"))
                return "/" + queryString.Substring(0, queryString.LastIndexOf("/"));
            return "";
        }
        public static string getMD5HashOfFile(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new Exception("No args provided");
                if (!File.Exists(path))
                    throw new Exception("File " + path + " doesn't exist.");
                Stream inputStream = File.OpenRead(path);
                HashAlgorithm algorithm = MD5.Create();
                byte[] hash = algorithm.ComputeHash(inputStream);
                SoapHexBinary output = new SoapHexBinary(hash);
                return output.ToString();
            }
            catch (Exception e)
            {
                sendError("MD5 Error --- " + e.Message.ToString());
                return "";
            }
        }
        public static bool isImage(string filepath)
        {
            string theFileEnding = filepath.Substring(filepath.LastIndexOf(".") + 1, filepath.Length - filepath.LastIndexOf(".") - 1);
            string[] possibleImageFormats = { "jpg", "png", "gif" };
            foreach (string ending in possibleImageFormats)
            {
                if (theFileEnding == ending)
                    return true;
            }
            return false;
        }
        public static XElement getUserControlSpecificXElement(string page, string item)
        {
            return newDoc().Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Elements("item").Where(s => s.Attribute("id").Value.ToLower().Equals(item.ToLower())).Single().Element("content");
        }

    }
}
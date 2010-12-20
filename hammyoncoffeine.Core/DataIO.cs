#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Caching;
using System.IO;
using System.Xml.Linq;
#endregion

namespace hammyoncoffeine.Core
{
    public class DataIO
    {
        #region VirtualLocation
        /// <summary>
        /// location where the data gets stored, aka. ../App_Data
        /// </summary>
        private static string _VirtualLocation;
        public static string VirtualLocation
        {
            get
            {
                if (_VirtualLocation == null)
                {
                    string p = WebConfigurationManager.AppSettings.Get("StorageLocation").ToString().Replace("~/", "");
                    _VirtualLocation = Path.Combine(HttpRuntime.AppDomainAppPath, p);
                }
                return _VirtualLocation;
            }
        }
        #endregion

        #region StorageLocation
        /// <summary>
        /// location of the data.xml file
        /// </summary>
        private static string _StorageLocation;
        public static string StorageLocation
        {
            get
            {
                if (string.IsNullOrEmpty(_StorageLocation))
                {
                    string p = WebConfigurationManager.AppSettings.Get("StorageLocation").ToString().Replace("~/", "");
                    _StorageLocation = Path.Combine(HttpRuntime.AppDomainAppPath, p) + "Data.xml";
                }
                return _StorageLocation;
            }
        }
        #endregion

        #region SettingsLocation
        /// <summary>
        /// location of the settings.xml file
        /// </summary>
        private static string _SettingsLocation;
        public static string SettingsLocation
        {
            get
            {
                if (string.IsNullOrEmpty(_SettingsLocation))
                {
                    string p = WebConfigurationManager.AppSettings.Get("StorageLocation").ToString().Replace("~/", "");
                    _SettingsLocation = Path.Combine(HttpRuntime.AppDomainAppPath, p) + "Settings.xml";
                }
                return _SettingsLocation;
            }
        }
        #endregion

        #region UsersLocation
        /// <summary>
        /// location of the users.xml file where all the passwords and profile settings get stored
        /// </summary>
        private static string _UsersLocation;
        public static string UsersLocation
        {
            get
            {
                if (string.IsNullOrEmpty(_UsersLocation))
                {
                    string p = WebConfigurationManager.AppSettings.Get("StorageLocation").ToString().Replace("~/", "");
                    _UsersLocation = Path.Combine(HttpRuntime.AppDomainAppPath, p) + "Users.xml";
                }
                return _UsersLocation;
            }
        }
        #endregion

        #region LogLocation
        /// <summary>
        /// location of the log folder
        /// </summary>
        private static string _LogLocation;
        public static string LogLocation
        {
            get
            {
                if (string.IsNullOrEmpty(_LogLocation))
                {
                    string p = WebConfigurationManager.AppSettings.Get("StorageLocation").ToString().Replace("~/", "");
                    _LogLocation = Path.Combine(HttpRuntime.AppDomainAppPath, p) + "/logs";
                }
                return _LogLocation;
            }
        }
        #endregion

        #region LoadData ...uses StorageLocation
        /// <summary>
        /// load data.xml and cache the file
        /// </summary>
        public static XDocument LoadData
        {
            get
            {
                XDocument doc = (XDocument)HttpContext.Current.Cache["data"];
                if (doc == null)
                {
                    // check if data.xml file exists
                    FileInfo fi = new FileInfo(StorageLocation);
                    if (fi.Exists)
                    {
                        doc = XDocument.Load(StorageLocation);     
                    }
                    else
                    {
                        // create an empty data.xml file and save to disk
                        doc = new XDocument(
                            new XDeclaration("1.0", "utf-8", "yes"),
                            new XElement("root", ""));
                        doc.Save(StorageLocation);
                    }

                    CacheDependency cd = new CacheDependency(StorageLocation);
                    HttpContext.Current.Cache.Insert("data", doc, cd);

                }
                return doc;
            }
        }
        #endregion

        #region LoadSettings ...uses SettingsLocation
        /// <summary>
        /// load settings.xml and cache the file
        /// </summary>
        public static XDocument LoadSettings
        {
            get
            {
                XDocument doc;
                doc = (XDocument)HttpContext.Current.Cache["settings"];
                if (doc == null)
                {
                    doc = XDocument.Load(SettingsLocation);
                    CacheDependency cd = new CacheDependency(SettingsLocation);
                    HttpContext.Current.Cache.Insert("settings", doc, cd);
                }
                return doc;
            }
        }
        #endregion

        #region get mail stuff from LoadSettings
        private static string _smtpServer;
        public static string smtpServer
        {
            get
            {
                if (!string.IsNullOrEmpty(_smtpServer))
                    _smtpServer = LoadSettings.Element("root").Element("emailSettings").Element("smtpServer").Value.ToString();
                return _smtpServer;
            }
        }

        private static string _emailAddress;
        public static string emailAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(_emailAddress))
                    _emailAddress = LoadSettings.Element("root").Element("emailSettings").Element("emailAddress").Value.ToString();
                return _emailAddress;
            }
        }

        private static string _errorEmailAddress;
        public static string errorEmailAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(_errorEmailAddress))
                    _errorEmailAddress = LoadSettings.Element("root").Element("emailSettings").Element("errorEmailAddress").Value.ToString();
                return _errorEmailAddress;
            }
        }

        #endregion

        #region LoadUsers ...uses UsersLocation
        /// <summary>
        /// load users.xml and cache the file
        /// </summary>
        public static XDocument LoadUsers
        {
            get
            {
                XDocument doc = (XDocument)HttpContext.Current.Cache["users"];
                if (doc == null)
                {
                    doc = XDocument.Load(UsersLocation);
                    CacheDependency cd = new CacheDependency(UsersLocation);
                    HttpContext.Current.Cache.Insert("users", doc, cd);
                }
                return doc;
            }
        }
        #endregion

        #region websiteroot for URLRewrite --> important if site is hosted in a subfolder
        private static string _websiteroot;
        public static string websiteroot
        {
            get
            {
                if(string.IsNullOrEmpty(_websiteroot))
                    _websiteroot = LoadSettings.Element("root").Element("websiteroot").Value.ToString().ToLower();
                return _websiteroot;
            }
        }

        #endregion

        #region PagesDirectory
        /// <summary>
        /// returns the Directory where the original (non-cached) pages are stored
        /// </summary>
        private static string _PagesDirectory;
        public static string PagesDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_PagesDirectory))
                    _PagesDirectory = HttpContext.Current.Server.MapPath("~/App_Data/pages/");

                return _PagesDirectory;
            }
        }
        #endregion

        #region CacheDirectory
        /// <summary>
        /// returns the Directory where the original (cached) pages are stored
        /// </summary>
        private static string _CacheDirectory;
        public static string CacheDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_CacheDirectory))
                    _CacheDirectory = HttpContext.Current.Server.MapPath("~/App_Data/cache/");

                return _CacheDirectory;
            }
        }
        #endregion

        #region read all html files in App_Data/pages
        /// <summary>
        /// returns a list of page names from the App_Data/pages folder without the *.htm ending
        /// </summary>
        private static List<string> _PagesList;
        public static List<string> PagesList
        {
            get
            {
                if (_PagesList == null)
                {
                    _PagesList = new List<string>();
                    DirectoryInfo pagesDirectoryInfo = new DirectoryInfo(PagesDirectory);
                    foreach (FileInfo page in pagesDirectoryInfo.GetFiles("*.htm"))
                    {
                        if (page.Attributes != FileAttributes.Hidden)
                        {
                            _PagesList.Add(page.Name.ToString().Replace(".htm", ""));
                        }
                    }
                }
                return _PagesList;
            }
        }

        /// <summary>
        /// method to clear the pages cache if a new page gets added.
        /// </summary>
        public static void PagesList_Clear()
        {
            _PagesList = null;
        }
        #endregion

        #region methods used by files and folder management category
        public static XDocument loadFilesandFolders()
        {
            XDocument doc;
            doc = (XDocument)HttpContext.Current.Cache["files"];
            if (doc == null)
            {
                #region Hier wird doc gefüllt... (bis jetzt einmal nur auf 2 Etagen)
                DirectoryInfo masterDirectory = new DirectoryInfo(DataIO.VirtualLocation + "files/");
                doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                //doc.Add(new XElement("root","bla"));
                doc.Add(getCurrentFolderContent(masterDirectory, true));
                doc.Save(DataIO.VirtualLocation + "files.xml");
                #endregion
                CacheDependency cd = new CacheDependency(DataIO.VirtualLocation + "files/");
                HttpContext.Current.Cache.Insert("files", doc, cd);
            }
            return doc;
        }

        public static XElement getCurrentFolderContent(DirectoryInfo masterDirectory, bool isRoot)
        {
            XElement doc;
            if (isRoot)
            {
                doc = new XElement("root", "");
            }
            else
            {
                doc = new XElement("folder",
                    new XAttribute("name", masterDirectory.Name.ToString()));
            }
            try
            {
                foreach (DirectoryInfo directory in masterDirectory.GetDirectories())
                {
                    if (directory.Attributes != FileAttributes.Hidden)
                    {
                        if (directory.GetFiles().Count() != 0 || directory.GetDirectories().Count() != 0)
                        {
                            XElement folderElement = getCurrentFolderContent(directory, false);
                            doc.Add(folderElement);
                        }
                        else
                        {
                            XElement folderElement = new XElement("folder",
                                new XAttribute("name", directory.Name.ToString()));
                            doc.Add(folderElement);
                        }
                    }
                }
                foreach (FileInfo file in masterDirectory.GetFiles())
                {
                    if (file.Attributes != FileAttributes.Hidden)
                    {
                        XElement fileElement = new XElement("file",
                            new XAttribute("name", file.Name.ToString()));
                        doc.Add(fileElement);
                    }
                }
            }
            catch
            {
                doc.Add(new XElement("error", "Leider ist beim durchsuchen ein Fehler aufgetreten"));
            }

            return doc;
        }

        public static bool saveItemContent(ItemContent ic)
        {
            if (saveItemContent(ic.elementContent,ic.folder, ic.page, ic.item, ic.type, ic.shared_item))
                return true;
            else
                return false;
        }

        //public static XElement getFolder(XElement element, string[] path, int current_folder)
        //{
        //    if (element.Name == "root" && string.IsNullOrEmpty(path[0]) && path.Length == 1)
        //        return element;
        //    else
        //    {
        //        if ((element.Name == "folder") && (element.Attribute("name").Value.ToLower() == path[current_folder].ToLower()) && (path.Length == (current_folder + 1)))
        //        {
        //            return element;
        //        }
        //        else
        //        {
        //            if (element.Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(path[current_folder + 1].ToLower())).Any())
        //                return getFolder(element.Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(path[current_folder + 1].ToLower())).Single(), path, current_folder + 1);
        //            else
        //                return null;
        //        }
        //    }
        //}

        public static bool saveItemContent(XElement elementContent, string folder, string page, string item, string type, string shared_item) // Hier wird nicht geprüft ob die Seite überhaupt existiert...
        {
            #region save shared item
            if (!string.IsNullOrEmpty(shared_item))
			{
				try
                {  
                    // get element with same shared_item id, type and same page and item
                    if (string.IsNullOrEmpty(folder))
                    {
                        var items = DataIO.LoadData.Element("root").Element("shared").Elements("item").Where(t => t.Attribute("type").Value.ToLower().Equals(type.ToLower())).Where(w => w.Attribute("id").Value.ToLower().Equals(shared_item.ToLower())).Where(s => s.Element("pages").Elements("page").Any(p => (p.Attribute("name").Value.ToLower().Equals(page.ToLower())) && p.Elements("item").Any(r => r.Value.ToLower().Equals(item.ToLower()))));
                        XElement Xitem;
                        if (!items.Any())
                            Xitem = null;
                        else
                            Xitem = items.Single();

                        if (Xitem == null) // the item does not yet exist, so lets create it
                        {
                            Xitem = new XElement("item",
                                                 new XAttribute("id", shared_item),
                                                 new XAttribute("type", type),
                                                 new XElement("pages",
                                                              new XElement("page",
                                                                  new XAttribute("name", page),
                                                                  new XAttribute("folder", folder),
                                                                  new XElement("item", item)
                                                                  )
                                                 ),
                                                 elementContent);
                            DataIO.LoadData.Element("root").Element("shared").Add(Xitem);
                        }
                        else // the element already exists, so we only have to save the new data
                        {
                            if (Xitem.Elements("content").Any())
                                Xitem.Element("content").Remove();
                            Xitem.Add(elementContent);
                        }
                        DataIO.LoadData.Save(DataIO.StorageLocation);

                        // remove the page from the cache to display the changed data to the customer
                        HttpContext.Current.Cache.Remove("page_string" + page.ToLower());
                        HttpContext.Current.Cache.Remove("page_placeholder" + page.ToLower());

                        return true;
                    }
                    else
                    {
                        
                        var items = DataIO.LoadData.Element("root").Element("shared").Elements("item").Where(t => t.Attribute("type").Value.ToLower().Equals(type.ToLower())).Where(w => w.Attribute("id").Value.ToLower().Equals(shared_item.ToLower())).Where(s => s.Element("pages").Elements("page").Any(p => (p.Attribute("name").Value.ToLower().Equals(page.ToLower())) && (p.Attributes("folder").Where(x => x.Value.ToLower().Equals(folder.ToLower())).Any()) && p.Elements("item").Any(r => r.Value.ToLower().Equals(item.ToLower()))));
                        XElement Xitem;
                        if (!items.Any())
                            Xitem = null;
                        else
                            Xitem = items.Single();

                        if (Xitem == null) // the item does not yet exist, so lets create it
                        {
                            Xitem = new XElement("item",
                                                 new XAttribute("id", shared_item),
                                                 new XAttribute("type", type),
                                                 new XElement("pages",
                                                              new XElement("page",
                                                                  new XAttribute("name", page),
                                                                  new XAttribute("folder", folder),
                                                                  new XElement("item", item)
                                                                  )
                                                 ),
                                                 elementContent);
                            DataIO.LoadData.Element("root").Element("shared").Add(Xitem);
                        }
                        else // the element already exists, so we only have to save the new data
                        {
                            if (Xitem.Elements("content").Any())
                                Xitem.Element("content").Remove();
                            Xitem.Add(elementContent);
                        }
                        DataIO.LoadData.Save(DataIO.StorageLocation);

                        // remove the page from the cache to display the changed data to the customer
                        HttpContext.Current.Cache.Remove("page_string" + page.ToLower());
                        HttpContext.Current.Cache.Remove("page_placeholder" + page.ToLower());

                        return true;
                    }
					
				}
				catch (Exception ex)
                {
                    Website_Helpers.sendError(ex);
					return false;
				}
            }
            #endregion

            #region save non-shared item
            else
            {
	            try
	            {
                    XElement Xfolder;
                    if (string.IsNullOrEmpty(folder))
                        Xfolder = DataIO.LoadData.Element("root");
                    else
                    {
                        string[] folderpath = folder.Split('/');
                        if (folderpath.Length == 2)
                        {
                            if (DataIO.LoadData.Element("root").Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(folderpath[1])).Any())
                                Xfolder = DataIO.LoadData.Element("root").Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(folderpath[1])).Single();
                            else
                                return false;
                        }
                        else if (folderpath.Length == 3)
                        {
                            if (DataIO.LoadData.Element("root").Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(folderpath[1])).Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folderpath[2])).Any())
                                Xfolder = DataIO.LoadData.Element("root").Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(folderpath[1])).Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folderpath[2])).Single();
                            else
                                return false;
                        }
                        else if (folderpath.Length == 4)
                        {
                            if (DataIO.LoadData.Element("root").Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(folderpath[1])).Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folderpath[2])).Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folderpath[3])).Any())
                                Xfolder = DataIO.LoadData.Element("root").Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(folderpath[1])).Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folderpath[2])).Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folderpath[3])).Single();
                            else
                                return false;
                        }
                        else if (folderpath.Length == 5)
                        {
                            if (DataIO.LoadData.Element("root").Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(folderpath[1])).Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folderpath[2])).Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folderpath[3])).Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folderpath[4])).Any())
                                Xfolder = DataIO.LoadData.Element("root").Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(folderpath[1])).Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folderpath[2])).Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folderpath[3])).Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folderpath[4])).Single();
                            else
                                return false;
                        }
                        else // not yet implemented
                        {
                            return false;
                        }
                        //Xfolder = getFolder(elementContent, folder.Split('/'), 0);
                    }

	                //XElement Xpage = DataIO.LoadData.Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single();
                    XElement Xpage = Xfolder.Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single();
	                if (Xpage == null) // Die Seite existiert noch nicht, und muss zuerst angelegt werden
	                {
	                    Xpage = new XElement("page",
	                        new XAttribute("name", page.ToString()));
	                    //DataIO.LoadData.Element("root").Add(Xpage);
                        Xfolder.Add(Xpage);
	                }
	
	                XElement Xitem = Xpage.Elements("item").Where(t => t.Attribute("id").Value.ToLower().Equals(item.ToLower())).Single();
	                if (Xitem == null) // Das Element existiert noch nicht, und muss neu erstellt werden
	                {
	                    Xitem = new XElement("item",
	                        new XAttribute("id", item),
	                        new XAttribute("type", type), elementContent);
	                    //DataIO.LoadData.Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Add(Xitem);
                        Xfolder.Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Add(Xitem);
	                }
	                else // Das Element existiert schon, und muss daher einfach ersetzt werden...
	                {
	                    // Nur falls schon ein Knoten vorhanden ist, kann auch einer ersetzt werden...
	                    //if (DataIO.LoadData.Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Elements("item").Where(v => v.Attribute("id").Value.ToLower().Equals(item.ToLower())).Single().HasElements)
                        if (Xfolder.Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Elements("item").Where(v => v.Attribute("id").Value.ToLower().Equals(item.ToLower())).Single().HasElements)
	                    {
	                        //DataIO.LoadData.Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Elements("item").Where(v => v.Attribute("id").Value.ToLower().Equals(item.ToLower())).Single().FirstNode.Remove();
                            Xfolder.Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Elements("item").Where(v => v.Attribute("id").Value.ToLower().Equals(item.ToLower())).Single().FirstNode.Remove();
                        }
	                    //DataIO.LoadData.Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Elements("item").Where(v => v.Attribute("id").Value.ToLower().Equals(item.ToLower())).Single().Add(elementContent);
                        Xfolder.Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Elements("item").Where(v => v.Attribute("id").Value.ToLower().Equals(item.ToLower())).Single().Add(elementContent);
	                }
                    DataIO.LoadData.Save(DataIO.StorageLocation);

	                // Der Cache der Seite muss hier noch geleert werden, damit die Seiten mit den neuen Informationen angezeigt werden
	                HttpContext.Current.Cache.Remove("page_string" + page.ToLower());
	                HttpContext.Current.Cache.Remove("page_placeholder" + page.ToLower());
	
	                return true;
	            }
	            catch
	            {
	                //Website_Helpers.sendError("saveItemContent " + ex.Message.ToString());
	                return false;
	            }
            }
            #endregion
        }

        // single item content gets transformed into shared item content
        public static bool convertSingleToSharedItem(XElement elementContent, string folder, string page, string item, string type, string shared_item)
        {
            try
            {
                if (saveItemContent(elementContent, folder, page, item, type, shared_item))
                {
                    // delete the non-shared entry
                    DataIO.LoadData.Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Elements("item").Where(s => s.Attribute("id").Value.ToLower().Equals(item.ToLower())).Single().Remove();

                    DataIO.LoadData.Save(DataIO.StorageLocation);
                    HttpContext.Current.Cache.Remove("page_string" + page.ToLower());
                    HttpContext.Current.Cache.Remove("page_placeholder" + page.ToLower());
                }
                else
                {
                    return false;
                }                
                return true;
            }
            catch (Exception ex)
            {
                Website_Helpers.sendError(ex);
                return false;
            }
        }

        // single item content will be deleted and item element linked to the shared element content
        public static bool linkSingleToSharedItem(string folder, string page, string item, string shared_item)
        {
            try
            {
                #region add item to shared_item
                // check if shared_item exists
                var s_items = DataIO.LoadData.Element("root").Element("shared").Elements("item").Where(s => s.Attribute("id").Value.ToLower().Equals(shared_item.ToLower()));
                if (s_items.Any())
                {
                    // if there is no page with the same name as the current one, create it first
                    if (!s_items.Single().Element("pages").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Any())
                    {
                        s_items.Single().Element("pages").Add(new XElement("page", new XAttribute("name", page)));
                    }

                    // add the item to the page inside the shared_item
                    s_items.Single().Element("pages").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Add(new XElement("item", item));

                    DataIO.LoadData.Save(DataIO.StorageLocation);
                    HttpContext.Current.Cache.Remove("page_string" + page.ToLower());
                    HttpContext.Current.Cache.Remove("page_placeholder" + page.ToLower());
                }
                else
                {
                    // convertSingleToSharedItem with an empty content element
                    if (!convertSingleToSharedItem(new XElement("content", ""),folder , page, item, "html", shared_item))
                        return false;
                }
                #endregion

                #region remove item from single page
                // delete the matching single item entries
                DataIO.LoadData.Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Elements("item").Where(v => v.Attribute("id").Value.ToLower().Equals(item.ToLower())).Remove();
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Website_Helpers.sendError(ex);
                return false;
            }
        }

        // remove item from shared items and add a copy of the shared item content to the single item content
        public static bool convertSharedToSingleItem(string page, string item, string shared_item)
        {
            try
            {
                var s_items = DataIO.LoadData.Element("root").Element("shared").Elements("item").Where(t => t.Attribute("id").Value.ToLower().Equals(shared_item.ToLower()));

                #region make a copy of the content of shared_item content
                string type;
                XElement content;
                if (s_items.Any())
                {
                    content = s_items.Single().Element("content");
                    type = s_items.Single().Attribute("type").Value.ToString();
                }
                else
                {
                    content = new XElement("content", "");
                    type = "html";
                }
                #endregion

                #region add a new single item with the copied content
                XElement Xpage;

                if (!DataIO.LoadData.Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Any())
                {
                    // page doesn't exist yet, so create one from scratch
                    Xpage = new XElement("page",
                        new XAttribute("name", page));
                    DataIO.LoadData.Element("root").Add(Xpage);
                }

                Xpage = DataIO.LoadData.Element("root").Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single();

                XElement Xitem = new XElement("item",
                    new XAttribute("id", item),
                    new XAttribute("type", type),
                    content);

                Xpage.Add(Xitem);
                #endregion

                #region remove item from shared_item ---> removes only <item /> --> <page /> will still exist if item has been removed

                DataIO.LoadData.Element("root").Element("shared").Elements("item").Where(t => t.Attribute("id").Value.ToLower().Equals(shared_item.ToLower())).Elements("pages").Elements("page").Where(v => v.Attribute("name").Value.ToLower().Equals(page.ToLower())).Elements("item").Where(u => u.Value.ToLower().Equals(item.ToLower())).Remove();

                DataIO.LoadData.Save(DataIO.StorageLocation);
                HttpContext.Current.Cache.Remove("page_string" + page.ToLower());
                HttpContext.Current.Cache.Remove("page_placeholder" + page.ToLower());
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Website_Helpers.sendError(ex);
                return false;
            }
        }
        
        #endregion

        

        public static ItemContent loadItem(string folder, string page, string item)
        {
            ItemContent ic = new ItemContent();

            if (loadXItem(folder, page, item) != null)
            {
                if (loadXItem(folder, page, item).Elements("content").Any())
                    ic.elementContent = loadXItem(folder, page, item).Element("content");
                else
                    ic.elementContent = new XElement("content", "");
                ic.page = page;
                ic.item = item;
                ic.type = loadXItem(folder, page, item).Attribute("type").Value.ToLower();
                if (loadXItem(folder, page, item).Elements("pages").Any())
                    ic.shared_item = loadXItem(folder, page, item).Attribute("id").Value;
                else
                    ic.shared_item = null;
                return ic;
            }
            else
            {
                return null;
            }
        }

        // element = root element
        // folder = 'bla','foo','bla'
        // folder_array_position = 0

        public static XElement loadXItem_recursive(XElement element, string[] folder, int folder_array_position, string page, string item)
        {
            string current_folder = folder[folder_array_position].ToLower();
            if (element.Name == "folder" && element.Attribute("name").Value.ToLower().Equals(current_folder))
            {
                // check if this is the last folder element
                if (folder.Length == folder_array_position + 1)
                {
                    // there is a page element with the wanted name
                    if (element.Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Any())
                    {
                        // there is a item element with the wanted name
                        if (element.Elements("page").Where(s => s.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Elements("item").Where(u => u.Attribute("id").Value.ToLower().Equals(item.ToLower())).Any())
                        {
                            var xItem = element.Elements("page").Where(w => w.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Elements("item").Where(v => v.Attribute("id").Value.ToLower().Equals(item.ToLower())).Single();
                            return xItem;
                        }
                        else // item does not exist
                            return null;
                    }
                    else
                        return null;
                }
                else
                {
                    // there's a subfolder with the correct name
                    if (element.Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(folder[folder_array_position + 1].ToLower())).Any())
                        return loadXItem_recursive(element.Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folder[folder_array_position + 1])).Single(), folder, folder_array_position + 1, page, item);

                    // there's no subfolder with the requested name
                    else
                        return null;

                }
            }
            else // element is not a folder element or the folder does not exist
            {
                if (element.Name == "root")
                {
                    if (string.IsNullOrEmpty(folder[0]) && folder.Length == 1)
                    {
                        // there's is page inside the root directory
                        if (element.Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Any())
                            // there's a page and a element with the same name we're looking for.
                            if (element.Elements("page").Where(s => s.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Elements("item").Where(v => v.Attribute("id").Value.ToLower().Equals(item.ToLower())).Any())
                                return element.Elements("page").Where(s => s.Attribute("name").Value.ToLower().Equals(page.ToLower())).Single().Elements("item").Where(v => v.Attribute("id").Value.ToLower().Equals(item.ToLower())).Single();
                            else
                                return null;
                        else
                            return null;
                    }
                    else
                    {
                        // there's a folder with the name we're searching for
                        if (element.Elements("folder").Where(t => t.Attribute("name").Value.ToLower().Equals(folder[folder_array_position + 1].ToLower())).Any())
                            return loadXItem_recursive(element.Elements("folder").Where(s => s.Attribute("name").Value.ToLower().Equals(folder[folder_array_position + 1].ToLower())).Single(), folder, folder_array_position + 1, page, item);

                        //else if // is there a page with the requested name


                        else // there's no folder with the requested name
                            return null;
                    }
                }
                else
                    return null;
            }

            //if (folder.Length == folder_array_position + 1)
            //{
            //    // check if folder contains page
            //    if (element.Elements("page").Where(t => t.Attribute("name").Value.ToLower().Equals(page.ToLower())).Any())
            //    {
            //        return element;
            //    }
            //    else
            //        return null;
            //}
            //else
            //{
            //    return loadXItem_recursive(element.Element(folder[folder_array_position]), folder, folder_array_position + 1, page, item);
            //}

            //return new XElement("","");
        }

        public static XElement loadSharedXItem(string folder, string page, string item)
        {
            if (DataIO.LoadData.Element("root").Elements("shared").Any())
            {
                var xShared = DataIO.LoadData.Element("root").Element("shared");

                // item elements with a pages element
                var has_pages_element = xShared.Elements("item").Where(t => t.Elements("pages").Any());
                if (!has_pages_element.Any())
                    return null;

                // item elements with pages and some page elements in it.
                var has_page_elements = has_pages_element.Where(t => t.Element("pages").Elements("page").Any());
                if (!has_page_elements.Any())
                    return null;

                // item elements with pages, some page elements in it and one or more item elements inside the page elements
                var has_item_in_page_elements = has_page_elements.Where(t => t.Element("pages").Elements("page").Elements("item").Any());
                if (!has_item_in_page_elements.Any())
                    return null;

                if (string.IsNullOrEmpty(folder))
                {
                    // page elements with the correct name attribute
                    var has_correct_name_attribute_in_page = has_item_in_page_elements.Where(t => t.Element("pages").Elements("page").Where(s => s.Attribute("name").Value.ToLower().Equals(page.ToLower())).Any());
                    if (!has_correct_name_attribute_in_page.Any())
                        return null;


                    // item elements with item elements in it which have the correct value
                    var has_correct_item_value = has_correct_name_attribute_in_page.Where(t => t.Element("pages").Elements("page").Elements("item").Where(s => s.Value.ToLower().Equals(item.ToLower())).Any());
                    if (has_correct_item_value.Any())
                        return has_correct_item_value.Single();
                    else
                        return null;
                }
                else
                {
                    // item elements with page elements with an folder attribute
                    var has_page_with_folder_attribute = has_item_in_page_elements.Where(t => t.Element("pages").Elements("page").Attributes("folder").Any());
                    if (!has_page_with_folder_attribute.Any())
                        return null;


                    // item with page elements with the correct name and folder path
                    var has_correct_name_attribute_in_page = has_page_with_folder_attribute.Where(t => t.Element("pages").Elements("page").Where(s => (s.Attributes("folder").Any()) && (s.Attribute("folder").Value.ToLower().Equals(folder.ToLower())) && (s.Attribute("name").Value.ToLower().Equals(page.ToLower()))).Any());
                    if (!has_correct_name_attribute_in_page.Any())
                        return null;


                    // item with item elements inside page elements with the correct value
                    var has_correct_value_in_item_inside_page_element = has_correct_name_attribute_in_page.Where(t => t.Element("pages").Elements("page").Elements("item").Where(s => s.Value.ToLower().Equals(item.ToLower())).Any());
                    if (has_correct_value_in_item_inside_page_element.Any())
                        return has_correct_value_in_item_inside_page_element.Single();
                    else
                        return null;
                }
            }
            else
                return null;
        }

        public static XElement loadXItem(string folder, string page, string item)
        {
            // split folder string by '/' character
            // loop through Array to find the folder element with the correct name
            // EXC :: folder not null but item is shared...

            // folder is null
            if (string.IsNullOrEmpty(folder))
            {
                var xItem = loadXItem_recursive(DataIO.LoadData.Element("root"), new string[] {""}, 0, page, item);
                if (xItem != null)
                    return xItem;

                var sharedXItem = loadSharedXItem(folder, page, item);
                if (sharedXItem != null)
                    return sharedXItem;

                else
                    return null;

            }
            else
            {
                var xItem = loadXItem_recursive(DataIO.LoadData.Element("root"), folder.Split('/'), 0, page, item);
                if (xItem != null)
                    return xItem;

                var sharedXItem = loadSharedXItem(folder, page, item);
                if (sharedXItem != null)
                    return sharedXItem;
                
                else
                    return null;
            }
        }

        #region log stuff
        public static void addToLog(string message)
        {
            // check if log directory exists and create it if not.
            DirectoryInfo logDir = new DirectoryInfo(LogLocation);
            if (!logDir.Exists)
                logDir.Create();

            // create XML File for log and save it into the log folder
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("root",
                    new XElement("datetime", DateTime.Now.ToString()),
                    new XElement("message", message)));

            doc.Save(LogLocation + "/" + Guid.NewGuid().ToString() + ".xml");
        }
        #endregion
    }
    public class ItemContent
    {
        public XElement elementContent;
        public string folder;
        public string page;
        public string item;
        public string type;
        public string shared_item;
    }
}

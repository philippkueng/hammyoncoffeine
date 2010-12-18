using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace hammyoncoffeine.Core
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        public ImageHandler()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.QueryString["image"]))
            {
                string queryString = context.Request.QueryString["image"];
                OnServing(Website_Helpers.getFileNameFromQueryString(queryString));
                string fileName = Website_Helpers.getFileNameFromQueryString(queryString);
                try
                {
                    string folder = context.Server.MapPath("~/App_Data/files" + Website_Helpers.getFolderPathFromQueryString(queryString));
                    FileInfo info = new FileInfo(folder + Path.DirectorySeparatorChar + fileName);

                    if (info.Exists && info.Directory.FullName.StartsWith(folder, StringComparison.OrdinalIgnoreCase))
                    {
                        //context.Response.AppendHeader("Content-Disposition", "inline; filename=\"" + fileName + "\"");
                        //SetContentType(context, fileName);

                        //if (Helpers.SetConditionalGetHeaders(info.CreationTimeUtc))
                        //    return;

                        context.Response.Cache.SetCacheability(HttpCacheability.Public);
                        context.Response.Cache.SetExpires(DateTime.Now.AddYears(1));

                        if (Website_Helpers.SetConditionalGetHeaders(info.CreationTimeUtc))
                            return;

                        int index = fileName.LastIndexOf(".") + 1;
                        string extension = fileName.Substring(index).ToUpperInvariant();

                        // Fix for IE not handling jpg image types
                        if (string.Compare(extension, "JPG") == 0)
                            context.Response.ContentType = "image/jpeg";
                        else
                            context.Response.ContentType = "image/" + extension;
                        //####

                        context.Response.TransmitFile(info.FullName);
                        OnServed(fileName);
                    }
                    else
                    {
                        OnBadRequest(fileName);
                        context.Response.Redirect(Helpers.AbsoluteWebsiteRoot + "error404.aspx");
                    }
                }
                catch (Exception)
                {
                    OnBadRequest(fileName);
                    context.Response.Redirect(Helpers.AbsoluteWebsiteRoot + "error404.aspx");
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the requested file does not exist;
        /// </summary>
        public static event EventHandler<EventArgs> Serving;

        private static void OnServing(string file)
        {
            if (Serving != null)
            {
                Serving(file, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when a file is served;
        /// </summary>
        public static event EventHandler<EventArgs> Served;

        private static void OnServed(string file)
        {
            if (Served != null)
            {
                Served(file, EventArgs.Empty);
            }
            // Hier könnte man einen Counter einbauen...
        }

        /// <summary>
        /// Occurs when the requested file does not exist;
        /// </summary>
        public static event EventHandler<EventArgs> BadRequest;

        private static void OnBadRequest(string file)
        {
            if (BadRequest != null)
            {
                BadRequest(file, EventArgs.Empty);
            }
        }

        #endregion
    }
}
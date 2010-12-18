using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace hammyoncoffeine.Core
{
    /// <summary>
    /// Summary description for FileHandler
    /// </summary>
    public class FileHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.QueryString["file"]))
            {
                string queryString = context.Request.QueryString["file"];
                OnServing(Website_Helpers.getFileNameFromQueryString(queryString));
                string fileName = Website_Helpers.getFileNameFromQueryString(queryString);
                try
                {
                    string folder = context.Server.MapPath("~/App_Data/files" + Website_Helpers.getFolderPathFromQueryString(queryString));
                    FileInfo info = new FileInfo(folder + Path.DirectorySeparatorChar + fileName);

                    if (info.Exists && info.Directory.FullName.StartsWith(folder, StringComparison.OrdinalIgnoreCase))
                    {
                        context.Response.AppendHeader("Content-Disposition", "inline; filename=\"" + fileName + "\"");
                        SetContentType(context, fileName);

                        if (Website_Helpers.SetConditionalGetHeaders(info.CreationTimeUtc))
                            return;

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

        /// <summary>
        /// Sets the content type depending on the filename's extension.
        /// </summary>
        private static void SetContentType(HttpContext context, string fileName)
        {
            if (fileName.EndsWith(".pdf"))
            {
                context.Response.AddHeader("Content-Type", "application/pdf");
            }
            else
            {
                context.Response.AddHeader("Content-Type", "application/octet-stream");
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

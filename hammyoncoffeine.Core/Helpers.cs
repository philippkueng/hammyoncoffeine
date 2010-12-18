#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
#endregion

namespace hammyoncoffeine.Core
{
    public class Helpers
    {
        /// <summary>
        /// Gets the relative root of this website
        /// Example: /pkstudioCMS.Website/
        /// </summary>
        private static string _RelativeWebsiteRoot;
        public static string RelativeWebsiteRoot
        {
            get
            {
                if (string.IsNullOrEmpty(_RelativeWebsiteRoot))
                    _RelativeWebsiteRoot = VirtualPathUtility.ToAbsolute("~/"); // TODO :: make this settings editable via web.config file

                return _RelativeWebsiteRoot;
            }
        }

        /// <summary>
        /// Gets the absolute root of this website
        /// Example: http://localhost:55935/pkstudioCMS.Website/
        /// </summary>
        public static Uri AbsoluteWebsiteRoot
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context == null)
                    throw new System.Net.WebException("The current HttpContext is null");

                if (context.Items["absoluteurl"] == null)
                    context.Items["absoluteurl"] = new Uri(context.Request.Url.GetLeftPart(UriPartial.Authority) + RelativeWebsiteRoot);

                return context.Items["absoluteurl"] as Uri;
            }
        }

        /// <summary>
        /// Gets the defaultpage setting from settings.xml inside the App_Data folder and chaches it.
        /// Example: default
        /// </summary>
        private static string _defaultpage;
        public static string defaultpage
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultpage))
                    _defaultpage = DataIO.LoadSettings.Element("root").Elements("defaultpage").Single().Value.ToString();
                return _defaultpage;
            }
        }



    }
}

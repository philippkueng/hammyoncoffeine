#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml;
using System.Text.RegularExpressions;
#endregion

namespace hammyoncoffeine.Core
{
    /// <summary>
    /// class which handles the URL rewrite 
    /// </summary>
    public class URLRewrite: System.Web.IHttpModule 
    {

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        /// <summary>
        /// Gets called if an *.aspx page get requested. Rewrite URL from .../home.aspx to .../default.aspx?p=home
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            string currentURL = context.Request.Url.ToString().ToLower();
            if (!currentURL.Contains("/admin"))
            {
                string[] temp_Array = get_file_name_out_of_url(currentURL);

                //string pageName = get_file_name_out_of_url(currentURL)[0];
                string pageName = temp_Array[0];
                string folders = temp_Array[2];
                string query = temp_Array[1];

                if (!string.IsNullOrEmpty(pageName) && pageName != ">default" && pageName != ">error")
                {
                    // TODO :: 
                    //string query = get_file_name_out_of_url(currentURL)[1];
       
                    if (!string.IsNullOrEmpty(query))
                        if(!string.IsNullOrEmpty(folders))
                            context.RewritePath(Helpers.RelativeWebsiteRoot + Configuration.Name_of_ASPX_Site + "?p=" + pageName + "&f=" + folders + "&" + query);
                        else
                            context.RewritePath(Helpers.RelativeWebsiteRoot + Configuration.Name_of_ASPX_Site + "?p=" + pageName + "&" + query);
                    else
                        if(!string.IsNullOrEmpty(folders))
                            context.RewritePath(Helpers.RelativeWebsiteRoot + Configuration.Name_of_ASPX_Site + "?p=" + pageName + "&f=" + folders);
                        else
                            context.RewritePath(Helpers.RelativeWebsiteRoot + Configuration.Name_of_ASPX_Site + "?p=" + pageName);
                }
                else
                {
                    if (pageName == ">default")
                    {
                        pageName = Helpers.defaultpage; // defaultpage is defined inside settings.xml
                        if (!string.IsNullOrEmpty(pageName))
                        {
                            if(!string.IsNullOrEmpty(query))
                                if(!string.IsNullOrEmpty(folders))
                                    context.RewritePath(Helpers.RelativeWebsiteRoot + Configuration.Name_of_ASPX_Site + "?p=" + pageName + "&f=" + folders + "&" + query);
                                else
                                    context.RewritePath(Helpers.RelativeWebsiteRoot + Configuration.Name_of_ASPX_Site + "?p=" + pageName + "&" + query);
                            else
                                if (!string.IsNullOrEmpty(folders))
                                    context.RewritePath(Helpers.RelativeWebsiteRoot + Configuration.Name_of_ASPX_Site + "?p=" + pageName + "&f=" + folders);
                                else
                                    context.RewritePath(Helpers.RelativeWebsiteRoot + Configuration.Name_of_ASPX_Site + "?p=" + pageName);
                        }
                    }
                    // triggered by a non .aspx page -> pass without url rewrite
                }
            }
        }

        /// <summary>
        /// parses the url with regex. 
        /// Input: http://www.example.com/home.htm 
        /// Output: home
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string[] get_file_name_out_of_url(string p)
        {
            //Regex expression = new Regex("([\\w.-]+)(\\.[htm|html|aspx|php])", RegexOptions.None);
            //Regex expression = new Regex("https?://([\\w-]+).+[$\\/]([\\w-_+,]+)[$\\.aspx]", RegexOptions.None);
            
            // regex expression without the folder recognition
            // http://www.example.com/sample.aspx?bla=foo&foo=bla
            // groups[1] www.
            // groups[2] www
            // groups[3] sample
            // groups[4] aspx
            // groups[5] bla=foo&foo=bla
            //Regex expression = new Regex("https?://(([\\w]+)\\.)?.+[$\\/]([\\w-_,]+)?\\.?(aspx|axd)?[\\?|\\/]?(.+)?", RegexOptions.None);

            Regex expression;

            if(string.IsNullOrEmpty(hammyoncoffeine.Core.DataIO.websiteroot))
            // http://www.example.com:8080/folder1/folder2/folder3/sample.aspx?bla=foo&foo=bla
                expression = new Regex("https?://(([\\w]+)\\.)?([\\w\\d\\.\\:-]+)([\\/\\w\\.\\-_,]+)?[$\\/]([\\w-_,]+)?\\.?(aspx)?[\\?|\\/]?(.+)?", RegexOptions.None);
            // groups[1] www.
            // groups[2] www
            // groups[3] example.com:8080
            // groups[4] /folder1/folder2/folder3
            // groups[5] sample
            // groups[6] aspx
            // groups[7] bla=foo&foo=bla
            else
                expression = new Regex("https?://(([\\w]+)\\.)?([\\w\\d\\.\\:-]+)" + hammyoncoffeine.Core.DataIO.websiteroot + "([\\/\\w\\.\\-_,]+)?[$\\/]([\\w-_,]+)?\\.?(aspx)?[\\?|\\/]?(.+)?", RegexOptions.None);

            if (expression.IsMatch(p))
            {
                try
                {
                    Match current_expression = expression.Match(p);

                    // if the absolute website root gets called eg. http://www.example.com/
                    if (current_expression.Groups[5].Length == 0 && current_expression.Groups[6].Length == 0 && current_expression.Groups[4].Length == 0)
                        return new string[] { ">default", null, null };
                    // a regular aspx or axd page gets called
                    else if (current_expression.Groups[6].ToString() == "aspx")
                        if (current_expression.Groups[5].ToString() == "error")
                            return new string[] { ">error", null, null };
                        else // return array for a regular page
                            return new string[] { current_expression.Groups[5].ToString(), current_expression.Groups[7].ToString(), current_expression.Groups[4].ToString() };
                    return new string[] { null, null, null };
                }
                catch // fails if some strange characters get selected by the regex -> regex is not optimal then
                {
                    // TODO :: put the error into the log file for further analysis
                    return new string[] { null, null, null };
                }
            }
            else
                return new string[] { null, null, null };         
        }
    }
}

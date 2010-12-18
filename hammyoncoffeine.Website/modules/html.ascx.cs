using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace hammyoncoffeine.Website.Modules
{

    public partial class html : System.Web.UI.UserControl
    {
        public object[] Data;
        public string content;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Data[0] => pageName
            // Data[1] => XElement (elementContent)
            content = ((XElement)Data[1]).Value.ToString();


            //string foo = "/foo/bla/sample";
            //// { } -- foo -- bla -- sample --

            //string[] array = foo.Split('/');

            //content = "";
            //foreach (var item in array)
            //{
            //    content += item + " -- ";
            //}

        }
    }
}
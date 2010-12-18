#region using
using System;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Reflection;
#endregion

namespace hammyoncoffeine.Core
{
	public class ViewManager
	{
	    public static string RenderView(string path)
	    {
	        return RenderView(path, null, null);
	    }
	
	    public static string RenderView(string path, object data, string fieldName)
	    {
	        Page pageHolder = new Page();
	        UserControl viewControl = (UserControl) pageHolder.LoadControl(path);
	
	        if (data != null)
	        {
	            Type viewControlType = viewControl.GetType();    
	            FieldInfo field = viewControlType.GetField(fieldName);
	
	            if (field != null)
	            {
	                field.SetValue(viewControl, data);
	            }
	            else
	            {
	                throw new Exception("View file: " + path + " does not have a public Data property");
	            }
	        }
	
	        pageHolder.Controls.Add(viewControl);
	
	        StringWriter output = new StringWriter();
	
	
	        try
	        {
	            HttpContext.Current.Server.Execute(pageHolder, output, false);
	        }
	        catch
	        {
	            // TODO :: implement error logging here
	            //Website_Helpers.sendError(path + " || " + ex.Message);
	        }
	        return output.ToString();
	    }
	}
}
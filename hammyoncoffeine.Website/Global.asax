<%@ Application Language="C#" %>

<script runat="server">
    

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        // Code Example from http://blog.stackoverflow.com/2008/07/easy-background-tasks-in-aspnet/
        //AddTask("DoBackup", 60);
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        Exception objErr = Server.GetLastError().GetBaseException();
        string err = "Error Caught in Application_Error event\n" +
                "Error in: " + Request.Url.ToString() +
                "\nError Message:" + objErr.Message.ToString() +
                "\nStack Trace:" + objErr.StackTrace.ToString();
        //System.Diagnostics.EventLog.WriteEntry("Sample_WebApp", err, System.Diagnostics.EventLogEntryType.Error);
        // Damit ich die Fehler wärend der Entwicklung alle sehen kann, werden die Fehler an mich gesendet
        hammyoncoffeine.Core.Website_Helpers.sendError("Application Error ---- " + err);
        Server.ClearError();


    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
    private static CacheItemRemovedCallback onCacheRemove = null;
    private void AddTask(string name, int seconds)
    {
        onCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
        HttpRuntime.Cache.Insert(name, seconds, null,
            DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
            CacheItemPriority.NotRemovable, onCacheRemove);
    }

    public void CacheItemRemoved(string k, object v, CacheItemRemovedReason r)
    {
        // do stuff here if it matches our taskname, like WebRequest
        // re-add our task so it recurs
        AddTask(k, Convert.ToInt32(v));
    }

       
</script>

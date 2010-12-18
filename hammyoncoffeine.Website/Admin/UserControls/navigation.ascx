<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_navigation" Codebehind="navigation.ascx.cs" %>

<ul class="navigation" runat="server" id="navigation">
    <%= active[0] %> class="first"><a href="Default.aspx?c=content">Inhalt</a></li>
    <%= active[1] %> ><a href="Default.aspx?c=user">Benutzer</a></li>
    <%= active[2] %> ><a href="Default.aspx?c=files">Dateien</a></li>
    <%= active[3] %> ><a href="Default.aspx?c=properties">Einstellungen</a></li>
    <%--<%= active[4] %> ><a href="Default.aspx?c=myaccount">Mein Konto</a></li>--%>
    <%= active[5] %> class="last"><%= userLink %><%--<a href="Default.aspx?c=logout">Abmelden</a>--%></li>
</ul>

<%--<%= Test %>--%>
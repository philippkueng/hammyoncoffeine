<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_type_navigation" Codebehind="type_navigation.ascx.cs" %>


<div class="two_third">
    <span class="header"><a href="Default.aspx">Alle</a> > <%=page %> > <%=item %></span>
        <div class="box">
        <span class="title">Menu tiefe</span>
        <span class="description">Bsp. 1 für alle Seiten im aktuellen Ordner</span>
        <asp:TextBox runat="Server" ID="txtContent" CssClass="input"/>
        <asp:Button runat="server" ID="save" Text="Speichern" OnClick="save_OnClick" CssClass="button" />
        <span runat="server" id="message" visible="false"></span>
        </div>
</div>

<%--<div class="one_third">
    <span>&nbsp;</span>
</div>--%>
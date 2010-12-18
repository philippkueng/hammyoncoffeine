<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_type_html" Codebehind="type_html.ascx.cs" %>


<div class="two_third">
    <span class="header"><a href="Default.aspx">Alle</a> > <%=page %> > <%=item %></span>
<%--<form runat="server" id="form1">--%>
        <asp:TextBox runat="Server" ID="txtContent" CssClass="textarea" Width="584px" Height="250px" TextMode="MultiLine" />
        <asp:Button runat="server" ID="save" Text="Speichern" OnClick="save_OnClick" CssClass="button" />
        <span runat="server" id="message" visible="false"></span>
    <%--</form>--%>
</div>

<%--<div class="one_third">
    <span>&nbsp;</span>
</div>--%>
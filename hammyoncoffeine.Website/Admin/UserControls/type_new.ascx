<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_type_new" Codebehind="type_new.ascx.cs" %>


<div class="two_third">
    <span class="header"><a href="Default.aspx">Alle</a> > <%=page %> > <%=item %></span>
    <%--<form runat="server" id="form1">--%>
        <span class="title">Bitte wählen Sie den Typ für dieses Element </span>
        <span class="description">Je nach Typ können verschiedene Inhalte verwaltet werden</span>
        <asp:DropDownList runat="server" ID="availableTypes" CssClass="dropbox"></asp:DropDownList>
        <asp:Button runat="server" ID="save" Text="Zuweisen" OnClick="save_OnClick" CssClass="button" />
        <span runat="server" id="message" visible="false"></span>
    <%--</form>--%>
</div>
<div class="one_third">
    <span>&nbsp;</span>
</div>
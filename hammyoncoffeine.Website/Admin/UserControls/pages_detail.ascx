<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_pages_detail" Codebehind="pages_detail.ascx.cs" %>

<span class="pages_detail header_icon"><%= pageDetailsTitle %></span>
<div class="box">
    <asp:Button runat="server" ID="deletePage" CssClass="button delete-icon" />
</div>
<div class="pages_detail box">
    <%--<form runat="server" id="form1">--%>
        <span class="title title-icon">Seiten Titel</span>
        <span class="description">Dieser Titel wird im Browser angezeigt, und von den Suchmaschinen indexiert.</span>
        <input class="input" type="text" runat="server" id="pageTitleElement" />
        
        <span class="title file-embed">Seiten Quellcode</span>
        <span class="description">Falls Sie sich mit HTML nicht auskennen, lassen Sie hier die Finger davon.</span>
        <asp:TextBox runat="Server" ID="pageSourceCode" CssClass="textarea" Width="876px" Height="250px" TextMode="MultiLine" />
        
        <asp:Button runat="server" ID="save" Text="Speichern" CssClass="button" />
        <span class="message" runat="server" id="message"></span>
    <%--</form>--%>
</div>

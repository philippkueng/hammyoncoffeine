<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_user_item" Codebehind="user_item.ascx.cs" %>
<%@ Register Assembly="FreshClickmedia.Web" Namespace="FreshClickMedia.Web.UI.WebControls"
    TagPrefix="fcm" %>
<div runat="server" id="box" class="box relative" >
    <span class="title user-icon"><%= Data[0] %></span>
    <asp:Button runat="server" ID="delete" Text="Löschen" CssClass="delete" />
    <fcm:Gravatar class="gravatar" ID="Gravatar1" runat="server" OutputGravatarSiteLink="false" Size="80" />
    <ul>
        <li>
            <a class="mail" href="mailto:<%= Data[1] %>"><%= Data[1] %></a>
        </li>
        <li class="mobile-phone">
            <%= Data[2] %>
        </li>
    </ul>
</div>
<div class="box relative" runat="server" id="message">

</div>
<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_files_directory" Codebehind="files_directory.ascx.cs" %>
<span class="header_icon folders_icon"><%= DirectoryHeader %></span>
<asp:PlaceHolder runat="server" ID="controlsPlaceHolder">
    <div class="box">
        <asp:Button runat="server" ID="deleteFolder" CssClass="button delete-icon" />
    </div>
</asp:PlaceHolder>
<ul id="foldersUL" runat="server" class="folders">
    <%= Folders %>
</ul>
<ul id="filesUL" runat="server" class="files">
    <%= Files %>
</ul>
<ul id="messageUL" runat="server" class="messageUL">
    <%= Message %>
</ul>
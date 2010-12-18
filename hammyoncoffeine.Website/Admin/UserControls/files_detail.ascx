<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_files_detail" Codebehind="files_detail.ascx.cs" %>
<span class="header_icon folders_icon"><%= DirectoryHeader %></span>
<div class="box">
    <span class="title file-download">
        <%= DownloadFile %>
    </span>
    <span class="title file-link">
        Link
    </span>
    <span class="description">Klicken Sie ins Textfeld, und drücken Sie 'Ctrl + A' um den Text zu markieren.</span>
    <input type="text" class="input" runat="server" id="fileLink" />
    <span class="title file-embed">
        Datei einfügen
    </span>
    <textarea runat="server" id="fileEmbed" class="embed-textarea" ></textarea>
    
</div>
<%--<span><%= Message %></span>--%>
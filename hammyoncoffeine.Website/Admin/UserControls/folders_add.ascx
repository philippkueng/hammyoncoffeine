<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_folders_add" Codebehind="folders_add.ascx.cs" %>

<div class="pages_detail box">
    <span class="title left">Ordner Name </span>
    <asp:TextBox runat="server" ID="folder_name" CssClass="input"></asp:TextBox>
    
    <asp:Button runat="server" ID="addFolder" Text="Ordner hinzufügen" CssClass="button" />
    <span class="message" runat="server" id="message"></span>
</div>
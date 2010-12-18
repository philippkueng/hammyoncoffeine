<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_files" Codebehind="files.ascx.cs" %>
<form runat="server" id="form1" >
<div class="two_third">
    <%--<span class="header">
        Dateien
    </span> --%>
    <asp:PlaceHolder runat="server" ID="filesPlaceHolder" />
    <%--<div class="box" >
        <span class="title">Max Mustermann</span>
        <fcm:Gravatar class="gravatar" ID="Gravatar1" runat="server" Email="username@domain.com" DefaultImage="http://pkstudio.ch/cms/admin/gravatar_default.jpg" OutputGravatarSiteLink="false" Size="80" />
        <ul>
            <li>
                <a href="mailto:max.mustermann@test.com">max.mustermann@test.com</a>
            </li>
            <li>
                079 / 385 53 78
            </li>
        </ul>
    </div>--%>
</div>

<div class="one_third">
    <span class="header_icon file_add">
        Datei hochladen
    </span>
    <div class="box">
        <span class="title">Datei</span>
        <asp:FileUpload ID="filePath" runat="server" CssClass="input" />
        <%--<input type="file" runat="server" id="filePath" class="input" />--%>
<%--        <span class="title">Ordner</span>
        <asp:DropDownList runat="server" ID="file_availableFolders" CssClass="dropbox"></asp:DropDownList>--%>
        <asp:Button runat="server" ID="upload" Text="Datei hochladen" CssClass="button" />
        <span class="message" runat="server" id="file_message"></span>
    </div>
    
    <span class="header_icon folder_add" style="margin-top: 20px;">
        Ordner erstellen
    </span>
    <div class="box">
        <span class="title">Ordner Name</span>
        <input type="text" runat="server" id="folderName" class="input" />
<%--        <span class="title">Über-Ordner </span>
        <asp:DropDownList runat="server" ID="folder_availableFolders" CssClass="dropbox"></asp:DropDownList>--%>
        <asp:Button runat="server" ID="createFolder" Text="Ordner erstellen" CssClass="button" />
        <span class="message" runat="server" id="folder_message"></span>
    </div>

</div>
 </form>
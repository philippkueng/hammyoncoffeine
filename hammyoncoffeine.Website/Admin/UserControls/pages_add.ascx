<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_pages_add" Codebehind="pages_add.ascx.cs" %>

<div class="pages_detail box">
    <span class="title left">Neue Seite hinzufügen durch </span>
    <asp:DropDownList runat="server" ID="pageAddOptions" CssClass="input" AutoPostBack="true" >
    </asp:DropDownList>

    <asp:PlaceHolder runat="server" ID="scPlaceHolder">
        <hr />
        <span class="title title-icon">Dateiname der Seite</span>
        <span class="description">Bsp. Default.htm (Die Datei wird unter diesem Namen gespeichert)</span>
        <input type="text" class="input" runat="server" id="pageName" />

        <span class="title file-embed">Quellcode der Seite</span>
        <span class="description">Fügen Sie hier den gesamten Quellcode der Seite inkl. html, head und body Tags ein.</span>
        <asp:TextBox runat="Server" ID="pageSourceCode" CssClass="textarea" Width="876px" Height="250px" TextMode="MultiLine" />

        
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="fuPlaceHolder">
        <hr />
        <span class="title file-upload">Datei zum hochladen auswählen</span>
        <asp:FileUpload runat="server" ID="fileUpload" CssClass="input" />
    </asp:PlaceHolder>
    
    <asp:Button runat="server" ID="addPage" Text="Seite hinzufügen" CssClass="button" />
    <span class="message" runat="server" id="message"></span>
</div>
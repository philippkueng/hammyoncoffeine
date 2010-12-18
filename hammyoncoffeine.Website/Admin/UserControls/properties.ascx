<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_properties" Codebehind="properties.ascx.cs" %>

<div class="two_third">
    <span class="header_icon properties">
        Einstellungen
    </span>
    <form runat="server" id="form1" >
        <div class="box">
            <span class="title">Website Adresse </span>
            <span class="description">Beispiel: domain.com/virtualdirectory</span>
            <input class="input" runat="server" id="websiteroot" type="text" />
            
            <span class="title">Standard Seite</span>
            <span class="description">Dies ist die Seite die aufgerufen wird, falls http://ihre-domain.com aufgerufen wird.</span>
            <span class="description">Muss nur ausgefüllt werden, falls die Standard Seite nicht default oder index heisst.</span>
            <input class="input" runat="server" id="defaultpage" type="text" />
            
            <span class="title">Umzuwandelnde Subdomains (Optional)</span>
            <span class="description">Falls das pkstudioCMS noch unter anderen Subdomains eingesetzt wird, und diese hier angegeben sind, werden allfällige Links in den *.htm Seite automatisch angepasst.</span>
            <span class="description">Subdomains mit Leerschlag trennen. Und inkl. Punkt am Schluss. Bsp: blog. lorem. ipsum.</span>
            <input class="input" runat="server" id="subdomains" type="text" />
        </div>
    
    <asp:Button runat="server" ID="save" Text="Speichern" CssClass="button" />
    <span class="message" runat="server" id="message"></span>
    </form>

</div>

<div class="one_third">
</div>

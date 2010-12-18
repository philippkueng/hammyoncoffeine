<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_activate" Codebehind="activate.ascx.cs" %>
<div class="two_third">    
    <span class="header">Konto aktivieren</span>
    <form runat="server" id="form1">    
        <div class="box">
            <span class="title">Benutzername</span>
            <span class="description">Ihr Benutzername wurde Ihnen im Email zugesendet.</span>
            <input type="text" runat="server" id="userName" class="input" />
            <span>Passwort</span>
            <span class="description">Bitte geben Sie Ihr Passwort 2 mal ein.</span>
            <input class="input" runat="server" id="passWord1" type="password" />
            <input class="input" runat="server" id="passWord2" type="password" />
        </div>
        <div class="box">
            <ul class="split">
                <li>
                    <span class="title">Vorname</span>
                    <input class="input" type="text" runat="server" id="surname" />
                 </li>
                <li>
                    <span class="title">Nachname</span>
                    <input class="input" type="text" runat="server" id="name" />
                </li>
            </ul>
            <span class="title">Telefonnummer / Mobilnummer</span>
            <input class="input" type="text" runat="server" id="phoneNumber" />
        </div>
        <asp:Button runat="server" ID="save" Text="Speichern" CssClass="button" />
        <span class="message" runat="server" id="message"></span>
    </form>
</div>
<div class="one_third">
</div>
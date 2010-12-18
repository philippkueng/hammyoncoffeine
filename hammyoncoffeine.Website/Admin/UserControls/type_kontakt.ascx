<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_type_kontakt" Codebehind="type_kontakt.ascx.cs" %>


<div class="two_third">
    <span class="header"><a href="Default.aspx">Alle</a> > <%=page %> > <%=item %></span>
        <label for="emailAddress">E-mail Adresse</label>
        <asp:TextBox runat="server" ID="emailAddress" CssClass="input" Width="584px" TextMode="SingleLine" />
        <label for="smtpServer">SMTP Server</label>
        <asp:TextBox runat="server" ID="smtpServer" CssClass="input" Width="584px" TextMode="SingleLine" />
        <label for="portNumber">Port Nummer vom SMTP Server</label>
        <asp:TextBox runat="server" ID="portNumber" CssClass="input" Width="584px" TextMode="SingleLine" />
        <label for="username">SMTP Benutzername</label>
        <asp:TextBox runat="server" ID="username" CssClass="input" Width="584px" TextMode="SingleLine" />
        <label for="password">SMTP Passwort</label>
        <asp:TextBox runat="server" ID="password" CssClass="input" Width="584px" TextMode="SingleLine" />
        <label for="subjectPrefix">Prefix für das Betreff Feld</label>
        <asp:TextBox runat="server" ID="subjectPrefix" CssClass="input" Width="584px" TextMode="SingleLine" />
        <asp:Button runat="server" ID="save" Text="Speichern & Testmail senden" OnClick="save_OnClick" CssClass="button" />
        <span runat="server" id="message" visible="false"></span>

</div>
<div class="one_third">
    <span>&nbsp;</span>
</div>
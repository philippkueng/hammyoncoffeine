<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_myaccount" Codebehind="myaccount.ascx.cs" %>

<div class="two_third">
    <span class="header_icon user">
        Mein Konto
    </span>
    <form runat="server" id="form1" >
        <div class="box">
        <span class="title">Passwort ändern</span>
        <span class="description">Altes Passwort</span>
        <input class="input" runat="server" id="oldPassWord" type="password" />
        <span class="description">Bitte geben Sie das neue Passwort 2 mal ein.</span>
        <input class="input" runat="server" id="passWord1" type="password" />
        <input class="input" runat="server" id="passWord2" type="password" />
    </div>
    <div class="box">
        <span class="title">Emailadresse ändern</span>
        <input class="input" runat="server" id="emailAddress" type="text" />
    </div>
    <div class="box">
        <ul class="split">
            <li>
                <span class="title">Vorname</span>
                <input class="input" runat="server" id="surname" type="text" />
            </li>
            <li>
                <span class="title">Nachname</span>
                <input class="input" runat="server" id="lastname" type="text" />
            </li>
        </ul>
        <span class="title">Adresse</span>
        <input class="input" runat="server" id="address" type="text" />
        <ul class="split">
            <li>
                <span class="title">PLZ</span>
                <input class="input" runat="server" id="postcode" type="text" />
            </li>
            <li>
                <span class="title">Ort</span>
                <input class="input" runat="server" id="location" type="text" />
            </li>
        </ul>
        <ul class="split">
            <li><span class="title">Telefon</span>
                <input class="input" runat="server" id="phoneNumber" type="text" />
            </li>
            <li><span class="title">Mobil</span>
                <input class="input" runat="server" id="mobileNumber" type="text" />
            </li>
        </ul>
        <span class="title">Website</span>
        <input class="input" runat="server" id="website" type="text" />
    </div>
    <div class="box">
        <span class="title">Benachrichtigungen erhalten (Not Yet Implemented)</span>
        <span class="description">Falls mehrere Personen Daten auf der Seite ändern, ist es empfehlenswert Benachrichtigungen zu abonnieren, damit allfällige Fehler schnell beseitigt werden können.</span>
        <input class="checkbox" runat="server" id="notifications" type="checkbox" name="notifications" />
        <label class="description" for="notifications">Benachrichtigungen abonnieren</label>
    </div>
    
    <asp:Button runat="server" ID="save" Text="Speichern" CssClass="button" />
    <span class="message" runat="server" id="message"></span>
    </form>

</div>

<div class="one_third">
</div>

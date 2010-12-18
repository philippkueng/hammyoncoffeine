<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_users" Codebehind="users.ascx.cs" %>
<form runat="server" id="form1" >
<div class="two_third">
    <span class="header_icon users">
        Benutzer
    </span> 
    <asp:PlaceHolder runat="server" ID="usersPlaceHolder" />
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
    <span class="header_icon user_add">
        Neuen Benutzer anlegen
    </span>
    <div class="box">
        <span class="title">Benutzername</span>
        <span class="description">Bsp. max.mustermann</span>
        <input type="text" runat="server" id="userName" class="input" />
        <span class="title">Email Adresse</span>
        <input type="text" runat="server" id="emailAddress" class="input" />
        <asp:Button runat="server" ID="save" Text="Benutzer einladen" CssClass="button" />
        <span class="message" runat="server" id="message"></span>
    </div>
    

</div>
 </form>
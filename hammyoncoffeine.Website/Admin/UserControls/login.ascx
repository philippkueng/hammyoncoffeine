<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_login" Codebehind="login.ascx.cs" %>

<div class="one_third no-margin-left">

<form runat="server" id="form1" class="login">
        <span class="header_icon user">Anmelden </span>  
        <div class="box">  
            <span class="title">Benutzername</span>
            <input type="text" runat="server" id="userName" class="input"/>
            <span>Passwort</span>
            <input type="password" runat="server" id="passWord" class="input"/>
            <input type="checkbox" runat="server" id="checkBox" name="checkBox" class="checkbox" value="checked" />
            <label for="checkBox" class="description">Anmeldedaten speichern</label>    
        </div>
        <asp:Button runat="server" ID="loginUser" CssClass="button" Text="Anmelden" />
        <span class="error" runat="server" id="error"></span>
    </form>
</div>
<div class="two_third">
    

</div>
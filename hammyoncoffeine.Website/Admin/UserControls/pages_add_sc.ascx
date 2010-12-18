<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_pages_add_sc" Codebehind="pages_add_sc.ascx.cs" %>
<span class="title">Dateiname der Seite</span>
<span class="description">Bsp. Default.htm (Die Datei wird unter diesem Namen gespeichert)</span>
<input type="text" class="input" runat="server" id="pageName" />

<span class="title">Quellcode der Seite</span>
<span class="description">Fügen Sie hier den gesamten Quellcode der Seite inkl. html, head und body Tags ein.</span>
<asp:TextBox runat="Server" ID="pageSourceCode" CssClass="textarea" Width="876px" Height="250px" TextMode="MultiLine" />

<asp:Button runat="server" ID="addPage" Text="Seite hinzufügen" CssClass="button" />
<span class="message" runat="server" id="message"></span>
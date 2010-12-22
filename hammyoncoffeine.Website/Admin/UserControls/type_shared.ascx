<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_type_shared" Codebehind="type_shared.ascx.cs" %>

<div class="one_third">
    <asp:PlaceHolder runat="server" ID="convertSingleToShared">
    <span class="header_icon file_add">Erstelle gemeinsames Element</span>
    <div class="box">
        <span class="title">Name des Elements</span>
        <asp:TextBox runat="server" ID="cSiTSh_Textbox" CssClass="input"></asp:TextBox>
        <asp:Button runat="server" ID="cSiTSh_Button" Text="Erstellen" CssClass="button" />
        <asp:Label runat="server" ID="cSiTSh_Label"></asp:Label>
    </div>
    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="linkSingleToShared">
    <span class="header_icon file_add" style="margin-top: 20px;">Linke zu einem gemeinsamen Element</span>
    <div class="box">
        <asp:DropDownList runat="server" ID="lSiTSh_items" CssClass="input">
        
        </asp:DropDownList>
        <asp:Button runat="server" ID="lSiTSh_Button" Text="Verlinken" CssClass="button" />
        <asp:Label runat="server" ID="lSiTSh_Label"></asp:Label>
    </div>
    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="convertSharedToSingle">
    <span class="header_icon file_add" style="margin-top: 20px;">Verwandle in eigenständiges Element</span>
    <div class="box">
        <asp:Button runat="server" ID="cShTSi_Button" Text="Verwandle" CssClass="button" />
        <asp:Label runat="server" ID="cShTSi_Label"></asp:Label>
    </div>
    </asp:PlaceHolder>
</div>
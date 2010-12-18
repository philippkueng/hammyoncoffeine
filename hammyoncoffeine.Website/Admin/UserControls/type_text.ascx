<%@ Control Language="C#" AutoEventWireup="true" Inherits="hammyoncoffeine.Website.Admin_UserControls_type_text" Codebehind="type_text.ascx.cs" %>


<div class="two_third">
    <span class="header"><a href="Default.aspx">Alle</a> > <%=page %> > <%=item %></span>
    <%--<form runat="server" id="form1">--%>
        <script type="text/javascript" src="<%=hammyoncoffeine.Core.Helpers.RelativeWebsiteRoot %>Admin/tiny_mce/tiny_mce.js"></script>
        <script type="text/javascript">
	tinyMCE.init({
		// General options
		mode: "exact",
		language: "de",
		elements : "<%=txtContent.ClientID %>",
		theme: "advanced",
		plugins : "safari,pagebreak,style,layer,table,save,advhr,advcode,advimage,advlink,emotions,iespell,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",
		convert_urls: false,
		
	  // Theme options
		theme_advanced_buttons1: "undo,redo,|,copy,paste,removeformat,|,bold,italic,underline,|,bullist,numlist,blockquote,|,link,image,code,|,formatselect",
                    theme_advanced_buttons2: "",
                    theme_advanced_buttons3: "",
                    theme_advanced_toolbar_location: "top",
                    theme_advanced_toolbar_align: "center",
                    theme_advanced_statusbar_location: "",
                    theme_advanced_resizing: true
		
	});
</script>
        <%--<textarea runat="server" id="textarea" class="textarea"></textarea>--%>
        <asp:TextBox runat="Server" ID="txtContent" CssClass="textarea" Width="608px" Height="250px" TextMode="MultiLine" />
        <asp:Button runat="server" ID="save" Text="Speichern" OnClick="save_OnClick" CssClass="button" />
        <span runat="server" id="message" visible="false"></span>
    <%--</form>--%>
</div>
<div class="one_third">
    <span>&nbsp;</span>
</div>
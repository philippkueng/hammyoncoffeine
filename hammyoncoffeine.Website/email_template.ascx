<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="hammyoncoffeine.Website.email_template" Codebehind="email_template.ascx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Email Template - pkstudioCMS</title>
    <style type="text/css">
        .style1
        {
            color: #999999;
        }
        body
        {
            font-family: Helvetica,Arial,'Liberation Sans',FreeSans,sans-serif;
            background-color: #FFFFFF;
            background-repeat: no-repeat;
        }
        p
        {
            font-family: Helvetica,Arial,'Liberation Sans',FreeSans,sans-serif;
        }
        hr
        {
            background-color: #999999;
            height: 1px;
            width: 100%;
            border: 0;
        }
        h1
        {
            margin: 0;
        }
    </style>
</head>
<body style="font-family: Helvetica,Arial,'Liberation Sans',FreeSans,sans-serif bgcolor: #FFFFFF;">
    <div align="center">
        <table border="0" cellSpacing="0" cellPadding="0" style="width: 600; margin: 0 auto 0 auto; display: block; text-align: left;">
            <colgroup><col width="600" /></colgroup>
            <tbody>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td width="600" height="55">
                    <table>
                        <tr>
                            <td height="55" width="20">
                            </td>
                            <td height="55" width="560">
                                <img alt="pkstudioCMS Logo" src="http://www.pkstudio.ch/cms/pkstudio_logo.png"
                                    style="width: 438; height: 51" />
                            </td>
                            <td height="55" width="20">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="600">
                    <asp:ListView runat="server" ID="ListView1">
                        <LayoutTemplate>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <table style="width: 600" border="0" width="600" valign="top" cellpadding="0" cellspacing="0">
                                <%--<colgroup><col width=600 /></colgroup>--%>
                                <%--<tr>
                                    <td height="20" width="20">
                                        &nbsp;
                                    </td>
                                    <td height="20">
                                        &nbsp;
                                    </td>
                                    <td height="20" width="20">
                                        &nbsp;
                                    </td>
                                </tr>--%>
                                <%--<tr style="background-color: #F4F4F5; bgcolor="#F4F4F5" cellpadding="0" cellspacing="0" BORDER="0">
                                    <td height="10" width="20">
                                        &nbsp;
                                    </td>
                                    <td height="10">
                                        &nbsp;
                                    </td>
                                    <td height="10" width="20">
                                        &nbsp;
                                    </td>
                                </tr>--%>
                                <tr style="LINE-HEIGHT: 30px; background-color: #F4F4F5; FONT-SIZE: 20px; FONT-WEIGHT: normal" bgcolor="#F4F4F5" cellPadding="0" cellSpacing="0" border="0">
                                    <td width="20">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <h1 style="FONT-SIZE: 20px; FONT-WEIGHT: normal">
                                            <%# Eval("Title") %></h1>
                                        
                                    </td>
                                    <td width="20">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr style="padding-top: 10px;">
                                    <td width="20">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <p style="FONT-SIZE: 14px; FONT-WEIGHT: normal">
                                            <%# Eval("Content") %>
                                        </p>
                                    </td>
                                    <td width="20">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td height="20" width="20">
                                        &nbsp;
                                    </td>
                                    <td height="20">
                                        &nbsp;
                                    </td>
                                    <td height="20" width="20">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="text-align: right; FONT-SIZE: 16; color: #808080">
                        <tr>
                            <td width="20">
                                &nbsp;
                            </td>
                            <td width="560">
                                <span class="style1">brought to you by</span>
                                <img style="border: 0;" src="http://www.pkstudio.ch/images/heart.png" />
                                <a href="http://www.pkstudio.ch" style="color: #666666; text-decoration: none;">pkstudio</a>
                            </td>
                            <td width="20">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            </tbody>
        </table>
    </div>
</body>
</html>

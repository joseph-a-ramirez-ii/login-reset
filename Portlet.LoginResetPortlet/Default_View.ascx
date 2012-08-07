<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Default_View.ascx.cs" Inherits="CUS.ICS.LoginReset.Default_View" %>
<%@ Register TagPrefix="common" assembly="Jenzabar.Common" Namespace="Jenzabar.Common.Web.UI.Controls" %>

<div id="divAdminLink" style="text-align: center;" runat="server" visible="false">
	<table class="GrayBordered" style="background-color: #EFEFEF; text-align: center; border: 1px solid #B2B2B2; padding: 10px; margin-left: auto; margin-right: auto;" width="50%"  cellpadding="3" border="0">
		<tr>
			<td align="center"><img title="" alt="*" src="UI\Common\Images\PortletImages\Icons\portlet_admin_icon.gif" />&nbsp;<common:globalizedlinkbutton OnClick="glnkAdmin_Click" id="glnkAdmin" runat="server" TextKey="TXT_CS_ADMIN_THIS_PORTLET"></common:globalizedlinkbutton></td>
		</tr>
	</table>
</div>
<div class="pSection">

<common:GlobalizedLabel ID="IntroMessage" TextKey="CUS_LOGINRESET_INTROMESSAGE" runat="server" />
<table style="text-align: center;" border="0" cellpadding="10" cellspacing="0" 
    style="border-collapse:collapse;">
    <tr>
        <td>
            <asp:Label ID="CurrentUsernameLabel" runat="server" Text="Username" />
        </td>
        <td align="left">
            <asp:TextBox ID="CurrentUsername" runat="server" />
            <asp:RequiredFieldValidator ID="CurrentUsernameRequired" runat="server" 
                ControlToValidate="CurrentUsername" Display="Dynamic" ErrorMessage="New Password is required." 
                ToolTip="Current Username is required." ValidationGroup="ctl00$Username">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
    <td></td>
        <td align="left">
            <asp:LinkButton ID="ContinuePushButton" runat="server" 
                OnClick="ContinuePushButton_Click" Text="Change Password Form" 
                ValidationGroup="ctl00$Username" />
                        
        </td>
    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td align="left">
                            <asp:LinkButton ID="EditSecurityQuestionsPushButton" runat="server" OnClick="EditSecurityQuestions_Click" Text="Edit Security Questions" />
                        
                        </td>
        </tr>
</table>
</div>
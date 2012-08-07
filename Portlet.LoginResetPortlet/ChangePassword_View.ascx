<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword_View.ascx.cs" Inherits="CUS.ICS.LoginReset.ChangePassword_View" %>
<%@ Register TagPrefix="common" assembly="Jenzabar.Common" Namespace="Jenzabar.Common.Web.UI.Controls" %>

<div class="pSection">
<common:GlobalizedLabel ID="PasswordRequirmentsMessage" TextKey="CUS_LOGINRESET_PASSWORDREQUIREMENTS_MESSAGE" runat="server" />
    <table border="0" width="100%" cellpadding="1" cellspacing="0" 
        style="border-collapse:collapse;">
                    <tr>
                        <td style="white-space: nowrap;">
                            <asp:Label ID="CurrentUsernameLabel" runat="server"><b>Username</b></asp:Label>
                        </td>
                            <td>
                            <asp:TextBox ID="CurrentUsername" runat="server" Enabled="False" />
                            </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap;">
                            <asp:Label ID="CurrentPasswordLabel" runat="server" 
                                AssociatedControlID="CurrentPassword"><b>Current Password</b></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <common:GlobalizedLabel ID="AnswerSecurityQuestionMessage" TextKey="CUS_LOGINRESET_ANSWERSECURITYQUESTIONS_MESSAGE" runat="server" />
                            <common:GlobalizedLabel ID="AnswerSecurityQuestionMessageFailure" TextKey="CUS_LOGINRESET_ANSWERSECURITYQUESTIONS_MESSAGEFAILURE" runat="server" EnableViewState="False" />
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap">
                            <asp:Label ID="SecurityQuestionLabel1" runat="server" Text="<b>Security Question #1</b>"></asp:Label>
                        </td>
                        <td rowspan="2">
                            <asp:Label ID="SecurityQuestionText1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        </tr>
                    <tr>
                        <td>
                        <asp:Label ID="SecurityQuestionAnswerLabel1" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="SecurityQuestionAnswer1" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                                        <tr>
                    <td></td>
                    <td></td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap;">
                            <asp:Label ID="SecurityQuestionLabel2" runat="server" Text="<b>Security Question #2<b>"></asp:Label>
                        </td>
                        <td rowspan="2">
                            <asp:Label ID="SecurityQuestionText2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        </tr>
                    <tr>
                        <td>
                            <asp:Label ID="SecurityQuestionAnswerLabel2" runat="server" Text=""></asp:Label></td>
                        <td>
                            <asp:TextBox ID="SecurityQuestionAnswer2" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap;">
                            <asp:Label ID="NewPasswordLabel" runat="server" 
                                AssociatedControlID="NewPassword"><b>New Password</b></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" 
                                ControlToValidate="NewPassword" ErrorMessage="New Password is required." 
                                ToolTip="New Password is required." ValidationGroup="ctl00$ChangePassword1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap;">
                            <asp:Label ID="ConfirmNewPasswordLabel" runat="server" 
                                AssociatedControlID="ConfirmNewPassword"><b>Confirm New Password</b></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" 
                                ControlToValidate="ConfirmNewPassword" 
                                ErrorMessage="Confirm New Password is required." 
                                ToolTip="Confirm New Password is required." 
                                ValidationGroup="ctl00$ChangePassword1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:CompareValidator ID="NewPasswordCompare" runat="server" 
                                ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" 
                                Display="Dynamic" 
                                ErrorMessage="The Confirm New Password must match the New Password entry." 
                                ValidationGroup="ctl00$ChangePassword1"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                    <td></td>
                    <td></td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                        <td>
                            <asp:Button ID="ChangePasswordPushButton" runat="server" OnClick="ChangePasswordPushButton_Click" 
                                CommandName="ChangePassword" Text="Change Password" 
                                ValidationGroup="ctl00$ChangePassword1" />
                        
                        
                            <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" 
                                CommandName="Cancel" Text="Cancel" onclick="CancelPushButton_Click" />
                        </td>
        </tr>
    </table>
    </div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditQuestions_View.ascx.cs" Inherits="CUS.ICS.LoginReset.EditQuestions_View" %>



<style type="text/css">
    .style1
    {
        width: 25%;
    }
</style>



<table border="0" width="95%" cellpadding="1" cellspacing="0" style="border-collapse:collapse;">
    <tr>
        <td align="center" colspan="2">
            <br />
            <i>Define your security questions</i> 
            <br />  
            <br /> 
        </td>
    </tr>
    <tr>
        <td align="center" colspan="2" style="color:Red;">
            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td style="white-space: nowrap" class="style1">
            <asp:Label ID="SecurityQuestionLabel1" runat="server" Text="Security Question #1"></asp:Label>
        </td>
        <td align="left" rowspan="2">
            <asp:Textbox Width="95%" ID="SecurityQuestionText1" runat="server" TextMode="MultiLine" />
        </td>
    </tr>
    <tr>
        <td align="left"  class="style1">
        </td>
    </tr>
    <tr>
        <td class="style1" >
            <asp:Label ID="SecurityQuestionAnswerLabel1" runat="server" Text="Answer"></asp:Label>
        </td>
        <td align="left">
            <asp:TextBox ID="SecurityQuestionAnswer1" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="white-space: nowrap;" class="style1">
            <asp:Label ID="SecurityQuestionLabel2" runat="server" Text="Security Question #2"></asp:Label>
        </td>
        <td rowspan="2" style="text-align: left">
            <asp:Textbox Width="95%" ID="SecurityQuestionText2" runat="server" 
                TextMode="MultiLine" />        
        </td>
    </tr>
    <tr>
        <td class="style1">
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="SecurityQuestionAnswerLabel2" runat="server" Text="Answer"></asp:Label></td>
        <td>
            <asp:TextBox ID="SecurityQuestionAnswer2" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="style1"></td>
        <td></td>
    </tr>
    <tr>
        <td class="style1">
        </td>
        <td>
            <asp:Button ID="SavePushButton" runat="server" OnClick="SavePushButton_Click" 
                CommandName="Save" Text="Save" />
        
            <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" 
                CommandName="Cancel" Text="Cancel" onclick="CancelPushButton_Click" />
        </td>
        </tr>
    </table>
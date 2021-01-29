<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="190298T_ChangePassword.aspx.cs" Inherits="_190298T_IT2163ASSIGNMENT._190298T_ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #forget_form{
            width:30%;
            margin-left: auto;
            margin-right: auto;
        }
        #check_email_form{
            width:30%;
            margin-left: auto;
            margin-right: auto;
        }
        .first_column {
            width: 126px;
        }
    </style>


    <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
    <script type="text/javascript">
        function CheckEmailFeedback() {
            var email = document.getElementById('<%=tb_check_email.ClientID%>').value;
            if (email.search(/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i) == -1) {
                document.getElementById('<%=lbl_checkemailfeedback.ClientID%>').innerHTML = "Email is invalid";
                document.getElementById('<%=lbl_checkemailfeedback.ClientID%>').style.color = "Red";
            }
            else {
                document.getElementById('<%=lbl_checkemailfeedback.ClientID%>').innerHTML = " ";
            }
        }

        function NewPasswordFeedback() {
            var newpassword = document.getElementById('<%=tb_newpwd.ClientID%>').value;

            if (newpassword.length < 10) {
                document.getElementById('<%=lbl_newpwdfeedback.ClientID%>').innerHTML = "Password Length Must Be At Least 10 Characters";
                document.getElementById('<%=lbl_newpwdfeedback.ClientID%>').style.color = "Red";
                return ("too_short");
            }
            else if (newpassword.search(/[0-9]/) == -1) {
                document.getElementById('<%=lbl_newpwdfeedback.ClientID%>').innerHTML = "Password Must Include At Least 1 Number";
                document.getElementById('<%=lbl_newpwdfeedback.ClientID%>').style.color = "Red";
                return ("no_num");
            }
            else if (newpassword.search(/[a-z]/) == -1) {
                document.getElementById('<%=lbl_newpwdfeedback.ClientID%>').innerHTML = "Password Must Include At Least 1 Lowercase Character";
                document.getElementById('<%=lbl_newpwdfeedback.ClientID%>').style.color = "Red";
                return ("no_lower");
            }
            else if (newpassword.search(/[A-Z]/) == -1) {
                document.getElementById('<%=lbl_newpwdfeedback.ClientID%>').innerHTML = "Password Must Include At Least 1 Uppercase Character";
                document.getElementById('<%=lbl_newpwdfeedback.ClientID%>').style.color = "Red";
                return ("no_upper");
            }
            else if (newpassword.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById('<%=lbl_newpwdfeedback.ClientID%>').innerHTML = "Password Must Include At Least 1 Special Character<br>";
                document.getElementById('<%=lbl_newpwdfeedback.ClientID%>').style.color = "Red";
                strike += 1
            }
            else {
                document.getElementById('<%=lbl_newpwdfeedback.ClientID%>').innerHTML = "Password is acceptable";
                document.getElementById('<%=lbl_newpwdfeedback.ClientID%>').style.color = "Green";
                return ("good");
            }
        }

        function CfmPasswordFeedback() {
            var newpassword = document.getElementById('<%=tb_newpwd.ClientID%>').value;
            var cfmpassword = document.getElementById('<%=tb_cfmpwd.ClientID%>').value;

            if (cfmpassword != newpassword) {
                document.getElementById('<%=lbl_cfmpwdfeedback.ClientID%>').innerHTML = "Password Do Not Match";
                document.getElementById('<%=lbl_cfmpwdfeedback.ClientID%>').style.color = "Red";
                return ("no_upper");
            }
            else {
                document.getElementById('<%=lbl_cfmpwdfeedback.ClientID%>').innerHTML = " ";
                return ("good");
            }
        }

        grecaptcha.ready(function () {
            grecaptcha.execute('6Lc1hhUaAAAAAF-ixVcjtgb8HHQ_PKEG1Lhwo6Oi', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
    <div style="text-align:center;">
            <asp:Label ID="lbl_header" runat="server" Text="SITConnect Forget Password"></asp:Label>
    </div>
    <br>
    <table id="check_email_form">
        <tr>
            <td colspan="2">
                <asp:Panel ID="Min_Reset_Failure" Visible="false" runat="server" CssClass="alert alert-dismissable alert-danger">Unable to reset password. Please wait at least 5 mins</asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel ID="check_email_failure" Visible="false" runat="server" CssClass="alert alert-dismissable alert-danger">Email is invalid.</asp:Panel>

            </td>
        </tr>
        <tr>
            <td class="first_column">
                <asp:Label ID="lbl_check_email" runat="server" Text="Email address"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_check_email" runat="server" TextMode="Email" onKeyUp="javascript:CheckEmailFeedback()"></asp:TextBox>
                <br />
                <asp:Label ID="lbl_checkemailfeedback" runat="server"></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button ID="check_email" runat="server" Text="Submit" OnClick="check_email_Click"/>
            </td>
        </tr>
    </table>

    <table id="forget_form">
        <tr>
            <td colspan="2">
                <asp:Panel ID="changpwd_Failure" Visible="false" runat="server" CssClass="alert alert-dismissable alert-danger">Change Password Failed. Check the fields</asp:Panel>

            </td>
        </tr>
        <tr>
            <td class="first_column">
                <asp:Label ID="lbl_email" runat="server" Text="Email address"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_email" runat="server" TextMode="Email" Enabled="false"></asp:TextBox>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td class="first_column">
                <asp:Label ID="lbl_newpwd" runat="server" Text="New Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_newpwd" runat="server" TextMode="Password" onKeyUp="javascript:NewPasswordFeedback()"></asp:TextBox>
                <br />
                <asp:Label ID="lbl_newpwdfeedback" runat="server"></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td class="first_column">
                <asp:Label ID="lbl_cfmpwd" runat="server" Text="Confirm Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_cfmpwd" runat="server" TextMode="Password" onKeyUp="javascript:CfmPasswordFeedback()"></asp:TextBox>
                <br />
                <asp:Label ID="lbl_cfmpwdfeedback" runat="server"></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button ID="btn_changepwd" runat="server" Text="Change Password" OnClick="btn_changepwd_Click"/>
            </td>
        </tr>
    </table>
</asp:Content>

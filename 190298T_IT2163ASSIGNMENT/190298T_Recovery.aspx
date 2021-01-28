<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="190298T_Recovery.aspx.cs" Inherits="_190298T_IT2163ASSIGNMENT._190298T_Recovery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #recovery_form{
            width:30%;
            margin-left: auto;
            margin-right: auto;
        }
        .first_column {
            width: 126px;
        }
    </style>
    <script src="https://www.google.com/recaptcha/api.js?render=6Lc1hhUaAAAAAF-ixVcjtgb8HHQ_PKEG1Lhwo6Oi"></script>
    <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
    <script type="text/javascript">
        function LastNameFeedback() {
            var lastname = document.getElementById('<%=tb_lastname.ClientID%>').value;

            if (lastname.search(/[a-zA-z]/) == -1) {
                document.getElementById('<%=lbl_lnamefeedback.ClientID%>').innerHTML = "Last Name Must Include Alphabtes";
                document.getElementById('<%=lbl_lnamefeedback.ClientID%>').style.color = "Red";
                return ("no_lowerupper");
            }
            else if (lastname.search(/[0-9]/) != -1) {
                document.getElementById('<%=lbl_lnamefeedback.ClientID%>').innerHTML = "Last Name Must Not Include Numbers";
                document.getElementById('<%=lbl_lnamefeedback.ClientID%>').style.color = "Red";
                return ("cannot_num");
            }
            else if (lastname.search(/[^A-Za-z0-9]/) != -1) {
                document.getElementById('<%=lbl_lnamefeedback.ClientID%>').innerHTML = "Last Name Must Not Include Special Characters";
                document.getElementById('<%=lbl_lnamefeedback.ClientID%>').style.color = "Red";
                return ("cannot_special");
            }
            else if (lastname.length < 3) {
                document.getElementById('<%=lbl_lnamefeedback.ClientID%>').innerHTML = "Last Name Must Be At Least 3 Characters";
                document.getElementById('<%=lbl_lnamefeedback.ClientID%>').style.color = "Red";
                return ("too_short");
            }
            else {
                document.getElementById('<%=lbl_lnamefeedback.ClientID%>').innerHTML = " ";
                return ("good");
            }
        }

        function CCFeedback() {
            var cc = document.getElementById('<%=tb_cc.ClientID%>').value;

            if (cc.length != 16) {
                document.getElementById('<%=lbl_ccfeedback.ClientID%>').innerHTML = "Credit Card Must Only Have 16 Numbers";
                document.getElementById('<%=lbl_ccfeedback.ClientID%>').style.color = "Red";
                return ("too_short");
            }
            else if (cc.search(/[0-9]/) == -1) {
                document.getElementById('<%=lbl_ccfeedback.ClientID%>').innerHTML = "Credit Card Must Contain Numeric Characters";
                document.getElementById('<%=lbl_ccfeedback.ClientID%>').style.color = "Red";
                return ("no_num");
            }
            else if (cc.search(/[A-Z]/) != -1) {
                document.getElementById('<%=lbl_ccfeedback.ClientID%>').innerHTML = "Credit Card Must Only Contain Numeric Characters (0-9)";
                document.getElementById('<%=lbl_ccfeedback.ClientID%>').style.color = "Red";
                return ("cannot_upper");
            }
            else if (cc.search(/[a-z]/) != -1) {
                document.getElementById('<%=lbl_ccfeedback.ClientID%>').innerHTML = "Credit Card Must Only Contain Numeric Characters (0-9)";
                document.getElementById('<%=lbl_ccfeedback.ClientID%>').style.color = "Red";
                return ("cannot_lower");
            }
            else if (cc.search(/[^A-Za-z0-9]/) != -1) {
                document.getElementById('<%=lbl_ccfeedback.ClientID%>').innerHTML = "Credit Card Must Only Contain Numeric Characters (0-9)";
                document.getElementById('<%=lbl_ccfeedback.ClientID%>').style.color = "Red";
                return ("cannot_special");
            }
            else {
                document.getElementById('<%=lbl_ccfeedback.ClientID%>').innerHTML = " ";
                return ("good");
            }
        }

        function DateFeedback() {
            var current = new Date();
            var expiry = document.getElementById('<%=tb_expiry.ClientID%>').value;
            if (expiry.search(/^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$/) == -1) {
                document.getElementById('<%=lbl_expiryfeedback.ClientID%>').innerHTML = "Expiry Date Not In Correct Format: MM/YY";
                document.getElementById('<%=lbl_expiryfeedback.ClientID%>').style.color = "Red";
                return ("invalid_date");
            }
            else {
                var expiry_month = expiry.slice(0, 2);
                var expiry_year = expiry.slice(3, 5);
                var current_month = current.getMonth();
                var current_year = current.getFullYear() % 100;
                if (expiry_year < current_year) {
                    document.getElementById('<%=lbl_expiryfeedback.ClientID%>').innerHTML = "Credit Card Has Expired";
                    document.getElementById('<%=lbl_expiryfeedback.ClientID%>').style.color = "Red";
                    return ("expired");
                }
                else {
                    if (expiry_month < current_month) {
                        document.getElementById('<%=lbl_expiryfeedback.ClientID%>').innerHTML = "Credit Card Has Expired";
                        document.getElementById('<%=lbl_expiryfeedback.ClientID%>').style.color = "Red";
                        return ("expired");
                    }
                    else {
                        document.getElementById('<%=lbl_expiryfeedback.ClientID%>').innerHTML = " ";
                        return ("good");
                    }
                }
            }
        }

        grecaptcha.ready(function () {
            grecaptcha.execute('6Lc1hhUaAAAAAF-ixVcjtgb8HHQ_PKEG1Lhwo6Oi', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });

    </script>
    <div style="text-align:center;">
            <asp:Label ID="lbl_header" runat="server" Text="SITConnect Account Recovery"></asp:Label>
            <br>
            <asp:Label ID="lbl_para" runat="server" Text="Account was previously locked. Unlock it by answering the following questions"></asp:Label>
    </div>
    <br>
    <table id="recovery_form">
        <tr>
            <td colspan="2">
                <asp:Panel ID="Verify_Failure" Visible="false" runat="server" CssClass="alert alert-dismissable alert-danger">Verification Failure.</asp:Panel>
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
                <asp:Label ID="lbl_lastname" runat="server" Text="Last Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_lastname" runat="server" onKeyUp="javascript:LastNameFeedback()"></asp:TextBox>
                <br />
                <asp:Label ID="lbl_lnamefeedback" runat="server"></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td class="first_column">
                <asp:Label ID="lbl_cc" runat="server" Text="Credit Card Number"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_cc" runat="server" onKeyUp="javascript:CCFeedback()"></asp:TextBox>
                <br />
                <asp:Label ID="lbl_ccfeedback" runat="server"></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td class="first_column">
                <asp:Label ID="lbl_expiry" runat="server" Text="Expiry Date"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_expiry" runat="server" onKeyUp="javascript:DateFeedback()"></asp:TextBox>
                <br />
                <asp:Label ID="lbl_expiryfeedback" runat="server"></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button ID="verifyunlock" runat="server" Text="Verify and Unlock" OnClick="verifyunlock_Click"/>
            </td>
        </tr>
    </table>
</asp:Content>

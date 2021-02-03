<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="190298T_RegistrationForm.aspx.cs" Inherits="_190298T_IT2163ASSIGNMENT._190298T_RegistrationForm" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>

        #reg_form{
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
        function FirstNameFeedback() {
            var firstname = document.getElementById('<%=tb_firstname.ClientID%>').value;

            if (firstname.search(/[a-zA-z]/) == -1) {
                document.getElementById('<%=lbl_fnamefeedback.ClientID%>').innerHTML = "First Name Must Include Alphabtes";
                document.getElementById('<%=lbl_fnamefeedback.ClientID%>').style.color = "Red";
                return ("no_lowerupper");
            }
            else if (firstname.search(/[0-9]/) != -1) {
                document.getElementById('<%=lbl_fnamefeedback.ClientID%>').innerHTML = "First Name Must Not Include Numbers";
                document.getElementById('<%=lbl_fnamefeedback.ClientID%>').style.color = "Red";
                return ("cannot_num");
            }
            else if (firstname.search(/[^A-Za-z0-9]/) != -1) {
                document.getElementById('<%=lbl_fnamefeedback.ClientID%>').innerHTML = "First Name Must Not Include Special Characters";
                document.getElementById('<%=lbl_fnamefeedback.ClientID%>').style.color = "Red";
                return ("cannot_special");
            }
            else if (firstname.length < 3) {
                document.getElementById('<%=lbl_fnamefeedback.ClientID%>').innerHTML = "First Name Must Be At Least 3 Characters";
                document.getElementById('<%=lbl_fnamefeedback.ClientID%>').style.color = "Red";
                return ("too_short");
            }
            else {
                document.getElementById('<%=lbl_fnamefeedback.ClientID%>').innerHTML = " ";
                return ("good");
            }
        }

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

        function CVVFeedback() {
            var cvv = document.getElementById('<%=tb_cvv.ClientID%>').value;

            if (cvv.length != 3) {
                document.getElementById('<%=lbl_cvvfeedback.ClientID%>').innerHTML = "CVV Must Only Have 3 Numbers";
                document.getElementById('<%=lbl_cvvfeedback.ClientID%>').style.color = "Red";
                return ("too_short");
            }
            else if (cvv.search(/[0-9]/) == -1) {
                document.getElementById('<%=lbl_cvvfeedback.ClientID%>').innerHTML = "CVV Must Contain Numeric Characters";
                document.getElementById('<%=lbl_cvvfeedback.ClientID%>').style.color = "Red";
                return ("no_num");
            }
            else if (cvv.search(/[A-Z]/) != -1) {
                document.getElementById('<%=lbl_cvvfeedback.ClientID%>').innerHTML = "CVV Must Only Contain Numeric Characters (0-9)";
                document.getElementById('<%=lbl_cvvfeedback.ClientID%>').style.color = "Red";
                return ("cannot_upper");
            }
            else if (cvv.search(/[a-z]/) != -1) {
                document.getElementById('<%=lbl_cvvfeedback.ClientID%>').innerHTML = "CVV Must Only Contain Numeric Characters (0-9)";
                document.getElementById('<%=lbl_cvvfeedback.ClientID%>').style.color = "Red";
                return ("cannot_lower");
            }
            else if (cvv.search(/[^A-Za-z0-9]/) != -1) {
                document.getElementById('<%=lbl_cvvfeedback.ClientID%>').innerHTML = "CVV Must Only Contain Numeric Characters (0-9)";
                document.getElementById('<%=lbl_cvvfeedback.ClientID%>').style.color = "Red";
                return ("cannot_special");
            }
            else {
                document.getElementById('<%=lbl_cvvfeedback.ClientID%>').innerHTML = " ";
                return ("good");
            }
        }

        function DateFeedback() {
            var current = new Date();
            var expiry = document.getElementById('<%=tb_expiry.ClientID%>').value;
            if (expiry.search(/^(0[1-9]|1[0-2])\/?([0-9]{2})$/) == -1) {
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


        function EmailFeedback() {
            var email = document.getElementById('<%=tb_email.ClientID%>').value;

            if (email.search(/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i) == -1) {
                document.getElementById('<%=lbl_emailfeedback.ClientID%>').innerHTML = "Email is invalid";
                document.getElementById('<%=lbl_emailfeedback.ClientID%>').style.color = "Red";
                return ("invalid_email");
            }
            else {
                document.getElementById('<%=lbl_emailfeedback.ClientID%>').innerHTML = " ";
                return ("good");
            }
        }

        function PasswordFeedback() {
            var password = document.getElementById('<%=tb_pwd.ClientID%>').value;

            if (password.length < 10) {
                document.getElementById('<%=lbl_pwdfeedback.ClientID%>').innerHTML = "Password Length Must Be At Least 10 Characters";
                document.getElementById('<%=lbl_pwdfeedback.ClientID%>').style.color = "Red";
                return ("too_short");
            }
            else if (password.search(/[0-9]/) == -1) {
                document.getElementById('<%=lbl_pwdfeedback.ClientID%>').innerHTML = "Password Must Include At Least 1 Number";
                document.getElementById('<%=lbl_pwdfeedback.ClientID%>').style.color = "Red";
                return ("no_num");
            }
            else if (password.search(/[a-z]/) == -1) {
                document.getElementById('<%=lbl_pwdfeedback.ClientID%>').innerHTML = "Password Must Include At Least 1 Lowercase Character";
                document.getElementById('<%=lbl_pwdfeedback.ClientID%>').style.color = "Red";
                return ("no_lower");
            }
            else if (password.search(/[A-Z]/) == -1) {
                document.getElementById('<%=lbl_pwdfeedback.ClientID%>').innerHTML = "Password Must Include At Least 1 Uppercase Character";
                document.getElementById('<%=lbl_pwdfeedback.ClientID%>').style.color = "Red";
                return ("no_upper");
            }
            else if (password.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById('<%=lbl_pwdfeedback.ClientID%>').innerHTML = "Password Must Include At Least 1 Special Character<br>";
                document.getElementById('<%=lbl_pwdfeedback.ClientID%>').style.color = "Red";
                return ("no_special");
            }
            else {
                document.getElementById('<%=lbl_pwdfeedback.ClientID%>').innerHTML = "Password is acceptable";
                document.getElementById('<%=lbl_pwdfeedback.ClientID%>').style.color = "Green";
                return ("good");
            }
        }

        function DOBFeedback() {
            var current = new Date();
            var dob = document.getElementById('<%=tb_dob.ClientID%>').value;
            if (dob.search(/^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$/) == -1) {
                document.getElementById('<%=lbl_dobfeedback.ClientID%>').innerHTML = "Date Of Birth Not In Correct Format: DD/MM/YYYY";
                document.getElementById('<%=lbl_dobfeedback.ClientID%>').style.color = "Red";
                return ("invalid_date");
            }
            else {
                var dob_day = dob.slice(0,2);
                var dob_month = dob.slice(3,5);
                var dob_year = dob.slice(6, 10);
                var current_date = current.getDate();
                var current_month = current.getMonth();
                var current_year = current.getFullYear();
                if (dob_year > current_year) {
                    document.getElementById('<%=lbl_dobfeedback.ClientID%>').innerHTML = "Invalid Date Of Birth";
                    document.getElementById('<%=lbl_dobfeedback.ClientID%>').style.color = "Red";
                    return ("invalid");
                }
                else if (dob_year == current_year) {
                    if (dob_month > current_month+1) {
                        document.getElementById('<%=lbl_dobfeedback.ClientID%>').innerHTML = "Invalid Date Of Birth";
                        document.getElementById('<%=lbl_dobfeedback.ClientID%>').style.color = "Red";
                        return ("invalid");
                    }
                    else if (dob_month == current_month+1) {
                        if (dob_day > current_date) {
                            document.getElementById('<%=lbl_dobfeedback.ClientID%>').innerHTML = "Invalid Date Of Birth";
                            document.getElementById('<%=lbl_dobfeedback.ClientID%>').style.color = "Red";
                            return ("invalid");
                        }
                        else {
                            document.getElementById('<%=lbl_dobfeedback.ClientID%>').innerHTML = " ";
                            return ("good");
                        }
                    }
                    else {
                        document.getElementById('<%=lbl_dobfeedback.ClientID%>').innerHTML = " ";
                        return ("good");
                    }
                }
                else {
                    document.getElementById('<%=lbl_dobfeedback.ClientID%>').innerHTML = " ";
                    return ("good");
                }
            }
        }

        
    </script>

    <div style="text-align:center;">
            <asp:Label ID="lbl_header" runat="server" Text="SITConnect Registration Form"></asp:Label>
    </div>
    <br />
    <table id="reg_form">

        <tr>
            <td class="first_column">
                <asp:Label ID="lbl_firstname" runat="server" Text="First Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_firstname" runat="server" onKeyUp="javascript:FirstNameFeedback()"></asp:TextBox>
                <br />
                <asp:Label ID="lbl_fnamefeedback" runat="server"></asp:Label>
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
                <asp:Label ID="lbl_cvv" runat="server" Text="CVV"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_cvv" runat="server" onKeyUp="javascript:CVVFeedback()"></asp:TextBox>
                <br />
                <asp:Label ID="lbl_cvvfeedback" runat="server"></asp:Label>
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
            <td class="first_column">
                <asp:Label ID="lbl_email" runat="server" Text="Email address"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_email" runat="server" onKeyUp="javascript:EmailFeedback()"></asp:TextBox>
                <br />
                <asp:Label ID="lbl_emailfeedback" runat="server"></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td class="first_column">
                <asp:Label ID="lbl_pwd" runat="server" Text="Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_pwd" runat="server" onKeyUp="javascript:PasswordFeedback()" TextMode="Password"></asp:TextBox>
                <br />
                <asp:Label ID="lbl_pwdfeedback" runat="server"></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td class="first_column">
                <asp:Label ID="lbl_dob" runat="server" Text="Date of Birth"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_dob" runat="server" onKeyUp="javascript:DOBFeedback()"></asp:TextBox>
                <br />
                <asp:Label ID="lbl_dobfeedback" runat="server"></asp:Label>
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button ID="btn_submit" runat="server" Text="Submit" OnClick="btn_submit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>

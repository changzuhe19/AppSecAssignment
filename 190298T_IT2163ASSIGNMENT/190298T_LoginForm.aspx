 <%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="190298T_LoginForm.aspx.cs" Inherits="_190298T_IT2163ASSIGNMENT._190298T_LoginForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #login_form{
            width:30%;
            margin-left: auto;
            margin-right: auto;
        }
        .first_column {
            width: 126px;
        }
        #Login_Failure{
            text-align:center;
        }
        #forget_pwd{
            text-align:center;
        }
    </style>
    <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
    <div style="text-align:center;">
            <asp:Label ID="lbl_header" runat="server" Text="SITConnect Login Page"></asp:Label>
    </div>
    <br>
    <table id="login_form">
        <tr>
            <td colspan="2">
                <asp:Panel ID="Login_Failure" Visible="false" runat="server" CssClass="alert alert-dismissable alert-danger">Login Failure. Email or Password is Incorrect.</asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="first_column">
                <asp:Label ID="lbl_email" runat="server" Text="Email address"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_email" runat="server" TextMode="Email"></asp:TextBox>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td class="first_column">
                <asp:Label ID="lbl_pwd" runat="server" Text="Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password"></asp:TextBox>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td class="first_column">
                <asp:LinkButton ID="register_here" runat="server" OnClick="register">Not a member yet? Register here</asp:LinkButton>
            </td>
            <td>
                <asp:LinkButton ID="forget_pwd" runat="server" OnClick="forgetpwd">Forget Password</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button ID="btn_login" runat="server" Text="Login" OnClick="btn_login_Click"/>
            </td>
        </tr>
    </table>
</asp:Content>

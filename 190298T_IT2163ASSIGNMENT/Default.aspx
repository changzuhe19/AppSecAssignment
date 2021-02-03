<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_190298T_IT2163ASSIGNMENT._Default"  ValidateRequest="false"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>

        #additem_form{
            margin-left: auto;
            margin-right: auto;
        }
        .first_column {
            width: 126px;
        }
    </style>

    <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div style="text-align:center;">
            <asp:Label ID="lbl_header" runat="server" Text="SITConnect Request Form"></asp:Label>
            </div>
            <br>
            <table id="additem_form">
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="Panel1" Visible="false" runat="server" CssClass="alert alert-dismissable alert-success">Request Success.</asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="Add_Failure" Visible="false" runat="server" CssClass="alert alert-dismissable alert-danger">Request Failure. Check the fields again.</asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="first_column">
                        <asp:Label ID="lbl_name" runat="server" Text="Item Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tb_name" runat="server"></asp:TextBox>
                        <br />
                        <asp:Label ID="lbl_namefeedback" runat="server"></asp:Label>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="first_column">
                        <asp:Label ID="lbl_quantity" runat="server" Text="Quantity"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tb_quantity" runat="server"></asp:TextBox>
                        <br />
                        <asp:Label ID="lbl_quantityfeedback" runat="server"></asp:Label>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="first_column"  colspan="2" style="text-align:center;">
                        <asp:Button ID="submit" runat="server" Text="Submit" OnClick="submit_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-md-6">
            <asp:Label ID="result" runat="server" Text="Submit the form on the side to see the result here"></asp:Label>
        </div>
    </div>
</asp:Content>

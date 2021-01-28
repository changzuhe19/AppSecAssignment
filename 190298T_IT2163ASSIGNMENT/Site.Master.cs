using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _190298T_IT2163ASSIGNMENT
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsLoggedIn"] != null && Session["GUIDAuthToken"] != null && Request.Cookies["GUIDAuthToken"] != null)
            {
                if (Session["GUIDAuthToken"].ToString().Equals(Request.Cookies["GUIDAuthToken"].Value))
                {
                    login.Visible = false;
                }
                else
                {
                    logout.Visible = false;
                    Response.Redirect("/", false);
                }
            }
            else
            {
                logout.Visible = false;
            }
        }

        protected void Slogout(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["GUIDAuthToken"] != null)
            {
                Response.Cookies["GUIDAuthToken"].Value = string.Empty;
                Response.Cookies["GUIDAuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            logout.Visible = false;
            login.Visible = true;
        }
    }
}
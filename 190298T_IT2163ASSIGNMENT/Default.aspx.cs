using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _190298T_IT2163ASSIGNMENT
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submit_Click(object sender, EventArgs e)
        {
            string name = tb_name.Text.ToString();
            string qty = tb_quantity.Text.ToString();

            result.Text += "<br /><br />" + "Name: " + HttpUtility.HtmlEncode(name) + "<br />" + "Quantity: " + HttpUtility.HtmlEncode(qty);
        }
    }
}
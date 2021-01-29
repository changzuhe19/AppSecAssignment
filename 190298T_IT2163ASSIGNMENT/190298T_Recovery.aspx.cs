using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _190298T_IT2163ASSIGNMENT
{
    public partial class _190298T_Recovery : System.Web.UI.Page
    {
        string IT2163DB = System.Configuration.ConfigurationManager.ConnectionStrings["IT2163DB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_header.Font.Bold = true;
            lbl_header.Font.Size = FontUnit.Large;

            if (Session["ToRecover"] != null)
            {
                tb_email.Text = Session["ToRecover"].ToString();
            }
            else
            {
                Response.Redirect("/190298T_LoginForm", false);
            }
        }

        protected void verifyunlock_Click(object sender, EventArgs e)
        {
            int error = 0;

            lbl_lnamefeedback.Text = "";
            if (Regex.IsMatch(tb_lastname.Text, "[a-zA-z]"))
            {
                if (Regex.IsMatch(tb_lastname.Text, "[0-9]"))
                {
                    error += 1;
                    lbl_lnamefeedback.Text = "Last Name Must Not Include Numbers";
                    lbl_lnamefeedback.ForeColor = Color.Red;
                }
                else
                {
                    if (Regex.IsMatch(tb_lastname.Text, "[^A-Za-z0-9]"))
                    {
                        error += 1;
                        lbl_lnamefeedback.Text = "Last Name Must Not Include Special Characters";
                        lbl_lnamefeedback.ForeColor = Color.Red;
                    }
                    else
                    {
                        if (tb_lastname.Text.Length < 3)
                        {
                            error += 1;
                            lbl_lnamefeedback.Text = "Last Name Must Be At Least 3 Characters";
                            lbl_lnamefeedback.ForeColor = Color.Red;
                        }
                        else
                        {
                            lbl_lnamefeedback.Text += " ";
                        }
                    }
                }
            }
            else
            {
                error += 1;
                lbl_lnamefeedback.Text = "Last Name Must Include Alphabtes";
                lbl_lnamefeedback.ForeColor = Color.Red;
            }

            lbl_ccfeedback.Text = "";
            if (Regex.IsMatch(tb_cc.Text, "[0-9]"))
            {
                if (Regex.IsMatch(tb_cc.Text, "[A-Z]"))
                {
                    error += 1;
                    lbl_ccfeedback.Text = "Credit Card Must Only Contain Numeric Characters (0-9)";
                    lbl_ccfeedback.ForeColor = Color.Red;
                }
                else
                {
                    if (Regex.IsMatch(tb_cc.Text, "[a-z]"))
                    {
                        error += 1;
                        lbl_ccfeedback.Text = "Credit Card Must Only Contain Numeric Characters (0-9)";
                        lbl_ccfeedback.ForeColor = Color.Red;
                    }
                    else
                    {
                        if (Regex.IsMatch(tb_cc.Text, "[^A-Za-z0-9]"))
                        {
                            error += 1;
                            lbl_ccfeedback.Text = "Credit Card Must Only Contain Numeric Characters (0-9)";
                            lbl_ccfeedback.ForeColor = Color.Red;
                        }
                        else
                        {
                            if (tb_cc.Text.Length != 16)
                            {
                                error += 1;
                                lbl_ccfeedback.Text = "Credit Card Must Only Have 16 Numbers";
                                lbl_ccfeedback.ForeColor = Color.Red;
                            }
                            else
                            {
                                lbl_ccfeedback.Text += " ";
                            }
                        }
                    }
                }
            }
            else
            {
                error += 1;
                lbl_ccfeedback.Text = "Credit Card Must Contain Numeric Characters";
                lbl_ccfeedback.ForeColor = Color.Red;
            }

            lbl_expiryfeedback.Text = "";
            if (Regex.IsMatch(tb_expiry.Text, @"^(0[1-9]|1[0-2])\/?([0-9]{2})$"))
            {
                DateTime current = DateTime.Now;
                int month = Convert.ToInt32(tb_expiry.Text.Substring(0, 2));
                int year = Convert.ToInt32(tb_expiry.Text.Substring(3, 2));
                if (year < current.Year % 100)
                {
                    System.Diagnostics.Debug.WriteLine(year);
                    System.Diagnostics.Debug.WriteLine(current.Year);
                    error += 1;
                    lbl_expiryfeedback.Text = "Credit Card Has Expired";
                    lbl_expiryfeedback.ForeColor = Color.Red;
                }
                else
                {
                    if (month < current.Month)
                    {
                        error += 1;
                        lbl_expiryfeedback.Text = "Credit Card Has Expired";
                        lbl_expiryfeedback.ForeColor = Color.Red;
                    }
                    else
                    {
                        lbl_expiryfeedback.Text = "";
                    }
                }
            }
            else
            {
                error += 1;
                lbl_expiryfeedback.Text = "Expiry Date Not In Correct Format: MM/YY";
                lbl_expiryfeedback.ForeColor = Color.Red;
            }

            if (tb_lastname.Text != RetrieveLName() || tb_cc.Text != decryptCC() || tb_expiry.Text != RetrieveExpiry())
            {
                Verify_Failure.Visible = true;
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(IT2163DB))
                    {
                        using (SqlCommand cmd = new SqlCommand("UPDATE SITConnect SET Lockout = @Lockout WHERE Email_Address = @Email_Address"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@Lockout", 0);
                                cmd.Parameters.AddWithValue("@Email_Address", tb_email.Text.Trim());
                                cmd.Connection = con;
                                try
                                {
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    Response.Redirect("/190298T_LoginForm", false);
                                }
                                catch (Exception ex)
                                {
                                    Response.Redirect("/Error/GenericError.htmL", false);
                                }
                                finally
                                {
                                    con.Close();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Redirect("/Error/GenericError.htmL", false);
                }
            }

        }

        protected string RetrieveLName()
        {
            string LName = null;
            try
            {
                using (SqlConnection con = new SqlConnection(IT2163DB))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Last_Name FROM SITConnect WHERE Email_Address = @Email_Address"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email_Address", tb_email.Text.Trim());
                            cmd.Connection = con;
                            con.Open();
                            LName = cmd.ExecuteScalar().ToString();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Error/GenericError.htmL", false);
            }
            finally { }
            return LName;
        }

        protected byte[] RetrieveCC()
        {
            byte[] CC = null;
            try
            {
                using (SqlConnection con = new SqlConnection(IT2163DB))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT CC FROM SITConnect WHERE Email_Address = @Email_Address"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email_Address", tb_email.Text.Trim());
                            cmd.Connection = con;
                            con.Open();
                            CC = Convert.FromBase64String(cmd.ExecuteScalar().ToString());
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Error/GenericError.htmL", false);
            }
            finally { }
            return CC;
        }

        protected byte[] RetrieveCCKey()
        {
            byte[] CCKey = null;
            try
            {
                using (SqlConnection con = new SqlConnection(IT2163DB))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT CCKey FROM SITConnect WHERE Email_Address = @Email_Address"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email_Address", tb_email.Text.Trim());
                            cmd.Connection = con;
                            con.Open();
                            CCKey = Convert.FromBase64String(cmd.ExecuteScalar().ToString());
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Error/GenericError.htmL", false);
            }
            finally { }
            return CCKey;
        }

        protected byte[] RetrieveCCIV()
        {
            byte[] CCIV = null;
            try
            {
                using (SqlConnection con = new SqlConnection(IT2163DB))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT CCIV FROM SITConnect WHERE Email_Address = @Email_Address"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email_Address", tb_email.Text.Trim());
                            cmd.Connection = con;
                            con.Open();
                            CCIV = Convert.FromBase64String(cmd.ExecuteScalar().ToString());
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Error/GenericError.htmL", false);
            }
            finally { }
            return CCIV;
        }

        protected string decryptCC()
        {
            string CCplainText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = RetrieveCCIV();
                cipher.Key = RetrieveCCKey();
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                using (MemoryStream msDecrypt = new MemoryStream(RetrieveCC()))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            CCplainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return CCplainText;
        }

        protected string RetrieveExpiry()
        {
            string expiry = null;
            try
            {
                using (SqlConnection con = new SqlConnection(IT2163DB))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Expiry_Date FROM SITConnect WHERE Email_Address = @Email_Address"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email_Address", tb_email.Text.Trim());
                            cmd.Connection = con;
                            con.Open();
                            expiry = cmd.ExecuteScalar().ToString();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Error/GenericError.htmL", false);
            }
            finally { }
            return expiry;
        }
    }
}
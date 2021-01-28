using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace _190298T_IT2163ASSIGNMENT
{
    public partial class _190298T_ChangePassword : System.Web.UI.Page
    {
        string IT2163DB = System.Configuration.ConfigurationManager.ConnectionStrings["IT2163DB"].ConnectionString;
        static string NewHash;
        static string NewSalt;
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_header.Font.Bold = true;
            lbl_header.Font.Size = FontUnit.Large;

            if (Session["IsLoggedIn"] != null)
            {
                lbl_check_email.Visible = false;
                tb_check_email.Visible = false;
                lbl_checkemailfeedback.Visible = false;
                check_email.Visible = false;
                lbl_header.Text = "SITConnect Change New Password";
                tb_email.Text = Session["IsLoggedIn"].ToString();
            }
            else
            {
                lbl_email.Visible = false;
                tb_email.Visible = false;
                tb_newpwd.Visible = false;
                lbl_newpwd.Visible = false;
                lbl_newpwdfeedback.Visible = false;
                lbl_cfmpwd.Visible = false;
                tb_cfmpwd.Visible = false;
                lbl_cfmpwdfeedback.Visible = false;
                btn_changepwd.Visible = false;
            }
        }
        protected void check_email_Click(object sender, EventArgs e)
        {
            string check_emailtext = tb_check_email.Text.Trim();
            try
            {
                using (SqlConnection con = new SqlConnection(IT2163DB))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM SITConnect WHERE Email_Address = @Email_Address"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email_Address", check_emailtext);
                            cmd.Connection = con;
                            try
                            {
                                con.Open();
                                int result = Convert.ToInt32(cmd.ExecuteScalar());
                                DateTime Min = GetMin_Pwd(check_emailtext);
                                if (result == 1)
                                {
                                    if (DateTime.Now > Min)
                                    {
                                        tb_email.Text = check_emailtext;
                                        check_email_failure.Visible = false;
                                        lbl_check_email.Visible = false;
                                        tb_check_email.Visible = false;
                                        lbl_checkemailfeedback.Visible = false;
                                        check_email.Visible = false;
                                        lbl_email.Visible = true;
                                        tb_email.Visible = true;
                                        tb_newpwd.Visible = true;
                                        lbl_newpwd.Visible = true;
                                        lbl_newpwdfeedback.Visible = true;
                                        lbl_cfmpwd.Visible = true;
                                        tb_cfmpwd.Visible = true;
                                        lbl_cfmpwdfeedback.Visible = true;
                                        btn_changepwd.Visible = true;
                                        Min_Reset_Failure.Visible = false;
                                    }
                                    else
                                    {
                                        check_email_failure.Visible = false;
                                        Min_Reset_Failure.Visible = true;
                                    }
                                }
                                else
                                {
                                    Min_Reset_Failure.Visible = false;
                                    check_email_failure.Visible = true;
                                }
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

        protected void btn_changepwd_Click(object sender, EventArgs e)
        {
            int error = 0;

            lbl_newpwdfeedback.Text = " ";
            lbl_cfmpwdfeedback.Text = " ";

            if (tb_newpwd.Text.Length < 10)
            {
                error += 1;
                lbl_newpwdfeedback.Text = "Password Length Must Be At Least 10 Characters";
                lbl_newpwdfeedback.ForeColor = Color.Red;
            }
            if (Regex.IsMatch(tb_newpwd.Text, "[0-9]"))
            {
                lbl_newpwdfeedback.Text += " ";
            }
            else
            {
                error += 1;
                lbl_newpwdfeedback.Text = "Password Must Include At Least 1 Number";
                lbl_newpwdfeedback.ForeColor = Color.Red;
            }
            if (Regex.IsMatch(tb_newpwd.Text, "[a-z]"))
            {
                lbl_newpwdfeedback.Text += " ";
            }
            else
            {
                error += 1;
                lbl_newpwdfeedback.Text = "Password Must Include At Least 1 Lowercase Character";
                lbl_newpwdfeedback.ForeColor = Color.Red;
            }
            if (Regex.IsMatch(tb_newpwd.Text, "[A-Z]"))
            {
                lbl_newpwdfeedback.Text += " ";
            }
            else
            {
                error += 1;
                lbl_newpwdfeedback.Text = "Password Must Include At Least 1 Uppercase Character";
                lbl_newpwdfeedback.ForeColor = Color.Red;
            }
            if (Regex.IsMatch(tb_newpwd.Text, "[^A-Za-z0-9]"))
            {
                lbl_newpwdfeedback.Text += " ";
            }
            else
            {
                error += 1;
                lbl_newpwdfeedback.Text = "Password Must Include At Least 1 Special Character";
                lbl_newpwdfeedback.ForeColor = Color.Red;
            }


            if (tb_cfmpwd.Text != tb_newpwd.Text)
            {
                error += 1;
                lbl_cfmpwdfeedback.Text = "Password Do Not Match";
                lbl_cfmpwdfeedback.ForeColor = Color.Red;
            }
            else
            {
                lbl_cfmpwdfeedback.Text = " ";
                
            }

            if (error == 0)
            {
                changpwd_Failure.Visible = false;

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] newsaltbyte = new byte[8];

                rng.GetBytes(newsaltbyte);
                NewSalt = Convert.ToBase64String(newsaltbyte);

                SHA512Managed newhash = new SHA512Managed();
                string pwdwithsalt = tb_cfmpwd.Text + NewSalt;
                string currentpwdwithsalt = tb_cfmpwd.Text + getDBCurrentSalt(tb_email.Text);
                string pwd2withsalt = tb_cfmpwd.Text + getDBSecondSalt(tb_email.Text);

                byte[] hashpwd = newhash.ComputeHash(Encoding.UTF8.GetBytes(pwdwithsalt));
                byte[] currenthashpwd = newhash.ComputeHash(Encoding.UTF8.GetBytes(currentpwdwithsalt));
                byte[] pwd2hashpwd = newhash.ComputeHash(Encoding.UTF8.GetBytes(pwd2withsalt));
                if (Convert.ToBase64String(currenthashpwd) == getDBCurrentHash(tb_email.Text) || Convert.ToBase64String(pwd2hashpwd) == getDBSecondHash(tb_email.Text))
                {
                    changpwd_Failure.Visible = true;
                    lbl_email.Visible = true;
                    tb_email.Visible = true;
                    tb_newpwd.Visible = true;
                    lbl_newpwd.Visible = true;
                    lbl_newpwdfeedback.Visible = true;
                    lbl_cfmpwd.Visible = true;
                    tb_cfmpwd.Visible = true;
                    lbl_cfmpwdfeedback.Visible = true;
                    btn_changepwd.Visible = true;
                    lbl_newpwdfeedback.Text = "Please use another password that has not been used before";
                    lbl_newpwdfeedback.ForeColor = Color.Red;
                }
                else
                {
                    NewHash = Convert.ToBase64String(hashpwd);

                    string DBCurrentHash = getDBCurrentHash(tb_email.Text.Trim());
                    string DBCurrentSalt = getDBCurrentSalt(tb_email.Text.Trim());

                    try
                    {
                        using (SqlConnection con = new SqlConnection(IT2163DB))
                        {
                            using (SqlCommand cmd = new SqlCommand("UPDATE SITConnect SET Password_Salt = @NewSalt, Password_Hash = @NewHash, Min_Pwd = @Min_Pwd, Max_Pwd = @Max_Pwd, Pwd2_Salt = @DBCurrentSalt, Pwd2_Hash = @DBCurrentHash WHERE Email_Address = @Email_Address"))
                            {
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.AddWithValue("@NewSalt", NewSalt);
                                    cmd.Parameters.AddWithValue("@NewHash", NewHash);
                                    DateTime now = DateTime.Now;
                                    cmd.Parameters.AddWithValue("@Min_Pwd", now.AddMinutes(5));
                                    cmd.Parameters.AddWithValue("@Max_Pwd", now.AddMinutes(15));
                                    cmd.Parameters.AddWithValue("@DBCurrentSalt", DBCurrentSalt);
                                    cmd.Parameters.AddWithValue("@DBCurrentHash", DBCurrentHash);
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
            else
            {
                changpwd_Failure.Visible = true;
                lbl_email.Visible = true;
                tb_email.Visible = true;
                tb_newpwd.Visible = true;
                lbl_newpwd.Visible = true;
                lbl_newpwdfeedback.Visible = true;
                lbl_cfmpwd.Visible = true;
                tb_cfmpwd.Visible = true;
                lbl_cfmpwdfeedback.Visible = true;
                btn_changepwd.Visible = true;
            }
        }

        protected string getDBCurrentHash(string email)
        {
            string DBCurrentHash = null;
            string IT2163DB = ConfigurationManager.ConnectionStrings["IT2163DB"].ConnectionString;
            SqlConnection con = new SqlConnection(IT2163DB);
            string sql = "SELECT Password_Hash FROM SITConnect WHERE Email_Address = @email";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                con.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Password_Hash"] != null)
                        {
                            if (reader["Password_Hash"] != DBNull.Value)
                            {
                                DBCurrentHash = reader["Password_Hash"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Error/GenericError.htmL", false);
            }
            finally
            {
                con.Close();
            }
            return DBCurrentHash;
        }

        protected string getDBCurrentSalt(string email)
        {
            string DBCurrentSalt = null;
            string IT2163DB = ConfigurationManager.ConnectionStrings["IT2163DB"].ConnectionString;
            SqlConnection con = new SqlConnection(IT2163DB);
            string sql = "SELECT Password_Salt FROM SITConnect WHERE Email_Address = @email";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                con.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Password_Salt"] != null)
                        {
                            if (reader["Password_Salt"] != DBNull.Value)
                            {
                                DBCurrentSalt = reader["Password_Salt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Error/GenericError.htmL", false);
            }
            finally
            {
                con.Close();
            }
            return DBCurrentSalt;
        }
        protected string getDBSecondHash(string email)
        {
            string DBSecondHash = null;
            string IT2163DB = ConfigurationManager.ConnectionStrings["IT2163DB"].ConnectionString;
            SqlConnection con = new SqlConnection(IT2163DB);
            string sql = "SELECT Pwd2_Hash FROM SITConnect WHERE Email_Address = @email";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                con.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Pwd2_Hash"] != null)
                        {
                            if (reader["Pwd2_Hash"] != DBNull.Value)
                            {
                                DBSecondHash = reader["Pwd2_Hash"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Error/GenericError.htmL", false);
            }
            finally
            {
                con.Close();
            }
            return DBSecondHash;
        }
        protected string getDBSecondSalt(string email)
        {
            string DBSecondSalt = null;
            string IT2163DB = ConfigurationManager.ConnectionStrings["IT2163DB"].ConnectionString;
            SqlConnection con = new SqlConnection(IT2163DB);
            string sql = "SELECT Pwd2_Salt FROM SITConnect WHERE Email_Address = @email";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                con.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Pwd2_Salt"] != null)
                        {
                            if (reader["Pwd2_Salt"] != DBNull.Value)
                            {
                                DBSecondSalt = reader["Pwd2_Salt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Error/GenericError.htmL", false);
            }
            finally
            {
                con.Close();
            }
            return DBSecondSalt;
        }

        protected DateTime GetMin_Pwd(string email)
        {
            DateTime Min_Pwd = DateTime.Now;
            string IT2163DB = ConfigurationManager.ConnectionStrings["IT2163DB"].ConnectionString;
            SqlConnection con = new SqlConnection(IT2163DB);
            string sql = "SELECT Min_Pwd FROM SITConnect WHERE Email_Address = @email";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                con.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Min_Pwd"] != null)
                        {
                            if (reader["Min_Pwd"] != DBNull.Value)
                            {
                                Min_Pwd = Convert.ToDateTime(reader["Min_Pwd"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Error/GenericError.htmL", false);
            }
            finally
            {
                con.Close();
            }

            return Min_Pwd;
        }

        
    }
}
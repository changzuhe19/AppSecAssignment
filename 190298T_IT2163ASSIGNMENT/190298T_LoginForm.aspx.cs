using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;

namespace _190298T_IT2163ASSIGNMENT
{
    public partial class _190298T_LoginForm : System.Web.UI.Page
    {
        public class MyObjectLogin
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_header.Font.Bold = true;
            lbl_header.Font.Size = FontUnit.Large;
        }
        public bool ValidateCaptcha()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                ("  " + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        MyObjectLogin jsonObject = js.Deserialize<MyObjectLogin>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                result = false;
            }
            return result;
        }
        protected void register(object sender, EventArgs e)
        {
            Response.Redirect("190298T_RegistrationForm.aspx");
        }
        protected void forgetpwd(object sender, EventArgs e)
        {
            Response.Redirect("190298T_ChangePassword.aspx");
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            string email = tb_email.Text.ToString();
            string pwd = tb_pwd.Text.ToString();

            SHA512Managed hash = new SHA512Managed();
            string DBHash = getDBHash(email);
            string DBSalt = getDBSalt(email);
            int Lockout = getLockOut(email);

            try
            {
                if (DBHash != null && DBHash.Length > 0 && DBSalt != null && DBSalt.Length > 0 && ValidateCaptcha() == true)
                {
                    string pwdwithsalt = pwd + DBSalt;
                    byte[] pwdhash = hash.ComputeHash(Encoding.UTF8.GetBytes(pwdwithsalt));
                    string userpwdhash = Convert.ToBase64String(pwdhash);

                    if (userpwdhash.Equals(DBHash))
                    {
                        if (Lockout < 3)
                        {
                            //login (working)
                            Session["IsLoggedIn"] = email;
                            string GUIDAuthToken = Guid.NewGuid().ToString();
                            Session["GUIDAuthToken"] = GUIDAuthToken;
                            Response.Cookies.Add(new HttpCookie("GUIDAuthToken", GUIDAuthToken));

                            if (DateTime.Now < GetMax_Pwd(email))
                            {
                                Response.Redirect("/", false);
                            }
                            else
                            {
                                Response.Redirect("190298T_ChangePassword", false);
                            }
                        }
                        else
                        {
                            Session["ToRecover"] = email;
                            Response.Redirect("190298T_Recovery", false);
                        }
                    }
                    else
                    {
                        increaseLockout(email, Lockout);
                        Login_Failure.Visible = true;
                    }
                }
                else
                {
                    Login_Failure.Visible = true;
                }
            }
            catch(Exception ex)
            {
                Login_Failure.Visible = true;
            }
        }

        protected string getDBHash(string email)
        {
            string DBHash = null;
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
                                DBHash = reader["Password_Hash"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return DBHash;
        }

        protected string getDBSalt(string email)
        {
            string DBSalt = null;
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
                                DBSalt = reader["Password_Salt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return DBSalt;
        }

        protected int getLockOut(string email)
        {
            int Lockout = 0;
            string IT2163DB = ConfigurationManager.ConnectionStrings["IT2163DB"].ConnectionString;
            SqlConnection con = new SqlConnection(IT2163DB);
            string sql = "SELECT Lockout FROM SITConnect WHERE Email_Address = @email";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                con.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Lockout"] != null)
                        {
                            if (reader["Lockout"] != DBNull.Value)
                            {
                                Lockout = (int)reader["Lockout"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return Lockout;
        }

        protected void increaseLockout(string email, int currentlockout)
        {
            string IT2163DB = ConfigurationManager.ConnectionStrings["IT2163DB"].ConnectionString;
            SqlConnection con = new SqlConnection(IT2163DB);
            string sql = "UPDATE SITConnect SET Lockout = @newlockout WHERE Email_Address = @email";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@newlockout", currentlockout + 1);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                con.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        protected DateTime GetMax_Pwd(string email)
        {
            DateTime Max_Pwd = DateTime.Now;
            string IT2163DB = ConfigurationManager.ConnectionStrings["IT2163DB"].ConnectionString;
            SqlConnection con = new SqlConnection(IT2163DB);
            string sql = "SELECT Max_Pwd FROM SITConnect WHERE Email_Address = @email";
            SqlCommand command = new SqlCommand(sql, con);
            command.Parameters.AddWithValue("@email", email);

            try
            {
                con.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Max_Pwd"] != null)
                        {
                            if (reader["Max_Pwd"] != DBNull.Value)
                            {
                                Max_Pwd = Convert.ToDateTime(reader["Max_Pwd"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                con.Close();
            }

            return Max_Pwd;
        }
    }
}
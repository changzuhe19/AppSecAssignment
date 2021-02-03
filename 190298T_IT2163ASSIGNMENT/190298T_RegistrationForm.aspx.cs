using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace _190298T_IT2163ASSIGNMENT
{
    public class MyObjectRegistration
    {
        public string success { get; set; }
        public List<string> ErrorMessage { get; set; }
    }

    public partial class _190298T_RegistrationForm : System.Web.UI.Page
    {
        string IT2163DB = System.Configuration.ConfigurationManager.ConnectionStrings["IT2163DB"].ConnectionString;
        static string Hash;
        static string Salt;
        byte[] CCKey;
        byte[] CCIV;
        byte[] CVVKey;
        byte[] CVVIV;

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
                (" " + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        MyObjectRegistration jsonObject = js.Deserialize<MyObjectRegistration>(jsonResponse);
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

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            int error = 0;

            bool verify = ValidateCaptcha();
            if (verify == false)
            {
                error += 1;
            }

            lbl_fnamefeedback.Text = "";
            if (Regex.IsMatch(tb_firstname.Text, "[a-zA-z]"))
            {
                if (Regex.IsMatch(tb_firstname.Text, "[0-9]"))
                {
                    error += 1;
                    lbl_fnamefeedback.Text = "First Name Must Not Include Numbers";
                    lbl_fnamefeedback.ForeColor = Color.Red;
                }
                else
                {
                    if (Regex.IsMatch(tb_firstname.Text, "[^A-Za-z0-9]"))
                    {
                        error += 1;
                        lbl_fnamefeedback.Text = "First Name Must Not Include Special Characters";
                        lbl_fnamefeedback.ForeColor = Color.Red;
                    }
                    else
                    {
                        if (tb_firstname.Text.Length < 3)
                        {
                            error += 1;
                            lbl_fnamefeedback.Text = "First Name Must Be At Least 3 Characters";
                            lbl_fnamefeedback.ForeColor = Color.Red;
                        }
                        else
                        {
                            lbl_fnamefeedback.Text += " ";
                        }
                    }
                }
            }
            else
            {
                error += 1;
                lbl_fnamefeedback.Text = "First Name Must Include Alphabtes";
                lbl_fnamefeedback.ForeColor = Color.Red;
            }


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
                    if (Regex.IsMatch(tb_cc.Text, "[a-z]")){
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


            lbl_cvvfeedback.Text = "";
            if (Regex.IsMatch(tb_cvv.Text, "[0-9]"))
            {
                if (Regex.IsMatch(tb_cvv.Text, "[A-Z]"))
                {
                    error += 1;
                    lbl_cvvfeedback.Text = "CVV Must Only Contain Numeric Characters (0-9)";
                    lbl_cvvfeedback.ForeColor = Color.Red;
                }
                else
                {
                    if (Regex.IsMatch(tb_cvv.Text, "[a-z]"))
                    {
                        error += 1;
                        lbl_cvvfeedback.Text = "CVV Must Only Contain Numeric Characters (0-9)";
                        lbl_cvvfeedback.ForeColor = Color.Red;
                    }
                    else
                    {
                        if (Regex.IsMatch(tb_cvv.Text, "[^A-Za-z0-9]"))
                        {
                            error += 1;
                            lbl_cvvfeedback.Text = "CVV Must Only Contain Numeric Characters (0-9)";
                            lbl_cvvfeedback.ForeColor = Color.Red;
                        }
                        else
                        {
                            if (tb_cvv.Text.Length != 3)
                            {
                                error += 1;
                                lbl_cvvfeedback.Text = "CVV Must Only Have 3 Numbers";
                                lbl_cvvfeedback.ForeColor = Color.Red;
                            }
                            else
                            {
                                lbl_cvvfeedback.Text += " ";
                            }
                        }
                    }
                }
            }
            else
            {
                error += 1;
                lbl_cvvfeedback.Text = "CVV Must Contain Numeric Characters";
                lbl_cvvfeedback.ForeColor = Color.Red;
            }

            lbl_expiryfeedback.Text = "";
            if (Regex.IsMatch(tb_expiry.Text, @"^(0[1-9]|1[0-2])\/?([0-9]{2})$"))
            {
                DateTime current = DateTime.Now;
                int month = Convert.ToInt32(tb_expiry.Text.Substring(0, 2));
                int year = Convert.ToInt32(tb_expiry.Text.Substring(3, 2));
                if (year < current.Year%100)
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

            lbl_emailfeedback.Text = " ";
            if (Regex.IsMatch(tb_email.Text, @"^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$")){
                SqlConnection conn = new SqlConnection(IT2163DB);
                string sql = "SELECT * FROM SITConnect WHERE Email_Address = @email";
                DataTable dt = new DataTable();
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@email", tb_email.Text.Trim());
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);

                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        error += 1;
                        lbl_emailfeedback.Text += "Please try again with another email";
                        lbl_emailfeedback.ForeColor = Color.Red;
                    }
                    else
                    {
                        lbl_emailfeedback.Text += " ";
                    }
                }
                catch (Exception ex)
                {
                    Response.Redirect("/Error/GenericError.htmL", false);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                error += 1;
                lbl_emailfeedback.Text = "Email is invalid";
                lbl_emailfeedback.ForeColor = Color.Red;
            }
            

            lbl_pwdfeedback.Text = "";
            if (tb_pwd.Text.Length < 10)
            {
                error += 1;
                lbl_pwdfeedback.Text = "Password Length Must Be At Least 10 Characters";
                lbl_pwdfeedback.ForeColor = Color.Red;
            }
            else
            {
                lbl_pwdfeedback.Text += " ";
            }
            if (Regex.IsMatch(tb_pwd.Text, "[0-9]"))
            {
                lbl_pwdfeedback.Text += " ";
            }
            else
            {
                error += 1;
                lbl_pwdfeedback.Text = "Password Must Include At Least 1 Number";
                lbl_pwdfeedback.ForeColor = Color.Red;
            }
            if (Regex.IsMatch(tb_pwd.Text, "[a-z]"))
            {
                lbl_pwdfeedback.Text += " ";
            }
            else
            {
                error += 1;
                lbl_pwdfeedback.Text = "Password Must Include At Least 1 Lowercase Character";
                lbl_pwdfeedback.ForeColor = Color.Red;
            }
            if (Regex.IsMatch(tb_pwd.Text, "[A-Z]"))
            {
                lbl_pwdfeedback.Text += " ";
            }
            else
            {
                error += 1;
                lbl_pwdfeedback.Text = "Password Must Include At Least 1 Uppercase Character";
                lbl_pwdfeedback.ForeColor = Color.Red;
            }
            if (Regex.IsMatch(tb_pwd.Text, "[^A-Za-z0-9]"))
            {
                lbl_pwdfeedback.Text += " ";
            }
            else
            {
                error += 1;
                lbl_pwdfeedback.Text = "Password Must Include At Least 1 Special Character";
                lbl_pwdfeedback.ForeColor = Color.Red;
            }

            lbl_dobfeedback.Text = "";
            if (Regex.IsMatch(tb_dob.Text, @"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$"))
            {
                DateTime current = DateTime.Now;
                int day = Convert.ToInt32(tb_dob.Text.Substring(0,2));
                int month = Convert.ToInt32(tb_dob.Text.Substring(3, 2));
                int year = Convert.ToInt32(tb_dob.Text.Substring(6, 4));
                if (year > current.Year)
                {
                    error += 1;
                    lbl_dobfeedback.Text = "Invalid Date Of Birth";
                    lbl_dobfeedback.ForeColor = Color.Red;
                }
                else if (year == current.Year)
                {
                    if (month > current.Month)
                    {
                        error += 1;
                        lbl_dobfeedback.Text = "Invalid Date Of Birth";
                        lbl_dobfeedback.ForeColor = Color.Red;
                    }
                    else if (month == current.Month)
                    {
                        if (day > current.Day)
                        {
                            error += 1;
                            lbl_dobfeedback.Text = "Invalid Date Of Birth";
                            lbl_dobfeedback.ForeColor = Color.Red;
                        }
                        else
                        {
                            lbl_dobfeedback.Text = "";
                        }
                    }
                    else
                    {
                        lbl_dobfeedback.Text = "";
                    }
                }
                else
                {
                    lbl_dobfeedback.Text = "";
                }
            }
            else
            {
                error += 1;
                lbl_dobfeedback.Text = "Date Of Birth Not In Correct Format: DD/MM/YYYY";
                lbl_dobfeedback.ForeColor = Color.Red;
            }


            if (error == 0)
            {
                string pwd = tb_pwd.Text.ToString().Trim();

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltbyte = new byte[8];

                rng.GetBytes(saltbyte);
                Salt = Convert.ToBase64String(saltbyte);

                SHA512Managed hash = new SHA512Managed();
                string pwdwithsalt = pwd + Salt;
                byte[] hashpwd = hash.ComputeHash(Encoding.UTF8.GetBytes(pwdwithsalt));
                Hash = Convert.ToBase64String(hashpwd);

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                CCKey = cipher.Key;
                CCIV = cipher.IV;

                RijndaelManaged cipher2 = new RijndaelManaged();
                cipher.GenerateKey();
                CVVKey = cipher.Key;
                CVVIV = cipher2.IV;

                try
                {
                    using (SqlConnection con = new SqlConnection(IT2163DB))
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO SITConnect VALUES(@SITID, @Email_Address, @First_Name, @Last_Name, @CC, @CCKey, @CCIV, @CVV, @CVVKey, @CVVIV, @Expiry, @Password_Salt, @Password_Hash, @Date_Of_Birth, @Lockout, @Min_Pwd, @Max_Pwd, @Pwd2_Salt, @Pwd2_Hash)"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@SITID", Guid.NewGuid().ToString());
                                cmd.Parameters.AddWithValue("@Email_Address", tb_email.Text.Trim());
                                cmd.Parameters.AddWithValue("@First_Name", tb_firstname.Text.Trim());
                                cmd.Parameters.AddWithValue("@Last_Name", tb_lastname.Text.Trim());
                                cmd.Parameters.AddWithValue("@CC", Convert.ToBase64String(EncryptCC(tb_cc.Text.Trim())));
                                cmd.Parameters.AddWithValue("@CCKey", Convert.ToBase64String(CCKey));
                                cmd.Parameters.AddWithValue("@CCIV", Convert.ToBase64String(CCIV));
                                cmd.Parameters.AddWithValue("@CVV", Convert.ToBase64String(EncryptCVV(tb_cvv.Text.Trim())));
                                cmd.Parameters.AddWithValue("@CVVKey", Convert.ToBase64String(CVVKey));
                                cmd.Parameters.AddWithValue("@CVVIV", Convert.ToBase64String(CVVIV));
                                cmd.Parameters.AddWithValue("@Expiry", tb_expiry.Text.Trim());
                                cmd.Parameters.AddWithValue("@Password_Salt", Salt);
                                cmd.Parameters.AddWithValue("@Password_Hash", Hash);
                                cmd.Parameters.AddWithValue("@Date_Of_Birth", tb_dob.Text.Trim());
                                cmd.Parameters.AddWithValue("@Lockout", 0);
                                DateTime now = DateTime.Now;
                                cmd.Parameters.AddWithValue("@Min_Pwd", now.AddMinutes(5));
                                cmd.Parameters.AddWithValue("@Max_Pwd", now.AddMinutes(15));
                                cmd.Parameters.AddWithValue("@Pwd2_Salt", DBNull.Value);
                                cmd.Parameters.AddWithValue("@Pwd2_Hash", DBNull.Value);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                                Response.Redirect("190298T_LoginForm", false);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Response.Redirect("/Error/GenericError.htmL", false);
                }
            }
            else
            {
                tb_firstname.Text = tb_firstname.Text;
                tb_lastname.Text = tb_lastname.Text;
                tb_cc.Text = tb_cc.Text;
                tb_email.Text = tb_email.Text;
                tb_pwd.Text = tb_pwd.Text;
                tb_dob.Text = tb_dob.Text;
            }
        }

        protected byte[] EncryptCC (string data)
        {
            byte[] ciphertext = null;

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = CCIV;
                cipher.Key = CCKey;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                ciphertext = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return ciphertext;
        }

        protected byte[] EncryptCVV(string data)
        {
            byte[] ciphertext = null;

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = CVVIV;
                cipher.Key = CVVKey;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                ciphertext = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return ciphertext;
        }
    }
}
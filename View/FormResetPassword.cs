using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Bunifu.Framework.UI;
using Bunifu.UI.WinForms;
using DO_AN_CUA_HAN.Model;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DO_AN_CUA_HAN.Functional;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Net.Mail;

namespace DO_AN_CUA_HAN.View
{
    public partial class FormResetPassword : Form
    {
        public FormResetPassword()
        {
            
            InitializeComponent();
        }

        private void bunifuButtonResetPassword_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable dtb;
            string USERNAME = bunifuTextBoxUsername.Text;
            string sqlSelect = @"SELECT * FROM STAFF WHERE STAFFID = @Staffid";
            SqlParameter[] sqlParameters = { new SqlParameter("@Staffid", USERNAME) };
            dtb = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (!(dtb.Rows.Count > 0))
            {
                bunifuSnackbar1.Show(this, "Tài khoản không hợp lệ", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error,
                1000, "Lỗi đăng nhập", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);
                return;
            }
            string EMAIL = dtb.Rows[0]["EMAIL"].ToString();
            string newPassword = GenerateRandomPassword(8, 14);
            string subject = "Reset password (eHospital)";
            string body = "Mật khẩu mới: " + newPassword;
            string hashedPassword = Model.Bcrypt.CreateMD5(newPassword);
            string sqlUpdate = @"UPDATE STAFF SET  PASSWORD = @Password WHERE (STAFFID = @StaffID)";
            SqlParameter[] sqlParametersUpdate = { new SqlParameter("@Password", hashedPassword),
                                                    new SqlParameter("@StaffID", USERNAME)};
            SqlResult.ExecuteNonQuery(sqlUpdate, sqlParametersUpdate);
            Send(EMAIL, subject, body);
            bunifuSnackbar1.Show(this, "Reset password thành công. Vui lòng kiểm tra email", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 
            3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
            this.Cursor = Cursors.Default;
        }

        private void bunifuButtonClose_Click(object sender, EventArgs e)
        {
            FormLogin_SignUp formLogin = new FormLogin_SignUp();
            formLogin.FormClosed += new FormClosedEventHandler(FormResetClose);
            formLogin.Show();
            this.Hide();
        }
        private void FormResetClose(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
        private string GenerateRandomPassword(int minLength, int maxLength)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-=_+";

            using (var rng = new RNGCryptoServiceProvider())
            {
                int length = new Random().Next(minLength, maxLength + 1);

                char[] password = new char[length];
                byte[] randomData = new byte[length];

                rng.GetBytes(randomData);

                for (int i = 0; i < length; i++)
                {
                    password[i] = validChars[randomData[i] % validChars.Length];
                }

                return new string(password);
            }
        }
        public SmtpClient client = new SmtpClient();
        public MailMessage msg = new MailMessage();
        public System.Net.NetworkCredential smtpCreds = new System.Net.NetworkCredential("dat123mall@gmail.com", "rifwqsevegcrcuip");
        public void Send(string sendTo, string subject, string body)
        {
            try
            {
                //setup SMTP Host Here
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = smtpCreds;
                client.EnableSsl = true;

                //converte string to MailAdress
                MailAddress to = new MailAddress(sendTo);
                MailAddress from = new MailAddress("dat123mall@gmail.com", "IT008_Nhóm17");

                //set up message settings
                msg.Subject = subject;
                msg.Body = body;
                msg.From = from;
                msg.To.Add(to);

                // Enviar E-mail
                client.Send(msg);

            }
            catch (Exception error)
            {
                bunifuSnackbar1.Show(this, "Unexpected Error: " + error, Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error,
                1000,null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);
            }
        }
    }
}


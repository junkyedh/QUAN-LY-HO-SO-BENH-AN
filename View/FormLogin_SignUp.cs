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

namespace DO_AN_CUA_HAN.View
{
    public partial class FormLogin_SignUp : Form
    {
        public FormLogin_SignUp()
        {
            
            InitializeComponent();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            Login();
            bunifuTextBoxUsername.Clear();
            bunifuTextBoxPassword.Clear();
        }
        private void FormLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
        private void Login()
        {
            int staffID;
            Staff loginStaff;
            

            // If fields are not validated then do nothing
            if (string.IsNullOrEmpty(bunifuTextBoxUsername.Text))
            {
                bunifuSnackbar1.Show(this, "Nhập tài khoản", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);

            }
            if (string.IsNullOrEmpty(bunifuTextBoxPassword.Text))
            {
                bunifuSnackbar1.Show(this, "Nhập mật khẩu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);

            }

            try
            {
                // Check if username is number only
                if (int.TryParse(bunifuTextBoxUsername.Text, out staffID))
                {
                    loginStaff = Staff.GetStaff(staffID);

                    // Check if username and password is valid
                    if ((loginStaff.StaffID != 0) && (loginStaff.Password.Trim().Equals(Model.Bcrypt.CreateMD5(bunifuTextBoxPassword.Text))))
                    {
                        // Show FormMain and hide FormLogin
                        FormMain formMain = new FormMain(loginStaff);
                        formMain.FormClosed += new FormClosedEventHandler(FormLogin_FormClosed);
                        formMain.Show();
                        this.Hide();
                    }
                    else
                    {
                        bunifuSnackbar1.Show(this, "Tài khoản không hợp lệ", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 1000, "Sai mật khẩu", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);
                    }
                }
                else
                {
                    bunifuSnackbar1.Show(this, "Tài khoản không hợp lệ", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 1000, "Lỗi đăng nhập", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);
                }
            }
            catch
            {
                bunifuSnackbar1.Show(this, "Không thể kết nối với cơ sở dữ liệu. Vui lòng kiểm tra lại tùy chỉnh", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 1000, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopLeft);
            }
        }

        private void bunifuTextBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void bunifuTextBoxUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if(bunifuTextBoxPassword.PasswordChar == '*')
            {
                bunifuPictureBoxEyeOpen.BringToFront();
                bunifuTextBoxPassword.PasswordChar = '\0';
            }    
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            if (bunifuTextBoxPassword.PasswordChar == '\0')
            {
                bunifuPictureBoxEyeClose.BringToFront();
                bunifuTextBoxPassword.PasswordChar = '*';
            }
        }

        private void bunifuButtonSignUp_Click(object sender, EventArgs e)
        {
            FormResetPassword formResetPassword = new FormResetPassword();
            formResetPassword.FormClosed += new FormClosedEventHandler(FormLogin_FormClosed);
            formResetPassword.Show();
            this.Hide();
        }
    }
}


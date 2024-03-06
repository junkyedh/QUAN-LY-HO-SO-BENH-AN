using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Model;
using System.Windows.Forms.VisualStyles;

namespace DO_AN_CUA_HAN.View
{
    public partial class FormStaffDetail : Form
    {
        // Property to store staffdetail and useraction
        private Staff StaffDetail { get; set; }
        private string UserAction { get; set; }

        // Cannot call default constructor
        public FormStaffDetail()
        {
            InitializeComponent();
        }

        // Constructor with useraction and staffdetail
        public FormStaffDetail(string userAction, Staff staff)
        {
            InitializeComponent();

            // Set useraction and staff
            this.StaffDetail = staff;
            this.UserAction = userAction;

            // Set default gender
            dropDownGender.SelectedIndex = 0;
            dropDownState.SelectedIndex = 0;

            // Get department list and set it to dropDown
            dropDownDepartment.DataSource = Department.GetListDepartment();
            dropDownDepartment.ValueMember = "DEPARTMENTID";
            dropDownDepartment.DisplayMember = "DEPARTMENTNAME";

            // Get major list and set it to dropDown
            dropDownMajor.DataSource = Major.GetListMajor();
            dropDownMajor.ValueMember = "MAJORID";
            dropDownMajor.DisplayMember = "MAJORNAME";

            dropDownRole.DataSource = Role.GetListRole();
            dropDownRole.ValueMember = "ROLEID";
            dropDownRole.DisplayMember = "ROLENAME";

            // If useraction is edit then set staffdetail to staffdetail form
            if ("edit".Equals(userAction))
            {
                textBoxPassword.ReadOnly = false;
                textBoxPasswordCheck.ReadOnly = false;
                setStaffDetail(staff);
            }
            else if ("personalEdit".Equals(userAction))
            {
                SetPersonalDetail(staff);
            }
            else if ("add".Equals(userAction))
            {
                textBoxPassword.ReadOnly = true;
                textBoxPasswordCheck.ReadOnly = true;
            }
        }

        // Handle event ok button click
        private void buttonOk_Click(object sender, EventArgs e)
        {
            decimal tempDecimal;
            if (textBoxPassword.Text != textBoxPasswordCheck.Text)
            {
                bunifuSnackbar1.Show(this, "Mật khẩu xác nhận không khớp", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            // If fields is not validated then do nothing
            if (string.IsNullOrEmpty(textBoxFirstName.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu tên", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            if (string.IsNullOrEmpty(textBoxLastName.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu họ", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            if (string.IsNullOrEmpty(textBoxIdentityCard.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu thông tin CCCD", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            if (string.IsNullOrEmpty(textBoxEmail.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu email", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            if (string.IsNullOrEmpty(textBoxAddress.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu địa chỉ", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
           

            // Set StaffDetail property with value in staffdetail form            
            StaffDetail.DepartmentID = Convert.ToInt32(dropDownDepartment.SelectedValue.ToString());
            StaffDetail.MajorID = Convert.ToInt32(dropDownMajor.SelectedValue.ToString());
            StaffDetail.RoleID = Convert.ToInt32(dropDownRole.SelectedValue.ToString());
            //StaffDetail.Password = textBoxPassword.Text;
            StaffDetail.FirstName = textBoxFirstName.Text;
            StaffDetail.LastName = textBoxLastName.Text;
            StaffDetail.BirthDay = dateBirthday.Value;

            if (Decimal.TryParse(textBoxIdentityCard.Text, out tempDecimal))
            {
                StaffDetail.ICN = Convert.ToDecimal(textBoxIdentityCard.Text);
            }

            if ("Nam".Equals(dropDownGender.SelectedItem.ToString()))
            {
                StaffDetail.Gender = Staff.GENDER_MALE;
            }
            else
            {
                StaffDetail.Gender = Staff.GENDER_FEMALE;
            }

            StaffDetail.Email = textBoxEmail.Text;

            StaffDetail.Address = textBoxAddress.Text;

            if (dropDownState.SelectedIndex == 0)
            {
                StaffDetail.State = 0;
            }
            else
            {
                StaffDetail.State = 1;
            }

            // Process useraction
            try
            {
                // If useraction is add then insert to database else update
                if ("add".Equals(this.UserAction))
                {
                    StaffDetail.Password = Model.Bcrypt.CreateMD5(StaffDetail.ICN.ToString()); //password mới tạo là cccd
                    Staff.InsertStaff(StaffDetail);
                }
                else if ("edit".Equals(this.UserAction))
                {
                    DialogResult dialogResult = MessageBox.Show("Xác nhận cập nhập thông tin nhân viên", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Staff.UpdateStaff(StaffDetail);
                    }
                }
                else if ("personalEdit".Equals(this.UserAction))
                {
                    DialogResult dialogResult = MessageBox.Show("Xác nhận cập nhập thông tin tài khoản cá nhân", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (textBoxPassword.Text != "")
                        {
                            StaffDetail.Password = Model.Bcrypt.CreateMD5(textBoxPassword.Text);
                        }
                        Staff.UpdateStaff(StaffDetail);
                    }
                }
            }
            catch(Exception ex)
            {
                bunifuSnackbar1.Show(this, ex.Message, Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }

            // After process then close this form
            this.Close();
        }

        // Close form when click close button
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void SetPersonalDetail(Staff staff)
        {
            this.StaffDetail = staff;
            textBoxStaffID.Text = staff.StaffID.ToString();
            textBoxFirstName.Text = staff.FirstName;
            textBoxLastName.Text = staff.LastName;
            dateBirthday.Value = staff.BirthDay;
            if (staff.ICN != 0)
            {
                textBoxIdentityCard.Text = staff.ICN.ToString();
            }

            if (Staff.GENDER_MALE.Equals(staff.Gender))
            {
                dropDownGender.SelectedIndex = Staff.GENDER_MALE;
            }
            else
            {
                dropDownGender.SelectedIndex = Staff.GENDER_FEMALE;
            }

            textBoxEmail.Text = staff.Email;

            textBoxAddress.Text = staff.Address;

            if (staff.State == 0)
            {
                dropDownState.Text = "Đang làm việc";
            }
            else
            {
                dropDownState.Text = "Đã thôi việc";
            }

            dropDownDepartment.SelectedValue = (object)staff.DepartmentID;
            dropDownMajor.SelectedValue = (object)staff.MajorID;
            dropDownRole.SelectedValue = (object)staff.RoleID;

            dropDownDepartment.Enabled = false;
            dropDownMajor.Enabled = false;
            dropDownRole.Enabled = false;
            dropDownState.Enabled = false;
            textBoxFirstName.ReadOnly = true;
            textBoxLastName.ReadOnly = true;
            textBoxIdentityCard.ReadOnly = true;
            dropDownGender.Enabled = false;
            dateBirthday.Enabled = false;
            textBoxEmail.ReadOnly = false;
        }
        // Set staffdetail to staffdetail form
        public void setStaffDetail(Staff staff)
        {
            this.StaffDetail = staff;
            textBoxStaffID.Text = staff.StaffID.ToString();
            textBoxFirstName.Text = staff.FirstName;
            textBoxLastName.Text = staff.LastName;
            dateBirthday.Value = staff.BirthDay;
            if (staff.ICN != 0)
            {
                textBoxIdentityCard.Text = staff.ICN.ToString();
            }

            if (Staff.GENDER_MALE.Equals(staff.Gender))
            {
                dropDownGender.SelectedIndex = Staff.GENDER_MALE;
            }
            else
            {
                dropDownGender.SelectedIndex = Staff.GENDER_FEMALE;
            }
            
            textBoxEmail.Text = staff.Email;
            textBoxAddress.Text = staff.Address;

            if (staff.State == 0)
            {
                dropDownState.Text = "Đang làm việc";
            }
            else
            {
                dropDownState.Text = "Đã thôi việc";
            }

            dropDownDepartment.SelectedValue = (object)staff.DepartmentID;
            dropDownMajor.SelectedValue = (object)staff.MajorID;
            dropDownRole.SelectedValue = (object)staff.RoleID;

            textBoxPassword.ReadOnly = true;
            textBoxPasswordCheck.ReadOnly = true;


        }

        private void textBoxFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = !((e.KeyChar >= 'A' && e.KeyChar <= 'Z') ||
              (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
              e.KeyChar == 8 || // Backspace
              e.KeyChar == 32 || // Space
              e.KeyChar == 16 || // Shift
              e.KeyChar == 46 || // Delete
              (e.KeyChar >= 'À' && e.KeyChar <= 'ỹ') || // Vietnamese characters and accented vowels
              (e.KeyChar >= 'à' && e.KeyChar <= 'ỹ'));
        }

        private void textBoxLastName_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = !((e.KeyChar >= 'A' && e.KeyChar <= 'Z') ||
              (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
              e.KeyChar == 8 || // Backspace
              e.KeyChar == 32 || // Space
              e.KeyChar == 16 || // Shift
              e.KeyChar == 46 || // Delete
              (e.KeyChar >= 'À' && e.KeyChar <= 'ỹ') || // Vietnamese characters and accented vowels
              (e.KeyChar >= 'à' && e.KeyChar <= 'ỹ'));
        }

        private void textBoxIdentityCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép chỉ là số hoặc các phím điều khiển (Backspace, Delete)
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar))
            {
                // Nếu chiều dài của chuỗi là 9, không cho phép nhập thêm
                if (textBoxIdentityCard.Text.Length >= 9 && e.KeyChar != 8 && e.KeyChar != 127)
                {
                    e.Handled = true;
                }
            }
            else
            {
                // Nếu ký tự không phải là số hoặc không phải là các phím điều khiển, không cho phép nhập
                e.Handled = true;
            }
        }

    }
}

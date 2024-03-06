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
using Bunifu.UI.WinForms;

namespace  DO_AN_CUA_HAN.View
{
    public partial class FormPatientDetail : Form
    {
        // Property to store patientdetail and useraction
        private Patient PatientDetail { get; set; }
        private string UserAction { get; set; }

        // Cannot call default constructor
        public FormPatientDetail()
        {
            InitializeComponent();
        }

        //Constructor with useraction and patientdetail
        public FormPatientDetail(string userAction, Patient patient)
        {
            InitializeComponent();

            // Set useraction and patientdetail
            this.PatientDetail = patient;
            this.UserAction = userAction;

            // Set useraction and staff
            dropDownGender.SelectedIndex = 0;
            dropDownState.SelectedIndex = 0;

            // If useraction is edit then set patientdetail to patientdetail form
            if ("edit".Equals(userAction))
            {
                setPatientDetail(patient);
            }
        }

        // Handle event ok button click
        private void buttonOk_Click(object sender, EventArgs e)
        {
            decimal tempDecimal;

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
                bunifuSnackbar1.Show(this, "Thiếu thông CCCD", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }

            // Set PatientDetail property with value in patientdetail form
            PatientDetail.FirstName = textBoxFirstName.Text;
            PatientDetail.LastName = textBoxLastName.Text;
            PatientDetail.BirthDay = dateBirthday.Value;

            if (Decimal.TryParse(textBoxIdentityCard.Text, out tempDecimal))
            {
                PatientDetail.ICN = Convert.ToDecimal(textBoxIdentityCard.Text);
            }

            if ("Nam".Equals(dropDownGender.SelectedItem.ToString()))
            {
                PatientDetail.Gender = Patient.GENDER_MALE;
            }
            else
            {
                PatientDetail.Gender = Patient.GENDER_FEMALE;
            }

            PatientDetail.Profession = textBoxProfession.Text;
            PatientDetail.Address = textBoxAddress.Text;

            if (!"".Equals(textBoxDeposit.Text))
            {
                PatientDetail.Deposit = Convert.ToDecimal(textBoxDeposit.Text);
            }

            if (dropDownState.SelectedIndex == 0)
            {
                PatientDetail.State = 0;
            }
            else
            {
                PatientDetail.State = 1;
            }

            // Process useraction
            try
            {
                // If useraction is add then insert to database else update
                if ("add".Equals(this.UserAction))
                {
                    Patient.InsertPatient(PatientDetail);
                }
                else if ("edit".Equals(this.UserAction))
                {
                    DialogResult dialogResult = MessageBox.Show("Xác nhận cập nhập thông tin bệnh nhân", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Patient.UpdatePatient(PatientDetail);
                    }
                }
            }
            catch
            {
                bunifuSnackbar1.Show(this, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }

            // After process then close this form
            this.Close();
        }

        // Close this form when click close button
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Set value in patientdetail form with PatienDetail 
        public void setPatientDetail(Patient patient)
        {
            this.PatientDetail = patient;

            textBoxPatientID.Text = patient.PatientID.ToString();
            textBoxFirstName.Text = patient.FirstName;
            textBoxLastName.Text = patient.LastName;
            dateBirthday.Value = patient.BirthDay;

            if (patient.ICN != 0)
            {
                textBoxIdentityCard.Text = patient.ICN.ToString();
            }

            if (patient.Gender == Patient.GENDER_MALE)
            {dropDownGender.Text = "Nam";
            }
            else
            {
                dropDownGender.Text = "Nữ";
            }
            textBoxProfession.Text = patient.Profession;
            textBoxAddress.Text = patient.Address;

            if (patient.State == 0)
            {
                dropDownState.Text = "Ngoại trú";
            }
            else
            {
                dropDownState.Text = "Nội trú";
            }
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

        private void textBoxProfession_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxDeposit_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}

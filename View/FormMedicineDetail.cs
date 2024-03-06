using DO_AN_CUA_HAN.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DO_AN_CUA_HAN.View
{
    public partial class FormMedicineDetail : Form
    {
        public Medicine MedicineDetail { get; set; }
        public String UserAction { get; set; }
        public FormMedicineDetail()
        {
            InitializeComponent();
        }
        public FormMedicineDetail(Medicine medicineDetail, String userAction)
        {
            InitializeComponent();
            this.MedicineDetail = medicineDetail;
            this.UserAction = userAction;
            if (this.UserAction == "edit")
                SetMedicineDetail(medicineDetail);
        }
        private void SetMedicineDetail(Medicine medicineDetail)
        {
            bunifuTextBoxMedicineID.Text = medicineDetail.MedicineID.ToString();
            bunifuTextBoxMedicineName.Text = medicineDetail.MedicineName;
            bunifuTextBoxQuantity.Text = medicineDetail.Quantity.ToString();
            bunifuTextBoxPrice.Text = Convert.ToInt64(medicineDetail.Price).ToString();
        }

        private void bunifuButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuButtonOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bunifuTextBoxMedicineName.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu tên thuốc", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            if (string.IsNullOrEmpty(bunifuTextBoxQuantity.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu số lượng thuốc", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            if (string.IsNullOrEmpty(bunifuTextBoxPrice.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu đơn giá", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            try
            {
                if (UserAction == "edit")
                {
                    MedicineDetail.MedicineName = bunifuTextBoxMedicineName.Text;
                    MedicineDetail.Quantity = int.Parse(bunifuTextBoxQuantity.Text);
                    MedicineDetail.Price = decimal.Parse(bunifuTextBoxPrice.Text);
                    DialogResult dialogResult = MessageBox.Show("Xác nhận cập nhập thông tin thuốc", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (Medicine.UpdateMedicine(MedicineDetail) > 0)
                        {
                            bunifuSnackbar1.Show(this, "Cập nhật thông tin thuốc thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                            return;
                        }

                    }

                }
                else
                {
                    Medicine newMedicine = new Medicine(0, bunifuTextBoxMedicineName.Text, int.Parse(bunifuTextBoxQuantity.Text), decimal.Parse(bunifuTextBoxPrice.Text));
                    if (Medicine.InsertMedicine(newMedicine) > 0)
                    {
                        bunifuSnackbar1.Show(this, "Thêm thuốc thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                        return;
                    }

                }
            }
            catch
            {
                bunifuSnackbar1.Show(this, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }

            this.Close();
        }

        private void bunifuTextBoxMedicineName_KeyPress(object sender, KeyPressEventArgs e)
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

        private void bunifuTextBoxQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}

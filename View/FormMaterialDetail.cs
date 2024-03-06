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

namespace DO_AN_CUA_HAN.View
{
    public partial class FormMaterialDetail : Form
    {
        public Material MaterialDetail { get; set; }
        public String UserAction { get; set; }
        public FormMaterialDetail()
        {
            InitializeComponent();
        }
        public FormMaterialDetail(Material materialDetail, String userAction)
        {
            InitializeComponent();
            this.MaterialDetail = materialDetail;
            this.UserAction = userAction;
            if (this.UserAction == "edit")
                SetMaterialDetail(materialDetail);
        }
        private void SetMaterialDetail(Material materialDetail)
        {
            textBoxMaterialID.Text = materialDetail.MaterialID.ToString();
            textBoxMaterialName.Text = materialDetail.MaterialName;
            textBoxQuantity.Text = materialDetail.Quantity.ToString();
            textBoxPrice.Text = materialDetail.Price.ToString();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBoxMaterialName.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu tên vật tư", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            if (string.IsNullOrEmpty(textBoxQuantity.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu số lượng vật tư", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu thông tên giá vật tư", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            try
            {
                if (UserAction == "edit")
                {
                    MaterialDetail.MaterialName = textBoxMaterialName.Text;
                    MaterialDetail.Quantity = int.Parse(textBoxQuantity.Text);
                    MaterialDetail.Price = decimal.Parse(textBoxPrice.Text);
                    DialogResult dialogResult = MessageBox.Show("Xác nhận cập nhập thông tin vật tư", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (Material.UpdateMaterial(MaterialDetail) > 0)
                        {
                            bunifuSnackbar1.Show(this, "Cập nhập thông tin vật tư thành công thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                            return;
                        }

                    }
                }
                else
                {
                    Material newMaterial = new Material(0, textBoxMaterialName.Text, int.Parse(textBoxQuantity.Text), decimal.Parse(textBoxPrice.Text));
                    if (Material.InsertMaterial(newMaterial) > 0)
                    {
                        bunifuSnackbar1.Show(this, "Thêm vật tư thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                        return;
                    }
                    
                }
            }
            catch
            {
                bunifuSnackbar1.Show(this, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }

            this.Close();
        }

        private void textBoxQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxMaterialName_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}

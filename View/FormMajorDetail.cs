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
using System.Windows.Forms.VisualStyles;

namespace DO_AN_CUA_HAN.View
{
    public partial class FormMajorDetail : Form
    {
        public Major MajorDetail { get; set; }
        public String UserAction { get; set; }
        public FormMajorDetail()
        {
            InitializeComponent();
        }
        public FormMajorDetail(Major majorDetail, String userAction)
        {
            InitializeComponent();
            this.MajorDetail = majorDetail;
            this.UserAction = userAction;
            SetMajorDetail(majorDetail);
        }
        private void SetMajorDetail(Major majorDetail)
        {
            bunifuTextBoxMajorID.Text = majorDetail.MajorID.ToString();
            bunifuTextBoxMajorName.Text = majorDetail.MajorName;
        }

        private void bunifuButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuButtonOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bunifuTextBoxMajorName.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu tên phòng ban", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            try
            {
                Major newMajor = new Major();

                newMajor.MajorName = bunifuTextBoxMajorName.Text;
                if (UserAction == "edit")
                {
                    newMajor.MajorID = Convert.ToInt32(bunifuTextBoxMajorID.Text);
                    DialogResult dialogResult = MessageBox.Show("Xác nhận cập nhập thông tin chuyên ngành", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (Major.UpdateMajor(newMajor) > 0)
                        {
                            bunifuSnackbar1.Show(this, "Cập nhập thông tin chuyên ngành thành công thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                            return;
                        }
                    }

                }
                else
                {
                    newMajor.MajorID = 0;
                    if (Major.InsertMajor(newMajor) > 0)
                    {
                        bunifuSnackbar1.Show(this, "Thêm phòng khoa thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
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
    }
}

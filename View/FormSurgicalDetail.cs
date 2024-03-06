using Bunifu.UI.WinForms;
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
    public partial class FormSurgicalDetail : Form
    {
        public Surgical surgicalDetail { get; set; }
        public String UserAction { get; set; }
        private static List<Staff> listStaff = new List<Staff>();
        private static List<SurgicalDetail> listSD = new List<SurgicalDetail>();
        public FormSurgicalDetail()
        {
            InitializeComponent();
        }
        public FormSurgicalDetail(int patientID)
        {
            InitializeComponent();
            SetSDForInsert(patientID);
        }
        private void SetSDForInsert(int patientID)
        {
            bunifuTextBoxPatientID.Text = patientID.ToString();
            bunifuDatePickerCreate.Value = DateTime.Today;
            bunifuDropdownState.SelectedIndex = 0;
            bunifuDropdownState.Enabled = false;

            DataTable dtStaff = Staff.GetListStaff();
            for (int i = 0; i < dtStaff.Rows.Count; i++)
            {
                String staffName = dtStaff.Rows[i][6].ToString() + " " + dtStaff.Rows[i][5].ToString();
                Staff newStaff = Staff.GetStaff(Convert.ToInt32(dtStaff.Rows[i][0]));
                listStaff.Add(newStaff);
                listBoxSystemStaff.Items.Add(staffName);
            }
            listBoxSystemStaff.SelectedIndex = 0;
        }
        public FormSurgicalDetail(Surgical sDetail, String userAction)
        {
            InitializeComponent();
            this.surgicalDetail = sDetail;
            this.UserAction = userAction;
            SetSDForUpdate(sDetail);
        }
        private void SetSDForUpdate(Surgical sDetail)
        {
            bunifuTextBoxPatientID.Text = sDetail.PatientID.ToString();
            bunifuTextBoxSurgicalID.Text = sDetail.SurgicalID.ToString();
            bunifuDatePickerCreate.Value = sDetail.Date;
            bunifuTextBoxDescription.Text = sDetail.Description;
            bunifuDropdownState.SelectedIndex = sDetail.State;

            DataTable dtStaff = Staff.GetListStaff();
            for (int i = 0; i < dtStaff.Rows.Count; i++)
            {
                String staffName = dtStaff.Rows[i][6].ToString() + " " + dtStaff.Rows[i][5].ToString();
                Staff newStaff = Staff.GetStaff(Convert.ToInt32(dtStaff.Rows[i][0]));
                listStaff.Add(newStaff);
                listBoxSystemStaff.Items.Add(staffName);
            }
            listBoxSystemStaff.SelectedIndex = 0;

            DataTable dtSD = SurgicalDetail.GetListSurgicalDetail(sDetail.SurgicalID);
            for (int i = 0; i < dtSD.Rows.Count; i++)
            {
                String staffName = dtSD.Rows[i][2].ToString() + " " + dtSD.Rows[i][3].ToString();
                SurgicalDetail newSD = new SurgicalDetail();
                newSD.SurgicalID = Convert.ToInt32(dtSD.Rows[i][0]);
                newSD.StaffID = Convert.ToInt32(dtSD.Rows[i][1]);
                listSD.Add(newSD);
                listBoxCurrentStaff.Items.Add(staffName);
            }
            if (listBoxCurrentStaff.Items.Count > 0)
                listBoxCurrentStaff.SelectedIndex = 0;
        }

        private void bunifuImageButtonRemoveFunction_Click(object sender, EventArgs e)
        {
            if (listBoxCurrentStaff.SelectedIndex != -1)
            {
                int selectedStaff = listBoxCurrentStaff.SelectedIndex;
                listSD.RemoveAt(selectedStaff);
                listBoxCurrentStaff.Items.RemoveAt(selectedStaff);
                listBoxCurrentStaff.SelectedIndex = listBoxCurrentStaff.Items.Count - 1;
            }
        }

        private void bunifuImageButtonInsertFunction_Click(object sender, EventArgs e)
        {
            int selectedStaff = listBoxSystemStaff.SelectedIndex;
            if (CheckStaffInSurgical(listStaff[selectedStaff].StaffID))
            {
                bunifuSnackbar1.Show(this, "Nhân viên đã có trong danh sách tham gia ca phẩu thuật", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            else
            {
                SurgicalDetail newSD = new SurgicalDetail(0, listStaff[selectedStaff].StaffID);
                listSD.Add(newSD);
                listBoxCurrentStaff.Items.Add(listBoxSystemStaff.Items[selectedStaff].ToString());
                listBoxCurrentStaff.SelectedIndex = 0;
            }
        }
        private Boolean CheckStaffInSurgical(int staffID)
        {
            for (int i = 0; i < listSD.Count; i++)
            {
                if (staffID == listSD[i].StaffID)
                    return true;
            }
            return false;
        }

        private void bunifuButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuButtonOK_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(bunifuTextBoxDescription.Text))
            {
                bunifuSnackbar1.Show(this, "Thiếu mô tả ca phẫu thuật", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
            if (listBoxCurrentStaff.Items.Count > 0)
            {
                try
                {
                    Surgical newSurgical = new Surgical();
                    newSurgical.PatientID = Convert.ToInt32(bunifuTextBoxPatientID.Text);
                    newSurgical.Date = bunifuDatePickerCreate.Value;
                    newSurgical.State = bunifuDropdownState.SelectedIndex;
                    newSurgical.Description = bunifuTextBoxDescription.Text;
                    if (this.UserAction == "edit")
                    {
                        newSurgical.SurgicalID = Convert.ToInt32(bunifuTextBoxSurgicalID.Text);
                        DialogResult dialogResult = MessageBox.Show("Xác nhận cập nhập thông tin ca phẩu thuật này không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.OK)
                        {
                            if (Surgical.UpdateSurgical(newSurgical) > 0)
                            {
                                SurgicalDetail.DeleteSurgicalDetail(newSurgical.SurgicalID);
                                for (int i = 0; i < listSD.Count; i++)
                                {
                                    SurgicalDetail newSD = listSD[i];
                                    newSD.SurgicalID = newSurgical.SurgicalID;
                                    SurgicalDetail.InsertSurgicalDetail(newSD);
                                }
                                listSD.Clear();
                                bunifuSnackbar1.Show(this, "Cập nhật thông tin ca phẩu thuật thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (Surgical.InsertSurgical(newSurgical) > 0)
                        {
                            int curSurgicalID = Surgical.GetCurrentIdentity();
                            for (int i = 0; i < listSD.Count; i++)
                            {
                                listSD[i].SurgicalID = curSurgicalID;
                                SurgicalDetail.InsertSurgicalDetail(listSD[i]);
                            }
                            bunifuSnackbar1.Show(this, "Thêm ca phẫu thuật thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                            return;
                        }
                    }
                }
                catch
                {
                    bunifuSnackbar1.Show(this, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                    return;
                }
                listSD.Clear();
                this.Close();
            }
            else
            {
                bunifuSnackbar1.Show(this, "Chưa có nhân viên nào tham gia ca phẫu thuật", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                return;
            }
        }
    }
}

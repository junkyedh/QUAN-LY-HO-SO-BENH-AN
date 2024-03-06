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
    public partial class FormMainStaff : UserControl
    {
        public FormMainStaff()
        {
            InitializeComponent();
        }
        public void tabItemStaff_Click()
        {
            refreshDataViewStaff();
        }
        private void refreshDataViewStaff()
        {
            try
            {
                // Get Staff's datatable
                DataTable staffTable = Staff.GetListStaff();

                // Add Vietnamese column's name
                staffTable.Columns.Add("Mã nhân viên", typeof(string), "[STAFFID]");
                staffTable.Columns.Add("Họ tên", typeof(string), "[LASTNAME] + ' ' + [FIRSTNAME]");
                staffTable.Columns.Add("Quyền", typeof(string), "[ROLENAME]");
                staffTable.Columns.Add("Khoa", typeof(string), "[DEPARTMENTNAME]");
                staffTable.Columns.Add("Chuyên ngành", typeof(string), "[MAJORNAME]");
                staffTable.Columns.Add("CMND", typeof(string), "[ICN]");
                staffTable.Columns.Add("Giới tính", typeof(string), "IIF([GENDER] = 0, 'Nam', 'Nữ')");
                staffTable.Columns.Add("Ngày sinh", typeof(DateTime), "[BIRTHDAY]");
                staffTable.Columns.Add("Email", typeof(string), "[EMAIL]");
                staffTable.Columns.Add("Địa chỉ", typeof(string), "[ADDRESS]");
                staffTable.Columns.Add("Trạng thái", typeof(string), "IIF([STATE] = 0, 'Đã thôi việc', 'Đang làm việc')");

                // Set data source to dataview for searching
                bunifuDataGridViewStaff.DataSource = staffTable.DefaultView;

                // Hide English columns'name
                for (int i = 0; i < 13; i++)
                {
                    bunifuDataGridViewStaff.Columns[i].Visible = false;
                }
            }
            catch
            {
                bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                
            }

        }

        private void bunifuButtonStaffAdd_Click(object sender, EventArgs e)
        {
            // Open staffdetail form for add
            FormStaffDetail staffDetailForm = new FormStaffDetail("add", new Staff());
            staffDetailForm.ShowDialog();

            // Refresh datagridview after add
            refreshDataViewStaff();
        }

        private void bunifuDataGridViewStaff_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuDataGridViewStaff.SelectedRows.Count > 0)
            {
                // Get staff for edit
                Staff StaffDetail = Staff.GetStaff(Convert.ToInt32(bunifuDataGridViewStaff.SelectedRows[0].Cells[0].Value.ToString()));

                // Open staffdetail form for edit
                FormStaffDetail staffDetailForm = new FormStaffDetail("edit", StaffDetail);
                staffDetailForm.ShowDialog();

                // Refresh datagridview after edit
                refreshDataViewStaff();
            }
        }

        private void bunifuButtonStaffEdit_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridViewStaff.SelectedRows.Count > 0)
            {
                // Get staff for edit
                Staff StaffDetail = Staff.GetStaff(Convert.ToInt32(bunifuDataGridViewStaff.SelectedRows[0].Cells[0].Value.ToString()));

                // Open staffdetail form for edit
                FormStaffDetail staffDetailForm = new FormStaffDetail("edit", StaffDetail);
                staffDetailForm.ShowDialog();

                // Refresh datagridview after edit
                refreshDataViewStaff();
            }
        }

        private void bunifuButtonStaffDelete_Click(object sender, EventArgs e)
        {
            int staffID;

            try
            {

                // Warning before delete
                if (MessageBox.Show("Xác nhận xóa nhân viên", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    == DialogResult.Yes)

                    // Get staffid for delete
                    if (bunifuDataGridViewStaff.SelectedRows.Count > 0)

                    {

                        // Get staffid for delete
                        if (int.TryParse(bunifuDataGridViewStaff.SelectedRows[0].Cells[0].Value.ToString(), out staffID))
                        {
                            Staff.DeleteStaff(staffID);
                        }
                    }
            }
            catch
            {
                bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                
            }
            // Refresh datagridview after delete
            refreshDataViewStaff();
        }

        private void bunifubuttonStaffDeleteSearch_Click(object sender, EventArgs e)
        {
            bunifuTextBoxStaffSearch.Text = "";
            searchStaff();
        }

        private void bunifuTextBoxStaffSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchStaff();
            }
        }
        private void bunifuTextBoxStaffSearch_TextChange(object sender, EventArgs e)
        {
            searchStaff();
        }
        private void searchStaff()
        {
            // Not search it search string is empty
            if (!"".Equals(bunifuTextBoxStaffSearch.Text))
            {
                // Search with RowFilter
                ((DataView)bunifuDataGridViewStaff.DataSource).RowFilter = "[Họ tên] LIKE '*" + bunifuTextBoxStaffSearch.Text.Trim() + "*'"
                                                                + "OR [Mã nhân viên] LIKE '*" + bunifuTextBoxStaffSearch.Text.Trim() + "*'"
                                                                + "OR [CMND] LIKE '*" + bunifuTextBoxStaffSearch.Text.Trim() + "*'";
            }
            else
                ((DataView)bunifuDataGridViewStaff.DataSource).RowFilter = "";
        }

    }
}

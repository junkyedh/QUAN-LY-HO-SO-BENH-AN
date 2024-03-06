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

namespace DO_AN_CUA_HAN.View
{
    public partial class FormMainAssignment : UserControl
    {
        public FormMainAssignment()
        {
            InitializeComponent();
        }
        // Refresh datagridview when click assignment tab
        public void tabItemAssignment_Click()
        {
            refreshDataViewAssignment();
            refreshDataViewAssignmentDetail();
        }
        private void refreshDataViewAssignment()
        {
            try
            {
                // Get assignment's datatable
                DataTable assignmentTable = Assignment.GetListAssignment();

                // Add Vietnamese column's name
                assignmentTable.Columns.Add("Mã phân công", typeof(string), "[ASSIGNID]");
                /*assignmentTable.Columns.Add("Mã bệnh nhân", typeof(string), "[PATIENTID]");*/
                assignmentTable.Columns.Add("Tên bệnh nhân", typeof(string), "[PATIENT NAME]");
                assignmentTable.Columns.Add("Ngày lập", typeof(DateTime), "[DATE]");
                assignmentTable.Columns.Add("Ngày nhập viện", typeof(DateTime), "[HOPITALIZATEDATE]");
                assignmentTable.Columns.Add("Ngày xuât viện", typeof(DateTime), "[DISCHARGEDDATE]");
                // Set data source to dataview for searching
                bunifuDataGridViewAssignment.DataSource = assignmentTable.DefaultView;

                // Hide English columns'name
                for (int i = 0; i < 6; i++)
                {
                    bunifuDataGridViewAssignment.Columns[i].Visible = false;
                }
            }
            catch
            {
                bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                
            }
        }
        private void refreshDataViewAssignmentDetail()
        {
            if (bunifuDataGridViewAssignment.SelectedRows.Count > 0)
            {
                try
                {
                    // Get AssignmentDetail's datatable
                    int assignmentID = Convert.ToInt32(bunifuDataGridViewAssignment.Rows[0].Cells[0].Value);
                    DataTable assignmentDetailTable = AssignmentDetail.GetListAssignmentDetails(assignmentID);
                    // Set data source to dataview for searching
                    bunifuDataGridViewAssignmentDetail.DataSource = assignmentDetailTable.DefaultView;
                }
                catch
                {
                    bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                    
                }
            }
        }

        private void bunifuTextBoxAssignmentSearch_TextChange(object sender, EventArgs e)
        {
            searchAsssignment();
        }

        private void bunifuTextBoxAssignmentSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchAsssignment();
            }
        }
        private void searchAsssignment()
        {
            // Not search it search string is empty
            if (bunifuTextBoxAssignmentSearch.Text != "")
            {
                // Search with RowFilter
                ((DataView)bunifuDataGridViewAssignment.DataSource).RowFilter = "[Mã phân công] LIKE '*" + bunifuTextBoxAssignmentSearch.Text.Trim() + "*'"
                                                                + "OR [Mã bệnh nhân] LIKE '*" + bunifuTextBoxAssignmentSearch.Text.Trim() + "*'";
                refreshDataViewAssignmentDetail();
            }
            else
            {
                ((DataView)bunifuDataGridViewAssignment.DataSource).RowFilter = "";
            }
        }

        private void bunifubuttonAssignmentDeleteSearch_Click(object sender, EventArgs e)
        {
            bunifuTextBoxAssignmentSearch.Text = "";
            searchAsssignment();
        }

        private void bunifuDataGridViewAssignment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuDataGridViewAssignment.SelectedRows.Count > 0)
            {
                int assignID = Convert.ToInt32(bunifuDataGridViewAssignment.SelectedRows[0].Cells[0].Value);
                DataTable dtAD = AssignmentDetail.GetListAssignmentDetails(assignID);

                bunifuDataGridViewAssignmentDetail.DataSource = dtAD.DefaultView;
            }
        }

        private void bunifuButtonASDelete_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridViewAssignment.SelectedRows.Count > 0)
            {
                int assignID = Convert.ToInt32(bunifuDataGridViewAssignment.SelectedRows[0].Cells[0].Value);
                DialogResult dialogResult = MessageBox.Show("Xác nhận xóa bản phân công", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        if (AssignmentDetail.DeleteAssignmentDetails(assignID) > 0 && Assignment.DeleteAssignment(assignID) > 0)
                        {
                            bunifuSnackbar1.Show(Form.ActiveForm, "Xóa bảng phân công thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                            

                        }
                    }
                    catch
                    {
                        bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                        

                    }
                }
                refreshDataViewAssignment();
                refreshDataViewAssignmentDetail();
            }
        }

        private void bunifuDataGridViewAssignment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuDataGridViewAssignment.SelectedRows.Count > 0)
            {
                int assignID = Convert.ToInt32(bunifuDataGridViewAssignment.SelectedRows[0].Cells[0].Value);
                Assignment updateAssign = Assignment.GetAssignment(assignID);

                FormAssignDetail formAD = new FormAssignDetail(updateAssign, "edit");
                formAD.ShowDialog();
                refreshDataViewAssignment();
                refreshDataViewAssignmentDetail();
            }
        }

        private void bunifuButtonASEdit_Click(object sender, EventArgs e)
        {

            if (bunifuDataGridViewAssignment.SelectedRows.Count > 0)
            {
                int assignID = Convert.ToInt32(bunifuDataGridViewAssignment.SelectedRows[0].Cells[0].Value);
                Assignment updateAssign = Assignment.GetAssignment(assignID);

                FormAssignDetail formAD = new FormAssignDetail(updateAssign, "edit");
                formAD.ShowDialog();
                refreshDataViewAssignment();
                refreshDataViewAssignmentDetail();
            }
        }
    }
}

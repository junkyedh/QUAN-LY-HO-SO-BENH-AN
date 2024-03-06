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
    public partial class FormMainSurgery : UserControl
    {
        public FormMainSurgery()
        {
            InitializeComponent();
        }
        public void tabItemSurgery_Click()
        {
            refreshDataViewSurgical();
            refreshDataViewSurgicalDetail();
        }
        private void refreshDataViewSurgical()
        {
            try
            {
                // Get surgical's datatable
                DataTable surgicalTable = Surgical.GetListSurgical();

                // Add Vietnamese column's name
                surgicalTable.Columns.Add("Mã ca phẩu thuật", typeof(string), "[SURGICALID]");
 /*               surgicalTable.Columns.Add("Mã bệnh nhân", typeof(string), "[PATIENTID]");*/
                surgicalTable.Columns.Add("Tên bệnh nhân", typeof(string), "[PATIENT NAME]");
                surgicalTable.Columns.Add("Ngày thực hiện", typeof(DateTime), "[DATE]");
                surgicalTable.Columns.Add("Mô tả", typeof(string), "[DESCRIPTION]");
                surgicalTable.Columns.Add("Trạng thái", typeof(string), "IIF([STATE] = 0, 'Chưa thực hiện', 'Đã thực hiện')");
                // Set data source to dataview for searching
                bunifuDataGridViewSurgery.DataSource = surgicalTable.DefaultView;

                // Hide English columns'name
                for (int i = 0; i < 6; i++)
                {
                    bunifuDataGridViewSurgery.Columns[i].Visible = false;
                }
            }
            catch
            {
                bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                
            }
        }
        private void refreshDataViewSurgicalDetail()
        {
            if (bunifuDataGridViewSurgery.SelectedRows.Count > 0)
            {
                try
                {
                    // Get SurgicalDetail's datatable
                    int surgicalID = Convert.ToInt32(bunifuDataGridViewSurgery.Rows[0].Cells[0].Value);
                    DataTable surgicalDetailTable = SurgicalDetail.GetListSurgicalDetail(surgicalID);
                    // Set data source to dataview for searching
                    bunifuDataGridViewSurgeryDetail.DataSource = surgicalDetailTable.DefaultView;
                }
                catch
                {
                    bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                    
                }
            }
        }

        private void bunifuDataGridViewSurgery_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuDataGridViewSurgery.SelectedRows.Count > 0)
            {
                // Get SurgicalnDetail's datatable
                int surgicalID = Convert.ToInt32(bunifuDataGridViewSurgery.SelectedRows[0].Cells[0].Value);
                DataTable surgicalDetailTable = SurgicalDetail.GetListSurgicalDetail(surgicalID);

                // Set data source to dataview for searching
                bunifuDataGridViewSurgeryDetail.DataSource = surgicalDetailTable;
            }
        }

        private void bunifuTextBoxSurgerySearch_TextChange(object sender, EventArgs e)
        {
            searchSurgical();
        }
        private void searchSurgical()
        {
            // Not search it search string is empty
            if (bunifuTextBoxSurgerySearch.Text != "")
            {
                // Search with RowFilter
                ((DataView)bunifuDataGridViewSurgery.DataSource).RowFilter = "[Mã ca phẩu thuật] LIKE '*" + bunifuTextBoxSurgerySearch.Text.Trim() + "*'"
                                                                + "OR [Tên bệnh nhân] LIKE '*" + bunifuTextBoxSurgerySearch.Text.Trim() + "*'"
                                                                + "OR [Trạng thái] LIKE '*" + bunifuTextBoxSurgerySearch.Text.Trim() + "*'";
                refreshDataViewSurgicalDetail();
            }
            else
            {
                ((DataView)bunifuDataGridViewSurgery.DataSource).RowFilter = "";
            }
        }

        private void bunifuTextBoxSurgerySearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchSurgical();
            }
        }

        private void bunifubuttonSurgeryDeleteSearch_Click(object sender, EventArgs e)
        {
            bunifuTextBoxSurgerySearch.Text = "";
            searchSurgical();
        }

        private void bunifuButtonSurgeryDelete_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridViewSurgery.SelectedRows.Count > 0)
            {
                int surgicalID = Convert.ToInt32(bunifuDataGridViewSurgery.SelectedRows[0].Cells[0].Value);
                Surgical deleteSurgical = Surgical.GetSurgical(surgicalID);
                if (deleteSurgical.State != 1)
                {
                    DialogResult dialogResult = MessageBox.Show("Xác nhận xóa ca phẩu thuật", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        try
                        {
                            if (SurgicalDetail.DeleteSurgicalDetail(surgicalID) > 0 && Surgical.DeleteSurgical(surgicalID) > 0)
                            {
                                bunifuSnackbar1.Show(Form.ActiveForm, "Xóa ca phẩu thuật thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                                
                            }
                        }
                        catch
                        {
                            bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                            
                        }
                    }
                }
                else
                {
                    bunifuSnackbar1.Show(Form.ActiveForm, "Không thể xóa ca phẩu thuật đã được thực hiện", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                    
                }
                refreshDataViewSurgical();
                refreshDataViewSurgicalDetail();
            }
        }

        private void bunifuDataGridViewSurgery_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuDataGridViewSurgery.SelectedRows.Count > 0)
            {
                int surgicalID = Convert.ToInt32(bunifuDataGridViewSurgery.SelectedRows[0].Cells[0].Value);
                Surgical updateSurgical = Surgical.GetSurgical(surgicalID);
                FormSurgicalDetail formSD = new FormSurgicalDetail(updateSurgical, "edit");
                formSD.ShowDialog();

                refreshDataViewSurgical();
                refreshDataViewSurgicalDetail();
            }
        }

        private void bunifuButtonSurgeryEdit_Click(object sender, EventArgs e)
        {

            if (bunifuDataGridViewSurgery.SelectedRows.Count > 0)
            {
                int surgicalID = Convert.ToInt32(bunifuDataGridViewSurgery.SelectedRows[0].Cells[0].Value);
                Surgical updateSurgical = Surgical.GetSurgical(surgicalID);
                FormSurgicalDetail formSD = new FormSurgicalDetail(updateSurgical, "edit");
                formSD.ShowDialog();

                refreshDataViewSurgical();
                refreshDataViewSurgicalDetail();
            }
        }
    }
}

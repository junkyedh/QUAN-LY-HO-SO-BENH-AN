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
    public partial class FormMainMonitor : UserControl
    {
        public FormMainMonitor()
        {
            InitializeComponent();
        }
        public void tabItemMonitor_Click()
        {
            refreshDataViewHeathNote();
        }
        //Refresh datagridview in monitor tab
        private void refreshDataViewHeathNote()
        {
            try
            {
                // Get heath note's datatable
                DataTable heathNoteTable = HeathMonitoringNote.GetListHN();

                // Add Vietnamese column's name
                heathNoteTable.Columns.Add("Mã phiếu theo dõi", typeof(string), "[HNID]");
/*                heathNoteTable.Columns.Add("Mã bệnh nhân", typeof(string), "[PATIENTID]");*/
                heathNoteTable.Columns.Add("Tên bệnh nhân", typeof(string), "[PATIENT NAME]");
/*                heathNoteTable.Columns.Add("Mã nhân viên", typeof(string), "[STAFFID]");*/
                heathNoteTable.Columns.Add("Tên nhân viên", typeof(string), "[STAFF NAME]");
                heathNoteTable.Columns.Add("Ngày lập", typeof(DateTime), "[DATE]");
                heathNoteTable.Columns.Add("Cân nặng", typeof(string), "[WEIGHT]");
                heathNoteTable.Columns.Add("Huyết áp", typeof(string), "[BLOODPRESSURE]");
                heathNoteTable.Columns.Add("Tình trạng bệnh nhân", typeof(string), "[PATIENTSTATE]");
                // Set data source to dataview for searching
                bunifuDataGridViewHealthNote.DataSource = heathNoteTable.DefaultView;

                // Hide English columns'name
                for (int i = 0; i < 9; i++)
                {
                    bunifuDataGridViewHealthNote.Columns[i].Visible = false;
                }
            }
            catch
            {
                bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                
            }
        }
        //Search in datagridview
        private void searchHeathNote()
        {
            // Not search it search string is empty
            if (bunifuTextBoxHealthNoteSearch.Text != "")
            {
                // Search with RowFilter
                ((DataView)bunifuDataGridViewHealthNote.DataSource).RowFilter = "[Mã bệnh nhân] LIKE '*" + bunifuTextBoxHealthNoteSearch.Text.Trim() + "*'"
                                                                + "OR [Mã phiếu theo dõi] LIKE '*" + bunifuTextBoxHealthNoteSearch.Text.Trim() + "*'";
            }
            else
            {
                ((DataView)bunifuDataGridViewHealthNote.DataSource).RowFilter = "";
            }
        }

        private void bunifuTextBoxHNSearch_TextChange(object sender, EventArgs e)
        {
            searchHeathNote();
        }

        private void bunifuTextBoxHNSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchHeathNote();
            }
        }

        private void bunifubuttonHFDeleteSearch_Click(object sender, EventArgs e)
        {
            bunifuTextBoxHealthNoteSearch.Text = "";
            searchHeathNote();
        }

        private void bunifuButtonHNDelete_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridViewHealthNote.SelectedRows.Count > 0)
            {
                int heathNoteID = Convert.ToInt32(bunifuDataGridViewHealthNote.SelectedRows[0].Cells[0].Value);
                DialogResult dialogResult = MessageBox.Show("Xác nhận xóa phiếu theo dõi", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        if (HeathMonitoringNote.DeleteHN(heathNoteID) > 0)
                        {
                            bunifuSnackbar1.Show(Form.ActiveForm, "Xóa phiếu theo dõi thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Success, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                            
                        }
                    }
                    catch
                    {
                        bunifuSnackbar1.Show(Form.ActiveForm, "Không xóa phiếu theo dõi này", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                        

                    }
                }

                refreshDataViewHeathNote();
            }
        }

        private void bunifuButtonHNEdit_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridViewHealthNote.SelectedRows.Count > 0)
            {
                int heathNoteID = Convert.ToInt32(bunifuDataGridViewHealthNote.SelectedRows[0].Cells[0].Value);
                FormHNDetail formHNDetail = new FormHNDetail(HeathMonitoringNote.GetHN(heathNoteID), "edit");
                formHNDetail.ShowDialog();

                refreshDataViewHeathNote();
            }
        }

        private void bunifuDataGridViewHN_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuDataGridViewHealthNote.SelectedRows.Count > 0)
            {
                int heathNoteID = Convert.ToInt32(bunifuDataGridViewHealthNote.SelectedRows[0].Cells[0].Value);
                FormHNDetail formHNDetail = new FormHNDetail(HeathMonitoringNote.GetHN(heathNoteID), "edit");
                formHNDetail.ShowDialog();

                refreshDataViewHeathNote();
            }
        }

    }
}

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
    public partial class FormMainTest : UserControl
    {
        public FormMainTest()
        {
            InitializeComponent();
        }
        public void tabItemTest_Click()
        {
            refreshDataViewTest();
            refreshDataViewTestDetail();
        }
        private void refreshDataViewTest()
        {
            try
            {
                // Get test's datatable
                DataTable testTable = TestCertificate.GetListTC();

                // Add Vietnamese column's name
                testTable.Columns.Add("Mã phiếu xét nghiệm", typeof(string), "[TCID]");
                /*testTable.Columns.Add("Mã nhân viên", typeof(string), "[STAFFID]");*/
                testTable.Columns.Add("Tên nhân viên", typeof(string), "[STAFF NAME]");
/*                testTable.Columns.Add("Mã bệnh nhân", typeof(string), "[PATIENTID]");
*/                testTable.Columns.Add("Tên bệnh nhân", typeof(string), "[STAFFID]");
                testTable.Columns.Add("Ngày lập", typeof(DateTime), "[DATE]");
                testTable.Columns.Add("Trạng thái", typeof(string), "IIF([STATE] = 0, 'Chưa xét nghiệm', 'Đã xét nghiệm')");

                // Set data source to dataview for searching
                bunifuDataGridViewTC.DataSource = testTable.DefaultView;

                // Hide English columns'name
                for (int i = 0; i < 7; i++)
                {
                    bunifuDataGridViewTC.Columns[i].Visible = false;
                }
            }
            catch
            {
                bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                
            }
        }
        private void refreshDataViewTestDetail()
        {
            if (bunifuDataGridViewTC.SelectedRows.Count > 0)
            {
                try
                {
                    // Get Test's datatable
                    int testID = Convert.ToInt32(bunifuDataGridViewTC.Rows[0].Cells[0].Value);
                    DataTable testDetailTable = TestDetail.GetListTestDetail(testID);
                    // Set data source to dataview for searching
                    bunifuDataGridViewTCDetail.DataSource = testDetailTable.DefaultView;
                }
                catch
                {
                    bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                    
                }
            }
        }
        private void searchTest()
        {
            // Not search it search string is empty
            if (bunifuTextBoxTestSearch.Text != "")
            {
                // Search with RowFilter
                ((DataView)bunifuDataGridViewTC.DataSource).RowFilter = "[Mã phiếu xét nghiệm] LIKE '*" + bunifuTextBoxTestSearch.Text.Trim() + "*'"
                                                                + "OR [Mã bệnh nhân] LIKE '*" + bunifuTextBoxTestSearch.Text.Trim() + "*'"
                                                                 + "OR [Mã nhân viên] LIKE '*" + bunifuTextBoxTestSearch.Text.Trim() + "*'"
                                                                  + "OR [Trạng thái] LIKE '*" + bunifuTextBoxTestSearch.Text.Trim() + "*'";
                refreshDataViewTestDetail();
            }
            else
            {
                ((DataView)bunifuDataGridViewTC.DataSource).RowFilter = "";
            }
        }

        private void bunifuTextBoxTestSearch_TextChange(object sender, EventArgs e)
        {
            searchTest();
        }

        private void bunifuTextBoxTestSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                searchTest();
            }
        }

        private void bunifubuttonTestSearch_Click(object sender, EventArgs e)
        {
            bunifuTextBoxTestSearch.Text = "";
            searchTest();
        }

        private void bunifuDataGridViewTC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuDataGridViewTC.SelectedRows.Count > 0)
            {
                int testID = Convert.ToInt32(bunifuDataGridViewTC.SelectedRows[0].Cells[0].Value);
                DataTable dtTD = TestDetail.GetListTestDetail(testID);

                bunifuDataGridViewTCDetail.DataSource = dtTD.DefaultView;
            }
        }

        private void bunifuButtonTestDelete_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridViewTC.SelectedRows.Count > 0)
            {
                int testID = Convert.ToInt32(bunifuDataGridViewTC.SelectedRows[0].Cells[0].Value);
                DialogResult dialogResult = MessageBox.Show("Xác nhận xóa phiếu xét nghiệm", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        if (TestDetail.DeleteTestDetail(testID) > 0 && TestCertificate.DeleteTC(testID) > 0)
                        {
                            bunifuSnackbar1.Show(Form.ActiveForm, "Xóa phiếu xét nghiệm thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                            
                        }
                    }
                    catch
                    {
                        bunifuSnackbar1.Show(Form.ActiveForm, "Không thể xóa phiếu xét nghiệm", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                        
                    }
                }
                refreshDataViewTest();
                refreshDataViewTestDetail();
            }
        }

        private void bunifuDataGridViewTC_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuDataGridViewTC.SelectedRows.Count > 0)
            {
                int testID = Convert.ToInt32(bunifuDataGridViewTC.SelectedRows[0].Cells[0].Value);
                TestCertificate updateTD = TestCertificate.GetTC(testID);
                FormTestDetail formTD = new FormTestDetail(updateTD, "edit");
                formTD.ShowDialog();

                refreshDataViewTest();
                refreshDataViewTestDetail();
            }
        }

        private void bunifuButtonTestEdit_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridViewTC.SelectedRows.Count > 0)
            {
                int testID = Convert.ToInt32(bunifuDataGridViewTC.SelectedRows[0].Cells[0].Value);
                TestCertificate updateTD = TestCertificate.GetTC(testID);
                FormTestDetail formTD = new FormTestDetail(updateTD, "edit");
                formTD.ShowDialog();

                refreshDataViewTest();
                refreshDataViewTestDetail();
            }
        }
    }
}

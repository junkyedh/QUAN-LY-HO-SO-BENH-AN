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
    public partial class FormMainService : UserControl
    {
        public FormMainService()
        {
            InitializeComponent();
        }
        public void tabItemService_Click()
        {
            refreshDataViewService();
        }
        private void refreshDataViewService()
        {
            try
            {
                // Get service's datatable
                DataTable serviceTable = Service.GetListService();

                // Add Vietnamese column's name
                serviceTable.Columns.Add("Mã dịch vụ", typeof(string), "[SERVICEID]");
                serviceTable.Columns.Add("Tên dịch vụ", typeof(string), "[SERVICENAME]");
                serviceTable.Columns.Add("Đơn giá", typeof(string), "[PRICE]");
                // Set data source to dataview for searching
                bunifuDataGridViewService.DataSource = serviceTable.DefaultView;

                // Hide English columns'name
                for (int i = 0; i < 3; i++)
                {
                    bunifuDataGridViewService.Columns[i].Visible = false;
                }

                //Add auto complete datasource to textbox
                bunifuTextBoxServiceSearch.AutoCompleteCustomSource.Clear();
                for (int i = 0; i < serviceTable.Rows.Count; i++)
                {
                    String strserviceName = serviceTable.Rows[i][1].ToString();
                    bunifuTextBoxServiceSearch.AutoCompleteCustomSource.Add(strserviceName);
                }
            }
            catch
            {
                bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                
            }
        }
        private void searchService()
        {
            // Not search it search string is empty
            if (bunifuTextBoxServiceSearch.Text != "")
            {
                // Search with RowFilter
                ((DataView)bunifuDataGridViewService.DataSource).RowFilter = "[Mã dịch vụ] LIKE '*" + bunifuTextBoxServiceSearch.Text.Trim() + "*'"
                                                                + "OR [Tên dịch vụ] LIKE '*" + bunifuTextBoxServiceSearch.Text.Trim() + "*'";
            }
            else
            {
                ((DataView)bunifuDataGridViewService.DataSource).RowFilter = "";
            }
        }

        private void bunifuTextBoxServiceSearch_TextChange(object sender, EventArgs e)
        {
            searchService();
        }

        private void bunifubuttonServiceSearch_Click(object sender, EventArgs e)
        {
            bunifuTextBoxServiceSearch.Text = "";
            searchService();
        }

        private void bunifuTextBoxServiceSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchService();
            }
        }

        private void bunifuButtonServiceDelete_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridViewService.SelectedRows.Count > 0)
            {
                int serviceID = Convert.ToInt16(bunifuDataGridViewService.SelectedRows[0].Cells[0].Value);
                DialogResult dialogResult = MessageBox.Show("Xác nhận xóa dịch vụ", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        if (Service.DeleteService(serviceID) > 0)
                        {
                            bunifuSnackbar1.Show(Form.ActiveForm, "Xóa dịch thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                            
                        }
                    }
                    catch
                    {
                        bunifuSnackbar1.Show(Form.ActiveForm, "Dịch vụ đã hoặc đang được sử dụng", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                        

                    }
                }

                refreshDataViewService();
            }

        }

        private void bunifuButtonServiceEdit_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridViewService.SelectedRows.Count > 0)
            {
                int serviceID = Convert.ToInt16(bunifuDataGridViewService.SelectedRows[0].Cells[0].Value);
                FormServiceDetail formServiceDetail = new FormServiceDetail(Service.GetService(serviceID), "edit");
                formServiceDetail.ShowDialog();

                refreshDataViewService();
            }
        }

        private void bunifuDataGridViewService_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuDataGridViewService.SelectedRows.Count > 0)
            {
                int serviceID = Convert.ToInt16(bunifuDataGridViewService.SelectedRows[0].Cells[0].Value);
                FormServiceDetail formServiceDetail = new FormServiceDetail(Service.GetService(serviceID), "edit");
                formServiceDetail.ShowDialog();

                refreshDataViewService();
            }
        }

        private void bunifuButtonServiceAdd_Click(object sender, EventArgs e)
        {
            int serviceID = Convert.ToInt16(bunifuDataGridViewService.SelectedRows[0].Cells[0].Value);
            FormServiceDetail formServiceDetail = new FormServiceDetail();
            formServiceDetail.ShowDialog();

            refreshDataViewService();
        }
    }
}

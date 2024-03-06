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
    public partial class FormMainMedicine : UserControl
    {
        public FormMainMedicine()
        {
            InitializeComponent();
        }
        public void tabItemMedicine_Click()
        {
            refreshDataViewMedicine();
        }
        private void bunifuButtonMedicineDelete_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridViewMedicine.SelectedRows.Count > 0)
            {
                int medicineID = Convert.ToInt32(bunifuDataGridViewMedicine.SelectedRows[0].Cells[0].Value);
                DialogResult dialogResult = MessageBox.Show("Xác nhận xóa thuốc", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        if (Medicine.DeleteMedicne(medicineID) > 0)
                        {
                            bunifuSnackbar1.Show(Form.ActiveForm, "Xóa thuốc thành công", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Information, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                            
                        }
                    }
                    catch
                    {
                        bunifuSnackbar1.Show(Form.ActiveForm, "Thuốc đã hoặc đang được sử dụng", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                        
                    }
                }

                refreshDataViewMedicine();
            }
        }

        private void bunifuDataGridViewMedicine_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bunifuDataGridViewMedicine.SelectedRows.Count > 0)
            {
                int medicineID = Convert.ToInt32(bunifuDataGridViewMedicine.SelectedRows[0].Cells[0].Value);
                FormMedicineDetail formMedicineDetail = new FormMedicineDetail(Medicine.GetMedicine(medicineID), "edit");
                formMedicineDetail.ShowDialog();

                refreshDataViewMedicine();
            }
        }

        private void bunifuButtonMedicineEdit_Click(object sender, EventArgs e)
        {
            if (bunifuDataGridViewMedicine.SelectedRows.Count > 0)
            {
                int medicineID = Convert.ToInt32(bunifuDataGridViewMedicine.SelectedRows[0].Cells[0].Value);
                FormMedicineDetail formMedicineDetail = new FormMedicineDetail(Medicine.GetMedicine(medicineID), "edit");
                formMedicineDetail.ShowDialog();

                refreshDataViewMedicine();
            }
        }

        private void bunifuButtonMedicineAdd_Click(object sender, EventArgs e)
        {
            FormMedicineDetail formMedicineDetail = new FormMedicineDetail();
            formMedicineDetail.ShowDialog();

            refreshDataViewMedicine();
        }

        private void bunifuTextBoxMedicineSearch_TextChange(object sender, EventArgs e)
        {
            searchMedicine();
        }

        private void bunifubuttonMedicineDeleteSearch_Click(object sender, EventArgs e)
        {
            bunifuTextBoxMedicineSearch.Text = "";
            searchMedicine();
        }

        private void bunifuTextBoxMedicineSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchMedicine();
            }
        }
        //Refresh datagridview in medicine tab
        private void refreshDataViewMedicine()
        {
            try
            {
                // Get medicine's datatable
                DataTable medicineTable = Medicine.GetListMedicine();

                // Add Vietnamese column's name
                medicineTable.Columns.Add("Mã thuốc", typeof(string), "[MEDICINEID]");
                medicineTable.Columns.Add("Tên thuốc", typeof(string), "[MEDICINENAME]");
                medicineTable.Columns.Add("Số lượng", typeof(string), "[QUANTITY]");
                medicineTable.Columns.Add("Đơn giá", typeof(string), "[PRICE]");
                // Set data source to dataview for searching
                bunifuDataGridViewMedicine.DataSource = medicineTable.DefaultView;

                // Hide English columns'name
                for (int i = 0; i < 4; i++)
                {
                    bunifuDataGridViewMedicine.Columns[i].Visible = false;
                }

                //Add auto complete datasource to textbox
                bunifuTextBoxMedicineSearch.AutoCompleteCustomSource.Clear();
                for (int i = 0; i < medicineTable.Rows.Count; i++)
                {
                    String strMaterialName = medicineTable.Rows[i][1].ToString();
                    bunifuTextBoxMedicineSearch.AutoCompleteCustomSource.Add(strMaterialName);
                }
            }
            catch
            {
                bunifuSnackbar1.Show(Form.ActiveForm, "Lỗi dữ liệu", Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Error, 3000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);
                
            }
        }
        //Search in datagridview
        private void searchMedicine()
        {
            // Not search it search string is empty
            if (bunifuTextBoxMedicineSearch.Text != "")
            {
                // Search with RowFilter
                ((DataView)bunifuDataGridViewMedicine.DataSource).RowFilter = "[Mã thuốc] LIKE '*" + bunifuTextBoxMedicineSearch.Text.Trim() + "*'"
                                                                + "OR [Tên thuốc] LIKE '*" + bunifuTextBoxMedicineSearch.Text.Trim() + "*'";
            }
            else
            {
                ((DataView)bunifuDataGridViewMedicine.DataSource).RowFilter = "";
            }
        }
    }
}

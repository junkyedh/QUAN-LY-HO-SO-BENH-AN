using DO_AN_CUA_HAN.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DO_AN_CUA_HAN.View
{
    public partial class FormPayment : Form
    {
        public Patient patient { get; set; }
        public int HICID { get; set; }
        public FormPayment()
        {
            InitializeComponent();
        }
        private void refreshbunifuDataGridViewBill()
        {
            try
            {
                decimal totalPrice = Bill.GetPatientPriceNeedPay(patient.PatientID);

                if (HIC.CheckHIC(patient.PatientID))
                {
                    HIC newHIC = HIC.GetPatientHIC(patient.PatientID);
                    if (HIC.CheckHICExpiration(newHIC.HICID))
                    {
                        bunifuLabelHICD.Text = "Đã hết hạn";
                        this.HICID = 0;
                    }
                    else
                    {
                        bunifuLabelHICD.Text = "Còn hạn sử dụng";
                        this.HICID = newHIC.HICID;
                    }
                }
                else
                {
                    bunifuLabelHICD.Text = "Không có";
                    this.HICID = 0;
                }

                if (HICID != 0)
                {
                    totalPrice = totalPrice / 4;
                }

                bunifuLabelTotalPrice.Text = totalPrice.ToString("C", CultureInfo.CreateSpecificCulture("vi"));

                // Get Bill's datatable
                DataTable billTable = Bill.GetPatientNotPayBill(patient.PatientID);

                // Add Vietnamese column's name
                billTable.Columns.Add("Mã hóa đơn", typeof(string), "[BILLID]");
                billTable.Columns.Add("Loại hóa đơn", typeof(string), @"IIF([BILLTYPEID] = 100, 'Thuốc',
                                                                            IIF([BILLTYPEID] = 101, 'Dịch vụ', 'Đồ dùng'))");
                billTable.Columns.Add("Ngày lập", typeof(DateTime), "[DATE]");
                billTable.Columns.Add("Tổng tiền", typeof(decimal), "[TOTALPRICE]");
                billTable.Columns.Add("Trạng thái", typeof(string), "IIF([STATE] = 0, 'Chưa thanh toán', 'Đã thanh toán')");
                billTable.Columns.Add("Mã nhân viên", typeof(string), "[STAFFID]");

                // Set data source to dataview for searching
                bunifuDataGridViewBill.DataSource = billTable.DefaultView;

                //Hide English columns'name
                for (int i = 0; i < 7; i++)
                {
                    bunifuDataGridViewBill.Columns[i].Visible = false;
                }
            }
            catch (SqlException exception)
            {
                bunifuSnackbar1.Show(this, exception.Message, Bunifu.UI.WinForms.BunifuSnackbar.MessageTypes.Warning, 1000, null, Bunifu.UI.WinForms.BunifuSnackbar.Positions.TopCenter);

            }
        }

        private void bunifuButtonEdit_Click(object sender, EventArgs e)
        {
            // Get bill for edit
            Bill billDetail = Bill.GetBill(Convert.ToInt32(bunifuDataGridViewBill.SelectedRows[0].Cells[0].Value.ToString()));

            // Open billdetail form for edit
            FormBillDetail billDetailForm = new FormBillDetail("edit", billDetail);
            billDetailForm.ShowDialog();

            // Refresh datagridview after edit
            refreshbunifuDataGridViewBill();
        }

        private void FormPayment_Load(object sender, EventArgs e)
        {
            bunifuTextBoxPatientID.Text = patient.PatientID.ToString();
            bunifuTextBoxPatientName.Text = patient.LastName + " " + patient.FirstName;

            refreshbunifuDataGridViewBill();
        }

        private void bunifuButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

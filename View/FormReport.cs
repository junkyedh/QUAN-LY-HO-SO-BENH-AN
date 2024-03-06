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
    public partial class FormReport : Form
    {
        private DO_AN_CUA_HAN.Report reportDataset;
        private Microsoft.Reporting.WinForms.ReportDataSource reportDataSource;
        public string ReportType { get; set; }
        public int ObjectID { get; set; }
        public DateTime DATEFROM { get; set; }
        public DateTime DATETO { get; set; }
        public FormReport()
        {
            InitializeComponent();
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            // Tao file report moi
            this.reportDataset = new Report();
            this.reportDataSource = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
            switch(ReportType)
            {
                case "EC":
                    ReportTableAdapters.ECTableAdapter ECTableAdapter = new ReportTableAdapters.ECTableAdapter();
                    ECTableAdapter.Fill(reportDataset.EC);
                    this.reportDataSource.Name = "EC";
                    this.reportDataSource.Value = this.reportDataset.EC;

                    this.reportViewer1.LocalReport.ReportEmbeddedResource = "DO_AN_CUA_HAN.Report.EC.rdlc";
                    break;
                case "MEDICINEBILL":
                    ReportTableAdapters.MEDICINEBILLTableAdapter MEDICINEBILLTableAdapter = new ReportTableAdapters.MEDICINEBILLTableAdapter();
                    MEDICINEBILLTableAdapter.Fill(reportDataset.MEDICINEBILL);

                    this.reportDataSource.Name = "MEDICINEBILL";
                    this.reportDataSource.Value = this.reportDataset.MEDICINEBILL;

                    this.reportViewer1.LocalReport.ReportEmbeddedResource = "DO_AN_CUA_HAN.Report.EC.rdlc";

                    break;
            }  
            this.reportViewer1.LocalReport.DataSources.Add(this.reportDataSource);
            this.reportViewer1.RefreshReport();
            
            
            this.reportViewer1.RefreshReport();
        }
    }
}

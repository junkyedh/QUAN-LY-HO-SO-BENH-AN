using Bunifu.Charts.WinForms.ChartTypes;
using DO_AN_CUA_HAN.Functional;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace DO_AN_CUA_HAN.Chart
{
    public partial class FormLineChart : Form
    {
        public FormLineChart()
        {
            InitializeComponent();
            renderLineChart();
        }
        public DataTable DataSourceQuery()
        {
            DataTable dtbBarchar = new DataTable();
            string sqlSelect = @"WITH TopMedicines AS (
    SELECT 
        MONTH(b.DATE) AS [Month],
        m.MEDICINENAME,
        SUM(mbd.QUANTITY) AS TotalQuantity,
        ROW_NUMBER() OVER (PARTITION BY MONTH(b.DATE) ORDER BY SUM(mbd.QUANTITY) DESC) AS RowNum
    FROM BILL b
    INNER JOIN MEDICINEBILLDETAIL mbd ON b.BILLID = mbd.BILLID
    INNER JOIN MEDICINE m ON mbd.MEDICINEID = m.MEDICINEID
    INNER JOIN BILLTYPE bt ON b.BILLTYPEID = bt.BILLTYPEID
    WHERE bt.TYPENAME = 'Medicine'
    GROUP BY MONTH(b.DATE), m.MEDICINENAME
)
SELECT
    [Month],
    MEDICINENAME,
    TotalQuantity
FROM TopMedicines
WHERE RowNum <= 5
ORDER BY [Month], TotalQuantity DESC;
";
            dtbBarchar = SqlResult.ExecuteQuery(sqlSelect);
            //dtDisease.Columns[0].ColumnName = "Mã bệnh";
            //dtDisease.Columns[1].ColumnName = "Tên bệnh";
            //dtDisease.Columns[2].ColumnName = "Triệu chứng";
            return dtbBarchar;
        }

        void renderLineChart()
        {
            /*
             * For this example we will use random numbers
             */
            var r = new Random();
            DataTable dtb = DataSourceQuery();
            string luachon = bunifuDropdown1.Text;

            /*
             * Add your data from your source - accepts double list
             * Below is an example from a random number
             */
            Queue<string> danhSach = new Queue<string>();

            List<double> data = new List<double>();
            for (int i = 0; i < dtb.Rows.Count; i++)
            {
                if (dtb.Rows[i][0].ToString() == luachon)
                {
                    double value = Convert.ToDouble(dtb.Rows[i][2]);
                    data.Add(value);
                    danhSach.Enqueue(dtb.Rows[i][1].ToString());
                }
            }
            bunifuChartCanvas1.Labels = danhSach.ToArray();
          
            /*
             * Set your data             
             */
            bunifuLineChart1.Data = data;
            var color = System.Drawing.Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
            bunifuLineChart1.BackgroundColor = color;
            bunifuLineChart1.BorderColor = color;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            renderLineChart();
        }
    }
}

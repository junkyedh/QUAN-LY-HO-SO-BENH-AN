using Bunifu.Charts.WinForms.ChartTypes;
using DO_AN_CUA_HAN.Functional;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DO_AN_CUA_HAN.Chart
{
    public partial class FormBubbleChart : Form
    {
        public FormBubbleChart()
        {
            InitializeComponent();

        }
        void renderDoughtnut()
        {
            /*
             * For this example we will use random numbers
             */
            var r = new Random();

            /*
             * Add your data from your source - accepts double list
             * Below is an example from a random number
             */
            List<double> data = new List<double>();
            DataTable dtb = DataSourceQuery();
            string luachon = bunifuDropdown1.Text.ToString();
            for (int i = 0; i < dtb.Rows.Count; i++)
            {
                if (dtb.Rows[i][0].ToString() == luachon.ToString())
                {
                    double value = Convert.ToDouble(dtb.Rows[i][2]);
                    data.Add(value);
                }
            }
            /*
             * Set your data             
             */
            bunifuDoughnutChart1.Data = data;
            bunifuChartCanvas1.Labels = new string[] { "Mùng", "Chăn", "Gối", "Gối ngắn", "Gối dài" };
            List<Color> bgColors = new List<Color>();
            for (int i = 0; i < data.Count; i++)
            {
                bgColors.Add(Color.FromArgb(r.Next(256), r.Next(256), r.Next(256)));
            }
            bunifuDoughnutChart1.BackgroundColor = bgColors;

            /*
             * Add labels to your canvas
             * Label count should correspond to data count for charts like Bar charts
             */
            data.Clear();
            bgColors.Clear();
        }
        public DataTable DataSourceQuery()
        {
            DataTable dtbBarchar = new DataTable();
            string sqlSelect = @"WITH TopMaterials AS (
    SELECT 
        MONTH(b.DATE) AS [Month],
        m.MATERIALNAME,
        SUM(r.QUANTITY) AS TotalQuantity,
        ROW_NUMBER() OVER (PARTITION BY MONTH(b.DATE) ORDER BY SUM(r.QUANTITY) DESC) AS RowNum
    FROM BILL b
    INNER JOIN RENTMATERIALBILLDETAIL r ON b.BILLID = r.BILLID
    INNER JOIN MATERIAL m ON r.MATERIALID = m.MATERIALID
    INNER JOIN BILLTYPE bt ON b.BILLTYPEID = bt.BILLTYPEID
    WHERE bt.TYPENAME = 'Material'
    GROUP BY MONTH(b.DATE), m.MATERIALNAME
)
SELECT
    [Month],
    MATERIALNAME,
    TotalQuantity,
    CAST((TotalQuantity * 100.0) / SUM(TotalQuantity) OVER (PARTITION BY [Month]) AS DECIMAL(5,2)) AS Percentage
FROM TopMaterials
WHERE RowNum <= 5
ORDER BY [Month], TotalQuantity DESC;";
            dtbBarchar = SqlResult.ExecuteQuery(sqlSelect);
            //dtDisease.Columns[0].ColumnName = "Mã bệnh";
            //dtDisease.Columns[1].ColumnName = "Tên bệnh";
            //dtDisease.Columns[2].ColumnName = "Triệu chứng";
            return dtbBarchar;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            renderDoughtnut();
        }
    }
}

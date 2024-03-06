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
    public partial class FormHorizontal : Form
    {
        public FormHorizontal()
        {
            InitializeComponent();
            renderHorizontalChart();
        }
        public DataTable DataSourceQuery()
        {
            DataTable dtbBarchar = new DataTable();
            string sqlSelect = @"SELECT
    d.DEPARTMENTNAME,
    COUNT(*) AS TotalEmployees,
    SUM(CASE WHEN s.GENDER = 0 THEN 1 ELSE 0 END) AS TotalMale,
    SUM(CASE WHEN s.GENDER = 1 THEN 1 ELSE 0 END) AS TotalFemale
FROM DEPARTMENT d
LEFT JOIN STAFF s ON d.DEPARTMENTID = s.DEPARTMENTID
GROUP BY d.DEPARTMENTNAME;
";
            dtbBarchar = SqlResult.ExecuteQuery(sqlSelect);
            //dtDisease.Columns[0].ColumnName = "Mã bệnh";
            //dtDisease.Columns[1].ColumnName = "Tên bệnh";
            //dtDisease.Columns[2].ColumnName = "Triệu chứng";
            return dtbBarchar;
        }
        void renderHorizontalChart()
        {
            /*
             * For this example we will use random numbers
             */
            var r = new Random();

            /*
             * Add your data from your source - accepts double list
             * Below is an example from a random number
             */
            List<double> data1 = new List<double>();
            List<double> data2 = new List<double>();
            DataTable dtb = DataSourceQuery();

            for (int i = 0; i < dtb.Rows.Count ; i++)
            {
                data1.Add(Convert.ToDouble(dtb.Rows[i][2]));
                data2.Add(Convert.ToDouble(dtb.Rows[i][3]));
            }
            /*
             * Set your data             
             */
            bunifuHorizontalBarChart1.Data = data1;
            bunifuHorizontalBarChart2.Data = data2;

            /*
             * Specify the target canvas
             */
            /*
             * Add labels to your canvas
             * Label count should correspond to data count for charts like Bar charts
             */
            bunifuChartCanvas1.Labels = new string[] { "Khoa chuẩn đoán hình ảnh", "Khoa dược", "Khoa ngoại", "Khoa nội", "Khoa phẩu thuật", "Khoa sản", "Khoan kiểm soát nhiểm khuẩn", "Phòng chiến lược phát triển", "Phòng IT", "Phòng kế hoạch tổng hợp", " Phòng vật tư thiết bị y tế", "Tổ dinh dưỡng tiết chế", };

            /*
             * Beautify the chart by sepcifying the colors
             * Color count should correspond to data count
             */
            bunifuHorizontalBarChart1.Label = "Nam";
            bunifuHorizontalBarChart2.Label = "Nữ";
            // Define colors for genders
            List < Color > bgColors1 = new List<Color>();
            List < Color > bgColors2 = new List<Color>();

            // Assign colors for genders
            bgColors1.Add(Color.FromArgb(r.Next(256), r.Next(256), r.Next(256)));
            bgColors2.Add(Color.FromArgb(r.Next(256), r.Next(256), r.Next(256)));

            // Assign colors to the charts based on genders and months

            bunifuHorizontalBarChart1.BackgroundColor = bgColors1;
            bunifuHorizontalBarChart2.BackgroundColor = bgColors2;
        }

    }
}

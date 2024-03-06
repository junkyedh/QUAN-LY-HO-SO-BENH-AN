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
using static DO_AN_CUA_HAN.Report;

namespace DO_AN_CUA_HAN.Chart
{
    public partial class FormBarChart : Form
    {
        public FormBarChart()
        {
            InitializeComponent();
            renderBarchart1();
            renderBarchart2();
            renderBarchart3();
        }
        public DataTable DataSourceQuery()
        {
            DataTable dtbBarchar = new DataTable();
            string sqlSelect = @"SELECT
    SUM(CASE WHEN GENDER = 0 THEN 1 ELSE 0 END) AS Nam,
    SUM(CASE WHEN GENDER = 1 THEN 1 ELSE 0 END) AS Nu
FROM
    PATIENT;";
            dtbBarchar = SqlResult.ExecuteQuery(sqlSelect);
            //dtDisease.Columns[0].ColumnName = "Mã bệnh";
            //dtDisease.Columns[1].ColumnName = "Tên bệnh";
            //dtDisease.Columns[2].ColumnName = "Triệu chứng";
            return dtbBarchar;
        }
        public DataTable DataSourceQuery2()
        {
            DataTable dtbBarchar = new DataTable();
            string sqlSelect = @"WITH Months AS (
    SELECT 1 AS MonthNumber UNION ALL
    SELECT 2 UNION ALL
    SELECT 3 UNION ALL
    SELECT 4 UNION ALL
    SELECT 5 UNION ALL
    SELECT 6 UNION ALL
    SELECT 7 UNION ALL
    SELECT 8 UNION ALL
    SELECT 9 UNION ALL
    SELECT 10 UNION ALL
    SELECT 11 UNION ALL
    SELECT 12
)
SELECT 
    Months.MonthNumber AS Month,
    SUM(CASE WHEN GENDER = 0 THEN 1 ELSE 0 END) AS MalePatients,
    SUM(CASE WHEN GENDER = 1 THEN 1 ELSE 0 END) AS FemalePatients
FROM 
    Months
LEFT JOIN (
    SELECT 
        MONTH(HC.DATE) AS Month,
        P.GENDER
    FROM 
        PATIENT P
        JOIN HOSPITALIZATIONCERTIFICATE HC ON P.PATIENTID = HC.PATIENTID
) AS Data ON Months.MonthNumber = Data.Month
GROUP BY
    Months.MonthNumber
ORDER BY
    Months.MonthNumber";
            dtbBarchar = SqlResult.ExecuteQuery(sqlSelect);
            //dtDisease.Columns[0].ColumnName = "Mã bệnh";
            //dtDisease.Columns[1].ColumnName = "Tên bệnh";
            //dtDisease.Columns[2].ColumnName = "Triệu chứng";
            return dtbBarchar;
        }
        public DataTable DataSourceQuery3()
        {
            DataTable dtbBarchar = new DataTable();
            string sqlSelect = @"WITH Months AS (
    SELECT 1 AS MonthNumber UNION ALL
    SELECT 2 UNION ALL
    SELECT 3 UNION ALL
    SELECT 4 UNION ALL
    SELECT 5 UNION ALL
    SELECT 6 UNION ALL
    SELECT 7 UNION ALL
    SELECT 8 UNION ALL
    SELECT 9 UNION ALL
    SELECT 10 UNION ALL
    SELECT 11 UNION ALL
    SELECT 12
)
SELECT 
    Months.MonthNumber AS Month,
    SUM(CASE WHEN GENDER = 0 THEN 1 ELSE 0 END) AS MalePatients,
    SUM(CASE WHEN GENDER = 1 THEN 1 ELSE 0 END) AS FemalePatients
FROM 
    Months
LEFT JOIN (
    SELECT 
        MONTH(DC.DATE) AS Month,
        P.GENDER
    FROM 
        PATIENT P
        JOIN DISCHARGEDCERTIFICATE DC ON P.PATIENTID = DC.PATIENTID
) AS Data ON Months.MonthNumber = Data.Month
GROUP BY
    Months.MonthNumber
ORDER BY
    Months.MonthNumber;";
            dtbBarchar = SqlResult.ExecuteQuery(sqlSelect);
            //dtDisease.Columns[0].ColumnName = "Mã bệnh";
            //dtDisease.Columns[1].ColumnName = "Tên bệnh";
            //dtDisease.Columns[2].ColumnName = "Triệu chứng";
            return dtbBarchar;
        }
        void renderBarchart1()
        {
            /*
             * For this example we will use random numbers
             */
            DataTable dtb = DataSourceQuery();
            var r = new Random();

            List<double> data = new List<double>();


            for (int i = 0; i < dtb.Rows.Count; i++)
            {
                // Duyệt qua từng cột trong mỗi dòng
                for (int j = 0; j < dtb.Columns.Count; j++)
                {
                    double value = Convert.ToDouble(dtb.Rows[i][j]);

                    data.Add(value);

                }
            }
            bunifuPieChart1.Data = data;
            bunifuChartCanvas1.Labels = new string[] { "Nam", "Nữ" };
            bunifuChartCanvas1.Title = "Số lượng nam nữ bệnh nhân ";

            List<Color> bgColors = new List<Color>();

            for (int i = 0; i < data.Count; i++)
            {

                bgColors.Add(Color.FromArgb(r.Next(256), r.Next(256), r.Next(256)));
            }
            bunifuPieChart1.BackgroundColor = bgColors;

        }
        void renderBarchart2()
        {
            /*
             * For this example we will use random numbers
             */
            DataTable dtb = DataSourceQuery2();
            var r = new Random();

            List<double> data1 = new List<double>();
            List<double> data2 = new List<double>();

            for (int i = 0; i < dtb.Rows.Count; i++)
            {
                // Duyệt qua từng cột trong mỗi dòng
                for (int j = 1; j < dtb.Columns.Count; j++)
                {
                    if (j == 1)
                    {
                        double value = Convert.ToDouble(dtb.Rows[i][j]);
                        data1.Add(value);
                    }
                    else
                    {
                        double value = Convert.ToDouble(dtb.Rows[i][j]);
                        data2.Add(value);
                    }
                    // ...
                }
            }
            bunifuBarChart2.Data = data1;
            bunifuBarChart3.Data = data2;
            bunifuBarChart2.Label = "Nam";
            bunifuBarChart3.Label = "Nữ";
            bunifuChartCanvas2.Labels = new string[] { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };
            bunifuChartCanvas2.Title = "Số lượng nam nữ bệnh nhân Nhập viện từng tháng ";

            // Define colors for genders
            List<Color> bgColors1 = new List<Color>();
            List<Color> bgColors2 = new List<Color>();

            // Assign colors for genders
            bgColors1.Add(Color.FromArgb(r.Next(256), r.Next(256), r.Next(256)));
            bgColors2.Add(Color.FromArgb(r.Next(256), r.Next(256), r.Next(256)));

            // Assign colors to the charts based on genders and months

            bunifuBarChart2.BackgroundColor = bgColors1;
            bunifuBarChart3.BackgroundColor = bgColors2;

        }
        void renderBarchart3()
        {
            /*
             * For this example we will use random numbers
             */
            DataTable dtb = DataSourceQuery3();
            var r = new Random();



            List<double> data1 = new List<double>();
            List<double> data2 = new List<double>();

            for (int i = 0; i < dtb.Rows.Count; i++)
            {
                // Duyệt qua từng cột trong mỗi dòng
                for (int j = 1; j < dtb.Columns.Count; j++)
                {
                    if (j == 1)
                    {
                        double value = Convert.ToDouble(dtb.Rows[i][j]);
                        data1.Add(value);
                    }
                    else
                    {
                        double value = Convert.ToDouble(dtb.Rows[i][j]);
                        data2.Add(value);
                    }
                    // ...
                }
            }
            bunifuBarChart1.Data = data1;
            bunifuBarChart4.Data = data2;
            bunifuBarChart1.Label = "Nam";
            bunifuBarChart4.Label = "Nữ";
            bunifuChartCanvas3.Labels = new string[] { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };
            bunifuChartCanvas3.Title = "Số lượng nam nữ bệnh nhân Xuất viện từng tháng ";


            // Define colors for genders
            List < Color > bgColors1 = new List<Color>();
            List < Color > bgColors2 = new List<Color>();

            // Assign colors for genders
            bgColors1.Add(Color.FromArgb(r.Next(256), r.Next(256), r.Next(256)));
            bgColors2.Add(Color.FromArgb(r.Next(256), r.Next(256), r.Next(256)));

            // Assign colors to the charts based on genders and months
            bunifuBarChart1.BackgroundColor = bgColors1;
            bunifuBarChart4.BackgroundColor = bgColors2;

        }
    }
}

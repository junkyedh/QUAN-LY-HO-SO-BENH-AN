using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DO_AN_CUA_HAN.Excel
{
    public partial class FormMainExcel : Form
    {
        public FormMainExcel()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            //Phải có một cái Lít để hứng dữ liệu
            List<UserInfo> userlist = new List<UserInfo>();

            try
            {
                //Mở file excel
                var packet = new ExcelPackage(new FileInfo("E:/Lập trình Trực quan/neww/Danhsach.xlsx"));

                //Lay ra sheet tuong ung de do vao du lieu

                  ExcelWorksheet worksheet = packet.Workbook.Worksheets[0];

                //Duyet tuan tu cac dong cua file den cot cuoi cung 
                for(int i=worksheet.Dimension.Start.Row+1;i<=worksheet.Dimension.End.Row;i++)
                {
                    try
                    {
                        int j = 1;
                        string name = worksheet.Cells[i, j++].Value.ToString();
                        string ngaysinh = worksheet.Cells[i, j++].Value.ToString();

                        UserInfo user = new UserInfo();
                        user.Name = name;
                        user.Birthay = ngaysinh;
       

                        userlist.Add(user);
                    }
                    catch(Exception exe) {
                        MessageBox.Show(exe.Message);
                    }
                    
                }
            }catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }

            dataGridView1.DataSource = userlist;

        }

        private void MakeUser()
        {
            object[] rowData = new object[] { "Phong Nguyen", "1234" };
            object[] rowData1 = new object[] { "Minh Minh", "1234" };
            object[] rowData2 = new object[] { "Lan Lan", "1234" };

            
  
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            /*string filePath = "";
            //Tao cai file Save nay de luu file
            SaveFileDialog dialog = new SaveFileDialog();

            //Loc ra file dinh dang excel
            dialog.Filter = "Excel|*.xlsx|Excel 2013|*.xls";

            dialog.ShowDialog();
            filePath = dialog.FileName;
            

            if( string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Duong dan bao cao khong hop le");
                return;
            }
            try
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    p.Workbook.Properties.Author = "HoDangManhhung";
                    p.Workbook.Properties.Title = "Bao Cao";
                    p.Workbook.Worksheets.Add("New Sheet");


                    ExcelWorksheet ws = p.Workbook.Worksheets[1];
                    ws.Name = "ManhHungSheet";
                    ws.Cells.Style.Font.Size = 11;
                    ws.Cells.Style.Font.Name = "Calibri";


                    //Danh sach cac cot
                    String[] cot = { "Hovaten", "Namsinh" };

                    //Lay ra so luong cot can dung
                    var countcot = cot.Count();
                    ws.Cells[1, 1].Value = "Thong ke user";
                    ws.Cells[1, 1,1, countcot].Merge = true;

                    //in dam
                    ws.Cells[1, 1, 1, countcot].Style.Font.Bold = true;
                    // can giua 

                    ws.Cells[1, 1, 1, countcot].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int colindex = 1;
                    int rowindex = 2;
                    // tao cac header tu colum header da tao tu ben tren 
                 *//*   foreach (var item in cot)
                    {
                        var cell = ws.Cells[rowindex, colindex];

                        //Set mau thanh grey
                        var fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                        // can chinh border
                        var border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        // Gan gia tri

                        cell.Value = item;

                        colindex++;
                    }*//*

                    // Lay danh sach Userinfo tuw ItemSOuce cuar DataGrid

                    List<UserInfo> danhsach = new List<UserInfo>();
                    

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        UserInfo user = new UserInfo();
                        if (!row.IsNewRow)
                        {
                           

                            string name = row.Cells[1].Value.ToString();
                            string Birthday = row.Cells[2].Value.ToString();

                            user.Name = name;
                            user.Birthay = Birthday;

                        }
                        danhsach.Add(user);
                    }
                    foreach(var item  in danhsach)
                    {
                        colindex = 1;
                        rowindex++;
                        ws.Cells[rowindex, colindex++].Value = item.Name;

                        ws.Cells[rowindex, colindex++].Value = item.Birthay;
                    }

                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                }
    


                
            }catch(Exception ex)
            {
                MessageBox.Show("Co loi khi luu file");
            }
*/
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            saveFileDialog.DefaultExt = "xlsx";
            if(saveFileDialog.ShowDialog()== DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet User");
                    worksheet.Cells[1, 1].Value = "Thong ke thong tin User";
                    worksheet.Cells[1, 1, 3, 2].Merge = true;
                    worksheet.Cells[1, 1, 3, 2].Style.Font.Bold = true;
                    worksheet.Cells[1, 1, 3, 2].Style.Fill.SetBackground(Color.Red);
                    worksheet.Cells[1, 1, 3, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    int rowcount = 4;
                    foreach(DataGridViewRow row in dataGridView1.Rows)
                    {
                        for(int i=0;i<dataGridView1.Columns.Count;i++)
                        {
                            worksheet.Cells[rowcount, i + 1].Value = row.Cells[i].Value;
                        }
                        rowcount++;
                    }
                    FileInfo excelFile = new FileInfo(filePath);
                    excelPackage.SaveAs(excelFile);
                }

                MessageBox.Show("Xuat file excel thanh cong");
            }







        }

        private void btnMakeUser_Click(object sender, EventArgs e)
        {
            object[] row = new object[] { "ADCD", "1234" };
            dataGridView1.Rows.Add(row);

        }
    }
}

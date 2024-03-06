using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;
namespace DO_AN_CUA_HAN.Model
{
    public class Medicine
    {
        public int MedicineID { get; set; }
        public String MedicineName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Medicine() { }
        public Medicine(int medicineID, String medicineName, int quantity, decimal price)
        {
            this.MedicineID = medicineID;
            this.MedicineName = medicineName;
            this.Quantity = quantity;
            this.Price = price;
        }
        public static int InsertMedicine(Medicine newMedicine)
        {
            String sqlInsert = @"INSERT INTO MEDICINE(MEDICINENAME, QUANTITY, PRICE)
                                VALUES        (@MEDICINENAME,@QUANTITY,@PRICE)";
            SqlParameter[] sqlParameters = { new SqlParameter("@MEDICINENAME", newMedicine.MedicineName),
                                            new SqlParameter("@QUANTITY", newMedicine.Quantity),
                                           new SqlParameter("@PRICE",newMedicine.Price)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateMedicine(Medicine updateMedicine)
        {
            string sqlUpdate = @"UPDATE       MEDICINE
                                SET                MEDICINENAME =@MEDICINENAME, QUANTITY =@QUANTITY, PRICE =@PRICE
                                WHERE         MEDICINEID=@MEDICINEID ";
            SqlParameter[] sqlParameters = { new SqlParameter("@MEDICINEID", updateMedicine.MedicineID),
                                            new SqlParameter("@MEDICINENAME", updateMedicine.MedicineName),
                                           new SqlParameter("@QUANTITY", updateMedicine.Quantity),
                                           new SqlParameter("@PRICE", updateMedicine.Price)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteMedicne(int medicineID)
        {
            string sqlDelete = @"DELETE FROM MEDICINE
                                WHERE        (MEDICINEID = @MEDICINEID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@MEDICINEID", medicineID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListMedicine()
        {
            DataTable dtM = new DataTable();
            string sqlSelect = @"SELECT        MEDICINEID, MEDICINENAME, QUANTITY, PRICE
                                FROM            MEDICINE";
            dtM = SqlResult.ExecuteQuery(sqlSelect);
            //dtM.Columns[0].ColumnName = "Mã thuốc";
            //dtM.Columns[1].ColumnName = "Tên thuốc";
            //dtM.Columns[2].ColumnName = "Số lượng";
            //dtM.Columns[3].ColumnName = "Đơn giá";
            return dtM;
        }
        public static Medicine GetMedicine(int medicineID)
        {
            Medicine newMedicine = new Medicine();
            int tempInterger;
            string sqlSelect = @"SELECT        MEDICINEID,MEDICINENAME, QUANTITY, PRICE
                                FROM            MEDICINE
                                WHERE        MEDICINEID=@MEDICINEID";
            SqlParameter[] sqlParameters = { new SqlParameter("@MEDICINEID", medicineID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0][0].ToString(), out tempInterger);
                newMedicine.MedicineID = tempInterger;
                newMedicine.MedicineName = dataTable.Rows[0][1].ToString();
                newMedicine.Quantity = int.Parse(dataTable.Rows[0][2].ToString());
                newMedicine.Price = (decimal)dataTable.Rows[0][3];
            }
            return newMedicine;
        }
    }
}


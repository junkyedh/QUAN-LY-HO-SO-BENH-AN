using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;

namespace DO_AN_CUA_HAN.Model
{
    public class PrescriptionDetail
    {
        public int MedicineID { get; set; }
        public int PrescriptionID { get; set; }
        public int Quantity { get; set; }
        public String Instruction { get; set; }

        public PrescriptionDetail() { }

        public PrescriptionDetail(int medicineID, int prescriptionID, int quantity, String instruction)
        {
            this.MedicineID = medicineID;
            this.PrescriptionID = prescriptionID;
            this.Quantity = quantity;
            this.Instruction = instruction;
        }

        public static int InsertPrescriptionDetail(PrescriptionDetail newPD)
        {
            String sqlInsert = @"INSERT INTO PRESCRIPTIONDETAIL(PRESCRIPTIONID, MEDICINEID, QUANTITY, INSTRUCTION)
                                VALUES        (@PRESCRIPTIONID,@MEDICINEID,@QUANTITY,@INSTRUCTION)";

            SqlParameter[] sqlParameters = { new SqlParameter("@PRESCRIPTIONID", newPD.PrescriptionID),
                                            new SqlParameter("@MEDICINEID", newPD.MedicineID),
                                            new SqlParameter("@QUANTITY", newPD.Quantity),
                                           new SqlParameter("@INSTRUCTION",newPD.Instruction)};

            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }

        public Boolean UpdatePrescriptionDetail()
        {
            return true;
        }

        public static int DeletePrescriptionDetail(int pID, int medicineID)
        {
            string sqlDelete = @"DELETE FROM PRESCRIPTIONDETAIL
                                WHERE PRESCRIPTIONID=@PRESCRIPTIONID AND MEDICINEID=@MEDICINEID";

            SqlParameter[] sqlParameters = { new SqlParameter("@PRESCRIPTIONID", pID),
                                           new SqlParameter("@MEDICINEID", medicineID)};

            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }

        public static int DeletePrescriptionDetail(int pID)
        {
            string sqlDelete = @"DELETE FROM PRESCRIPTIONDETAIL
                                WHERE PRESCRIPTIONID=@PRESCRIPTIONID";

            SqlParameter[] sqlParameters = { new SqlParameter("@PRESCRIPTIONID", pID) };

            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListPrescriptionDetail(int pID)
        {
            DataTable dtPD = new DataTable();

            string sqlSelect = @"SELECT        PRESCRIPTIONID, MEDICINEID, QUANTITY, INSTRUCTION
                                FROM            PRESCRIPTIONDETAIL
                                WHERE        PRESCRIPTIONID=@PRESCRIPTIONID";

            SqlParameter[] sqlParameters = { new SqlParameter("@PRESCRIPTIONID", pID) };

            dtPD = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);

            dtPD.Columns[0].ColumnName = "Mã đơn thuốc";
            dtPD.Columns[1].ColumnName = "Mã thuốc";
            dtPD.Columns[2].ColumnName = "Số lượng";
            dtPD.Columns[3].ColumnName = "Hướng dẫn";

            return dtPD;
        }

        public static DataTable GetListPrescriptionDetailWithMedicine(int pID)
        {
            DataTable dtPD;

            string sqlSelect = @"SELECT     MEDICINE.MEDICINEID, MEDICINE.MEDICINENAME, PRESCRIPTIONDETAIL.QUANTITY, MEDICINE.PRICE * PRESCRIPTIONDETAIL.QUANTITY AS PRICE
                                FROM        PRESCRIPTIONDETAIL INNER JOIN
                                            MEDICINE ON PRESCRIPTIONDETAIL.MEDICINEID = MEDICINE.MEDICINEID
                                WHERE       (PRESCRIPTIONDETAIL.PRESCRIPTIONID = @PRESCRIPTIONID)";

            SqlParameter[] sqlParameters = { new SqlParameter("@PRESCRIPTIONID", pID) };

            dtPD = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);

            return dtPD;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;
namespace DO_AN_CUA_HAN.Model
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public int StaffID { get; set; }
        public int PatientID { get; set; }
        public DateTime Date { get; set; }

        public Prescription() { }
        public Prescription(int pID, int staffID, int patientID, DateTime date)
        {
            this.PrescriptionID = pID;
            this.PatientID = patientID;
            this.StaffID = staffID;
            this.Date = date;
        }

        public static int InsertPrescription(Prescription newP)
        {
            String sqlInsert = @"INSERT INTO PRESCRIPTION(STAFFID, PATIENTID, DATE)
                                VALUES        (@STAFFID,@PATIENTID,@DATE)";
            SqlParameter[] sqlParameters = { new SqlParameter("@STAFFID", newP.StaffID),
                                            new SqlParameter("@PATIENTID", newP.PatientID),
                                           new SqlParameter("@DATE",newP.Date)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdatePrescription(Prescription updateP)
        {
            string sqlUpdate = @"UPDATE       PRESCRIPTION
                                SET                PATIENTID =@PATIENTID, DATE =@DATE
                                WHERE         PRESCRIPTIONID=@PRESCRIPTIONID ";
            SqlParameter[] sqlParameters = { new SqlParameter("@PRESCRIPTIONID", updateP.PrescriptionID),
                                            new SqlParameter("@PATIENTID", updateP.PatientID),
                                           new SqlParameter("@DATE", updateP.Date)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeletePrescription(int pID)
        {
            string sqlDelete = @"DELETE FROM PRESCRIPTION
                                WHERE        (PRESCRIPTIONID=@PRESCRIPTIONID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@PRESCRIPTIONID", pID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListPrescription()
        {
            DataTable dtP = new DataTable();
            string sqlSelect = @"SELECT        PRESCRIPTIONID, p.STAFFID, p.PATIENTID, DATE, pa.LASTNAME +' '+pa.FIRSTNAME as 'PATIENT NAME', s.LASTNAME+' '+s.FIRSTNAME as 'STAFF NAME'
                                FROM            PRESCRIPTION p join PATIENT pa on p.PATIENTID = pa.PATIENTID join STAFF s on s.STAFFID = p.PATIENTID";
            dtP = SqlResult.ExecuteQuery(sqlSelect);
            //dtP.Columns[0].ColumnName = "Mã đơn thuốc";
            //dtP.Columns[1].ColumnName = "Mã nhân viên";
            //dtP.Columns[2].ColumnName = "Mã bệnh nhân";
            //dtP.Columns[3].ColumnName = "Ngày lập";
            //dtP.Columns[4].ColumnName = "Tên bệnh nhân";
            //dtP.Columns[5].ColumnName = "Tên nhân viên";
            return dtP;

        }
        public static Prescription GetPrescription(int pID)
        {
            Prescription newPrescription = new Prescription();
            string sqlSelect = @"SELECT        PRESCRIPTIONID, STAFFID, PATIENTID, DATE
                                FROM            PRESCRIPTION
                                WHERE        PRESCRIPTIONID=@PRESCRIPTIONID";
            SqlParameter[] sqlParameters = { new SqlParameter("@PRESCRIPTIONID", pID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                newPrescription.PrescriptionID = Convert.ToInt32(dataTable.Rows[0][0]);
                newPrescription.StaffID = Convert.ToInt32(dataTable.Rows[0][1]);
                newPrescription.PatientID = Convert.ToInt32(dataTable.Rows[0][2]);
                newPrescription.Date = DateTime.Parse(dataTable.Rows[0][3].ToString());
            }
            return newPrescription;
        }
        public static int GetPrescriptionInsertedID()
        {
            string sqlSelect = @"SELECT IDENT_CURRENT('PRESCRIPTION')  as currIdent";
            object ob = SqlResult.ExecuteScalar(sqlSelect);
            return Convert.ToInt32(ob);
        }

        public static DataTable GetPatientIDInPrescription(int pID)
        {
            DataTable dtPD;

            string sqlSelect = @"SELECT        PATIENTID
                                FROM           PRESCRIPTION
                                WHERE        PRESCRIPTIONID=@PRESCRIPTIONID";
            SqlParameter[] sqlParameters = { new SqlParameter("@PRESCRIPTIONID", pID) };
            dtPD = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);

            return dtPD;
        }

    }
}


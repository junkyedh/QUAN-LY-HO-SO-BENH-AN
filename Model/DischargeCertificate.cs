using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;

namespace DO_AN_CUA_HAN.Model
{
    public class DischargeCertificate
    {
        public int DCID { get; set; }
        public int StaffID { get; set; }
        public int PatientID { get; set; }
        public DateTime Date { get; set; }
        public int State { get; set; }


        public static int InsertDC(DischargeCertificate newDC)
        {
            String sqlInsert = @"INSERT INTO DISCHARGEDCERTIFICATE(STAFFID, PATIENTID, DATE, STATE)
                                VALUES        (@STAFFID,@PATIENTID,@DATE,@STATE)";
            SqlParameter[] sqlParameters = { new SqlParameter("@STAFFID", newDC.StaffID),
                                            new SqlParameter("@PATIENTID", newDC.PatientID),
                                            new SqlParameter("@DATE", newDC.Date),
                                           new SqlParameter("@STATE",newDC.State)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateDC(DischargeCertificate updateDC)
        {
            string sqlUpdate = @"UPDATE       DISCHARGEDCERTIFICATE
                                SET                PATIENTID =@PATIENTID, STAFFID =@STAFFID, DATE =@DATE, STATE =@STATE
                                WHERE         DCID=@DCID ";
            SqlParameter[] sqlParameters = { new SqlParameter("@DCID", updateDC.DCID ),
                                            new SqlParameter("@PATIENTID", updateDC.PatientID),
                                           new SqlParameter("@STAFFID",updateDC.StaffID),
                                           new SqlParameter("@DATE", updateDC.Date),
                                           new SqlParameter("STATE", updateDC.State)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteDC(int dCID)
        {
            string sqlDelete = @"DELETE FROM DISCHARGEDCERTIFICATE
                                WHERE        (DCID=@DCID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@DCID", dCID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListDC()
        {
            DataTable dtEC = new DataTable();
            string sqlSelect = @"SELECT        DCID, d.STAFFID, d.PATIENTID, DATE, d.STATE, p.LASTNAME+' '+p.FIRSTNAME AS 'PATIENT NAME', s.LASTNAME+' '+s.FIRSTNAME AS 'STAFF NAME'
                                FROM            DISCHARGEDCERTIFICATE d join PATIENT p on d.PATIENTID = p.PATIENTID join STAFF s on s.STAFFID = d.STAFFID";
            dtEC = SqlResult.ExecuteQuery(sqlSelect);
            //dtEC.Columns[0].ColumnName = "Mã giấy xuất viện";
            //dtEC.Columns[1].ColumnName = "Mã nhân viên";
            //dtEC.Columns[2].ColumnName = "Mã bệnh nhân";
            //dtEC.Columns[3].ColumnName = "Ngày lập";
            //dtEC.Columns[4].ColumnName = "Trạng thái";
            //dtEC.Columns[5].ColumnName = "Tên bệnh nhân";
            //dtEC.Columns[6].ColumnName = "Tên nhân viên";
            return dtEC;
        }
        public static DischargeCertificate GetDC(int dCID)
        {
            DischargeCertificate dC = new DischargeCertificate();
            string sqlSelect = @"SELECT        DCID, STAFFID, PATIENTID, DATE, STATE
                                FROM            DISCHARGEDCERTIFICATE
                                WHERE        DCID=@DCID";
            SqlParameter[] sqlParameters = { new SqlParameter("@DCID", dCID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                dC.DCID = Convert.ToInt32(dataTable.Rows[0][0]);
                dC.StaffID = Convert.ToInt32(dataTable.Rows[0][1]);
                dC.PatientID = Convert.ToInt32(dataTable.Rows[0][2]);
                dC.Date = (DateTime)dataTable.Rows[0][3];
                dC.State = (int)dataTable.Rows[0][4];
            }
            return dC;
        }
        public static Boolean IsPatientHadDC(int patientID)
        {
            DataTable dtDC = new DataTable();
            string sqlSelect = @"SELECT        DCID, STAFFID, PATIENTID, DATE, STATE
                                FROM            DISCHARGEDCERTIFICATE
                                WHERE        PATIENTID=@PATIENTID";
            SqlParameter[] sqlParameters = { new SqlParameter("@PATIENTID", patientID) };
            dtDC = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dtDC.Rows.Count > 0)
                return true;
            return false;
        }
    }
}

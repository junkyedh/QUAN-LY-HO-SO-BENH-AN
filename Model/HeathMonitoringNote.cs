using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;
namespace DO_AN_CUA_HAN.Model
{
    public class HeathMonitoringNote
    {
        public int HNID { get; set; }
        public int PatientID { get; set; }
        public int StaffID { get; set; }
        public DateTime Date { get; set; }
        public String Weight { get; set; }
        public String BloodPressure { get; set; }
        public String PatientState { get; set; }

        public static int InsertHN(HeathMonitoringNote newHN)
        {
            String sqlInsert = @"INSERT INTO HEATHMONITORINGNOTE(PATIENTID, STAFFID, DATE, WEIGHT, BLOODPRESSURE, PATIENTSTATE)
                                VALUES        (@PATIENTID,@STAFFID,@DATE,@WEIGHT,@BLOODPRESSURE,@PATIENTSTATE)";
            SqlParameter[] sqlParameters = { new SqlParameter("@PATIENTID", newHN.PatientID),
                                            new SqlParameter("@STAFFID", newHN.StaffID),
                                            new SqlParameter("@DATE", newHN.Date),
                                            new SqlParameter("@WEIGHT", newHN.Weight),
                                            new SqlParameter("@BLOODPRESSURE", newHN.BloodPressure),
                                           new SqlParameter("@PATIENTSTATE",newHN.PatientState)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateHN(HeathMonitoringNote updateHN)
        {
            string sqlUpdate = @"UPDATE       HEATHMONITORINGNOTE
                                SET                PATIENTID =@PATIENTID, DATE =@DATE, WEIGHT =@WEIGHT, BLOODPRESSURE =@BLOODPRESSURE, PATIENTSTATE =@PATIENTSTATE
                                WHERE         HNID=@HNID ";
            SqlParameter[] sqlParameters = { new SqlParameter("@HNID", updateHN.HNID),
                                            new SqlParameter("@PATIENTID", updateHN.PatientID),
                                           new SqlParameter("@DATE", updateHN.Date),
                                           new SqlParameter("@WEIGHT", updateHN.Weight),
                                           new SqlParameter("@BLOODPRESSURE", updateHN.BloodPressure),
                                           new SqlParameter("@PATIENTSTATE", updateHN.PatientState)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteHN(int hNID)
        {
            string sqlDelete = @"DELETE FROM HEATHMONITORINGNOTE
                                WHERE        (HNID=@HNID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@HNID", hNID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListHN()
        {
            DataTable dtHN = new DataTable();
            string sqlSelect = @"SELECT        HNID, h.PATIENTID, h.STAFFID, DATE, WEIGHT, BLOODPRESSURE, PATIENTSTATE, p.LASTNAME+' '+p.FIRSTNAME AS 'PATIENT NAME',s.LASTNAME+' '+s.FIRSTNAME AS 'STAFF NAME'
                                FROM            HEATHMONITORINGNOTE h join PATIENT p on h.PATIENTID = p.PATIENTID join STAFF s on s.STAFFID = p.PATIENTID
                                ";

            dtHN = SqlResult.ExecuteQuery(sqlSelect);
            //dtHN.Columns[0].ColumnName = "Mã phiếu theo dõi sức khỏe";
            //dtHN.Columns[1].ColumnName = "Mã bệnh nhân";
            //dtHN.Columns[2].ColumnName = "Mã nhân viên";
            //dtHN.Columns[3].ColumnName = "Ngày lập";
            //dtHN.Columns[4].ColumnName = "Cân nặng";
            //dtHN.Columns[5].ColumnName = "Huyết áp";
            //dtHN.Columns[6].ColumnName = "Tình trạng bệnh nhân";
            //dtHN.Columns[7].ColumnName = "Tên bệnh nhân";
            //dtHN.Columns[8].ColumnName = "Tên nhân viên";
            return dtHN;
        }


        // Ở đây chúng ta có 2 cách thức để lấy bảng theo dõi bệnh án -> sẽ dựa vào mã bệnh nhân vì mỗi bệnh nhân sẽ chỉ có 1 bảng theo dõi.
        public static DataTable GetListHN(int patientID)
        {
            DataTable dtHN = new DataTable();
            string sqlSelect = @"SELECT        HNID, PATIENTID, STAFFID, DATE, WEIGHT, BLOODPRESSURE, PATIENTSTATE
                                FROM            HEATHMONITORINGNOTE
                                WHERE        PATIENTID=@PATIENTID";
            SqlParameter[] sqlParameters = { new SqlParameter("@PATIENTID", patientID) };
            dtHN = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            dtHN.Columns[0].ColumnName = "Mã phiếu theo dõi sức khỏe";
            dtHN.Columns[1].ColumnName = "Mã bệnh nhân";
            dtHN.Columns[2].ColumnName = "Mã nhân viên";
            dtHN.Columns[3].ColumnName = "Ngày lập";
            dtHN.Columns[4].ColumnName = "Cân nặng";
            dtHN.Columns[5].ColumnName = "Huyết áp";
            dtHN.Columns[6].ColumnName = "Tình trạng bệnh nhân";
            return dtHN;
        }


        // Lấy danh sách mã theo dõi bệnh dựa vào mã theo dõi bệnh
        public static HeathMonitoringNote GetHN(int hNID)
        {
            HeathMonitoringNote hN = new HeathMonitoringNote();
            string sqlSelect = @"SELECT        HNID, PATIENTID, STAFFID, DATE, WEIGHT, BLOODPRESSURE, PATIENTSTATE
                                FROM            HEATHMONITORINGNOTE
                                WHERE        HNID=@HNID";
            SqlParameter[] sqlParameters = { new SqlParameter("@HNID", hNID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                hN.HNID = Convert.ToInt32(dataTable.Rows[0][0]);
                hN.PatientID = Convert.ToInt32(dataTable.Rows[0][1]);
                hN.StaffID = Convert.ToInt32(dataTable.Rows[0][2]);
                hN.Date = (DateTime)dataTable.Rows[0][3];
                hN.Weight = (String)dataTable.Rows[0][4];
                hN.BloodPressure = (String)dataTable.Rows[0][5];
                hN.PatientState = (String)dataTable.Rows[0][6];
            }
            return hN;
        }
    }
}


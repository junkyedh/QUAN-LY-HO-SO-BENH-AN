using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;

namespace DO_AN_CUA_HAN.Model
{
    public class TestCertificate
    {
        public int TCID { get; set; }
        public int PatientID { get; set; }
        public int StafID { get; set; }
        public DateTime Date { get; set; }
        public int State { get; set; }

        public TestCertificate() { }
        public TestCertificate(int tCID, int patientID, int staffID, DateTime date, int state)
        {
            this.TCID = tCID;
            this.PatientID = patientID;
            this.StafID = staffID;
            this.Date = date;
            this.State = state;
        }
        public static int InsertTC(TestCertificate newTC)
        {
            String sqlInsert = @"INSERT INTO TESTCERTIFICATE(PATIENTID, STAFFID, DATE, STATE)
                                VALUES        (@PATIENTID,@STAFFID,@DATE,@STATE)";
            SqlParameter[] sqlParameters = { new SqlParameter("@PATIENTID", newTC.PatientID),
                                            new SqlParameter("@STAFFID", newTC.StafID),
                                            new SqlParameter("@DATE", newTC.Date),
                                            new SqlParameter("@STATE", newTC.State)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateTC(TestCertificate updateTC)
        {
            string sqlUpdate = @"UPDATE       TESTCERTIFICATE
                                SET                DATE =@DATE, STATE =@STATE
                                WHERE         TCID=@TCID ";
            SqlParameter[] sqlParameters = { new SqlParameter("@TCID", updateTC.TCID),
                                            new SqlParameter("@DATE", updateTC.Date),
                                           new SqlParameter("@STATE", updateTC.State)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteTC(int tCID)
        {
            string sqlDelete = @"DELETE FROM TESTCERTIFICATE
                                WHERE        (TCID=@TCID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@TCID", tCID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListTC()
        {
            DataTable dtTC = new DataTable();
            string sqlSelect = @"SELECT        TCID, t.PATIENTID, t.STAFFID, DATE, t.STATE,  p.LASTNAME+' '+p.FIRSTNAME AS 'PATIENT NAME', s.LASTNAME+' '+s.FIRSTNAME AS 'STAFF NAME'
                                FROM            TESTCERTIFICATE  t join PATIENT p on t.PATIENTID = p.PATIENTID join STAFF s on s.STAFFID = t.STAFFID";
            dtTC = SqlResult.ExecuteQuery(sqlSelect);
            //dtTC.Columns[0].ColumnName = "Mã phiếu xét nghiệm";
            //dtTC.Columns[1].ColumnName = "Mã bệnh nhân";
            //dtTC.Columns[2].ColumnName = "Mã nhân viên";
            //dtTC.Columns[3].ColumnName = "Ngày lập";
            //dtTC.Columns[4].ColumnName = "Trạng thái";
            //dtTC.Columns[5].ColumnName = "Tên bệnh nhân";
            //dtTC.Columns[6].ColumnName = "Tên nhân viên";
            return dtTC;
        }
        public static TestCertificate GetTC(int tCID)
        {
            TestCertificate newTC = new TestCertificate();
            string sqlSelect = @"SELECT        TCID, PATIENTID, STAFFID, DATE, STATE
                                FROM            TESTCERTIFICATE
                                WHERE        TCID=@TCID";
            SqlParameter[] sqlParameters = { new SqlParameter("@TCID", tCID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                newTC.TCID = Convert.ToInt32(dataTable.Rows[0][0]);
                newTC.PatientID = Convert.ToInt32(dataTable.Rows[0][1]);
                newTC.StafID = Convert.ToInt32(dataTable.Rows[0][2]);
                newTC.Date = DateTime.Parse((dataTable.Rows[0][3].ToString()));
                newTC.State = int.Parse(dataTable.Rows[0][4].ToString());
            }
            return newTC;
        }
        public static int GetCurrentIdentity()
        {
            string sqlSelect = @"SELECT IDENT_CURRENT('TESTCERTIFICATE')  as currIdent";
            object ob = SqlResult.ExecuteScalar(sqlSelect);
            return Convert.ToInt32(ob);
        }
    }
}

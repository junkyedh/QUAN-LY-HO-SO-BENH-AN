using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;

namespace DO_AN_CUA_HAN.Model
{
    public class ExaminationCertificate
    {
        public int ECID { get; set; }
        public int PatientID { get; set; }
        public int StaffID { get; set; }
        public DateTime Date { get; set; }
        public String Result { get; set; }
        public int State { get; set; }

        public ExaminationCertificate() { }
        public ExaminationCertificate(int eCID, int patientID, int staffID, DateTime date, String result, int state)
        {
            this.ECID = eCID;
            this.PatientID = patientID;
            this.StaffID = staffID;
            this.Date = date;
            this.Result = result;
            this.State = state;
        }
        public static int InsertEC(ExaminationCertificate newEC)
        {
            String sqlInsert = @"INSERT INTO EXAMINATIONCERTIFICATE(PATIENTID, STAFFID, DATE, RESULT, STATE)
                                VALUES        (@PATIENTID,@STAFFID,@DATE,@RESULT,@STATE)";
            SqlParameter[] sqlParameters = { new SqlParameter("@PATIENTID", newEC.PatientID),
                                            new SqlParameter("@STAFFID", newEC.StaffID),
                                            new SqlParameter("@DATE", newEC.Date),
                                            new SqlParameter("@RESULT", newEC.Result),
                                           new SqlParameter("@STATE",newEC.State)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateEC(ExaminationCertificate updateEC)
        {
            string sqlUpdate = @"UPDATE       EXAMINATIONCERTIFICATE
                                SET                DATE =@DATE, RESULT =@RESULT, STATE =@STATE
                                WHERE         ECID=@ECID ";
            SqlParameter[] sqlParameters = { new SqlParameter("@ECID", updateEC.ECID ),
                                           new SqlParameter("@DATE",updateEC.Date),
                                           new SqlParameter("@RESULT", updateEC.Result),
                                           new SqlParameter("STATE", updateEC.State)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteEC(int eCID)
        {
            string sqlDelete = @"DELETE FROM EXAMINATIONCERTIFICATE
                                WHERE        (ECID=@ECID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@ECID", eCID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListEC()
        {
            DataTable dtEC = new DataTable();
            string sqlSelect = @"SELECT        E.ECID, E.PATIENTID, E.STAFFID, E.DATE, E.RESULT, E.STATE, P.LASTNAME + ' ' + P.FIRSTNAME AS 'PATIENT NAME', s.LASTNAME +' '+s.FIRSTNAME 'STAF NAME'
                                FROM       EXAMINATIONCERTIFICATE e join PATIENT p on e.PATIENTID = p.PATIENTID join STAFF s on e.STAFFID = s.STAFFID";
            dtEC = SqlResult.ExecuteQuery(sqlSelect);
            //dtEC.Columns[0].ColumnName = "Mã phiếu khám bệnh";
            //dtEC.Columns[1].ColumnName = "Mã bệnh nhân";
            //dtEC.Columns[2].ColumnName = "Mã nhân viên khám";
            //dtEC.Columns[3].ColumnName = "Ngày lập";
            //dtEC.Columns[4].ColumnName = "Kết quả";
            //dtEC.Columns[5].ColumnName = "Trạng thái";
            //dtEC.Columns[6].ColumnName = "Tên bệnh nhân"
            //dtEC.Columns[7].columnName = "Tên nhân viên khám"
            return dtEC;
        }
        public static ExaminationCertificate GetEC(int eCID)
        {
            ExaminationCertificate newEC = new ExaminationCertificate();
            string sqlSelect = @"SELECT        ECID, PATIENTID, STAFFID, DATE, RESULT, STATE
                                FROM            EXAMINATIONCERTIFICATE
                                WHERE        ECID=@ECID";
            SqlParameter[] sqlParameters = { new SqlParameter("@ECID", eCID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                newEC.ECID = Convert.ToInt32(dataTable.Rows[0][0]);
                newEC.PatientID = Convert.ToInt32(dataTable.Rows[0][1]);
                newEC.StaffID = Convert.ToInt32(dataTable.Rows[0][2]);
                newEC.Date = (DateTime)dataTable.Rows[0][3];
                newEC.Result = (String)dataTable.Rows[0][4];
                newEC.State = (int)dataTable.Rows[0][5];
            }
            return newEC;
        }

        public static int GetCurrentECID()
        {
            string sqlSelect = @"SELECT IDENT_CURRENT('EXAMINATIONCERTIFICATE') as CURRENTIDENTITY";

            return Convert.ToInt32(SqlResult.ExecuteScalar(sqlSelect));
        }

    }
}

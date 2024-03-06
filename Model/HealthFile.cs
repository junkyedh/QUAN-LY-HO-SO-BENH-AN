using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;
namespace DO_AN_CUA_HAN.Model
{
    public class HeathFile
    {
        public int HeathFileID { get; set; }
        public int PatientID { get; set; }
        public DateTime Date { get; set; }
        public String PatientState { get; set; }
        public String PreHistory { get; set; }
        public String Disease { get; set; }
        public String Treament { get; set; }

        public HeathFile() { }
        public HeathFile(int heathFileID, int patientID, DateTime date, String patientState, String preHistory, String disease, String treatment)
        {
            this.HeathFileID = heathFileID;
            this.PatientID = patientID;
            this.Date = date;
            this.PatientState = patientState;
            this.PreHistory = preHistory;
            this.Disease = disease;
            this.Treament = treatment;
        }
        public static int InsertHeathFile(HeathFile newHF)
        {
            String sqlInsert = @"INSERT INTO HEATHFILE(PATIENTID, DATE, PATIENTSTATE, PREHISTORY, DISEASE, TREATMENT)
                                VALUES        (@PATIENTID,@DATE,@PATIENTSTATE,@PREHISTORY,@DISEASE,@TREATMENT)";
            SqlParameter[] sqlParameters = { new SqlParameter("@PATIENTID", newHF.PatientID),
                                            new SqlParameter("@DATE", newHF.Date),
                                            new SqlParameter("@PATIENTSTATE", newHF.PatientState),
                                            new SqlParameter("@PREHISTORY", newHF.PreHistory),
                                            new SqlParameter("@DISEASE", newHF.Disease),
                                           new SqlParameter("@TREATMENT",newHF.Treament)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateHeathFile(HeathFile updateHF)
        {
            string sqlUpdate = @"UPDATE       HEATHFILE
                                SET           DATE =@DATE, PATIENTSTATE =@PATIENTSTATE, PREHISTORY =@PREHISTORY, DISEASE =@DISEASE, TREATMENT =@TREATMENT
                                WHERE         HEATHFILEID=@HEATHFILEID ";
            SqlParameter[] sqlParameters = { new SqlParameter("@HEATHFILEID", updateHF.HeathFileID ),
                                               new SqlParameter("@DATE", updateHF.Date ),
                                            new SqlParameter("@PATIENTSTATE", updateHF.PatientState),
                                           new SqlParameter("@PREHISTORY",updateHF.PreHistory ),
                                           new SqlParameter("@DISEASE", updateHF.Disease),
                                           new SqlParameter("TREATMENT", updateHF.Treament)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteHeathFile(int heathFileID)
        {
            string sqlDelete = @"DELETE FROM HEATHFILE
                                WHERE        (HEATHFILEID=@HEATHFILEID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@HEATHFILEID", heathFileID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListHeathFile()
        {
            DataTable dtHeathFile = new DataTable();
            string sqlSelect = @"SELECT        HEATHFILEID, h.PATIENTID, DATE, PATIENTSTATE, PREHISTORY, DISEASE, TREATMENT,p.LASTNAME+' '+p.FIRSTNAME AS 'PATIENT NAME'
                                FROM            HEATHFILE h join PATIENT p on h.PATIENTID = p.PATIENTID";
            dtHeathFile = SqlResult.ExecuteQuery(sqlSelect);
            //dtHeathFile.Columns[0].ColumnName = "Mã bệnh án";
            //dtHeathFile.Columns[1].ColumnName = "Mã bệnh nhân";
            //dtHeathFile.Columns[2].ColumnName = "Ngày lập";
            //dtHeathFile.Columns[3].ColumnName = "Tình trạng bệnh nhân";
            //dtHeathFile.Columns[4].ColumnName = "Tiền sử bệnh lý";
            //dtHeathFile.Columns[5].ColumnName = "Các bệnh mắc phải";
            //dtHeathFile.Columns[6].ColumnName = "Hướng điều trị";
            //dtHeathFile.Columns[7].ColumnName = "Tên bệnh nhân";
            return dtHeathFile;
        }



        // Lấy ra 1 đối tượng hồ sơ cụ thể dựa vào mã hồ sơ bệnh án
        public static HeathFile GetHeathFile(int heathFileID)
        {
            HeathFile hF = new HeathFile();
            string sqlSelect = @"SELECT        HEATHFILEID, PATIENTID, DATE, PATIENTSTATE, PREHISTORY, DISEASE, TREATMENT
                                FROM            HEATHFILE
                                WHERE        HEATHFILEID=@HEATHFILEID";
            SqlParameter[] sqlParameters = { new SqlParameter("@HEATHFILEID", heathFileID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                hF.HeathFileID = Convert.ToInt32(dataTable.Rows[0][0]);
                hF.PatientID = Convert.ToInt32(dataTable.Rows[0][1]);
                hF.Date = (DateTime)dataTable.Rows[0][2];
                hF.PatientState = (String)dataTable.Rows[0][3];
                hF.PreHistory = (String)dataTable.Rows[0][4];
                hF.Disease = (String)dataTable.Rows[0][5];
                hF.Treament = (String)dataTable.Rows[0][6];
            }
            return hF;
        }

        // Hàm kiểm tra xem bệnh nhân đã có bệnh án chưa
        // Đối với hàm này thì mình sẽ dựa trên mã bệnh nhân khóa ngoại của bảng
        public static Boolean DidPatientHaveHF(int patientID)
        {
            string sqlSelect = @"SELECT        HEATHFILEID, PATIENTID, DATE, PATIENTSTATE, PREHISTORY, DISEASE, TREATMENT
                                FROM            HEATHFILE
                                WHERE        PATIENTID=@PATIENTID";
            SqlParameter[] sqlParameters = { new SqlParameter("@PATIENTID", patientID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
                return true;
            return false;
        }
    }
}

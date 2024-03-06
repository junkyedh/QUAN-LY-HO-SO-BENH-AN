using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;

namespace DO_AN_CUA_HAN.Model
{
    public class Surgical
    {
        public int SurgicalID { get; set; }
        public int PatientID { get; set; }
        public DateTime Date { get; set; }
        public String Description { get; set; }
        public int State { get; set; }

        public Surgical() { }
        public Surgical(int surgicalID, int patientID, DateTime date, String description, int state)
        {
            this.SurgicalID = surgicalID;
            this.PatientID = patientID;
            this.Date = date;
            this.Description = description;
            this.State = state;
        }
        public static int InsertSurgical(Surgical newSurgical)
        {
            String sqlInsert = @"INSERT INTO SURGICAL(PATIENTID, DATE, DESCRIPTION, STATE)
                                VALUES        (@PATIENTID,@DATE,@DESCRIPTION,@STATE)";
            SqlParameter[] sqlParameters = { new SqlParameter("@PATIENTID", newSurgical.PatientID),
                                            new SqlParameter("@DATE", newSurgical.Date),
                                            new SqlParameter("@DESCRIPTION", newSurgical.Description ),
                                           new SqlParameter("@STATE",newSurgical.State)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateSurgical(Surgical updateSurgical)
        {
            string sqlUpdate = @"UPDATE       SURGICAL
                                SET                DATE =@DATE, DESCRIPTION =@DESCRIPTION, STATE =@STATE
                                WHERE         SURGICALID=@SURGICALID ";
            SqlParameter[] sqlParameters = { new SqlParameter("@SURGICALID", updateSurgical.SurgicalID),
                                            new SqlParameter("@DATE", updateSurgical.Date),
                                           new SqlParameter("@DESCRIPTION", updateSurgical.Description),
                                           new SqlParameter("@STATE", updateSurgical.State)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteSurgical(int surgicalID)
        {
            string sqlDelete = @"DELETE FROM SURGICAL
                                WHERE        (SURGICALID=@SURGICALID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@SURGICALID", surgicalID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListSurgical()
        {
            DataTable dtS = new DataTable();
            string sqlSelect = @"SELECT        SURGICALID, s.PATIENTID, DATE, DESCRIPTION, s.STATE, p.LASTNAME +' '+p.FIRSTNAME as 'PATIENT NAME'
                                FROM            SURGICAL s join PATIENT p on s.PATIENTID = p.PATIENTID";
            dtS = SqlResult.ExecuteQuery(sqlSelect);
            //dtS.Columns[0].ColumnName = "Mã ca phẩu thuật";
            //dtS.Columns[1].ColumnName = "Mã bệnh nhân";
            //dtS.Columns[2].ColumnName = "Ngày thực hiện";
            //dtS.Columns[3].ColumnName = "Mô tả";
            //dtS.Columns[4].ColumnName = "Trạng thái";
            //dtS.Columns[5].ColumnName = "Tên bệnh nhân";
            return dtS;
        }
        public static Surgical GetSurgical(int surgicalID)
        {
            Surgical newSurgical = new Surgical();
            int tempInterger;
            string sqlSelect = @"SELECT        SURGICALID, PATIENTID, DATE, DESCRIPTION, STATE
                                FROM            SURGICAL
                                WHERE        SURGICALID=@SURGICALID";
            SqlParameter[] sqlParameters = { new SqlParameter("@SURGICALID", surgicalID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0][0].ToString(), out tempInterger);
                newSurgical.SurgicalID = tempInterger;
                newSurgical.PatientID = int.Parse(dataTable.Rows[0][1].ToString());
                newSurgical.Date = DateTime.Parse((dataTable.Rows[0][2].ToString()));
                newSurgical.Description = dataTable.Rows[0][3].ToString();
                newSurgical.State = int.Parse(dataTable.Rows[0][4].ToString());
            }
            return newSurgical;
        }
        public static int GetCurrentIdentity()
        {
            string sqlSelect = @"SELECT IDENT_CURRENT('SURGICAL')  as currIdent";
            object ob = SqlResult.ExecuteScalar(sqlSelect);
            return Convert.ToInt32(ob);
        }
    }
}


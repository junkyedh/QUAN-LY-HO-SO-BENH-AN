using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;
namespace DO_AN_CUA_HAN.Model
{
    public class HospitalBed
    {
        public int BedID { get; set; }
        public int Patient { get; set; }
        public int State { get; set; }

        public HospitalBed() { }
        public HospitalBed(int bedID, int patient, int state)
        {
            this.BedID = bedID;
            this.Patient = patient;
            this.State = state;
        }
        public static int InsertHospitalBed()
        {
            HospitalBed newHB = new HospitalBed(0, 0, 0);
            String sqlInsert = @"INSERT INTO HOSPITALBED(PATIENT,STATE)
                                VALUES        (@PATIENT,@STATE)";
            SqlParameter[] sqlParameters = { new SqlParameter("@PATIENT", newHB.Patient),
                                           new SqlParameter("@STATE",newHB.State)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateHospitalBed(HospitalBed updateHB)
        {
            string sqlUpdate = @"UPDATE       HOSPITALBED
                                SET           PATIENT = @PATIENT, STATE = @STATE
                                WHERE         BEDID=@BEDID";
            SqlParameter[] sqlParameters = { new SqlParameter("@BEDID", updateHB.BedID),
                                           new SqlParameter("@PATIENT", updateHB.Patient),
                                           new SqlParameter("@STATE", updateHB.State)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteHospitalBed(int bedID)
        {
            string sqlDelete = @"DELETE FROM HOSPITALBED
                                WHERE (BEDID = @BEDID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@BEDID", bedID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public Boolean ChangeHospitalState()
        {
            return true;
        }
        public static DataTable GetListHospitalBed()
        {
            DataTable dtHB = new DataTable();
            string sqlSelect = @"SELECT        BEDID,PATIENT, h.STATE, ISNULL(p.LASTNAME + ' ' + p.FIRSTNAME, '') AS 'PATIENT NAME'
                                FROM            HOSPITALBED h left join PATIENT p on h.PATIENT = p.PATIENTID";
            dtHB = SqlResult.ExecuteQuery(sqlSelect);
            //dtHB.Columns[0].ColumnName = "Mã giường bệnh";
            //dtHB.Columns[1].ColumnName = "Mã bệnh nhân";
            //dtHB.Columns[2].ColumnName = "Trạng thái";
            //dtHB.Columns[3].ColumnName = "Tên bệnh nhân";
            return dtHB;
        }
        public static HospitalBed GetHospitalBed(int bedID)
        {
            HospitalBed hB = new HospitalBed();
            int tempInterger;
            string sqlSelect = @"SELECT        BEDID,PATIENT, STATE
                                FROM            HOSPITALBED
                                WHERE          (BEDID=@BEDID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@BEDID", bedID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            int.TryParse(dataTable.Rows[0][0].ToString(), out tempInterger);
            hB.BedID = tempInterger;
            hB.Patient = int.Parse(dataTable.Rows[0][1].ToString());
            hB.State = int.Parse(dataTable.Rows[0][2].ToString());
            return hB;
        }
        public static HospitalBed GetHospitalBed(String patient)
        {
            HospitalBed hB = new HospitalBed();
            string sqlSelect = @"SELECT        BEDID, PATIENT, STATE, 
                                FROM            HOSPITALBED
                                WHERE          (PATIENT=@PATIENT)";
            SqlParameter[] sqlParameters = { new SqlParameter("@PATIENT", int.Parse(patient)) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                hB.BedID = Convert.ToInt32(dataTable.Rows[0][0]);
                hB.Patient = Convert.ToInt32(dataTable.Rows[0][1].ToString());
                hB.State = int.Parse(dataTable.Rows[0][2].ToString());
            }
            return hB;
        }
        //Check patient already have bed or not
        public static Boolean CheckPatient(int patientID)
        {
            HospitalBed hB = new HospitalBed();
            string sqlSelect = @"SELECT        PATIENT, STATE, BEDID
                                FROM            HOSPITALBED
                                WHERE          (PATIENT=@PATIENT)";
            SqlParameter[] sqlParameters = { new SqlParameter("@PATIENT", patientID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
                return true;
            else
                return false;
        }
        //Check can conform of patient, if there are a bed haven't return yet, return false
        public static Boolean ConfirmPatient(int patientID)
        {
            if (CheckPatient(patientID))
                return false;
            else
                return true;
        }
    }
}

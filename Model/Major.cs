using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;

namespace DO_AN_CUA_HAN.Model
{
    public class Major
    {
        public int MajorID { get; set; }
        public string MajorName { get; set; }

        //private static Major tempMajor;

        public Major() { }

        public Major(int majorID, string majorName)
        {
            MajorID = majorID;
            MajorName = majorName;
        }

        public static int InsertMajor(Major major)
        {
            string sqlInsert = @"INSERT INTO MAJOR(MAJORNAME)
                                VALUES (@MajorName)";
            SqlParameter[] sqlParameters = { new SqlParameter("@MajorName", major.MajorName) };
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }

        public static int UpdateMajor(Major major)
        {
            string sqlUpdate = @"UPDATE MAJOR
                                SET MAJORNAME = @MajorName
                                WHERE (MAJORID = @MajorID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@MajorID", major.MajorID),
                                           new SqlParameter("@MajorName", major.MajorName)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }

        public static int DeleteMajor(int majorID)
        {
            string sqlDelete = @"DELETE FROM MAJOR
                                WHERE (MAJORID = @MajorID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@MajorID", majorID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }

        public static DataTable GetListMajor()
        {
            string sqlSelect = @"SELECT MAJORID, MAJORNAME
                                FROM MAJOR";
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect);
            return dataTable;
        }

        public static Major GetMajor(int majorID)
        {
            int tempInterger;
            Major newMajor = new Major();
            string sqlSelect = @"SELECT MAJORID, MAJORNAME
                                FROM MAJOR
                                WHERE (MAJORID = @MAJORID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@MAJORID", majorID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0][0].ToString(), out tempInterger);
                newMajor.MajorID = tempInterger;
                newMajor.MajorName = dataTable.Rows[0][1].ToString();
            }
            return newMajor;
        }
    }
}


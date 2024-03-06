using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;
namespace DO_AN_CUA_HAN.Model
{
    public class TestDetail
    {
        public int TestTypeID { get; set; }
        public int TCID { get; set; }
        public String Result { get; set; }


        public TestDetail() { }
        public TestDetail(int testTypeID, int tcID, String result)
        {
            this.TestTypeID = testTypeID;
            this.TCID = tcID;
            this.Result = result;
        }
        public static int InsertTestDetail(TestDetail newTD)
        {
            String sqlInsert = @"INSERT INTO TESTDETAIL(TCID, TESTTYPEID, RESULT)
                                VALUES        (@TCID,@TESTTYPEID,@RESULT)";
            SqlParameter[] sqlParameters = { new SqlParameter("@TCID", newTD.TCID),
                                            new SqlParameter("@TESTTYPEID", newTD.TestTypeID),
                                           new SqlParameter("@RESULT",newTD.Result)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public Boolean UpdateTestDetail()
        {
            return true;
        }
        public static int DeleteTestDetail(int tCID, int testTypeID)
        {
            string sqlDelete = @"DELETE FROM TESTDETAIL
                                WHERE TCID=@TCID AND TESTTYPEID=@TESTTYPEID";
            SqlParameter[] sqlParameters = { new SqlParameter("@TCID", tCID),
                                           new SqlParameter("@TESTTYPEID", testTypeID)};
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static int DeleteTestDetail(int tCID)
        {
            string sqlDelete = @"DELETE FROM TESTDETAIL
                                WHERE TCID=@TCID";
            SqlParameter[] sqlParameters = { new SqlParameter("@TCID", tCID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListTestDetail(int tCID)
        {
            DataTable dtTD = new DataTable();
            string sqlSelect = @"SELECT        TESTDETAIL.TCID, TESTDETAIL.TESTTYPEID, TESTDETAIL.RESULT, TESTTYPE.TYPENAME
                                FROM            TESTTYPE INNER JOIN TESTDETAIL ON TESTTYPE.TESTTYPEID = TESTDETAIL.TESTTYPEID
                                WHERE        TESTDETAIL.TCID=@TCID";
            SqlParameter[] sqlParameters = { new SqlParameter("@TCID", tCID) };
            dtTD = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            dtTD.Columns[0].ColumnName = "Mã phiếu xét nghiệm";
            dtTD.Columns[1].ColumnName = "Mã loại xét nghiệm";
            dtTD.Columns[2].ColumnName = "Kết quả";
            dtTD.Columns[3].ColumnName = "Tên loại xét nghiệm";
            return dtTD;
        }
    }
}


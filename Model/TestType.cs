using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;

namespace DO_AN_CUA_HAN.Model
{
    public class TestType
    {
        public int TestTypeID { get; set; }
        public String TestName { get; set; }

        public TestType() { }
        public TestType(int testTypeID, String testName)
        {
            this.TestTypeID = testTypeID;
            this.TestName = testName;
        }

        public static DataTable GetListTestType()
        {
            DataTable dtT = new DataTable();
            string sqlSelect = @"SELECT        TESTTYPEID, TYPENAME
                                FROM            TESTTYPE";
            dtT = SqlResult.ExecuteQuery(sqlSelect);
            //dtT.Columns[0].ColumnName = "Mã loại xét nghiệm";
            //dtT.Columns[1].ColumnName = "Tên loại xét nghiệm";
            return dtT;
        }
        public static TestType GetTestType(int testTypeID)
        {
            TestType newTestType = new TestType();
            int tempInterger;
            string sqlSelect = @"SELECT        TESTTYPEID, TYPENAME
                                FROM            TESTTYPE
                                WHERE         TESTTYPEID=@TESTTYPEID";
            SqlParameter[] sqlParameters = { new SqlParameter("@TESTTYPEID", testTypeID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0][0].ToString(), out tempInterger);
                newTestType.TestTypeID = tempInterger;
                newTestType.TestName = dataTable.Rows[0][1].ToString();
            }
            return newTestType;
        }
    }
}

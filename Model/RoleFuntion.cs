using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;

namespace DO_AN_CUA_HAN.Model
{
    public class RoleFunction
    {
        public int FunctionID { get; set; }
        public String FucntionName { get; set; }
        public String Button { get; set; }

        public RoleFunction() { }
        public RoleFunction(int functionID, String functionName, String button)
        {
            this.FunctionID = functionID;
            this.FucntionName = functionName;
            this.Button = button;
        }
        public static int InsertFunction(RoleFunction newFunction)
        {
            String sqlInsert = @"INSERT INTO ROLEFUNCTION(FUNCTIONNAME, BUTTON)
                                VALUES        (@FUNCTIONNAME,@BUTTON)";
            SqlParameter[] sqlParameters = { new SqlParameter("@FUNCTIONNAME", newFunction.FucntionName),
                                            new SqlParameter("@BUTTON", newFunction.Button)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateFunction(RoleFunction updateFunction)
        {
            string sqlUpdate = @"UPDATE       ROLEFUNCTION
                                SET                FUNCTIONNAME =@FUNCTIONNAME, BUTTON =@BUTTON
                                WHERE         FUNCTIONID=@FUNCTIONID ";
            SqlParameter[] sqlParameters = { new SqlParameter("@FUNCTIONID", updateFunction.FunctionID),
                                            new SqlParameter("@FUNCTIONNAME", updateFunction.FucntionName),
                                           new SqlParameter("@BUTTON", updateFunction.Button)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteFunction(int functionID)
        {
            string sqlDelete = @"DELETE FROM ROLEFUNCTION
                                WHERE        (FUNCTIONID=@FUNCTIONID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@FUNCTIONID", functionID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListFunction()
        {
            DataTable dtF = new DataTable();
            string sqlSelect = @"SELECT        FUNCTIONID, FUNCTIONNAME, BUTTON
                                FROM            ROLEFUNCTION";
            dtF = SqlResult.ExecuteQuery(sqlSelect);
            //dtF.Columns[0].ColumnName = "Mã chức năng";
            //dtF.Columns[1].ColumnName = "Tên chức năng";
            //dtF.Columns[2].ColumnName = "Nút kích hoạt";
            return dtF;
        }
        public static RoleFunction GetFunction(int functionID)
        {
            RoleFunction newFunction = new RoleFunction();
            int tempInterger;
            string sqlSelect = @"SELECT        FUNCTIONID, FUNCTIONNAME, BUTTON
                                FROM            ROLEFUNCTION
                                WHERE        FUNCTIONID=@FUNCTIONID";
            SqlParameter[] sqlParameters = { new SqlParameter("@FUNCTIONID", functionID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0][0].ToString(), out tempInterger);
                newFunction.FunctionID = tempInterger;
                newFunction.FucntionName = dataTable.Rows[0][1].ToString();
                newFunction.Button = dataTable.Rows[0][2].ToString();
            }
            return newFunction;
        }
    }
}

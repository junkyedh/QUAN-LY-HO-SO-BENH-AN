using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;
namespace DO_AN_CUA_HAN.Model
{
    public class RoleDetail
    {
        public int RoleID { get; set; }
        public int FunctionID { get; set; }

        public RoleDetail() { }
        public RoleDetail(int roleID, int functionID)
        {
            this.RoleID = roleID;
            this.FunctionID = functionID;
        }
        public static int InsertRoleDetail(RoleDetail newRD)
        {
            String sqlInsert = @"INSERT INTO ROLEDETAIL(ROLEID, FUNCTIONID)
                                VALUES        (@ROLEID,@FUNCTIONID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@ROLEID", newRD.RoleID),
                                            new SqlParameter("@FUNCTIONID", newRD.FunctionID)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int DeleteRoleDetail(int roleID, int functionID)
        {
            string sqlDelete = @"UPDATE       ROLEDETAIL
                                WHERE ROLEID=@ROLEID AND FUNCTIONID=@FUNCTIONID";
            SqlParameter[] sqlParameters = { new SqlParameter("@ROLEID", roleID),
                                           new SqlParameter("@FUNCTIONID", functionID)};
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static int DeleteRoleDetail(int roleID)
        {
            string sqlDelete = @"DELETE FROM       ROLEDETAIL
                                WHERE ROLEID=@ROLEID";
            SqlParameter[] sqlParameters = { new SqlParameter("@ROLEID", roleID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListStaffFunction(int roleID)
        {
            DataTable dtRoleDetail = new DataTable();
            string sqlSelect = @"SELECT        ROLEFUNCTION.FUNCTIONID, ROLEFUNCTION.FUNCTIONNAME, ROLEFUNCTION.BUTTON
                                FROM            ROLEDETAIL INNER JOIN ROLEFUNCTION ON ROLEDETAIL.FUNCTIONID = ROLEFUNCTION.FUNCTIONID
                                WHERE        ROLEDETAIL.ROLEID=@ROLEID";
            SqlParameter[] sqlParameters = { new SqlParameter("@ROLEID", roleID) };
            dtRoleDetail = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            dtRoleDetail.Columns[0].ColumnName = "Mã chức năng";
            dtRoleDetail.Columns[1].ColumnName = "Tên chức năng";
            dtRoleDetail.Columns[2].ColumnName = "Nút kích hoạt";
            return dtRoleDetail;
        }
    }
}

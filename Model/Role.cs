using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;
namespace DO_AN_CUA_HAN.Model
{
    public class Role
    {
        public int RoleID { get; set; }
        public String RoleName { get; set; }

        public Role() { }
        public Role(int roleID, String roleName)
        {
            this.RoleID = roleID;
            this.RoleName = roleName;
        }
        public static int InsertRole(Role newRole)
        {
            String sqlInsert = @"INSERT INTO Role (RoleNAME)
                                VALUES        (@RoleNAME)";
            SqlParameter[] sqlParameters = { new SqlParameter("@RoleNAME", newRole.RoleName) };
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateRole(Role updateRole)
        {
            string sqlUpdate = @"UPDATE       Role
                                SET                RoleNAME =@RoleNAME
                                WHERE          (RoleID=@RoleID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@RoleNAME", updateRole.RoleName),
                                           new SqlParameter("@RoleID", updateRole.RoleID)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteRole(int RoleID)
        {
            string sqlDelete = @"DELETE FROM Role
                                WHERE          (RoleID=@RoleID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@RoleID", RoleID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListRole()
        {
            DataTable dtRole = new DataTable();
            string sqlSelect = @"SELECT        ROLEID, ROLENAME
                                FROM            ROLE";
            dtRole = SqlResult.ExecuteQuery(sqlSelect);
            //dtRole.Columns[0].ColumnName = "Mã phân quyền";
            //dtRole.Columns[1].ColumnName = "Tên phân quyền";
            return dtRole;
        }
        public static Role GetRole(int roleID)
        {
            int tempInterger;
            Role newRole = new Role();
            string sqlSelect = @"SELECT        ROLEID, ROLENAME
                                FROM            ROLE
                                WHERE (ROLEID = @ROLEID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@ROLEID", roleID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0][0].ToString(), out tempInterger);
                newRole.RoleID = tempInterger;
                newRole.RoleName = dataTable.Rows[0][1].ToString();
            }
            return newRole;
        }
        public static int GetCurrentIdentity()
        {
            string sqlSelect = @"SELECT IDENT_CURRENT('ROLE')  as currIdent";
            object ob = SqlResult.ExecuteScalar(sqlSelect);
            return Convert.ToInt16(ob);
        }
    }
}


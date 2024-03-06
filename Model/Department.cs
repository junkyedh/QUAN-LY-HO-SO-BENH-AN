using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DO_AN_CUA_HAN.Functional;
using System.Data.SqlClient;

namespace DO_AN_CUA_HAN.Model
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public String DepartmentName { get; set; }

        public Department() { }
        public Department(int departmentID, String departmentName)
        {
            this.DepartmentID = departmentID;
            this.DepartmentName = departmentName;
        }
        public static int InsertDepartment(Department newDepartment)
        {
            String sqlInsert = @"INSERT INTO DEPARTMENT(DEPARTMENTNAME)
                                VALUES        (@DEPARTMENTNAME)";
            SqlParameter[] sqlParameters = { new SqlParameter("@DEPARTMENTNAME", newDepartment.DepartmentName) };
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateDepartment(Department updateDepartment)
        {
            string sqlUpdate = @"UPDATE DEPARTMENT
                                SET DEPARTMENTNAME = @DEPARTMENTNAME
                                WHERE (DEPARTMENTID =@DEPARTMENTID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@DEPARTMENTID", updateDepartment.DepartmentID),
                                           new SqlParameter("@DEPARTMENTNAME", updateDepartment.DepartmentName)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteDepartment(int departmentID)
        {
            string sqlDelete = @"DELETE FROM DEPARTMENT
                                WHERE (DEPARTMENTID = @DEPARTMENTID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@DEPARTMENTID", departmentID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static Department GetDepartment(int departmentID)
        {
            Department newDepartment = new Department();
            int tempInterger;
            string sqlSelect = @"SELECT        DEPARTMENTID, DEPARTMENTNAME
                                FROM            DEPARTMENT
                                WHERE           (DEPARTMENTID=@DEPARTMENTID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@DEPARTMENTID", departmentID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0][0].ToString(), out tempInterger);
                newDepartment.DepartmentID = tempInterger;
                newDepartment.DepartmentName = dataTable.Rows[0][1].ToString();
            }
            return newDepartment;
        }
        public static DataTable GetListDepartment()
        {
            DataTable dtDepartment = new DataTable();
            string sqlSelect = @"SELECT DEPARTMENTID, DEPARTMENTNAME
                                 FROM DEPARTMENT";
            dtDepartment = SqlResult.ExecuteQuery(sqlSelect);
            //dtDepartment.Columns[0].ColumnName = "Mã khoa";
            //dtDepartment.Columns[1].ColumnName = "Tên khoa";
            return dtDepartment;
        }
    }
}

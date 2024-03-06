using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;

namespace DO_AN_CUA_HAN.Model
{
    public class Service
    {
        public int ServiceID { get; set; }
        public String ServiceName { get; set; }
        public decimal Price { get; set; }

        public Service() { }
        public Service(int serviceID, String serviceName, decimal price)
        {
            this.ServiceID = serviceID;
            this.ServiceName = serviceName;
            this.Price = price;
        }
        public static int InsertService(Service newService)
        {
            String sqlInsert = @"INSERT INTO SERVICE(SERVICENAME, PRICE)
                                VALUES        (@SERVICENAME,@PRICE)";
            SqlParameter[] sqlParameters = { new SqlParameter("@SERVICENAME", newService.ServiceName),
                                           new SqlParameter("@PRICE",newService.Price)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateService(Service updateService)
        {
            string sqlUpdate = @"UPDATE       SERVICE
                                SET                SERVICENAME = @SERVICENAME, PRICE = @PRICE
                                WHERE         SERVICEID=@SERVICEID";
            SqlParameter[] sqlParameters = { new SqlParameter("@SERVICEID", updateService.ServiceID),
                                            new SqlParameter("@SERVICENAME", updateService.ServiceName),
                                           new SqlParameter("@PRICE", updateService.Price)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteService(int serviceID)
        {
            string sqlDelete = @"DELETE     FROM SERVICE
                                WHERE        (SERVICEID = @SERVICEID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@SERVICEID", serviceID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListService()
        {
            DataTable dtS = new DataTable();
            string sqlSelect = @"SELECT        SERVICEID, SERVICENAME, PRICE
                                FROM            SERVICE";
            dtS = SqlResult.ExecuteQuery(sqlSelect);
            //dtS.Columns[0].ColumnName = "Mã dịch vụ";
            //dtS.Columns[1].ColumnName = "Tên dịch vụ";
            //dtS.Columns[2].ColumnName = "Đơn giá";
            return dtS;
        }
        public static Service GetService(int serviceID)
        {
            Service newService = new Service();
            int tempInterger;
            string sqlSelect = @"SELECT        SERVICEID, SERVICENAME, PRICE
                                FROM            SERVICE
                                WHERE        SERVICEID=@SERVICEID";
            SqlParameter[] sqlParameters = { new SqlParameter("@SERVICEID", serviceID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0][0].ToString(), out tempInterger);
                newService.ServiceID = tempInterger;
                newService.ServiceName = dataTable.Rows[0][1].ToString();
                newService.Price = (decimal)dataTable.Rows[0][2];
            }
            return newService;
        }

        public static Service GetServiceExamination()
        {
            Service newService = new Service();
            int tempInterger;
            string sqlSelect = @"SELECT        SERVICEID, SERVICENAME, PRICE
                                FROM            SERVICE
                                WHERE        SERVICEID=100";

            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect);
            if (dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0][0].ToString(), out tempInterger);
                newService.ServiceID = tempInterger;
                newService.ServiceName = dataTable.Rows[0][1].ToString();
                newService.Price = (decimal)dataTable.Rows[0][2];
            }
            return newService;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;

namespace DO_AN_CUA_HAN.Model
{
    public class BillType
    {
        public int BillTypeID { get; set; }
        public String TypeName { get; set; }

        public BillType() { }
        public BillType(int billTypeID, String typeName)
        {
            this.BillTypeID = billTypeID;
            this.TypeName = typeName;
        }
        public static BillType GetBillType(int billTypeID)
        {
            BillType billType = new BillType();
            int tempInterger;
            string sqlSelect = @"SELECT        BILLTYPEID, TYPENAME
                                FROM            BILLTYPE
                                WHERE        BILLTYPEID=@BILLTYPEID";
            SqlParameter[] sqlParameters = { new SqlParameter("@BILLTYPEID", billTypeID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0][0].ToString(), out tempInterger);
                billType.BillTypeID = tempInterger;
                billType.TypeName = dataTable.Rows[0][1].ToString();
            }
            return billType;
        }
        public static DataTable GetListBillType()
        {
            DataTable dtB = new DataTable();
            string sqlSelect = @"SELECT        BILLTYPEID, TYPENAME
                                FROM            BILLTYPE";
            dtB = SqlResult.ExecuteQuery(sqlSelect);
            dtB.Columns[0].ColumnName = "Mã loại hóa đơn";
            dtB.Columns[1].ColumnName = "Tên loại hóa đơn";
            return dtB;
        }
    }
}

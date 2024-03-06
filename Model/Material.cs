using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DO_AN_CUA_HAN.Functional;
namespace DO_AN_CUA_HAN.Model
{
    public class Material
    {
        public int MaterialID { get; set; }
        public String MaterialName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Material() { }
        public Material(int materialID, String materialName, int quantity, decimal price)
        {
            this.MaterialID = materialID;
            this.MaterialName = materialName;
            this.Quantity = quantity;
            this.Price = price;
        }
        public static int InsertMaterial(Material newMaterial)
        {
            String sqlInsert = @"INSERT INTO MATERIAL(MATERIALNAME, QUANTITY, PRICE)
                                VALUES        (@MATERIALNAME,@QUANTITY,@PRICE)";
            SqlParameter[] sqlParameters = { new SqlParameter("@MATERIALNAME", newMaterial.MaterialName),
                                            new SqlParameter("@QUANTITY", newMaterial.Quantity),
                                           new SqlParameter("@PRICE",newMaterial.Price)};
            return SqlResult.ExecuteNonQuery(sqlInsert, sqlParameters);
        }
        public static int UpdateMaterial(Material updateMaterial)
        {
            string sqlUpdate = @"UPDATE       MATERIAL
                                SET               MATERIALNAME =@MATERIALNAME, QUANTITY =@QUANTITY, PRICE =@PRICE
                                WHERE        (MATERIALID=@MATERIALID) ";
            SqlParameter[] sqlParameters = { new SqlParameter("@MATERIALID", updateMaterial.MaterialID),
                                            new SqlParameter("@MATERIALNAME", updateMaterial.MaterialName),
                                           new SqlParameter("@QUANTITY", updateMaterial.Quantity),
                                           new SqlParameter("@PRICE", updateMaterial.Price)};
            return SqlResult.ExecuteNonQuery(sqlUpdate, sqlParameters);
        }
        public static int DeleteMaterial(int materialID)
        {
            string sqlDelete = @"DELETE FROM MATERIAL
                                WHERE (MATERIALID=@MATERIALID)";
            SqlParameter[] sqlParameters = { new SqlParameter("@MATERIALID", materialID) };
            return SqlResult.ExecuteNonQuery(sqlDelete, sqlParameters);
        }
        public static DataTable GetListMaterial()
        {
            DataTable dtM = new DataTable();
            string sqlSelect = @"SELECT        MATERIALID, MATERIALNAME, QUANTITY, PRICE
                                FROM            MATERIAL";
            dtM = SqlResult.ExecuteQuery(sqlSelect);
            //dtM.Columns[0].ColumnName = "Mã vật tư";
            //dtM.Columns[1].ColumnName = "Tên vật tư";
            //dtM.Columns[2].ColumnName = "Số lượng";
            //dtM.Columns[3].ColumnName = "Đơn giá";
            return dtM;
        }
        public static Material GetMaterial(int materialID)
        {
            Material newMaterial = new Material();
            int tempInterger;
            string sqlSelect = @"SELECT        MATERIALID,MATERIALNAME, QUANTITY, PRICE
                                FROM            MATERIAL
                                WHERE        MATERIALID=@MATERIALID";
            SqlParameter[] sqlParameters = { new SqlParameter("@MATERIALID", materialID) };
            DataTable dataTable = SqlResult.ExecuteQuery(sqlSelect, sqlParameters);
            if (dataTable.Rows.Count > 0)
            {
                int.TryParse(dataTable.Rows[0][0].ToString(), out tempInterger);
                newMaterial.MaterialID = tempInterger;
                newMaterial.MaterialName = dataTable.Rows[0][1].ToString();
                newMaterial.Quantity = int.Parse(dataTable.Rows[0][2].ToString());
                newMaterial.Price = (decimal)dataTable.Rows[0][3];
            }
            return newMaterial;
        }
    }
}


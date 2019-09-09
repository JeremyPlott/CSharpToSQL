using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace CSharpToSQLLibrary {

    public class Products {

        #region Instance Properties
        public int Id { get; private set; }
        public string PartNbr { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public string PhotoPath { get; set; }
        public int VendorId { get; set; }
        public Vendors Vendor { get; private set; }
        #endregion

        #region SQL Statements
        private const string SqlGetAll = "SELECT * from Products ";
        private const string SqlGetByPK = SqlGetAll + " WHERE Id = @Id;";
        private const string SqlGetByPartNbr = SqlGetAll + "WHERE PartNbr = @PartNbr;";
        private const string SqlDelete = "DELETE from Products WHERE Id = @Id;";
        private const string SqlUpdate = "UPDATE Products Set " +
            "PartNbr = @PartNbr, Name = @Name, Price = @Price, Unit = @Unit, " +
            "PhotoPath = @PhotoPath, VendorId = @VendorId " +
            "WHERE Id = @Id;";
        private const string SqlInsert = "INSERT Products " +
            " (PartNbr, Name, Price, Unit, PhotoPath, VendorId) " +
            " VALUES (@PartNbr, @Name, @Price, @Unit, @PhotoPath, @VendorId) ";
        #endregion

        public override string ToString() { //prints the actual info instead of just a generic variable location
            return $"Id={Id}, PartNbr={PartNbr}, Name={Name}, Price={Price}, VendorId={VendorId}, Vendor={Vendor}";
        }

        public static Connection Connection { get; set; }

        public static List<Products> GetAll() {
            var sqlCmd = new SqlCommand(SqlGetAll, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            var products = new List<Products>();
            while (reader.Read()) {
                var product = new Products();
                products.Add(product);
                LoadProductFromSql(product, reader);
            }
            reader.Close();

            Vendors.Connection = Connection; //necessary to do this here because GetByPk and GetAll data readers will conflict.
            foreach(var prod in products) {
                var vendor = Vendors.GetByPK(prod.VendorId);
                prod.Vendor = vendor;
            }

            return products;
        }

        private static void SetParameterValues(Products product, SqlCommand sqlCmd) {
            sqlCmd.Parameters.AddWithValue("@PartNbr", product.PartNbr);
            sqlCmd.Parameters.AddWithValue("@Name", product.Name);
            sqlCmd.Parameters.AddWithValue("@Price", product.Price);
            sqlCmd.Parameters.AddWithValue("@Unit", product.Unit);
            sqlCmd.Parameters.AddWithValue("@PhotoPath", (object)product.PhotoPath ?? DBNull.Value);
            sqlCmd.Parameters.AddWithValue("@VendorId", product.VendorId);
            sqlCmd.Parameters.AddWithValue("@Vendor", product.Vendor);
        }

        public static bool Insert(Products product) {
            var sqlCmd = new SqlCommand(SqlInsert, Connection.sqlConnection);
            SetParameterValues(product, sqlCmd);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        public static bool Update(Products product) {
            var sqlCmd = new SqlCommand(SqlUpdate, Connection.sqlConnection);
            SetParameterValues(product, sqlCmd);
            sqlCmd.Parameters.AddWithValue("@Id", product.Id);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        public static bool Delete(Products product) {
            var sqlCmd = new SqlCommand(SqlDelete, Connection.sqlConnection);
            SetParameterValues(product, sqlCmd);
            sqlCmd.Parameters.AddWithValue("@Id", product.Id);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        public static Products GetByPartNbr(string partNbr) {
            var sqlCmd = new SqlCommand(SqlGetByPartNbr, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@PartNbr", partNbr);
            var reader = sqlCmd.ExecuteReader();
            if(!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var product = new Products();
            LoadProductFromSql(product, reader);
            reader.Close();
            return product;
        }


        public static bool Delete(int Id) {
            return false;
        }

        public static Products GetByPK(int id) {
            var sqlCmd = new SqlCommand(SqlGetByPK, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Id", id);
            var reader = sqlCmd.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var product = new Products();
            LoadProductFromSql(product, reader);

            reader.Close();

            Vendors.Connection = Connection;
            var vendor = Vendors.GetByPK(product.VendorId);
            product.Vendor = vendor;
            return product;
        }

        private static void LoadProductFromSql(Products product, SqlDataReader reader) {
            product.Id = (int)reader["Id"];
            product.PartNbr = reader["PartNbr"].ToString();
            product.Name = reader["Name"].ToString();
            product.Price = (decimal)reader["Price"];
            product.Unit = reader["Unit"].ToString();
            product.PhotoPath = reader["PhotoPath"]?.ToString();
            product.VendorId = (int)reader["VendorId"];
        }
    }
}

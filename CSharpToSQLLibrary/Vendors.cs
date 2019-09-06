using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CSharpToSQLLibrary {

    public class Vendors {

        public static Connection Connection { get; set; }

        private const string SqlGetAll = "SELECT * from Vendors ";
        private const string SqlGetByPK = SqlGetAll + " WHERE Id = @Id;";
        private const string SqlDelete = "DELETE from Vendors WHERE Id = @Id;";
        private const string SqlUpdate = "UPDATE Vendors Set " +
            "Code = @Code, Name = @Name, Address = @Address, City = @City, " +
            "State = @state, Zip = @Zip, Phone = @Phone, Email = @Email " +
            "WHERE Id = @Id;";
        private const string SqlInsert = "INSERT into Vendors " +
            " (Code, Name, Address, City, State, Zip, Phone, Email) " +
            " VALUES (@Code, @Name, @Address, @City, @State, @Zip, @Phone, @Email) ";

        public static List<Vendors> GetAll() {
            var sqlCmd = new SqlCommand(SqlGetAll, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            var vendors = new List<Vendors>();
            while (reader.Read()) {
                var vendor = new Vendors();
                vendors.Add(vendor);
                LoadVendorFromSql(vendor, reader);
            }
            reader.Close();
            return vendors;
        }

        public static bool Delete(int id) {
            var sql = SqlDelete;
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Id", id);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        public static bool Insert(Vendors vendor) {
            var sql = "INSERT into Vendors " +
                "(Code, Name, Address, City, State, Zip, Phone, Email) " +
                " VALUES " +
                "(@Code, @Name, @Address, @City, @State, @Zip, @Phone, @Email)";
            //$"({user.Username}, {user.Password}, {user.Firstname}, {user.Lastname}, " +
            //$" {user.Phone}, {user.Email}, {user.IsAdmin}, {user.IsReviewer})" +
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            SetParameterValues(vendor, sqlCmd);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        public static bool Update(Vendors vendor) {
            var sql = "UPDATE vendors Set " +
                " Code = @Code, " +
                " Name = @Name, " +
                " Address = @Address, " +
                " City = @City, " +
                " State = @State, " +
                " Zip = @Zip, " +
                " Phone = @Phone, " +
                " Email = @Email " +
                " WHERE Id = @Id";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Id", vendor.Id);
            SetParameterValues(vendor, sqlCmd);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        public static Vendors GetByPK(int id) {
            var sqlCmd = new SqlCommand(SqlGetByPK, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Id", id); 
            var reader = sqlCmd.ExecuteReader();
            if (!reader.HasRows) { 
                reader.Close();
                return null;
            }
            reader.Read();
            var vendor = new Vendors();
            LoadVendorFromSql(vendor, reader);

            reader.Close();
            return vendor;
        }

        private static void SetParameterValues(Vendors vendor, SqlCommand sqlCmd) {
            sqlCmd.Parameters.AddWithValue("@Code", vendor.Code);
            sqlCmd.Parameters.AddWithValue("@Name", vendor.Name);
            sqlCmd.Parameters.AddWithValue("@Address", vendor.Address);
            sqlCmd.Parameters.AddWithValue("@City", vendor.City);
            sqlCmd.Parameters.AddWithValue("@State", vendor.State); //sends SQL null if there isn't a defined value
            sqlCmd.Parameters.AddWithValue("@Zip", vendor.Zip);
            sqlCmd.Parameters.AddWithValue("@Phone", (object)vendor.Phone ?? DBNull.Value);
            sqlCmd.Parameters.AddWithValue("@Email", (object)vendor.Email ?? DBNull.Value);
        }

        private static void LoadVendorFromSql(Vendors vendor, SqlDataReader reader) {
            vendor.Id = (int)reader["Id"];
            vendor.Code = reader["Code"].ToString();
            vendor.Name = reader["Name"].ToString();
            vendor.Address = reader["Address"].ToString();
            vendor.City = reader["City"].ToString();
            vendor.State = reader["State"].ToString();
            vendor.Zip = reader["Zip"].ToString();
            vendor.Phone = reader["Phone"]?.ToString(); //? means nullable
            vendor.Email = reader["Email"]?.ToString();

        }
        #region properties
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        #endregion
        //public override string ToString() {
        //    return $"ID={Id}, Code={Code}, Name={Name}";
        //}
    }
}

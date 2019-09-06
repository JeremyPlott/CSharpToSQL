using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CSharpToSQLLibrary {
    public class Users {

        public static Connection Connection { get; set; }

        public static bool Insert(Users user) {
            var sql = "INSERT into Users " +
                "(Username, Password, Firstname, Lastname, Phone, Email, IsAdmin,IsReviewer) " +
                " VALUES " +
                "(@Username, @Password, @Firstname, @Lastname, @Phone, @Email, @IsAdmin, @Isreviewer)";
                //$"({user.Username}, {user.Password}, {user.Firstname}, {user.Lastname}, " +
                //$" {user.Phone}, {user.Email}, {user.IsAdmin}, {user.IsReviewer})" +
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            SetParameterValues(user, sqlCmd);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        public static bool Update(Users user) {
            var sql = "UPDATE Users Set " +
                " Username = @Username, " +
                " Password = @Password, " +
                " Firstname = @Firstname, " +
                " Lastname = @Lastname, " +
                " Phone = @Phone, " +
                " Email = @Email, " +
                " IsAdmin = @IsAdmin, " +
                " IsReviewer = @IsReviewer " +
                " WHERE Id = @Id";
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Id", user.Id);
            SetParameterValues(user, sqlCmd);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return rowsAffected == 1;
        }

        private static void SetParameterValues(Users user, SqlCommand sqlCmd) {
            sqlCmd.Parameters.AddWithValue("@Username", user.Username);
            sqlCmd.Parameters.AddWithValue("@Password", user.Password);
            sqlCmd.Parameters.AddWithValue("@Firstname", user.Firstname);
            sqlCmd.Parameters.AddWithValue("@Lastname", user.Lastname);
            sqlCmd.Parameters.AddWithValue("@Phone", (object)user.Phone ?? DBNull.Value); //sends SQL null if there isn't a defined value
            sqlCmd.Parameters.AddWithValue("@Email", (object)user.Email ?? DBNull.Value);
            sqlCmd.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);
            sqlCmd.Parameters.AddWithValue("@IsReviewer", user.IsReviewer);
        }

        const string SqlDelete = "DELETE from users where Id = @Id;"; //means this string cannot be changed. Safety net. These would often go up at the top for ease of editing multi lines.

        public static bool Delete(int id) {
            var sql = SqlDelete;
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Id", id);
            var rowsAffected = sqlCmd.ExecuteNonQuery();            
            return rowsAffected == 1;
        }

        public static Users Login(string username, string password) {
            var sql = "SELECT * from users where Username = @Username AND Password = @Password"; 
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Username", username); 
            sqlCmd.Parameters.AddWithValue("@Password", password); 
            var reader = sqlCmd.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var user = new Users();
            LoadUserFromSql(user, reader);

            reader.Close();
            return user;
        }

        public static Users GetByPK(int id) {
            var sql = "SELECT * from users where Id = @Id"; //this prevents SQL Injection where as "where Id = " + Id" concatenation is risky
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Id", id); //adds a value to the parameter in the above SQL statement
            var reader = sqlCmd.ExecuteReader();
            if(!reader.HasRows) { //if reader.HasRows == false
                reader.Close();
                return null;
            }
            reader.Read();
            var user = new Users();
            LoadUserFromSql(user, reader);

            reader.Close();
            return user;
        }

        public static List<Users> GetAll() {
            var sql = "SELECT * from Users;"; //make sure SQL statements work in SSMS first
            var sqlCmd = new SqlCommand(sql, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            var users = new List<Users>();
            while(reader.Read()) {
                var user = new Users();
                users.Add(user);
                LoadUserFromSql(user, reader);
            }
            reader.Close();  //only 1 data reader can be open at a time
            return users;
        }

        private static void LoadUserFromSql(Users user, SqlDataReader reader) {
            user.Id = (int)reader["Id"]; //(int) casts the type to an integer from an object
            user.Username = reader["Username"].ToString(); //could also use same casting method as above
            user.Password = reader["Password"].ToString();
            user.Firstname = reader["Firstname"].ToString();
            user.Lastname = reader["Lastname"].ToString();
            user.Phone = reader["Phone"]?.ToString(); //? here = if obj is null, return null, otherwise return obj.ToString()
            user.Email = reader["Email"]?.ToString();
            user.IsAdmin = (bool)reader["IsAdmin"];
            user.IsReviewer = (bool)reader["IsReviewer"];
        }

        //region markers
        #region Instance properties
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? IsAdmin { get; set; } //? makes it a nullable boolean. Important for nullable columns in SQL.
        public bool? IsReviewer { get; set; }
        #endregion

        public Users() {
        }

        public override string ToString() {
            return $"Id={Id}, Username={Username}, Password={Password}, " +
                $"Name={Firstname} {Lastname}, Admin?={IsAdmin}, Reviewer?={IsReviewer}";
        }
    }
}

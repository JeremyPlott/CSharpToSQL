using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CSharpToSQLLibrary {
    public class Users {

        public static Connection Connection { get; set; }

        public static Users Login(string username, string password) {
            var sql = "SELECT * from users where Username = @Username AND Password = @Password"; 
            var sqlCmd = new SqlCommand(sql, Connection._Connection);
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
            var sqlCmd = new SqlCommand(sql, Connection._Connection);
            sqlCmd.Parameters.AddWithValue("@Id", id); //adds a value to the parameter in the above SQL statement
            var reader = sqlCmd.ExecuteReader();
            if(!reader.HasRows) { //if reader.HasRows == false
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
            var sqlCmd = new SqlCommand(sql, Connection._Connection);
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

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? IsAdmin { get; set; } //? makes it a nullable boolean. Important for nullable columns in SQL.
        public bool? IsReviewer { get; set; }

        public Users() {
        }

        public override string ToString() {
            return $"Id={Id}, Username={Username}, Password={Password}, " +
                $"Name={Firstname} {Lastname}, Admin?={IsAdmin}, Reviewer?={IsReviewer}";
        }
    }
}

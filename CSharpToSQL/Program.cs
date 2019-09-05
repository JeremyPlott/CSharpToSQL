using System;
using System.Collections.Generic;
using CSharpToSQLLibrary;
using System.Diagnostics;

namespace CSharpToSQL {

    class Program {

        void Run() {
            var conn = new Connection(@"localhost\sqlexpress", "PRSdb"); //can use 2 backslash to get a single backslash as well. @ = treat each character as a regular character.
            conn.Open();
            Users.Connection = conn;
            var logintest = Users.Login("NotAGiraffe", "password");
            Console.WriteLine(logintest);
            Console.WriteLine();

            var failedlogin = Users.Login("NotAGiraffe", "asdaa");
            Console.WriteLine(failedlogin?.ToString() ?? "Not found"); //abbreviated ternary operator, but specifically for nulls.
            Console.WriteLine();

            var users = Users.GetAll();
            foreach(var u in users) {
                Console.WriteLine(u);
            }
            Console.WriteLine();

            var user = Users.GetByPK(2);
            System.Diagnostics.Debug.WriteLine(user); //shows up in the Output tab at the bottom

            var usernf = Users.GetByPK(100);
            conn.Close(); //add breakpoint here and the users var above will be populated with the user data from SQL
        }

        static void Main(string[] args) {
            var pgm = new Program();
            pgm.Run();
        }
    }
}

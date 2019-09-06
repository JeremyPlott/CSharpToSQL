using System;
using System.Collections.Generic;
using CSharpToSQLLibrary;
using System.Diagnostics;

namespace CSharpToSQL {

    class Program {

        void RunProductTest() {
            var conn = new Connection(@"localhost\sqlexpress", "PRSdb");
            conn.Open();
            Products.Connection = conn;

            var lemons = Products.GetByPK(7);
            Console.WriteLine(lemons);

            var products = Products.GetAll();
            foreach(var p in products) {
                Console.WriteLine($"Product {p.Name} from Vendor {p.Vendor.Name} is priced at ${p.Price}");
            }

            conn.Close();

        }

        void RunVendorTest() {
            var conn = new Connection(@"localhost\sqlexpress", "PRSdb");
            conn.Open();
            Vendors.Connection = conn;

            var vendidtest = Vendors.GetByPK(2);

            var success = Vendors.Delete(4);
            var vendors = Vendors.GetAll();
            foreach (var v in vendors) {
                Console.WriteLine(v.Name);
            }

            //var newvendora = new Vendors();
            //newvendora.Address = "bridge4";
            //newvendora.City = "City";
            //newvendora.Code = "BGHTS";
            //newvendora.Email = "emaiiil.email";
            //newvendora.Name = "Big Hats";
            //newvendora.Phone = "484-562-4636";
            //newvendora.State = "NM";
            //newvendora.Zip = "43532";
            //success = Vendors.Insert(newvendora); 

            foreach(var v in vendors) {
                Console.WriteLine(v.Name);
            }


            conn.Close();
        }

        void RunUserTest() {
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

            var userabc = Users.GetByPK(3);
            userabc.Firstname = "Leon";
            userabc.Lastname = "Kennedy";
            var success = Users.Update(userabc);
            Console.WriteLine(userabc);

            var newuser = new Users();
            newuser.Username = "abc";
            newuser.Password = "xyz";
            newuser.Firstname = "Hello";
            newuser.Lastname = "World";
            newuser.IsAdmin = false;
            newuser.IsReviewer = true;
            success = Users.Insert(newuser);


            conn.Close(); //add breakpoint here and the users var above will be populated with the user data from SQL
        }

        static void Main(string[] args) {
            var pgm = new Program();
            pgm.RunProductTest();
        }
    }
}

using System;
using System.Collections.Generic;
using CSharpToSQLLibrary;

namespace CSharpToSQL {

    class Program {

        void Run() {
            var conn = new Connection(@"localhost\sqlexpress", "PRSdb"); //can use 2 backslash to get a single backslash as well. @ = treat each character as a regular character.
            conn.Open();
            conn.Close();
        }

        static void Main(string[] args) {
            var pgm = new Program();
            pgm.Run();
        }
    }
}

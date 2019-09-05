using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CSharpToSQLLibrary {

    public class Connection {

        public SqlConnection _Connection { get; set; } = null; //_ denotes it as a property. Can't use exact same name as the class here. Alternatively, use a different name.

        public void Open() {
            this._Connection.Open();
            if(this._Connection.State != System.Data.ConnectionState.Open) { //if this doesn't open, it's almost always a connStr problem.
                throw new Exception("Connection did not open");
            }
        }

        public void Close() {
            if(this._Connection.State != System.Data.ConnectionState.Open) {
                return;
            }
            this._Connection.Close();
        }

        //constructor
        public Connection(string server, string database) {

            //this is a shortcut for the connection string. In reality, this information will be read from an external file so the code doesn't have to change.
            var connStr = $"server={server};database={database};trusted_connection=true;"; //one type of connection string. Works for most SQL databases. There's a website w/connStrs.
            this._Connection = new SqlConnection(connStr);
        }
    }
}

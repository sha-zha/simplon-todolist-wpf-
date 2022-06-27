using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace simplon_todolist_wpf
{
    public static class Model
    {
        public  static void RunSqlite()
        {
            using (var connection = new SqliteConnection("Data Source=./bdd/database.db"))
            {
                connection.Open();  //  <== The database file is created here.

                // more code here that creates tables etc.

            }
        }

    }
}


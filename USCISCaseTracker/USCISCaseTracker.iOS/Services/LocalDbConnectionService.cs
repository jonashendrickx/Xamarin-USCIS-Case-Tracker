using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using USCISCaseTracker.iOS.Services;
using USCISCaseTracker.Services;

namespace USCISCaseTracker.iOS.Services
{
    
public class LocalDbConnectionService : ILocalDbConnectionService
    {
        public LocalDbConnectionService() { }

        public SQLite.SQLiteConnection Connect()
        {
            var sqliteFilename = "TodoSQLite.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);
            // Create the connection
            var conn = new SQLite.SQLiteConnection(path);
            // Return the database connection
            return conn;
        }
    }
}

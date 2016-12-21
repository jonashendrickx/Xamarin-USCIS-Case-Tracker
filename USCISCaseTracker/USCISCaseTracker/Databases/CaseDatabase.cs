using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USCISCaseTracker.Models;

namespace USCISCaseTracker.Databases
{
    public class CaseDatabase
    {
        static object locker = new object();
        public SQLiteConnection database;
        public string path;

        public CaseDatabase(SQLiteConnection connection)
        {
            database = connection;

            database.CreateTable<Case>();
        }

        public IEnumerable<Case> GetItems()
        {
            lock (locker)
            {
                return (from i in database.Table<Case>() select i).ToList();
            }
        }

        public Case GetItem(int id)
        {
            lock (locker)
            {
                return database.Table<Case>().FirstOrDefault(x => x.Id == id);
            }
        }

        public int SaveItem(Case item)
        {
            lock (locker)
            {
                if (item.Id != 0)
                {
                    item.ModifiedDate = DateTime.Now;
                    database.Update(item);
                    return item.Id;
                }
                else
                {
                    item.CreatedDate = DateTime.Now;
                    return database.Insert(item);
                }
            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return database.Delete<Case>(id);
            }
        }
    }
}

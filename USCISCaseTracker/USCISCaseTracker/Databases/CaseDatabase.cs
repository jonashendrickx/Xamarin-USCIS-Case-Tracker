using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using USCISCaseTracker.Models;

namespace USCISCaseTracker.Databases
{
    public class CaseDatabase
    {
        static object locker = new object();
        private readonly SQLiteConnection _database;

        public CaseDatabase(SQLiteConnection connection)
        {
            _database = connection;

            _database.CreateTable<Case>();
        }

        public IEnumerable<Case> GetItems()
        {
            lock (locker)
            {
                return (from i in _database.Table<Case>() select i).ToList();
            }
        }

        public Case GetItem(int id)
        {
            lock (locker)
            {
                return _database.Table<Case>().FirstOrDefault(x => x.Id == id);
            }
        }

        public int SaveItem(Case item)
        {
            lock (locker)
            {
                if (item.Id != 0)
                {
                    _database.Update(item);
                    return item.Id;
                }
                else
                {
                    return _database.Insert(item) ;
                }
            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return _database.Delete<Case>(id);
            }
        }

        public int GetCount()
        {
            lock (locker)
            {
                return (from i in _database.Table<Case>() select i).Count();
            }
        }

        public int GetUnreadCount()
        {
            lock (locker)
            {
                return (from i in _database.Table<Case>() where i.LastReadDate < i.LastModifiedDate select i).Count();
            }
        }

        public DateTime GetLastSynchronizedTime()
        {
            lock (locker)
            {
                if (GetCount() > 0)
                    return _database.Table<Case>().Max(x => x.LastSyncedDate);
                return new DateTime(1, 1, 1, 0, 0, 0);
            }
        }

        public DateTime GetLastUpdatedTime()
        {
            lock (locker)
            {
                if (GetCount() > 0)
                    return _database.Table<Case>().Max(x => x.LastModifiedDate);
                return new DateTime(1, 1, 1, 0, 0, 0);
            }
        }

        public void SaveLastReadTime(Case item)
        {
            lock (locker)
            {
                item.LastReadDate = DateTime.Now;
                if (item.Id != 0)
                {
                    _database.Update(item);
                }
            }
        }
    }
}

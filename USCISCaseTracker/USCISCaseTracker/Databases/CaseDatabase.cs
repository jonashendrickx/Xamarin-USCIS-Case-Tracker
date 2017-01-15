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
                    database.Update(item);
                    return item.Id;
                }
                else
                {
                    return database.Insert(item);
                }
            }
        }

        public bool SaveItem(Case oldCase, Case newCase)
        {
            oldCase.LastSyncedDate = DateTime.Now;
            if (oldCase.Status != null && oldCase.Description != null)
            {
                if (!oldCase.Status.Equals(newCase.Status) || !oldCase.Description.Equals(newCase.Description))
                {
                    oldCase.Status = newCase.Status;
                    oldCase.Description = newCase.Description;
                    oldCase.LastModifiedDate = DateTime.Now;
                }
            }
            else
            {
                oldCase.Status = newCase.Status;
                oldCase.Description = newCase.Description;
                oldCase.LastModifiedDate = DateTime.Now;
            }

            lock (locker)
            {
                var id = database.Update(oldCase);
                if (id != 0)
                {
                    return true;
                }
                return false;
            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return database.Delete<Case>(id);
            }
        }

        public int GetCount()
        {
            lock (locker)
            {
                return (from i in database.Table<Case>() select i).Count();
            }
        }

        public int GetUnreadCount()
        {
            lock (locker)
            {
                return (from i in database.Table<Case>() where i.LastReadDate < i.LastModifiedDate select i).Count();
            }
        }

        public DateTime GetLastSynchronizedTime()
        {
            lock (locker)
            {
                if (GetCount() > 0)
                    return database.Table<Case>().Max(x => x.LastSyncedDate);
                return new DateTime(1, 1, 1, 0, 0, 0);
            }
        }

        public DateTime GetLastUpdatedTime()
        {
            lock (locker)
            {
                if (GetCount() > 0)
                    return database.Table<Case>().Max(x => x.LastModifiedDate);
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
                    database.Update(item);
                }
            }
        }
    }
}

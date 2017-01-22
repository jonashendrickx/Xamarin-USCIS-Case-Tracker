using SQLite;
using System;
using System.Collections.Generic;
using USCISCaseTracker.Databases;
using USCISCaseTracker.Models;

namespace USCISCaseTracker.Repositories
{
    public class CaseRepository
    {
        CaseDatabase db = null;

        public CaseRepository(SQLiteConnection connection)
        {
            db = new CaseDatabase(connection);
        }

        public Case Read(int id)
        {
            return db.GetItem(id);
        }

        public IEnumerable<Case> Read()
        {
            return db.GetItems();
        }

        public int Count()
        {
            return db.GetCount();
        }

        public int UnreadCount()
        {
            return db.GetUnreadCount();
        }

        public int Save(Case entity)
        {
            return db.SaveItem(entity);
        }

        public int Delete(int id)
        {
            return db.DeleteItem(id);
        }

        public DateTime LastSynchronizedTime()
        {
            return db.GetLastSynchronizedTime();
        }
        public DateTime LastUpdatedTime()
        {
            return db.GetLastUpdatedTime();
        }

        public void SaveLastReadTime(Case entity)
        {
            db.SaveLastReadTime(entity);
        }
    }
}

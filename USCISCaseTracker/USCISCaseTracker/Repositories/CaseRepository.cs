using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public int Save(Case entity)
        {
            return db.SaveItem(entity);
        }

        public int Delete(int id)
        {
            return db.DeleteItem(id);
        }
    }
}

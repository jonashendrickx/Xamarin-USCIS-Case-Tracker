using SQLite;

namespace USCISCaseTracker.Services
{
    public interface ILocalDbConnectionService
    {
         SQLiteConnection Connect();
    }
}

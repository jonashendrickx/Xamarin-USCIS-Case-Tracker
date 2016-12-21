using SQLite;
using System.IO;
using USCISCaseTracker.Services;
using Windows.Storage;

namespace USCISCaseTracker.UWP.Services
{
    internal class LocalDbConnectionService : ILocalDbConnectionService
    {
        public static SQLiteConnection Connect()
        {
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "jonashendrickx-usciscasetracker.db3");
            var conn = new SQLiteConnection(path);
            return conn;
        }
    }
}

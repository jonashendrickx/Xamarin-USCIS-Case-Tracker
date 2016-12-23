using SQLite;
using System.IO;
using Windows.Storage;

namespace USCISCaseTracker.UWP.Shared.Services
{
    public class LocalDbConnectionService
    {
        public static SQLiteConnection Connect()
        {
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "jonashendrickx-usciscasetracker.db3");
            var conn = new SQLiteConnection(path);
            return conn;
        }
    }
}

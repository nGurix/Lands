using SQLite.Net.Interop;

namespace Lands.Interfaces
{
    public interface IConfig
    {
        //se crea para utilizar sqlite
        string DirectoryDB { get; } 

        ISQLitePlatform Platform { get; }
    }
}

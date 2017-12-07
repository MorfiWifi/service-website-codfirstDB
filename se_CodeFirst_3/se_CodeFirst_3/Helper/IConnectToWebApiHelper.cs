using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace se_CodeFirst_3.Helper
{
    public interface IConnectToWebApiHelper
    {
        Task<List<T>> GetListOfItems<T>(string path);
        Task<T> GetItem<T>(string path);
        T CreateItem<T>(string path, T incomingCall);
        T ChangeItem<T>(string path, T incomingCall);
        bool DeleteItem(string path, int id);
    }
}

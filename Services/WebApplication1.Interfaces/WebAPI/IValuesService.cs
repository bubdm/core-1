using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces.WebAPI
{
    public interface IValuesService
    {
        IEnumerable<string> GetAll();
        string GetById(int id);
        void Add(string str);
        void Edit(int id, string str);
        bool Delete(int id);
    }
}

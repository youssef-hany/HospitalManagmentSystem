using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.Repositories
{
    public interface IRepository<T>
    {
        bool Save();
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Add(T param);
        T Remove(int id);
        T GetByRoomNumber(int Number);
        T Update(int id, T param);
    }
}

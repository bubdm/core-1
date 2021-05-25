using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StudentsDAL.Repos.Base
{
    public interface IRepo<T>
    {
        int Add(T Entity, bool Persist = true);
        int AddRange(IEnumerable<T> Entites, bool Persist = true);
        int Update(T Entity, bool Persist = true);
        int UpdateRange(IEnumerable<T> Entites, bool Persist = true);
        int Delete(T Entity, bool Persist = true);
        int Delete(int Id, byte[] Timestamp, bool Persist = true);
        int DeleteRange(IEnumerable<T> Entites, bool Persist = true);
        T Find(int? Id);
        IEnumerable<T> Get(Expression<Func<T, bool>> Where);
        IEnumerable<T> Get<TSortField>(Expression<Func<T, bool>> Where, Expression<Func<T, TSortField>> OrderBy, bool Ansc);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll<TSortField>(Expression<Func<T, TSortField>> OrderBy, bool Ansc);
        int SaveChanges();
    }
}

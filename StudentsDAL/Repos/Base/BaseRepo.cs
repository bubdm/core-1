using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StudentsDAL.EF;
using StudentsDAL.Models.Base;

namespace StudentsDAL.Repos.Base
{
    public class BaseRepo<T> : IDisposable, IRepo<T> where T : Entity, new()
    {
        private bool _IsDisposed;
        private readonly bool _DisposeContext;
        public DbSet<T> Table { get; }
        public StudentsContext Context { get; }
        public BaseRepo(StudentsContext context)
        {
            Context = context;
            Table = Context.Set<T>();
        }
        public BaseRepo(DbContextOptions<StudentsContext> options) : this(new StudentsContext(options))
        {
            _DisposeContext = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_IsDisposed)
                return;
            if (disposing)
                if (_DisposeContext)
                    Context.Dispose();
            _IsDisposed = true;
        }
        ~BaseRepo() => Dispose(false);


        public int Add(T Entity, bool Persist = true)
        {
            Table.Add(Entity);
            return Persist ? SaveChanges() : 0;
        }
        public int AddRange(IEnumerable<T> Entites, bool Persist = true)
        {
            Table.AddRange(Entites);
            return Persist ? SaveChanges() : 0;
        }
        public int Update(T Entity, bool Persist = true)
        {
            Table.Update(Entity);
            return Persist ? SaveChanges() : 0;
        }
        public int UpdateRange(IEnumerable<T> Entites, bool Persist = true)
        {
            Table.UpdateRange(Entites);
            return Persist ? SaveChanges() : 0;
        }
        public int Delete(T Entity, bool Persist = true)
        {
            Table.Remove(Entity);
            return Persist ? SaveChanges() : 0;
        }
        public int Delete(int Id, byte[] Timestamp, bool Persist = true)
        {
            Context.Entry(new T {Id = Id, Timestamp = Timestamp}).State = EntityState.Deleted;
            return Persist ? SaveChanges() : 0;
        }
        public int DeleteRange(IEnumerable<T> Entites, bool Persist = true)
        {
            Table.RemoveRange(Entites);
            return Persist ? SaveChanges() : 0;
        }
        public T Find(int? Id) => Table.Find(Id);
        public IEnumerable<T> Get(Expression<Func<T, bool>> Where) => Table.Where(Where);
        public IEnumerable<T> Get<TSortField>(Expression<Func<T, bool>> Where, Expression<Func<T, TSortField>> OrderBy, bool Ansc) => 
            (Ansc ? Table.Where(Where).OrderBy(OrderBy) : Table.Where(Where).OrderByDescending(OrderBy));
        public IEnumerable<T> GetAll() => Table;
        public IEnumerable<T> GetAll<TSortField>(Expression<Func<T, TSortField>> OrderBy, bool Ansc) => 
            (Ansc ? Table.OrderBy(OrderBy) : Table.OrderByDescending(OrderBy));
        public int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

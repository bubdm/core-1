using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebApplication.Domain.Entities;
using WebApplication1.Dal.Context;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class SqlPersonsData : IPersonsData
    {
        private readonly Application1DB _Context;
        private readonly ILogger<Application1DB> _Logger;
        public SqlPersonsData(Application1DB context, ILogger<Application1DB> logger)
        {
            _Context = context;
            _Logger = logger;
        }
        public IEnumerable<Person> GetAll() => _Context.Persons;
        public Person Get(int id) => _Context.Persons.FirstOrDefault(p => p.Id == id);
        public int Add(Person person)
        {
            if (person is null)
                throw new ArgumentNullException(nameof(person));
            _Context.Add(person);
            _Context.SaveChanges();
            return person.Id;
        }
        public void Update(Person person)
        {
            if (person is null)
                throw new ArgumentNullException(nameof(person));
            _Context.Update(person);
            _Context.SaveChanges();
        }
        public bool Delete(int id)
        {
            if (Get(id) is not { } person) return false;
            _Context.Remove(person);
            _Context.SaveChanges();
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WebApplication1.Dal.Context;
using WebApplication1.Domain.Entities;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Services.Services
{
    public class SqlPersonsData : IPersonsData
    {
        private readonly Application1Context _Context;
        private readonly ILogger<Application1Context> _Logger;
        public SqlPersonsData(Application1Context context, ILogger<Application1Context> logger)
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
            #region Log

            _Logger.LogInformation($"Работник {person.Id} {person.LastName} {person.FirstName} {person.Patronymic} добавлен в базу данных успешно");

            #endregion
            return person.Id;
        }
        public void Update(Person person)
        {
            if (person is null)
                throw new ArgumentNullException(nameof(person));

            _Context.Update(person);
            #region Log

            _Logger.LogInformation($"Работник id:{person.Id} {person.LastName} {person.FirstName} {person.Patronymic} упешно обновлен в базе данных");

            #endregion
            _Context.SaveChanges();
        }
        public bool Delete(int id)
        {
            if (Get(id) is not { } person) return false;

            _Context.Remove(person);
            _Context.SaveChanges();
            #region Log

            _Logger.LogInformation($"Работник id:{person.Id} {person.LastName} {person.FirstName} {person.Patronymic} успешно удален из базы данных");

            #endregion
            return true;
        }
    }
}

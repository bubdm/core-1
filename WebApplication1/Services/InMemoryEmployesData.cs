﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Infrastructure.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class InMemoryEmployesData : IPersonsData
    {
        private readonly ICollection<Person> _Persons = Person.GetPersons;
        private int _maxId;

        public InMemoryEmployesData()
        {
            _maxId = _Persons.Max(p => p.Id);
        }

        public IEnumerable<Person> GetAll() => _Persons;
        public Person Get(int id)
        {
            return _Persons.SingleOrDefault(p => p.Id == id);
        }
        public int Add(Person person)
        {
            if (person is null) 
                throw new ArgumentNullException(nameof(person));
            if (_Persons.Contains(person))
                return person.Id;
            person.Id = ++_maxId;
            _Persons.Add(person);
            return person.Id;
        }

        public void Update(Person person)
        {
            if (person is null) 
                throw new ArgumentNullException(nameof(person));
            if (_Persons.Contains(person))
                return;
            var item = Get(person.Id);
            if (item is null) return;
            item.LastName = person.LastName;
            item.FirstName = person.FirstName;
            item.Patronymic = person.Patronymic;
            item.Age = person.Age;
            item.Birthday = person.Birthday;
            item.CountChildren = person.CountChildren;
        }

        public bool Delete(int id)
        {
            var item = Get(id);
            if (item is null) return false;
            return _Persons.Remove(item);
        }
    }
}
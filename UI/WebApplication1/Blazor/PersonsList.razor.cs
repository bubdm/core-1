using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using WebApplication1.Domain.Entities;
using WebApplication1.Interfaces.Services;

namespace WebApplication1.Blazor
{
    public partial class PersonsList
    {
        [Inject]
        public IPersonsData PersonsData { get; set; }
        public IEnumerable<Person> Persons => PersonsData.GetAll();
        public int SelectedAge { get; set; }

        public string GetClassPerson(int age)
        {
            return SelectedAge >= age ? "danger" : null;
        }
    }
}

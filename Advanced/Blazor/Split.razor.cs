using System.Collections.Generic;
using System.Linq;
using Advanced.Models;
using Microsoft.AspNetCore.Components;

namespace Advanced.Blazor
{
    public partial class Split
    {
        [Inject]
        public DataContext Context { get; set; }

        public bool Ascending { get; set; } = false;

        public IEnumerable<string> Names => Context.Persons.Select(p => p.FirstName);
    }
}

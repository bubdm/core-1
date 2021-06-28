using System.Collections.Generic;

namespace WebApplication1.Domain.WebModel
{
    public class SectionWebModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public SectionWebModel Parent { get; set; }
        public List<SectionWebModel> ChildSections { get; set; } = new List<SectionWebModel>();
    }
}

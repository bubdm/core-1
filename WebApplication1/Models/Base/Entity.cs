using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Base
{
    public abstract class Entity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}

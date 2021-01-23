using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatingAPP.API.Model
{
    public class Group
    {
        public Group()
        {
        }

        public Group(string name)
        {
            Name = name;
  
        }

        [Key]
        public string Name { get; set; }
        public ICollection<Connection> Connection { get; set; } = new List<Connection>();
    }
}
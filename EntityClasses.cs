using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeopleSportsSandbox
{
    public class Person
    {

        public Person()
        {
            Sports = new HashSet<Sports>();
        }

        [Key]
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Sports>? Sports { get; set; }
    }

    public class Sports
    {
        [Key]
        public Sport Sport { get; set; }


        public ICollection<Person> People { get; set; }
    }

    public enum Sport: int
    {
        Football=1,
        Soccer=2,
        Baseball=3,
        Basketball=4,
        Tennis=5,
        Golf=6,
        Hockey=7
    }

}

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using mongodb_project.Models;
using System.ComponentModel.DataAnnotations;

namespace mongodb_project.VM
{
    public class HomeVM
    {
        public IList<User> User { get; set; }
        public IList<Event> Event { get; set; }
        public string search { get; set; }
        public DateTime DateTime { get; set; }
    }
}

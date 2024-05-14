using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using mongodb_project.Models;

namespace mongodb_project.VM
{
    public class HomeVM
    {
        public IList<User> User { get; set; }
        public string search { get; set; }
    }
}

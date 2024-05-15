using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using mongodb_project.Models;
using mongodb_project.VM;
using System.Diagnostics;

namespace mongodb_project.Controllers
{
    public class HomeController : Controller
    {
        private IMongoCollection<User> _user;

        public HomeController(IMongoClient client)
        {
            var database = client.GetDatabase("sample_mflix");
            _user = database.GetCollection<User>("users");
        }
        [HttpGet]
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                User = _user.Find(user => !user.Isdeleted).SortByDescending(x => x.Date).ToList(),
            };

            return View(homeVM);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(User user)
        {
            user.Date= DateTime.UtcNow.AddHours(4);
            _user.InsertOne(user);
            
            return RedirectToAction("Index");


        }
        [HttpGet]
        public IActionResult Update(string id)
        {
            var userList = _user.Find(user => !user.Isdeleted && user.Id.ToString()==id).FirstOrDefault();
            return View(userList);
        }
        [HttpPost]
        public IActionResult Update(string id, User updatedUser)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId))
            {
                return BadRequest("Invalid ObjectId format.");
            }

            var filter = Builders<User>.Filter.Eq("_id", objectId);
            var update = Builders<User>.Update.Set("date", DateTime.UtcNow.AddHours(4));
            if (updatedUser.Name != null)
            {
                update = update.Set("name", updatedUser.Name);
            }
            if (updatedUser.Email != null)
            {
                update = update.Set("email", updatedUser.Email); 
            }


            var result = _user.UpdateOne(filter, update);

            if (result.ModifiedCount == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest("Document not found or not updated.");
            }
        }
        //[HttpPost]
        public IActionResult Delete(string id)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId))
            {
                return BadRequest("Invalid ObjectId format.");
            }

            var filter = Builders<User>.Filter.Eq("_id", objectId);
            var update = Builders<User>.Update.Set("date", DateTime.UtcNow.AddHours(4));
            update = update.Set("is_deleted", true);


            // Diğer alanlar için benzer şekilde işlem yapılabilir

            var result = _user.UpdateOne(filter, update);

            if (result.ModifiedCount == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Document not found or not deleted.");
            }
        }

        public IActionResult Filter(HomeVM home)
        {
            HomeVM homeVM = new HomeVM()
            {
                User = _user.Find(user => !user.Isdeleted && user.Name.Contains(home.search)).SortByDescending(x => x.Date).ToList(),
            };
            //return Json(userList);
            return View("Index", homeVM);
        }
    }
}

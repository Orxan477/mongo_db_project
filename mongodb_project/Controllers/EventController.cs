using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using mongodb_project.Models;
using mongodb_project.VM;

namespace mongodb_project.Controllers
{
    public class EventController : Controller
    {
        private IMongoCollection<Event> _event;

        public EventController(IMongoClient client)
        {
            var database = client.GetDatabase("sample_mflix");
            _event = database.GetCollection<Event>("events");
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Event = _event.Find(ev => !ev.Isdeleted && ev.Date >= DateTime.UtcNow.AddHours(4)).ToList(),
            };
            return View(homeVM);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Date= DateTime.UtcNow.AddHours(4);
            return View();
        }
        [HttpPost]
        public IActionResult Create(Event ev)
        {
            ev.CreatedDate = DateTime.UtcNow.AddHours(4);
            _event.InsertOne(ev);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update(string id)
        {
            var ev = _event.Find(ev => !ev.Isdeleted && ev.Id.ToString() == id).FirstOrDefault();
            return View(ev);
        }
        [HttpPost]
        public IActionResult Update(string id, Event ev)
        {
            ObjectId objectId;
            if (!ObjectId.TryParse(id, out objectId))
            {
                return BadRequest("Invalid ObjectId format.");
            }

            var filter = Builders<Event>.Filter.Eq("_id", objectId);
            var update = Builders<Event>.Update.Set("created_date", DateTime.UtcNow.AddHours(4));
            if (ev.Name != null)
            {
                update = update.Set("name", ev.Name);
            }
            if (ev.Description != null)
            {
                update = update.Set("description", ev.Description);
            }
            if (ev.Date != null)
            {
                update = update.Set("date", ev.Date);
            }

            var result = _event.UpdateOne(filter, update);

            if (result.ModifiedCount == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Event not found or not updated.");
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

            var filter = Builders<Event>.Filter.Eq("_id", objectId);
            var update = Builders<Event>.Update.Set("date", DateTime.UtcNow.AddHours(4));
            update = update.Set("is_deleted", true);


            // Diğer alanlar için benzer şekilde işlem yapılabilir

            var result = _event.UpdateOne(filter, update);

            if (result.ModifiedCount == 1)
            {
                return RedirectToAction("Index");        
            }
            else
            {
                return NotFound("Event not found or not deleted.");
            }
        }
        public IActionResult Filter(HomeVM home)
        {
            var startDate = home.DateTime.Date;
            var endDate = startDate.AddDays(1);

            var filter = Builders<Event>.Filter.And(
       Builders<Event>.Filter.Eq(ev => ev.Isdeleted, false),
       Builders<Event>.Filter.Or(
           Builders<Event>.Filter.Gte(ev => ev.Date, startDate) & Builders<Event>.Filter.Lt(ev => ev.Date, endDate),
           Builders<Event>.Filter.Where(ev => ev.Name.Contains(home.search))
       )
   );

            List<Event> events = _event.Find(filter).SortByDescending(ev => ev.CreatedDate).ToList();

            HomeVM homeVM = new HomeVM()
            {
                Event = events
            };

         //   return Json( homeVM);
            //ViewBag.Date = DateTime.UtcNow.AddHours(4);
            //return Json(userList);
            return View("Index",homeVM);
        }
    }
}

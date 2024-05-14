using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using mongodb_project.Models;

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
            var userList = _event.Find(ev => !ev.Isdeleted && ev.Date>= DateTime.UtcNow.AddHours(4)).ToList();

            return View(userList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Event ev)
        {
            ev.CreatedDate = DateTime.UtcNow.AddHours(4);
            _event.InsertOne(ev);

            return Ok("Event created successfully.");
        }
        [HttpGet]
        public IActionResult Update(string id)
        {
            return View();
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
                return Ok("Event updated successfully.");
            }
            else
            {
                return NotFound("Event not found or not updated.");
            }
        }
        [HttpPost]
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
                return Ok("Event deleted successfully.");
            }
            else
            {
                return NotFound("Event not found or not deleted.");
            }
        }
    }
}

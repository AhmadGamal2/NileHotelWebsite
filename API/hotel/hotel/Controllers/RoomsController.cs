using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using hotel.Models;

namespace hotel.Controllers
{
    public class RoomsController : ApiController
    {
        private hotelContext db = new hotelContext();


        // GET: api/Rooms
        public IQueryable<Room> GetRooms()
        {
            return db.Rooms;
        }

        // Get all Rooms
        // GET: api/Rooms/1(Branch ID)
        [Route("api/Rooms/{id}")]

        public IHttpActionResult GetRooms(int id)
        {
            return Ok(db.Rooms.Where(n => n.BID == id).ToList());
        }



        // Get Specific Room By Branch Id and Room Id
        // GET: api/rm/2(Branch ID)/60(Room ID)
        [ResponseType(typeof(Room))]
        [Route("api/rm/{bid}/{rid}")]
        public IHttpActionResult GetRoom(int bid , int rid )
        {
            Room room = db.Rooms.Where(n => n.RID == rid && n.BID == bid).SingleOrDefault();
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }



        // Get Specific Rooms By User ID
        // GET: api/rm/1(User ID)
        [ResponseType(typeof(Room))]
        [Route("api/rm/{uid}")]
        public IHttpActionResult GetRoom(int uid)
        {
            List<Room> rm = db.Rooms.Where(n => n.UID==uid ).Where(n=>n.Status== "Reserved").ToList();
            if (rm == null)
            {
                return NotFound();
            }

            return Ok(rm);
        }



        // Get Available Rooms to book for each branch
        // GET: api/rm
        [ResponseType(typeof(Room))]
        [Route("api/rms/{id}/available")]
        public IHttpActionResult GetRom(int id)
        {
            
            List<Room> rm = db.Rooms.Where(n=>n.BID==id && n.Status == "Available").ToList();
            if (rm == null)
            {
                return NotFound();
            }

            return Ok(rm);
        }




        // Get Specific Rooms By Searching
        // GET: api/rom/{search}
        [ResponseType(typeof(Room))]
        [Route("api/rom/{search}")]
        public IHttpActionResult GetRoom(string search)
        {
            List<Room> r = db.Rooms.Where(n => n.Branch.BName.Contains(search) || n.type.Contains(search) || search == null).ToList();
            
            if (r == null)
            {
                return NotFound();
            }

            return Ok(r);
        }




        // Cancel or Book Room
        // PUT: api/Rooms/{uid}
        [ResponseType(typeof(void))]
        [Route("api/Rooms/{uid}")]
        public IHttpActionResult PutRoom( int uid , Room r )
        {
            if (r == null)
                return BadRequest();

            Room rm = db.Rooms.Where(n => n.RID == r.RID && r.BID == r.BID).SingleOrDefault();
            if (rm == null)
                return NotFound();
            else
            {
                if (rm.Status == "Reserved")
                {
                    rm.Status = "Available";
                    rm.UID = null;
                }
                else
                {
                    rm.Status = "Reserved";
                    rm.UID = uid;
                    
                }
                db.SaveChanges();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }




        // POST: api/Rooms
        [ResponseType(typeof(Room))]
        public IHttpActionResult PostRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rooms.Add(room);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (RoomExists(room.BID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = room.BID }, room);
        }


        // DELETE: api/Rooms/5
        [ResponseType(typeof(Room))]
        public IHttpActionResult DeleteRoom(int id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            db.Rooms.Remove(room);
            db.SaveChanges();

            return Ok(room);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomExists(int id)
        {
            return db.Rooms.Count(e => e.BID == id) > 0;
        }
    }
}
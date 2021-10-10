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
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

namespace hotel.Controllers
{
    public class UsersController : ApiController
    {
        private hotelContext db = new hotelContext();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }


        // GET: api/Users/1
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User us = db.Users.Find(id);
            if (us == null)
            {
                return NotFound();
            }
            return Ok(us);
        }




        // GET: api/us/{email}/{password}
        [ResponseType(typeof(User))]
        [Route("api/us/{email}/{password}")]
        public IHttpActionResult GetUser(string email, string password)
        {
            User us = db.Users.Where(n => n.Email == email && n.Password == password).SingleOrDefault();
            if (us == null)
            {
                return NotFound();
            }
            
            return Ok(us);
        }




        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }



        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult postUser(User user )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (user.UID == 0)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtRoute("DefaultApi", new { id = user.UID }, user);
        }




        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UID == id) > 0;
        }
    }
}
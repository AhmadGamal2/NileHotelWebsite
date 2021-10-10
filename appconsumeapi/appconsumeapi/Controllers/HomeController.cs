using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using appconsumeapi.Models;
using Newtonsoft.Json;

namespace appconsumeapi.Controllers
{
    public class HomeController : Controller
    {       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult allbranches()
        {
            IEnumerable<Branch> branch;
            HttpResponseMessage response = GlobalVariables.client.GetAsync("https://localhost:44319/api/Branches").Result;
            branch = response.Content.ReadAsAsync<IEnumerable<Branch>>().Result;
            return View(branch);
        }



        public ActionResult allrooms(int id)
        {
            IEnumerable<Room> r;
            HttpResponseMessage response = null;
            if (Session["user_id"] == null)
            {
                response = GlobalVariables.client.GetAsync($"https://localhost:44319/api/Rooms/{id}").Result;
            }
            else
            {
                response = GlobalVariables.client.GetAsync($"https://localhost:44319/api/rms/{id}/available"  ).Result;
            }

            if (response.IsSuccessStatusCode)
            {
                r = response.Content.ReadAsAsync<IList<Room>>().Result;
            }
            else
            {
                r = Enumerable.Empty<Room>();
                ModelState.AddModelError(string.Empty, "Server Error Occured");
            }
            
            return View(r);
        }


        public ActionResult RoomDetails (int rid, int bid)
        {
            Room room = null;
            HttpResponseMessage response = GlobalVariables.client.GetAsync($"https://localhost:44319/api/rm/{bid}/{rid}").Result;
            if (response.IsSuccessStatusCode)
            {
                room = response.Content.ReadAsAsync<Room>().Result;
            }
            return View(room);
        }

        public ActionResult editroom(int rid , int bid  )
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("login");
            }
            else
            {
                Room room = null;
                HttpResponseMessage response = GlobalVariables.client.GetAsync($"https://localhost:44319/api/rm/{bid}/{rid}").Result;
                if (response.IsSuccessStatusCode)
                {
                    room = response.Content.ReadAsAsync<Room>().Result;
                }

                int uid = int.Parse(Session["user_id"].ToString());

                HttpResponseMessage res = GlobalVariables.client.PutAsJsonAsync($"https://localhost:44319/api/Rooms/{uid}", room).Result;
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("profile");
                }
                return RedirectToAction("allrooms");
            }
        }





        public ActionResult register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult register(User u)
        {

            HttpResponseMessage response = GlobalVariables.client.PostAsJsonAsync<User>($"https://localhost:44319/api/Users", u ).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("login");
            }
            ViewBag.status = "Invalid Data";
            return View();
        }




        public ActionResult login()
        {
            if (Request.Cookies["full"] != null)
            {
                Session["user_id"] = Request.Cookies["full"].Values["id"];
                return RedirectToAction("profile");
            }
            return View();
        }


        [HttpPost]
        public ActionResult login([Bind(Include = "email,password")] User u, bool rememberme)
        {
            User us = null;
            HttpResponseMessage response = GlobalVariables.client.GetAsync($"https://localhost:44319/api/us/{u.Email}/{u.Password}").Result;

            if (response.IsSuccessStatusCode)
            {
                us = response.Content.ReadAsAsync<User>().Result;
                if (rememberme)
                {
                    HttpCookie c = new HttpCookie("full");
                    c.Values.Add("id", us.UID.ToString());
                    c.Values.Add("Name", us.UName);
                    c.Expires = DateTime.Now.AddDays(10);
                    Response.Cookies.Add(c);
                    ViewBag.user = Request.Cookies["full"].Values["Name"];
                }

                Session.Add("user_id", us.UID);
                Session.Add("user_uname", us.UName);
                ViewBag.user = Session["user_id"];
                return RedirectToAction("profile");
            }
            else
            {
                ViewBag.status = "Incorrect Email Or Password";
                return View();
            }
        }


        public ActionResult profile()
        {
            if (Session["user_id"] != null)
            {
                int id = int.Parse(Session["user_id"].ToString());
                User usr = null;
                IEnumerable<Room> r;
                HttpResponseMessage response = GlobalVariables.client.GetAsync($"https://localhost:44319/api/Users/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    usr = response.Content.ReadAsAsync<User>().Result;
                    HttpResponseMessage res = GlobalVariables.client.GetAsync($"https://localhost:44319/api/rm/{usr.UID}").Result;
                    r = res.Content.ReadAsAsync<IList<Room>>().Result;
                    ViewBag.Rooms = r;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server Error Occured");
                }
                return View(usr);
            }
            else
            {
                return RedirectToAction("login");
            }
        }

        public ActionResult logout()
        {
            Session["user_id"] = null;
            HttpCookie c = new HttpCookie("full");
            c.Expires = DateTime.Now.AddDays(-15);
            Response.Cookies.Add(c);
            return RedirectToAction("login");
        }

        

        public ActionResult search(string searching)
        {
            IEnumerable<Room> r=null;
            HttpResponseMessage response = GlobalVariables.client.GetAsync($"https://localhost:44319/api/rom/{searching}").Result;
            if (response.IsSuccessStatusCode)
            {
                r = response.Content.ReadAsAsync<IList<Room>>().Result;
            }
            return View(r);
        }
    }
}
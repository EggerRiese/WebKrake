using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebKrake.Models;
using WebKrake.Network;

namespace WebKrake.Controllers
{
    public class HomeController : Controller
    {
        private Connection conn;
        public ActionResult Index()
        {
            conn = new Connection();
            dynamic mymodel = new ExpandoObject();
            mymodel.Bar = conn.GetEvents("Oldenburg", "bar");
            mymodel.Party = conn.GetEvents("Oldenburg", "party");
            mymodel.Location = conn.GetLocations("Oldenburg");

            if(mymodel.Bar[0].Id == "-500")
            {
                return View("Error", 500);
            }
            else
            {
                ViewBag.Title = "Krake";
                return View(mymodel);
            }
        }

        [HttpPost]
        public ActionResult Verify(Account account)
        {
            //Directory.GetFiles("~/krake/img/", "*");
            conn = new Connection();
            if (conn.getAccount(account) == 200)
            {
                return View("Create", getModel());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Event(Event e)
        {
            int error = 0;
            if(e.Image == null)
            {
                return View();
            }
            conn = new Connection();

            if (conn.AddEvent(e, "party") == 200)
            {
                return View("Create", getModel());
            }
            else
            {
                return View("Error", error);
            }
        }

        [HttpPost]
        public ActionResult Bar(Event e)
        {
            int error = 0;
            conn = new Connection();

            if (conn.AddEvent(e, "bar") == 200)
            {
                return View("Create", getModel());
            }
            else
            {
                return View("Error", error);
            }
        }

        [HttpPost]
        public ActionResult Location(Location l)
        {
            int error = 0;
            conn = new Connection();

            if(conn.AddLocation(l) == 200)
            {
                return View("Create", getModel());
            }else
            {
                return View("Error", error);
            }
        }

        [HttpPost]
        public ActionResult Delete(string id, string table)
        {
            int error = 0;
            if (id == null || table == null)
            {
                return View("Error", 401);
            }

            conn = new Connection();

            if(table == "location")
            {
                error = conn.DeleteLocation(id);
            } else
            {
                error = conn.DeleteEvent(id, table);
            }

            if(error == 200)
            {
                return View("Create", getModel());
            }else
            {
                return View("Error", error);
            }
        }

        [HttpPost]
        public ActionResult Expired()
        {
            conn = new Connection();
            var array = conn.DeleteExpiredEvents();
            if(array[0] == 200)
            {
                return View("Create", getModel(200, array[1]));
            } else
            {
                return View("Error", array[0]);
            }
        }

        public ActionResult Impressum()
        {
            return View();
        }

        public ActionResult Datenschutz()
        {
            return View();
        }

        private dynamic getModel(int ErrorCode = 200, int affected = 0)
        {
            conn = new Connection();
            dynamic model = new ExpandoObject();
            model.City = conn.GetAllCitys();
            model.Location = conn.GetLocations("Oldenburg");
            model.Bar = conn.GetEvents("Oldenburg", "bar");
            model.Party = conn.GetEvents("Oldenburg", "party");
            model.ErrorCode = ErrorCode;
            model.AffectedRows = affected;
            return model;
        }
    }
}

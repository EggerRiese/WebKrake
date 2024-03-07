using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebKrake.Models;
using WebKrake.Network;

namespace WebKrake.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Verify(Account account)
        {
            Connection conn = new Connection();
            if (conn.getAccount(account) == 200)
            {
                return View("Create");
            }
            else
            {
                return View("Error");
            }
        }
    }
}
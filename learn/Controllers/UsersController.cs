using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using learn.Models;

namespace learn.Controllers
{
    public class UsersController : Controller
    {
        private MyDataBaseEntities db = new MyDataBaseEntities();

        // GET: Users
        public ActionResult Index()
        {
            if (Session["userName"] != null || Session["adminName"] != null)
            {
                if(Session["adminName"] != null)
                return View(db.Tickets.ToList());
                 else
                  return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Login");


        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name , Email , Password , ConfEmail , ConfPassword")] Users users)
        {
            users.Status = 0;
            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(users);
        }

    
        public ActionResult Approve(int id  )
        {
            var res = db.Tickets.AsNoTracking().Where(x => x.TicketId == id).ToList().FirstOrDefault(); ;
            res.Status = "Approve";
          
            db.Entry(res).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
       
        public ActionResult Reject(int id)
        {
            var res = db.Tickets.AsNoTracking().Where(x => x.TicketId == id).ToList().FirstOrDefault(); ;
            res.Status = "Reject";

            db.Entry(res).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session["userName"] = null;
            Session["adminName"] = null;

            return RedirectToAction("login");
        }

        [HttpPost]
        public ActionResult Login([Bind(Include ="Email , Password")]Users users )
        {
            var rec = db.Users.Where(x => x.Email == users.Email && x.Password == users.Password).ToList().FirstOrDefault();
            if (rec != null)
            {
                
                if (rec.Status == 0 )
                {
                    Session["userName"] = rec.Name;
                    Session["UserID"] = rec.UserId;
                    return RedirectToAction("ViewTickets");
                }
                else
                {
                    Session["adminName"] = rec.Name;
                    return RedirectToAction("Index");
                }

            }
            else
            {
                ViewBag.error = "Password or Email is Worng !!";
                return View(users);

            }
        }
        public ActionResult CreatTicket()
        {
            if (Session["userName"] != null || Session["adminName"] != null)
            {
                if (Session["userName"] != null)
                    return View();
                else
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Login");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatTicket([Bind(Exclude = "Status , UserId")]Tickets tickets)
        {

            tickets.Status = "sent";
            var id = Session["UserID"];
            tickets.UserId =(int) id ;
            if (ModelState.IsValid)
            {
                db.Tickets.Add(tickets);
                db.SaveChanges();
                return RedirectToAction("ViewTickets");
            }
            return View();
        }
        public ActionResult ViewTickets()
        {
            if (Session["userName"] != null || Session["adminName"] != null)
            {
                int id = (int)Session["UserID"];
                ViewData["UserTikets"] = db.Tickets.Find(id);

                if (Session["userName"] != null)

                    return View(db.Tickets.Where(x => x.UserId == id).ToList());
                else
                    return RedirectToAction("Index" , "Home");
            }
            else
                return RedirectToAction("Login");
            
        }


    }
}

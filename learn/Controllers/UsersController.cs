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

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name , Email , Password")] Users users)
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

        // GET: Users/Edit/5
       /** public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }**/

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
       // [HttpPost]
        //[ValidateAntiForgeryToken]
        /**public ActionResult Edit([Bind(Include = "UserId,Name,Email,Password,Status")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }
        **/
        public ActionResult Edit(int id  )
        {
            var res = db.Tickets.AsNoTracking().Where(x => x.TicketId == id).ToList().FirstOrDefault(); ;
            res.Status = "Approve";
          
            db.Entry(res).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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
            Session["userName"] = null;
            return View();
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
                //var UserTickets = db.Tickets.Find(id);
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

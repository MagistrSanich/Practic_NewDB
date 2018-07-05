using Practic_NewDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Practic_NewDB.Controllers
{
    public class HomeController : Controller
    {
        SoccerContextBD db = new SoccerContextBD();
        public ActionResult Index()
        {
            var players = db.Players.Include(p => p.Team);
            return View(players.ToList());
        }
        public ActionResult TeamDetails(int? Id)
        {
            if(Id == null)
            {
                return HttpNotFound();
            }
            Team team = db.Teams.Include(t => t.Players).FirstOrDefault(t => t.Id==Id);
            //FirstOrDefault - возвращает первый элемент последовательности удовлетворяющий условию, либо 
            //возвращает дефолтный эл.
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SelectList teams = new SelectList(db.Teams, "Id", "Name");
            ViewBag.Teams = teams;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Player player)
        {
            db.Players.Add(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if(id==null)
            {
                return HttpNotFound();
            }
            Player player = db.Players.Find(id);
            if(player!= null)
            {
                SelectList selects = new SelectList(db.Teams, "Id", "Name", player.TeamId);
                ViewBag.Teams = selects;
                return View(player);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(Player player)
        {
            db.Entry(player).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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


    }
}
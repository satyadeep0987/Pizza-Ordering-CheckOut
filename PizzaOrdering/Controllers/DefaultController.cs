using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using PizzaOrdering.Models;

namespace PizzaOrdering.Controllers
{
    public class DefaultController : Controller
    {
        ProjectEntities db = new ProjectEntities();
        // GET: Default
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string command)
        {
            CustomerCheckOut c = new CustomerCheckOut();
            if (command == "CheckOut")
            {
                return (CheckOut());
            }        
            else
                return (Check());    
        }

       

        [HttpPost]
        public ActionResult Check()
        {
            CustomerCheckOut c = new CustomerCheckOut();
            TempData["dno"] = Convert.ToInt64(Request.Form["search"]);
            TempData.Keep();
            c.contact = Convert.ToInt64(Request.Form["search"]);
            var res = db.CustomerCheckOuts.Where(x => x.contact == c.contact).SingleOrDefault();
            Session["data"] = res;
            return View();
            
        }

        [HttpPost]
        public ActionResult CheckOut()
        {
            CustomerCheckOut c = new CustomerCheckOut();
            c.contact = Convert.ToInt64(Request.Form["txtcontact"]);
            var res = db.CustomerCheckOuts.Where(x => x.contact == c.contact ).SingleOrDefault();
           
             if (res == null )
                {
                c.cname = Request.Form["txtcname"].ToString();
               // c.contact = Convert.ToInt64(Request.Form["txtcontact"]);
                c.orders = 1;
                c.total = Convert.ToInt64(Request.Form["txttotal"]);
                db.CustomerCheckOuts.Add(c);
                var op = db.SaveChanges();
                if (op > 0)
                    ModelState.AddModelError("", "New Customer Made an order");
                TempData["dno"] = null;
                
                }
            else
                {
                res.orders = res.orders + 1;
                res.total = res.total + Convert.ToInt64(Request.Form["txttotal"]);
                var op = db.SaveChanges();
                if (op > 0)
                    ModelState.AddModelError("", "One More Order Made By the Customer");
                TempData["dno"] = null; 
                }
            return View();
        }

       
    }
}
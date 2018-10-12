using Jedzenie.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Jedzenie.Controllers
{
    public class HomeController : Controller
    {
        JedzenieDbContext _db = new JedzenieDbContext();

        public ActionResult AdminPanel()
        {
            return View(_db.Orders.Where(x => x.Realized == false).ToList());
        }

        public void Refresh()
        {
            _db.Database.Delete();
            _db = new JedzenieDbContext();
        }

        [HttpGet]
        public ActionResult AcceptOrder(int id)
        {
            Order order = _db.Orders.Where(x => x.Id == id).FirstOrDefault();
            order.Realized = true;
            _db.SaveChanges();
            return RedirectToAction("AdminPanel");

        }
        [HttpGet]
        public PartialViewResult AddToCart(int id)
        {
            Dictionary<int, int> cart = new Dictionary<int, int>();
            if (Session["Cart"] == null)
            {
                Dish dbDish = _db.Dishes.Where(x => x.Id == id).FirstOrDefault();
                if (dbDish != null)
                {
                    cart.Add(dbDish.Id, 1);
                }
                Session["Cart"] = cart;
            }
            else
            {
                cart = Session["Cart"] as Dictionary<int, int>;
                Dish dbDish = _db.Dishes.Where(x => x.Id == id).FirstOrDefault();
                if (dbDish != null)
                {
                    if (cart.ContainsKey(dbDish.Id))
                    {
                        cart[dbDish.Id]++;
                    }
                    else
                    {
                        cart.Add(dbDish.Id, 1);
                    }
                }
                Session["Cart"] = cart;
            }
            Dictionary<Dish, int> dishes = new Dictionary<Dish, int>();

            foreach (KeyValuePair<int, int> item in cart)
            {
                dishes.Add(_db.Dishes.Where(x => x.Id == item.Key).FirstOrDefault(), item.Value);
            }

            ViewBag.CartItems = dishes;
            return PartialView("_PartialViewCart");
        }

        public ActionResult Index()
        {
            return View(_db.Websites.ToList());
        }

        [HttpGet]
        public ActionResult Ordered()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Order()
        {
            ViewBag.IsCartAvailable = (Session["Cart"] == null) ? false : true;
            return View();
        }

        [HttpPost]
        public ActionResult Order(OrderFormViewModel model)
        {
            ViewBag.IsCartAvailable = (Session["Cart"] == null) ? false : true;

            if (ModelState.IsValid)
            {
                if (Session["Cart"] != null)
                {
                    Dictionary<int, int> cart = Session["Cart"] as Dictionary<int, int>;
                    Order newOrder = new Order { Name = model.Name, Surname = model.Surname, Address = model.Address, Orders = new List<OrderedDish>() };

                    foreach (KeyValuePair<int, int> entry in cart)
                    {
                        newOrder.Orders.Add(new OrderedDish { Dish = _db.Dishes.Where(x => x.Id == entry.Key).FirstOrDefault(), Quantity = entry.Value });
                    }
                    decimal sum = 0;
                    foreach (var item in newOrder.Orders)
                    {
                        sum += item.Dish.Price * item.Quantity;
                    }
      
                    newOrder.Price = sum;
                    _db.Orders.Add(newOrder);
                    _db.SaveChanges();

                    Session["Cart"] = null;

                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult ClearCart()
        {
            Session["Cart"] = null;
            return PartialView("_PartialViewCart");
        }

    }
}
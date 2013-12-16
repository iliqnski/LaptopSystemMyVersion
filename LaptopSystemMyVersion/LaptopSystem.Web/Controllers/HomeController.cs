using LaptopSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaptopSystem.Web.Controllers
{
    public class HomeController : BaseController
    {
        //[OutputCache(Duration=60*60)]
        public ActionResult Index()
        {
            var listOfLaptops = this.Data.Laptops
                .OrderByDescending(x => x.Votes.Count())
                .Take(6)
                .Select(x => new LaptopViewModel
                {
                    Id = x.Id,
                    Manufacturer = x.Manufacturer.Name,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price,
                    Model = x.Model
                });
            return View(listOfLaptops);
        }
    }
}
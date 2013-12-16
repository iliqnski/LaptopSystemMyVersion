using LaptopSystem.Models;
using LaptopSystem.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net.Http;

namespace LaptopSystem.Web.Controllers
{
    public class LaptopsController : BaseController
    {
        const int PageSize = 5;

        private IQueryable<LaptopViewModel> GetAllLaptops()
        {
            var laptops = this.Data.Laptops.Select(x => new LaptopViewModel()
            {
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                Manufacturer = x.Manufacturer.Name,
                Model = x.Model,
                Price = x.Price
            }).OrderBy(x => x.Id);

            return laptops;
        }

        public ActionResult Details(int id)
        {
            var laptopDetailsViewModel = this.Data.Laptops
                .Where(x => x.Id == id)
                .Select(x => new LaptopDetailsViewModel()
                {
                    Id = x.Id,
                    AdditionalParts = x.AdditionalParts,
                    Comments = x.Comments.Select(y => new CommentViewModel()
                    {
                        AuthorUsername = y.Author.UserName,
                        Content = y.Content
                    }),
                    Description = x.Description,
                    HardDiskSize = x.HardDiskSize,
                    ImageUrl = x.ImageUrl,
                    Manufacturer = x.Manufacturer.Name,
                    Model = x.Model,
                    MonitorSize = x.MonitorSize,
                    Price = x.Price,
                    RamMemorySize = x.RamMemorySize,
                    Weight = x.Weight
                }).FirstOrDefault();

            return View(laptopDetailsViewModel);
        }

        public ActionResult List(int? id)
        {
            int pageNumber = id.GetValueOrDefault(1);
            var viewModel = 
                GetAllLaptops()
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize);

            ViewBag.Pages = Math.Ceiling((double)GetAllLaptops().Count() / PageSize);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult PostComment(SubmitCommentModel commentModel)
        {
            if (ModelState.IsValid)
            {
                this.Data.Comments.Add(new Comment()
                {
                    AuthorId = this.User.Identity.GetUserId(),
                    Content = commentModel.Comment,
                    LaptopId = commentModel.LaptopId
                });

                this.Data.SaveChanges();

                var viewModel = new CommentViewModel()
                {
                    AuthorUsername = this.User.Identity.GetUserName(),
                    Content = commentModel.Comment
                };
                return PartialView("_CommentPartial", viewModel);
            }
            else
            {
                return new HttpStatusCodeResult
                    (System.Net.HttpStatusCode.BadRequest, 
                    ModelState.Values.First().ToString());
            }
        }
    }
}
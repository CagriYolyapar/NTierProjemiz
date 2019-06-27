using Project.BLL.RepositoryPattern.ConcRep;
using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Admin.Controllers
{
    //[AdminAuthentication]
    public class CategoryController : Controller
    {
        CategoryRepository crep;
        public CategoryController()
        {
            crep = new CategoryRepository();
        }
        // GET: Admin/Category
        public ActionResult CategoryList()
        {
            return View();
        }

        public ActionResult CategoryDetail(int id)
        {
            return View(crep.GetByID(id));
        }

        public ActionResult AddCategory()
        {
            return View( new Category());
        }

        [HttpPost]
        public ActionResult AddCategory(Category item)
        {
            
            crep.Add(item);
            return RedirectToAction("ProductList");
        }
        public ActionResult DeleteCategory(int id)
        {
            crep.Delete(crep.GetByID(id));

            return RedirectToAction("ProductList");
        }

        public ActionResult UpdateCategory(int id)
        {
            return View( crep.GetByID(id));
        }

        [HttpPost]
        public ActionResult UpdateProduct(Category item)
        {
           
            crep.Update(item);
            return RedirectToAction("ProductList");
        }
    }
}
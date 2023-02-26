
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.ProductTestWork.Core.Class;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Shop.ProductTestWork.Core.Interface;

namespace Shop.ProductTestWork.Controllers
{
    public class HomeController: Controller
    {
        private ShopDataDb db = new ShopDataDb ();

        public ActionResult Index()
        {
            return View(db.ProductImage);
        }

        public ActionResult Create()
        {
            return View();
        }


    
    }
}

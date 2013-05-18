using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Navasthala.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Bhajras()
        {
            return View("Bhajras");
        }

    }
}

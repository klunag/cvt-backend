﻿using System.Web.Http;
using System.Web.Mvc;

namespace BCP.CVT.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        //public string Index()
        //{
        //    string API = "BCP-CVT-API";

        //    return API;
        //}
    }
}

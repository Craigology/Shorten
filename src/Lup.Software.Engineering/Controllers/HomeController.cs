﻿namespace Lup.Software.Engineering.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        [Route("error")]
        public IActionResult Error()
        {
            return this.View();
        }
    }
}

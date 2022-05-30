using Microsoft.AspNetCore.Mvc;
using TechDrum.Core.Constants;
using TechDrum.Web.Models;
using Serilog;
using System;
using System.Diagnostics;

namespace TechDrum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(IServiceProvider serviceProvider)
        {
            _logger = Log.Logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
                return View();
        }
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet("/version")]
        public IActionResult GetVersion()
        {
            var version = typeof(Program).Assembly.GetName().Version?.ToString();
            _logger.Information($"Version = {version}");
            return Ok(version);
        }
    }
}
using DepotSneakers.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepotSneakers.Controllers
{
    public class AkunController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AkunController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Login()
        {            
            return View();
        }
    }
}

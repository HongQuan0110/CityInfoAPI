using CityInfoAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Controllers
{
    public class DummyController : Controller
    {
        private readonly CityInfoContext _cityInfoContext;

        public DummyController(CityInfoContext cityInfoContext)
        {
            _cityInfoContext = cityInfoContext;
        }

        [HttpGet("/api/testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }            
    }
}

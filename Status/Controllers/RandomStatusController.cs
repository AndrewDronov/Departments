using System;
using Microsoft.AspNetCore.Mvc;
using Status.Models;

namespace Status.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RandomStatusController : ControllerBase
    {
        [HttpGet]
        public DepartmentStatus Get()
        {
            var values = Enum.GetValues(typeof(DepartmentStatus));
            var random = new Random();

            return (DepartmentStatus) values.GetValue(random.Next(values.Length))!;
        }
    }
}
using FullStackTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace FullStackTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            var json = new WebClient().DownloadString("./data.json");
            var people = JsonConvert.DeserializeObject<List<PersonDataModel>>(json);

            return View(people);
        }

        [HttpPost]
        public IActionResult Index(String actionBtn, IEnumerable<int> peopleChecked, PersonDataModel prsObj)
        {
            var json = new WebClient().DownloadString("./data.json");
            var people = JsonConvert.DeserializeObject<List<PersonDataModel>>(json);

            if (actionBtn == "Remove")
            {
                foreach (int CheckedId in peopleChecked)
                {
                    var item = people.SingleOrDefault(x => x.id == CheckedId);

                    people.Remove(item);
                }

                var convertedJson = JsonConvert.SerializeObject(people, Formatting.Indented);

                using (var writer = new StreamWriter("./data.json", false))
                {
                    writer.Write(convertedJson);

                }

            }
            else
            {
                

                prsObj.id = people[people.Count - 1].id + 1;

                people.Add(prsObj);

                var convertedJson = JsonConvert.SerializeObject(people, Formatting.Indented);

                using (var writer = new StreamWriter("./data.json", false))
                {
                    writer.Write(convertedJson);

                }
            }

            return View(people);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}

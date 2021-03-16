using FullStackTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        //On inital load Index with no parameters gets called
        public IActionResult Index()
        {
            //Get the json file and Deserialize it to a List of PersonDataModels
            var json = new WebClient().DownloadString("./data.json");
            var people = JsonConvert.DeserializeObject<List<PersonDataModel>>(json);

            //Return the list of people
            return View(people);
        }

        //When add or remove button is clicked this gets called
        [HttpPost]
        public IActionResult Index(String actionBtn, IEnumerable<int> peopleChecked, PersonDataModel prsObj)
        {
            //Read the information from the json file
            var json = new WebClient().DownloadString("./data.json");
            var people = JsonConvert.DeserializeObject<List<PersonDataModel>>(json);

            //Check which button was clicked
            if (actionBtn == "Remove")
            {
                //Loop through the CheckedId that are going to be removed
                foreach (int CheckedId in peopleChecked)
                {
                    //Find the mathching ids that were checked off in the json data
                    var item = people.SingleOrDefault(x => x.id == CheckedId);

                    //Remove the items that were checked off
                    people.Remove(item);
                }

                //Make new json from list with items removed
                var convertedJson = JsonConvert.SerializeObject(people, Formatting.Indented);

                //Save the new json file and overwrite the old one
                using (var writer = new StreamWriter("./data.json", false))
                {
                    writer.Write(convertedJson);

                }

            }
            else
            {
                //Get the total items in the table and add one for new id
                prsObj.id = people[people.Count - 1].id + 1;
                //Add the new person to the list
                people.Add(prsObj);
                //Make new json from list with new item added
                var convertedJson = JsonConvert.SerializeObject(people, Formatting.Indented);
                //Save the new json file and overwrite the old one
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

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ordering_Numbers.Data;
using Ordering_Numbers.Models;
using Ordering_Numbers.ViewModels;
using System.Diagnostics;


namespace Ordering_Numbers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OrderingNumbersDbContext _context;

        public HomeController(ILogger<HomeController> logger, OrderingNumbersDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SortAscending([Bind("inputList")] string inputList)
        {
            if (inputList == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                // Sort user's list
                NumbersList userList = StringToNumbersList(inputList);
                userList.SortAscending();

                // Update DB
                _context.Add(userList);
                _context.SaveChanges();

                // send list info to view
                return View(new SortingDetails
                {
                    sortedList = NumbersListToString(userList),
                    orderType = userList.OrderType,
                    orderingTimeMilliseconds = userList.OrderingTimeMilliseconds,
                    errorMessage = null,
                    sortedListJson = JsonConvert.SerializeObject(userList)

                });
            }
            catch (Exception ex)
            {
                return View(new SortingDetails
                {
                    errorMessage = ex.Message
                });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SortDescending([Bind("inputList")] string inputList)
        {
            if (inputList == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                // Sort user's list
                NumbersList userList = StringToNumbersList(inputList);
                userList.SortDescending();

                // Update DB
                _context.Add(userList);
                _context.SaveChanges();

                // send list info to view
                return View(new SortingDetails
                {
                    sortedList = NumbersListToString(userList),
                    orderType = userList.OrderType,
                    orderingTimeMilliseconds = userList.OrderingTimeMilliseconds,
                    errorMessage = null,
                    sortedListJson = JsonConvert.SerializeObject(userList)
                });
            }
            catch (Exception ex)
            {
                return View(new SortingDetails
                {
                    errorMessage = ex.Message
                });
            }
        }


        private NumbersList StringToNumbersList(string str)
        {
            // input string to NumbersList
            str = str.Trim();
            List<string> strList = str.Split(',').ToList();

            NumbersList outputList = new NumbersList();
            foreach (string s in strList)
            {
                outputList.Numbers.Add(new Number(Int32.Parse(s.Trim())));
            }
            return outputList;
        }

        private string NumbersListToString(NumbersList numbersList)
        {
            string outputString = "";
            foreach (Number number in numbersList.Numbers)
            {
                outputString += number.Value.ToString();
                if(number != numbersList.Numbers.Last()) outputString += ", ";
            }
            return outputString;
        }
    }
}
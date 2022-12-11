using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Ordering_Numbers.Data;
using Ordering_Numbers.Models;
using Ordering_Numbers.ViewModels;

namespace Ordering_Numbers.Controllers
{
    
    public class NumbersListsController : Controller
    { 
        private readonly OrderingNumbersDbContext _context;

        public NumbersListsController(OrderingNumbersDbContext context)
        {
            _context = context;
        }

        // GET: NumbersLists
        public async Task<IActionResult> Index()
        {
              return View(await _context.NumbersLists.ToListAsync());
        }

        // GET: NumbersLists/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.NumbersLists == null)
            {
                return NotFound();
            }

            // load list from DB
            NumbersList numbersList = _context.NumbersLists.Where(m => m.Id == id).Include(x => x.Numbers).Single();

            // Make sure it's sorted as the user expects
            switch (numbersList.OrderType)
            {
                case "Ascending":
                    numbersList.SortAscending();
                    break;
                case "Descending":
                    numbersList.SortDescending();
                    break;
                default:
                    break;
            }

            if (numbersList == null)
            {
                return NotFound();
            }

            return View(new SortingDetails
            {
                sortedList = NumbersListToString(numbersList),
                orderType = numbersList.OrderType,
                orderingTimeMilliseconds = numbersList.OrderingTimeMilliseconds,
                sortedListJson = JsonConvert.SerializeObject(numbersList)
            });
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
                if (number != numbersList.Numbers.Last()) outputString += ", ";
            }
            return outputString;
        }

        // GET: NumbersLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NumbersLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderType,OrderingTimeMilliseconds")] NumbersList numbersList)
        {
            if (ModelState.IsValid)
            {
                numbersList.Id = Guid.NewGuid();
                _context.Add(numbersList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(numbersList);
        }

        // GET: NumbersLists/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.NumbersLists == null)
            {
                return NotFound();
            }

            var numbersList = await _context.NumbersLists.FindAsync(id);
            if (numbersList == null)
            {
                return NotFound();
            }
            return View(numbersList);
        }

        // POST: NumbersLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,OrderType,OrderingTimeMilliseconds")] NumbersList numbersList)
        {
            if (id != numbersList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(numbersList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NumbersListExists(numbersList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(numbersList);
        }

        // GET: NumbersLists/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.NumbersLists == null)
            {
                return NotFound();
            }

            var numbersList = await _context.NumbersLists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (numbersList == null)
            {
                return NotFound();
            }

            return View(numbersList);
        }

        // POST: NumbersLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.NumbersLists == null)
            {
                return Problem("Entity set 'OrderingNumbersDbContext.NumbersLists'  is null.");
            }
            var numbersList = await _context.NumbersLists.FindAsync(id);
            if (numbersList != null)
            {
                _context.NumbersLists.Remove(numbersList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NumbersListExists(Guid id)
        {
          return _context.NumbersLists.Any(e => e.Id == id);
        }
    }
}

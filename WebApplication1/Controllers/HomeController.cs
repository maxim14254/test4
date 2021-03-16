
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {


        readonly UsersContext db;
        public HomeController(UsersContext context)
        {
            this.db = context;
            if (db.Сurrencies.Count() == 0)
            {
                var getJson = new GetJsom().Get("https://www.cbr-xml-daily.ru/daily_json.js").Result;

                var jTokens = getJson["Valute"].Children().ToList();

                for (int i = 0; i < jTokens.Count; i++)
                {
                    db.Сurrencies.Add(new Сurrencies { Name = jTokens[i].First().ToObject<Valute>().Name, Currency = new Currency { Value = jTokens[i].First().ToObject<Valute>().Value } });
                }
            }

            db.SaveChanges();

        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 4;

            IQueryable<Сurrencies> source = db.Сurrencies.Include(x => x.Currency);
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            
            if(page > pageViewModel.TotalPages)
            {
                return RedirectToAction("Error", "Home");
            }
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Сurrencies = items
            };
            return View(viewModel);
        }
        public IActionResult Currency(string Name)
        {
            try
            {
                Сurrencies currency = new Сurrencies { Currency = db.Сurrency.Where(s => s.Id == db.Сurrencies.Where(s => s.Name.Contains(Name)).First().Currency.Id).First(), Name = Name };
                return View(currency);
            }
            catch (System.InvalidOperationException)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

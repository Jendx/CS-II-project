using ASP_NET_Projekt_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Datalayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;

namespace ASP_NET_Projekt_2.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        #region INDEX
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Index(LoginForm form)
        {

            if (ModelState.IsValid)
            {
                var users = await JSONAPI.GetUsers();

                foreach(var user in users)
                {
                    if(user.Username == form.Name && user.Password == form.Password)
                    {
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "User Id"));
                        identity.AddClaim(new Claim(ClaimTypes.Name, "User Name"));
                        //ViewBag.User = user.Username;
                        ViewData["username"] = "Home Page";
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        HttpContext.Session.SetString("Username", user.Username);
                        return RedirectToAction("Home");
                    }
                }
            }
            return View();
        }
        #endregion

        public IActionResult Home()
        {
            return View();
        }

        public async Task<IActionResult> Receipts()
        {
            JSONAPI js = new JSONAPI();
            var uctenky = await js.GetUctenky();
            ViewBag.uctenky = uctenky;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Receipts(string data)
        {
            ViewBag.uctenky = await JSONAPI.Delete(int.Parse(data));

            return View();
        }

        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> Save() // From adding new one
        {
            JSONAPI js = new JSONAPI();
            List<Polozka> p = new List<Polozka>();
            object d;

            List<AddReceiptForm> Newdata = new List<AddReceiptForm>();
            if (TempData.TryGetValue("data", out d))
            {
                Newdata = JsonSerializer.Deserialize<List<AddReceiptForm>>((string)d);
            }
            TempData.Clear();

            foreach (var item in Newdata)
            {
                p.Add(new Polozka() 
                { 
                    id = int.Parse(item.PC), 
                    Cena = float.Parse(item.Price),
                    Nazev = item.Name, pocet = int.Parse(item.Count),
                    vaha = int.Parse(item.Weight) });
            }

            
            await js.save(p);

            //JSONAPI.delete(int.Parse(data));
            return RedirectToAction("Receipts");
        }

        [HttpPost]
        public async Task<IActionResult> Detail(string data)
        {
            JSONAPI js = new JSONAPI();
            var uctenky = await js.GetUctenky();
            ViewBag.uctenky = uctenky[int.Parse(data)];
            ViewData["Uctenka"] = data;
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");
        } 

        
        public async Task<IActionResult> AddReceipt()
        {
            JSONAPI JSON = new JSONAPI();
            ViewBag.data = await JSON.GetItems();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddReceipt(AddReceiptForm form)
        {
            JSONAPI JSON = new JSONAPI();
            ViewBag.data = await JSON.GetItems();

            List<AddReceiptForm> p = new List<AddReceiptForm>();
            object d;

            if (form.PC.Contains('#'))
            {
                var ToRemove = form.PC.Split(' ');

                if (TempData.TryGetValue("data", out d))
                {
                    p = JsonSerializer.Deserialize<List<AddReceiptForm>>((string)d);
                }
                p.RemoveAt(int.Parse(ToRemove[1]));
                
            }
            else
            {
                if (ModelState.IsValid)
                {
                    
                    if (TempData.TryGetValue("data", out d))
                    {
                        p = JsonSerializer.Deserialize<List<AddReceiptForm>>((string)d);
                    }
                    p.Add(form);
                    
                }
            }

            TempData["data"] = JsonSerializer.Serialize(p);
            TempData.Keep("data");
            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        public HomeController(MyContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    newUser.Password = hasher.HashPassword(newUser, newUser.Password);
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    return Redirect("/");
                }
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost("login")]
        public IActionResult Login(LoginUser userInfo)
        {
            if(ModelState.IsValid)
            {
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == userInfo.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "This email has not been registered.");
                    return View("Index");
                }
                else
                {
                    var hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(userInfo, userInDb.Password, userInfo.Password);
                    if(result == 0)
                    {
                        ModelState.AddModelError("Password", "Password is incorrect.");
                        return View("Index");
                    }
                    else
                    {
                        User userLoggedIn = _context.Users.FirstOrDefault(u => u.Email == userInfo.Email);
                        HttpContext.Session.SetInt32("UserId", userLoggedIn.UserId);
                        return RedirectToAction("Dashboard");
                    }
                }
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? userInSessions = HttpContext.Session.GetInt32("UserId");
            if(userInSessions == null)
            {
                return Redirect("/");
            }
            else
            {
                User user = _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
                ViewBag.User = user;
                List<Wedding> weddings = _context.Weddings.Include(w => w.Planner).Include(w => w.Guests).ThenInclude(g => g.User).ToList();
                return View(weddings);
            }
        }
        [HttpGet("rsvp/{weddingId}/{userId}")]
        public IActionResult RSVP(int weddingId, int userId)
        {
            Invite rsvp = new Invite();
            rsvp.WeddingId = weddingId;
            rsvp.UserId = userId;
            _context.Invites.Add(rsvp);
            _context.SaveChanges();
            return Redirect("/dashboard");
        }
        [HttpGet("rsvp/undo/{weddingId}/{userId}")]
        public IActionResult UnRSVP(int weddingId, int userId)
        {
            Invite unrsvp = _context.Invites.FirstOrDefault(i => i.UserId == userId && i.WeddingId == weddingId);
            _context.Invites.Remove(unrsvp);
            _context.SaveChanges();
            return Redirect("/dashboard");
        }
        [HttpGet("delete/{weddingId}")]
        public IActionResult Delete(int weddingId)
        {
            Wedding delete = _context.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
            _context.Weddings.Remove(delete);
            _context.SaveChanges();
            return Redirect("/dashboard");
        }
        [HttpGet("wedding/new")]
        public IActionResult NewWedding()
        {
            return View();
        }
        [HttpPost("wedding/new/create")]
        public IActionResult CreateWedding(Wedding newWedding)
        {
            if(ModelState.IsValid)
            {
                newWedding.UserId = (int)HttpContext.Session.GetInt32("UserId");
                _context.Weddings.Add(newWedding);
                _context.SaveChanges();
                return Redirect($"/wedding/info/{newWedding.WeddingId}");
            }
            else
            {
                return View("NewWedding");
            }
        }
        [HttpGet("wedding/info/{weddingId}")]
        public IActionResult WeddingInfo(int weddingId)
        {
            Wedding wedding = _context.Weddings.Include(w => w.Planner).Include(w => w.Guests).ThenInclude(g => g.User).FirstOrDefault(w => w.WeddingId == weddingId);
            return View(wedding);
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
    }
}

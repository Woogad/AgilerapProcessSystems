using AgilerapProcessSystems.Common;
using AgilerapProcessSystems.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

namespace AgilerapProcessSystems.Controllers
{
    public class HomeController : Controller
    {
        private Agile_Process_SystemsContext db = new Agile_Process_SystemsContext();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            if(HttpContext.Session.GetString(Agile.SessionName.UserSession.ToString()) != null)
            {
                return RedirectToAction("Index", "Work");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            try
            {
                //! Handle Error async Session and DB
                await HttpContext.Session.LoadAsync();
                User? userDB = await db.User.Where(s => s.Email == user.Email).FirstOrDefaultAsync();

                //! If not found user by email retrun fail
                if (userDB == null)
                {
                    ViewBag.FailMes = "Wrong Email or Password";
                    return View();
                }

                //! Verify Password
                bool isPassVerify = BCrypt.Net.BCrypt.EnhancedVerify(user.Password, userDB.Password);

                //! If Passwords is not match return fail
                if (!isPassVerify)
                {
                    ViewBag.FailMes = "Wrong Email or Password";
                    return View();
                }

                //! Set name of session and give value to that sassion
                HttpContext.Session.SetString(Agile.SessionName.UserSession.ToString(), userDB.Email);
                return RedirectToAction("Index", "Work");
            }
            catch (Exception ex)
            {
                //! if Error will sent Faill Message to view
                ViewBag.FailMes = "Error Try Agin!";
                Console.WriteLine("Err: " + ex);
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            try
            {
                //! remove UserSession from Session
                await HttpContext.Session.LoadAsync();
                HttpContext.Session.Remove(Agile.SessionName.UserSession.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Err: " + ex);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (user.Password != user.ConfirmPassword)
            {
                ViewBag.FailMes = "Password and Confirm Password is Unequal";
                return View(user);
            }
            string hashPasswords = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password);
            user.Password = hashPasswords;
            user.CreateDate = DateTime.Now;
            user.UpdateDate = DateTime.Now;
            user.IsDelete = false;

            await db.User.AddAsync(user);
            await db.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

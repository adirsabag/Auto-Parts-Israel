using HW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HW2.Dal;
using System.Web.Helpers;
using System.Threading;
using HW2.ModelView;

namespace HW2.Controllers
{
    public class UserController : Controller
    {
        // users list
        public UserDal dalUs = new UserDal();

        [NoDirectAccess] // prevents direct access from browser url to specific actions (in FilterConfig.cs)
        public ActionResult UserRegister()
        {
            return View();
        }

        [NoDirectAccess]
        public ActionResult UserLogin(User us)
        {
            return View();
        }

        // user register funcion
        [HttpPost]
        [NoDirectAccess]
        public ActionResult SubmitRegister(User us)
        {
            bool pass = true;

            // empty fields messages
            if (us.FirstName == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter first name');</script>";
                return View("UserRegister", us);
            }
            if (us.LastName == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter last name');</script>";
                return View("UserRegister", us);
            }
            if (us.Username == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter username');</script>";
                return View("UserRegister", us);
            }
            if (us.Password == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter password');</script>";
                return View("UserRegister", us);
            }
            if (us.Email == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter email');</script>";
                return View("UserRegister", us);
            }

            // validating password
            if (us.Password.Length >= 6 && us.Password.Any(char.IsUpper) && us.Password.Any(char.IsSymbol))
            {
                pass = true;
            }
            else
            {
                pass = false;
            }

            if (pass)
            {
                if (!dalUs.Users.Any(x => x.Username == us.Username))
                {
                    string oldPass = us.Password;
                    us.Password = Hash.PassHash(oldPass); // encrypt the password before storing it in the database
                    dalUs.Users.Add(us);
                    dalUs.SaveChanges();

                    return RedirectToAction("Welcome", "Home");
                }
                else
                {
                    TempData["UsernameExists"] = "<script>alert('This username already exists');</script>";
                }
            }
            else
            {
                TempData["Pass"] = "<script>alert('Password must have at least 6 characters and contain at least one upper-case letter and one symbol');</script>";
            }

            return View("UserRegister", us);
        }

        // user login function
        [NoDirectAccess]
        public ActionResult SubmitLogin(User us)
        {
            // empty fields messages
            if (us.Username == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter username');</script>";
                return View("UserLogin", us);
            }
            if (us.Password == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter password');</script>";
                return View("UserLogin", us);
            }

            if (dalUs.Users.Any(x => x.Username == us.Username))
            {
                var z = dalUs.Users.Where(x => x.Username == us.Username).Select(x => x.Password).Single();
                string s = z.ToString();
                bool decrypt = Hash.PassVerify(us.Password, s);// password decryption

                if (decrypt)
                {
                    Session["IsAdmin"] = "False";
                    Session["User"] = us.Username;

                    return RedirectToAction("Welcome", "Home");
                }
                else
                {
                    TempData["wrong1"] = "<script>alert('Wrong password');</script>";

                    return View("UserLogin", us);
                }
            }
            else
            {
                TempData["wrong2"] = "<script>alert('Wrong username');</script>";

                return View("UserLogin", us);
            }
        }

        [NoDirectAccess]
        public ActionResult UserSignout()
        {
            Session["User"] = null;

            return RedirectToAction("Welcome", "Home");
        }

        [NoDirectAccess]
        public ActionResult UserTable()
        {
            return View(dalUs.Users.ToList());
        }

    }
}
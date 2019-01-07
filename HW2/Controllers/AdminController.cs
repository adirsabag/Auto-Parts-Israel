using HW2.Dal;
using HW2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HW2.Controllers
{
    public class AdminController : Controller
    {
        // Admins list
        public AdminDal dal = new AdminDal();
        public string AdmCode = "Adm2019"; // special code given to an admin in order to register

        [NoDirectAccess] // prevents direct access from browser url to specific actions (in FilterConfig.cs)
        public ActionResult AdminRegister()
        {
            return View();
        }

        [NoDirectAccess]
        public ActionResult AdminLogin(Admin ad)
        {
            return View();
        }

        // admin register function
        [HttpPost]
        [NoDirectAccess]
        public ActionResult SubmitRegister(Admin ad)
        {
            bool pass = true;

            // empty fields messages
            if (ad.AdminFirstName == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter first name');</script>";
                return View("AdminRegister", ad);
            }
            if (ad.AdminLastName == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter last name');</script>";
                return View("AdminRegister", ad);
            }
            if (ad.AdminUsername == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter username');</script>";
                return View("AdminRegister", ad);
            }
            if (ad.AdminPassword == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter password');</script>";
                return View("AdminRegister", ad);
            }
            if (ad.AdminEmail == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter email');</script>";
                return View("AdminRegister", ad);
            }
            if (ad.AdminCode == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter admin code');</script>";
                return View("AdminRegister", ad);
            }

            // validating password
            if (ad.AdminPassword.Length >= 6 && ad.AdminPassword.Any(char.IsUpper) && ad.AdminPassword.Any(char.IsSymbol))
            {
                pass = true;
            }
            else
            {
                pass = false;
            }

            if (pass)
            {
                if (!dal.Admins.Any(x => x.AdminUsername == ad.AdminUsername))
                {
                    if (ad.AdminCode == AdmCode)
                    {
                        string oldPass = ad.AdminPassword;
                        ad.AdminPassword = Hash.PassHash(oldPass); // encrypt the password before storing it in the database
                        dal.Admins.Add(ad);
                        dal.SaveChanges();

                        return RedirectToAction("Welcome", "Home");
                    }
                    else
                    {
                        TempData["AdmCode"] = "<script>alert('Wrong admin registration code');</script>";
                    }
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

            return View("AdminRegister", ad); 
        }

        // admin login function
        [NoDirectAccess]
        public ActionResult SubmitLogin(Admin ad)
        {
            // empty fields messages
            if (ad.AdminUsername == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter username');</script>";
                return View("AdminLogin", ad);
            }
            if (ad.AdminPassword == null)
            {
                TempData["EmptyField"] = "<script>alert('Please enter password');</script>";
                return View("AdminLogin", ad);
            }

            if (dal.Admins.Any(x => x.AdminUsername == ad.AdminUsername))
            {
                var z = dal.Admins.Where(x => x.AdminUsername == ad.AdminUsername).Select(x => x.AdminPassword).Single();
                string s = z.ToString();
                bool decrypt = Hash.PassVerify(ad.AdminPassword, s); // password decryption

                if (decrypt)
                {
                    Session["User"] = ad.AdminUsername;
                    Session["IsAdmin"] = "True";

                    return RedirectToAction("AdminPriv");
                }
                else
                {
                    TempData["wrong3"] = "<script>alert('Wrong password');</script>";

                    return View("AdminLogin", ad);
                }
            }
            else
            {
                TempData["wrong4"] = "<script>alert('Wrong username');</script>";

                return View("AdminLogin", ad);
            }  
        }

        [NoDirectAccess]
        public ActionResult AdminSignout()
        {
            Session["User"] = null;
            Session["IsAdmin"] = "False";

            return RedirectToAction("Welcome", "Home");
        }

        // admin special privileges page
        [NoDirectAccess]
        public ActionResult AdminPriv()
        {
            return View();
        }
    }
}
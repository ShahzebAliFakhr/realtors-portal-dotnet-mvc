using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Realtors_Portal.Models;

namespace Realtors_Portal.Controllers
{
    public class HomeController : Controller
    {

        rolesManager roles = new rolesManager();
        RP_realtorsPortalEntities RPE = new RP_realtorsPortalEntities();

        Random rand = new Random();

        // GET: Home
        public ActionResult Index()
        {
            try
            {
                //Cities
                ViewBag.cities = RPE.RP_cities.ToList();
                ViewBag.popularcities = RPE.RP_cities.ToList().Take(9);

                //Property
                ViewBag.propertyTypes = RPE.RP_propertyTypes.ToList();
                ViewBag.checkProperties = RPE.RP_property.Where(x => x.p_status == "Active").Count();
                ViewBag.properties = RPE.RP_property.OrderByDescending(x => x.p_id).Take(9);

                //Agents
                ViewBag.checkAgent = RPE.RP_users.Where(x => x.u_role == 3).Count();
                ViewBag.agents = RPE.RP_users.Where(x => x.u_role == 3).ToList().Take(6);
                ViewBag.agentRole = RPE.RP_users.Where(x => x.u_role == 3).Single().RP_userRoles.r_name;
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(RP_users usr)
        {

            if (usr.u_username == null || usr.u_password == null)
            {
                ViewBag.Error = "Please Enter Username/Password First.";
            }
            else
            {
                try
                {
                    int _flag = RPE.RP_users.Where(x => x.u_username == usr.u_username && x.u_password == usr.u_password).Count();
                    if (_flag > 0)
                    {
                        string[] role = roles.GetRolesForUser(usr.u_username);
                        var user_status = RPE.RP_users.Where(x => x.u_username == usr.u_username).Single().u_status;

                        if (user_status == "Active")
                        {
                            FormsAuthentication.SetAuthCookie(usr.u_username, false);

                            var user_name = RPE.RP_users.Where(x => x.u_username == usr.u_username).Single().u_name;
                            var user_id = RPE.RP_users.Where(x => x.u_username == usr.u_username).Single().u_id;

                            Session["user"] = user_name;
                            Session["usrID"] = user_id;

                            if (role[0] == "Admin")
                            {
                                return RedirectToAction("Dashboard", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("UserProfile", "User");
                            }
                        }
                        else
                        {
                            ViewBag.Error = "Your Account is not Activated.. Please Wait.";
                        }

                    }
                    else
                    {
                        ViewBag.Error = "Invalid Username/Password.";
                    }

                }
                catch (Exception ex)
                {
                    this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("login", "Home");
        }

        public ActionResult Register()
        {
            ViewBag.cities = RPE.RP_cities.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Register(RP_users usr)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (usr.u_name == null || usr.u_username == null || usr.u_email == null || usr.u_password == null || usr.u_phone == null)
                    {
                        ViewBag.Error = "All Fields are required.";
                    }
                    else if (usr.u_username.Contains(" "))
                    {
                        ViewBag.Error = "Username can'be more than one word.";
                    }
                    else
                    {
                        int token = rand.Next(100000, 1000000);
                        RPE.RP_users.Add(new RP_users { u_role = 2, u_name = usr.u_name, u_username = usr.u_username, u_email = usr.u_email, u_password = usr.u_password, u_phone = usr.u_phone, u_city = usr.u_city, u_status = "Pending", u_token = token, u_created = DateTime.Now, u_image = "default.jpg", u_membership = 1 });
                        RPE.SaveChanges();
                        ViewBag.Success = "You've Signup Successfully.";
                        return RedirectToAction("Login", "Home");
                    }
                }
                catch (Exception ex)
                {
                    this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
                }
            }

            return View();
        }

        public JsonResult CheckUsernameAvailability(string userdata)
        {
            var SeachData = RPE.RP_users.Where(x => x.u_name == userdata).SingleOrDefault();
            if (SeachData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }

        public JsonResult CheckEmailAvailability(string userdata)
        {
            var SeachData = RPE.RP_users.Where(x => x.u_email == userdata).SingleOrDefault();
            if (SeachData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }

        public JsonResult CheckPhoneAvailability(string userdata)
        {
            var SeachData = RPE.RP_users.Where(x => x.u_phone == userdata).SingleOrDefault();
            if (SeachData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }

        public ActionResult About()
        {

            //Agents
            ViewBag.checkAgent = RPE.RP_users.Where(x => x.u_role == 3).Count();
            ViewBag.agents = RPE.RP_users.Where(x => x.u_role == 3).ToList().Take(6);
            ViewBag.agentRole = RPE.RP_users.Where(x => x.u_role == 3).Single().RP_userRoles.r_name;

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult FAQs()
        {
            return View();
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Realtors_Portal.Models;

namespace Realtors_Portal.Controllers
{
    [Authorize(Roles = "Buyer, Seller")]
    public class UserController : Controller
    {

        RP_realtorsPortalEntities RPE = new RP_realtorsPortalEntities();

        // GET: User
        public ActionResult UserProfile()
        {
            Int32 usrID = (int)Session["usrID"];
            ViewBag.userID = RPE.RP_users.Where(x => x.u_id == usrID).FirstOrDefault();
            ViewBag.cities = RPE.RP_cities.ToList();
            return View(ViewBag.userID);
        }

        public ActionResult SocialProfile()
        {
            Int32 usrID = (int)Session["usrID"];
            ViewBag.userID = RPE.RP_users.Where(x => x.u_id == usrID).FirstOrDefault();
            return View(ViewBag.userID);
        }

        public ActionResult MyProperties()
        {
            Int32 usrID = (int)Session["usrID"];
            ViewBag.userProperties = RPE.RP_property.Where(x => x.p_postedBy == usrID).ToList();
            return View();
        }

        [AllowAnonymous]
        public ActionResult Preview(int id)
        {
            try
            {

                int checkUser = RPE.RP_users.Where(x => x.u_id == id).Count();

                if (checkUser > 0)
                {
                    ViewBag.checkProperties = RPE.RP_property.Where(x => x.p_postedBy == id).Count();

                    var CityID = RPE.RP_users.Where(x => x.u_id == id).Single().RP_cities.c_id;
                    ViewBag.UserID = RPE.RP_users.Where(x => x.u_id == id).FirstOrDefault();
                    ViewBag.UserRole = RPE.RP_users.Where(x => x.u_id == id).Single().RP_userRoles.r_name;
                    ViewBag.Membership = RPE.RP_users.Where(x => x.u_id == id).Single().RP_membership.m_name;
                    ViewBag.UserCity = RPE.RP_users.Where(x => x.u_id == id).Single().RP_cities.c_name;
                    ViewBag.UserState = RPE.RP_cities.Where(x => x.c_id == CityID).Single().RP_states.s_name;
                    ViewBag.UserProperties = RPE.RP_property.Where(x => x.p_postedBy == id).ToList();
                    return View();
                }
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("PageNotFound", "Home");
        }

        [HttpPost]
        public ActionResult Update(RP_users usr)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Int32 usrID = (int)Session["usrID"];
                    var userID = RPE.RP_users.Where(x => x.u_id == usrID).FirstOrDefault();
                   
                    if (usr.userImageData != null)
                    {
                        usr.u_image = "~/assets/img/user/" + "user-id-" + usrID + "-" + usr.userImageData.FileName;
                        usr.userImageData.SaveAs(Server.MapPath(usr.u_image));
                        userID.u_image = "user-id-" + usrID + "-" + usr.userImageData.FileName;
                    }

                    if (usr.u_name != null)
                    {
                        userID.u_name = usr.u_name;
                    }

                    if (usr.u_email != null)
                    {
                        userID.u_email = usr.u_email;
                    }

                    if (usr.u_phone != null)
                    {
                        userID.u_phone = usr.u_phone;
                    }

                    if (usr.u_about != null)
                    {
                        userID.u_about = usr.u_about;
                    }

                    if (usr.u_city != null)
                    {
                        userID.u_city = usr.u_city;
                    }

                    if (usr.u_password != null)
                    {
                        userID.u_password = usr.u_password;
                    }

                    if (usr.u_facebook != null)
                    {
                        userID.u_facebook = usr.u_facebook;
                    }

                    if (usr.u_twitter != null)
                    {
                        userID.u_twitter = usr.u_twitter;
                    }

                    if (usr.u_instagram != null)
                    {
                        userID.u_instagram = usr.u_instagram;
                    }

                    RPE.Entry(userID).State = EntityState.Modified;
                    RPE.SaveChanges();

                }
                catch (Exception ex)
                {
                    this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
                }
            }

            return RedirectToAction("UserProfile", "User");
        }

        [HttpPost]
        public ActionResult BecomeaSeller(RP_users usr)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Int32 usrID = (int)Session["usrID"];
                    var userID = RPE.RP_users.Where(x => x.u_id == usrID).FirstOrDefault();

                    userID.u_role = 3;

                    RPE.Entry(userID).State = EntityState.Modified;
                    RPE.SaveChanges();
                }
                catch (Exception ex)
                {
                    this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
                }
            }

            return RedirectToAction("UserProfile", "User");
        }

        public ActionResult AddProperty()
        {
            ViewBag.cities = RPE.RP_cities.ToList();
            ViewBag.propertyTypes = RPE.RP_propertyTypes.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult AddProperty(RP_property pr)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Int32 usrID = (int)Session["usrID"];
                    ViewBag.userID = RPE.RP_users.Where(x => x.u_id == usrID).FirstOrDefault();

                    string pr_image1, pr_image2, pr_image3 = null;

                    if (pr.ImageData1 != null)
                    {
                        string filename = "property-id-" + pr.p_id + "-" + pr.ImageData1.FileName;
                        pr.p_image_1 = "~/assets/img/property/" + filename;
                        pr.ImageData1.SaveAs(Server.MapPath(pr.p_image_1));
                        pr_image1 = filename;
                    }
                    else
                    {
                        pr_image1 = "default.jpg";
                    }

                    if (pr.ImageData2 != null )
                    {
                        string filename = "property-id-" + pr.p_id + "-" + pr.ImageData2.FileName;
                        pr.p_image_2 = "~/assets/img/property/" + filename;
                        pr.ImageData2.SaveAs(Server.MapPath(pr.p_image_2));
                        pr_image2 = filename;
                    }
                    else
                    {
                        pr_image2 = null;
                    }

                    if (pr.ImageData3 != null)
                    {
                        string filename = "property-id-" + pr.p_id + "-" + pr.ImageData3.FileName;
                        pr.p_image_3 = "~/assets/img/property/" + filename;
                        pr.ImageData3.SaveAs(Server.MapPath(pr.p_image_3));
                        pr_image3 = filename;
                    }
                    else
                    {
                        pr_image3 = null;
                    }

                    RPE.RP_property.Add(new RP_property { p_type = pr.p_type, p_name = pr.p_name, p_demand = pr.p_demand, p_description = pr.p_description, p_purpose = pr.p_purpose, p_area = pr.p_area, p_size = pr.p_size, p_bedrooms = pr.p_bedrooms, p_bathrooms = pr.p_bathrooms, p_floors = pr.p_floors, p_garages = pr.p_garages, p_location = pr.p_location, p_city = pr.p_city, p_availability = "Availble", p_postedBy = usrID, p_video = pr.p_video, p_image_1 = pr_image1, p_image_2 = pr_image2, p_image_3 = pr_image3, p_status = "Pending", p_created = DateTime.Now });
                    RPE.SaveChanges();
                    ViewBag.Success = "Property Added Succeessfully.";
                    
                }
                catch (Exception ex)
                {
                    this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
                }
            }

            return RedirectToAction("Myproperties", "User");
        }
        
        public ActionResult UpdateProperty(int id)
        {
            try
            {

                var uID = RPE.RP_property.Where(x => x.p_id == id).FirstOrDefault().p_postedBy;
                Int32 usrID = (int)Session["usrID"];

                if (usrID == uID)
                {

                    ViewBag.userProperties = RPE.RP_property.Where(x => x.p_id == id).FirstOrDefault();
                    ViewBag.propertyTypes = RPE.RP_propertyTypes.ToList();
                    ViewBag.cities = RPE.RP_cities.ToList();
                    return View(ViewBag.userProperties);
                }

            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("PageNotFound", "Home");
        }

        [HttpPost]
        public ActionResult UpdateProperty(RP_property pr)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var prID = RPE.RP_property.Where(x => x.p_id == pr.p_id).FirstOrDefault();

                    if (pr.ImageData1 != null)
                    {
                        string filename = "property-id-" + pr.p_id + "-" + pr.ImageData1.FileName;
                        pr.p_image_1 = "~/assets/img/property/" + filename;
                        pr.ImageData1.SaveAs(Server.MapPath(pr.p_image_1));
                        prID.p_image_1 = filename;
                    }

                    if (pr.ImageData2 != null)
                    {
                        string filename = "property-id-" + pr.p_id + "-" + pr.ImageData2.FileName;
                        pr.p_image_2 = "~/assets/img/property/" + filename;
                        pr.ImageData2.SaveAs(Server.MapPath(pr.p_image_2));
                        prID.p_image_2 = filename;
                    }

                    if (pr.ImageData3 != null)
                    {
                        string filename = "property-id-" + pr.p_id + "-" + pr.ImageData3.FileName;
                        pr.p_image_3 = "~/assets/img/property/" + filename;
                        pr.ImageData3.SaveAs(Server.MapPath(pr.p_image_3));
                        prID.p_image_3 = filename;
                    }

                    if (prID.p_name != null)
                    {
                        prID.p_name = pr.p_name;
                    }

                    prID.p_description = pr.p_description;
                    prID.p_purpose = pr.p_purpose;
                    prID.p_type = pr.p_type;
                    prID.p_location = pr.p_location;
                    prID.p_city = pr.p_city;
                    prID.p_video = pr.p_video;
                    prID.p_demand = pr.p_demand;
                    prID.p_area = pr.p_area;
                    prID.p_size = pr.p_size;
                    prID.p_bedrooms = pr.p_bedrooms;
                    prID.p_bathrooms = pr.p_bathrooms;
                    prID.p_floors = pr.p_floors;
                    prID.p_garages = pr.p_garages;
                    prID.p_availability = pr.p_availability;

                    RPE.Entry(prID).State = EntityState.Modified;
                    RPE.SaveChanges();

                }
                catch (Exception ex)
                {
                    this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
                }
            }

            return RedirectToAction("MyProperties", "User");
        }
        
        public ActionResult DeleteProperty(int id)
        {
            try
            {

                var uID = RPE.RP_property.Where(x => x.p_id == id).FirstOrDefault().p_postedBy;
                Int32 usrID = (int)Session["usrID"];

                if (usrID == uID)
                {
                    var pID = RPE.RP_property.Where(x => x.p_id == id).Single();
                    RPE.RP_property.Remove(pID);
                    RPE.SaveChanges();
                    return RedirectToAction("MyProperties", "User");
                }

            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("PageNotFound", "Home");
        }

    }
}
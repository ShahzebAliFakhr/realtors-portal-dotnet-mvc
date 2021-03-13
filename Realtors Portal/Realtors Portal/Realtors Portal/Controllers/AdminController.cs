using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Realtors_Portal.Models;

namespace Realtors_Portal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        RP_realtorsPortalEntities RPE = new RP_realtorsPortalEntities();
        Random rand = new Random();

        // GET: Admin
        public ActionResult Dashboard()
        {
            //Users
            ViewBag.CountAdmin = RPE.RP_users.Where(x => x.u_role == 1).Count();
            ViewBag.CountBuyer = RPE.RP_users.Where(x => x.u_role == 2).Count();
            ViewBag.CountSeller = RPE.RP_users.Where(x => x.u_role == 3).Count();

            //Property
            ViewBag.CountProperties = RPE.RP_property.Count();

            return View();
        }



        //Users
        public ActionResult AddUser()
        {
            ViewBag.roles = RPE.RP_userRoles.ToList();
            ViewBag.memberships = RPE.RP_membership.ToList();
            ViewBag.cities = RPE.RP_cities.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult AddUser(RP_users usr)
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

                        if (usr.userImageData != null)
                        {
                            usr.u_image = "~/assets/img/user/" + "user-id-" + usr.u_id + "-" + usr.userImageData.FileName;
                            usr.userImageData.SaveAs(Server.MapPath(usr.u_image));
                            usr.u_image = "user-id-" + usr.u_id + "-" + usr.userImageData.FileName;
                        }
                        else
                        {
                            usr.u_image = "default.jpg";
                        }

                        int token = rand.Next(100000, 1000000);
                        RPE.RP_users.Add(new RP_users { u_role = usr.u_role, u_name = usr.u_name, u_username = usr.u_username, u_email = usr.u_email, u_password = usr.u_password, u_phone = usr.u_phone, u_city = usr.u_city, u_status = usr.u_status, u_token = token, u_created = DateTime.Now, u_image = usr.u_image, u_membership = usr.u_membership, u_about = usr.u_about, u_facebook = usr.u_facebook, u_twitter = usr.u_twitter, u_instagram = usr.u_instagram });
                        RPE.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
                }
            }

            return RedirectToAction("AllUsers", "Admin");
        }

        public ActionResult AllUsers()
        {
            ViewBag.users = RPE.RP_users.ToList();
            return View();
        }

        public ActionResult EditUser(int id)
        {
            try
            {
                ViewBag.roles = RPE.RP_userRoles.ToList();
                ViewBag.memberships = RPE.RP_membership.ToList();
                ViewBag.cities = RPE.RP_cities.ToList();

                ViewBag.users = RPE.RP_users.Where(x => x.u_id == id).SingleOrDefault();

                ViewBag.Usrimage = RPE.RP_users.Where(x => x.u_id == id).Single().u_image;
                return View(ViewBag.users);
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("AllUsers", "Admin");
        }

        public ActionResult DeleteUser(int id)
        {

            var checkID = RPE.RP_users.Where(x => x.u_id == id).Count();
            if (checkID > 0)
            {
                var uID = RPE.RP_users.Where(x => x.u_id == id).Single();
                RPE.RP_users.Remove(uID);
                RPE.SaveChanges();
                return RedirectToAction("AllUsers", "Admin");
            }

            return RedirectToAction("AllUsers", "Admin");

        }

        [HttpPost]
        public ActionResult UpdateUser(RP_users usr)
        {
                try
                {
                    var userID = RPE.RP_users.Where(x => x.u_id == usr.u_id).FirstOrDefault();

                    if (usr.userImageData != null)
                    {
                        usr.u_image = "~/assets/img/user/" + "user-id-" + userID + "-" + usr.userImageData.FileName;
                        usr.userImageData.SaveAs(Server.MapPath(usr.u_image));
                        userID.u_image = "user-id-" + userID + "-" + usr.userImageData.FileName;
                    }

                    if (usr.u_password == null)
                    {
                        usr.u_password = userID.u_password;
                    }else
                    {
                        userID.u_password = usr.u_password;
                    }

                    userID.u_facebook = usr.u_facebook;
                    userID.u_twitter = usr.u_twitter;
                    userID.u_instagram = usr.u_instagram;
                    userID.u_about = usr.u_about;
                    userID.u_name = usr.u_name;
                    userID.u_username = usr.u_username;
                    userID.u_email = usr.u_email;
                    userID.u_phone = usr.u_phone;
                    userID.u_city = usr.u_city;
                    userID.u_membership = usr.u_membership;
                    userID.u_role = usr.u_role;
                    userID.u_status = usr.u_status;
                    
                    RPE.Entry(userID).State = EntityState.Modified;
                    RPE.SaveChanges();
                }
                catch (Exception ex)
                {
                    this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
                }

            return RedirectToAction("AllUsers", "Admin");
        }


        //Roles
        public ActionResult UserRoles()
        {
            ViewBag.roles = RPE.RP_userRoles.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddRoles(RP_userRoles roles)
        {
            try
            {
                RPE.RP_userRoles.Add(new RP_userRoles { r_name = roles.r_name});
                RPE.SaveChanges();
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("UserRoles", "Admin");
        }

        public ActionResult EditRoles(int id)
        {
            try
            {
                ViewBag.role = RPE.RP_userRoles.Where(x => x.r_id == id).SingleOrDefault();
                ViewBag.roles = RPE.RP_userRoles.ToList();
                return View(ViewBag.role);
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("UserRoles", "Admin");
        }

        [HttpPost]
        public ActionResult EditRoles(RP_userRoles roles)
        {
            try
            {
                var roleID = RPE.RP_userRoles.Where(x => x.r_id == roles.r_id).FirstOrDefault();

                roleID.r_name = roles.r_name;

                RPE.Entry(roleID).State = EntityState.Modified;
                RPE.SaveChanges();
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("UserRoles", "Admin");
        }

        public ActionResult DeleteRoles(int id)
        {
            var checkID = RPE.RP_userRoles.Where(x => x.r_id == id).Count();
            if (checkID > 0)
            {
                var uID = RPE.RP_userRoles.Where(x => x.r_id == id).Single();
                RPE.RP_userRoles.Remove(uID);
                RPE.SaveChanges();
                return RedirectToAction("UserRoles", "Admin");
            }

            return RedirectToAction("UserRoles", "Admin");
        }



        //Property
        public ActionResult AddProperty()
        {
            ViewBag.cities = RPE.RP_cities.ToList();
            ViewBag.users = RPE.RP_users.Where(x => x.u_role != 2).ToList();
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
                    ViewBag.userID = RPE.RP_users.Where(x => x.u_id == pr.p_id).FirstOrDefault();

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

                    if (pr.ImageData2 != null)
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

                    RPE.RP_property.Add(new RP_property { p_type = pr.p_type, p_name = pr.p_name, p_demand = pr.p_demand, p_description = pr.p_description, p_purpose = pr.p_purpose, p_area = pr.p_area, p_size = pr.p_size, p_bedrooms = pr.p_bedrooms, p_bathrooms = pr.p_bathrooms, p_floors = pr.p_floors, p_garages = pr.p_garages, p_location = pr.p_location, p_city = pr.p_city, p_availability = pr.p_availability, p_postedBy = pr.p_postedBy, p_video = pr.p_video, p_image_1 = pr_image1, p_image_2 = pr_image2, p_image_3 = pr_image3, p_status = pr.p_status, p_created = DateTime.Now });
                    RPE.SaveChanges();
                }
                catch (Exception ex)
                {
                    this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
                }
            }

            return RedirectToAction("AllProperties", "Admin");
        }

        public ActionResult AllProperties()
        {
            ViewBag.properties = RPE.RP_property.ToList();
            return View();
        }

        public ActionResult DeleteProperty(int id)
        {

            var checkID = RPE.RP_property.Where(x => x.p_id == id).Count();
            if (checkID > 0)
            {
                var pID = RPE.RP_property.Where(x => x.p_id == id).Single();
                RPE.RP_property.Remove(pID);
                RPE.SaveChanges();
            }

            return RedirectToAction("AllProperties", "Admin");

        }

        public ActionResult EditProperty(int id)
        {

            try
            {
                var checkID = RPE.RP_property.Where(x => x.p_id == id).Count();
                if (checkID > 0)
                {
                    ViewBag.properties = RPE.RP_property.Where(x => x.p_id == id).FirstOrDefault();
                    ViewBag.cities = RPE.RP_cities.ToList();
                    ViewBag.users = RPE.RP_users.Where(x => x.u_role != 2).ToList();
                    ViewBag.propertyTypes = RPE.RP_propertyTypes.ToList();

                    return View(ViewBag.properties);
                }
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("AllProperties", "Admin");

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

                    
                    prID.p_name = pr.p_name;
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
                    prID.p_status = pr.p_status;
                    prID.p_postedBy = pr.p_postedBy;

                    RPE.Entry(prID).State = EntityState.Modified;
                    RPE.SaveChanges();

                }
                catch (Exception ex)
                {
                    this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
                }
            }

            return RedirectToAction("AllProperties", "Admin");
        }


        public ActionResult PropertyTypes()
        {
            ViewBag.propertyTypes = RPE.RP_propertyTypes.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddPropertyType(RP_propertyTypes pt)
        {
            try
            {
                RPE.RP_propertyTypes.Add(new RP_propertyTypes { pt_name = pt.pt_name });
                RPE.SaveChanges();
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("PropertyTypes", "Admin");
        }

        public ActionResult EditPropertyType(int id)
        {
            try
            {
                ViewBag.propertyType = RPE.RP_propertyTypes.Where(x => x.pt_id == id).SingleOrDefault();
                ViewBag.propertyTypes = RPE.RP_propertyTypes.ToList();
                return View(ViewBag.propertyType);
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("PropertyTypes", "Admin");
        }

        [HttpPost]
        public ActionResult EditPropertyType(RP_propertyTypes pt)
        {
            try
            {
                var ptID = RPE.RP_propertyTypes.Where(x => x.pt_id == pt.pt_id).FirstOrDefault();

                ptID.pt_name = pt.pt_name;

                RPE.Entry(ptID).State = EntityState.Modified;
                RPE.SaveChanges();
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("PropertyTypes", "Admin");
        }

        public ActionResult DeletePropertyType(int id)
        {
            var checkID = RPE.RP_propertyTypes.Where(x => x.pt_id == id).Count();
            if (checkID > 0)
            {
                var ptID = RPE.RP_propertyTypes.Where(x => x.pt_id == id).Single();
                RPE.RP_propertyTypes.Remove(ptID);
                RPE.SaveChanges();
            }

            return RedirectToAction("PropertyTypes", "Admin");
        }


        //Membership
        public ActionResult AllMemberships()
        {
            ViewBag.memberships = RPE.RP_membership.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddMemberships(RP_membership ms)
        {
            try
            {
                if (ms.membershipImageData != null)
                {
                    ms.m_icon = "~/assets/img/user/" + "membership-id-" + ms.m_id + "-" + ms.membershipImageData.FileName;
                    ms.membershipImageData.SaveAs(Server.MapPath(ms.m_icon));
                    ms.m_icon = "membership-id-" + ms.m_id + "-" + ms.membershipImageData.FileName;
                }
                else
                {
                    ms.m_icon = null;
                }

                RPE.RP_membership.Add(new RP_membership { m_name = ms.m_name, m_features = ms.m_features, m_icon = ms.m_icon });
                RPE.SaveChanges();
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("AllMemberships", "Admin");
        }

        public ActionResult EditMembership(int id)
        {
            try
            {
                ViewBag.membership = RPE.RP_membership.Where(x => x.m_id == id).SingleOrDefault();
                ViewBag.memberships = RPE.RP_membership.ToList();
                return View(ViewBag.membership);
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("AllMemberships", "Admin");
        }

        [HttpPost]
        public ActionResult EditMembership(RP_membership ms)
        {
            try
            {
                var membershipID = RPE.RP_membership.Where(x => x.m_id == ms.m_id).FirstOrDefault();

                if (ms.membershipImageData != null)
                {
                    ms.m_icon = "~/assets/img/user/" + "membership-id-" + ms.m_id + "-" + ms.membershipImageData.FileName;
                    ms.membershipImageData.SaveAs(Server.MapPath(ms.m_icon));
                    ms.m_icon = "membership-id-" + ms.m_id + "-" + ms.membershipImageData.FileName;
                }

                membershipID.m_name = ms.m_name;
                membershipID.m_features = ms.m_features;

                RPE.Entry(membershipID).State = EntityState.Modified;
                RPE.SaveChanges();
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return RedirectToAction("AllMemberships", "Admin");
        }

        public ActionResult DeleteMembership(int id)
        {
            var checkID = RPE.RP_membership.Where(x => x.m_id == id).Count();
            if (checkID > 0)
            {
                var mID = RPE.RP_membership.Where(x => x.m_id == id).Single();
                RPE.RP_membership.Remove(mID);
                RPE.SaveChanges();
            }

            return RedirectToAction("AllMemberships", "Admin");
        }

    }
}
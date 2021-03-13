using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Realtors_Portal.Models;

namespace Realtors_Portal.Controllers
{
    public class PropertyController : Controller
    {

        RP_realtorsPortalEntities RPE = new RP_realtorsPortalEntities();

        // GET: Property
        public ActionResult Index(string purpose, int city)
        {
            try
            {

                if (purpose == "Rent")
                {
                    ViewBag.properties = RPE.RP_property.Where(x => x.p_purpose == "Rent").ToList();
                    ViewBag.Title = "Property For Rent";
                }else if (purpose == "Sale")
                {
                    ViewBag.properties = RPE.RP_property.Where(x => x.p_purpose == "Sale").ToList();
                    ViewBag.Title = "Property For Buy";
                }else if (city > 0)
                {
                    ViewBag.properties = RPE.RP_property.Where(x => x.p_city == city).ToList();
                    ViewBag.Title = "Search by City";
                }
                else
                {
                    ViewBag.properties = RPE.RP_property.OrderByDescending(x => x.p_id).ToList();
                    ViewBag.Title = "Property";
                }
            }
            catch (Exception ex)
            {
                this.Response.Write("<script>alert(Error:" + ex.ToString() + ");</script>");
            }

            return View(ViewBag.properties);
        }

        public ActionResult View(int id)
        {
            try
            {

                int checkProperty = RPE.RP_property.Where(x => x.p_id == id).Count();

                if (checkProperty > 0)
                {

                    var propertyType = RPE.RP_property.Where(x => x.p_id == id).Single().p_type;
                    var usrID = RPE.RP_property.Where(x => x.p_id == id).Single().RP_users.u_id;
                    var cityID = RPE.RP_property.Where(x => x.p_id == id).Single().p_city;
                    ViewBag.propertyCity = RPE.RP_cities.Where(x => x.c_id == cityID).Single().c_name;
                    ViewBag.propertyState = RPE.RP_cities.Where(x => x.c_id == cityID).Single().RP_states.s_name;
                    ViewBag.AgentDetail = RPE.RP_users.Where(x => x.u_id == usrID).SingleOrDefault();
                    ViewBag.properties = RPE.RP_property.Where(x => x.p_id == id).SingleOrDefault();
                    ViewBag.Similarproperties = RPE.RP_property.Where(x => x.p_type == propertyType).ToList().Take(6);
                    return View();
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
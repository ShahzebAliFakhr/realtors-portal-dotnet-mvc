using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Realtors_Portal.Models;

namespace Realtors_Portal.Controllers
{
    public class AgentsController : Controller
    {

        RP_realtorsPortalEntities RPE = new RP_realtorsPortalEntities();

        // GET: Agents
        public ActionResult Index()
        {
            ViewBag.agents = RPE.RP_users.Where(x => x.u_role == 3).ToList();
            ViewBag.agentRole = RPE.RP_users.Where(x => x.u_role == 3).Single().RP_userRoles.r_name;
            return View();
        }
    }
}
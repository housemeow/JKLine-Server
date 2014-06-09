using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JKLineWebServices.Controllers
{
    public class EventController : Controller
    {
        //
        // POST: /Event/Create

        [HttpPost]
        public int GetNewEid()
        {
            JKLineDb db = new JKLineDb();
            int newEid = db.getNewEid();
            return newEid;
        }
    }
}

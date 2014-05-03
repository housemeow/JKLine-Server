using JKLineWebServices.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JKLineWebServices.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            using (var db = new JKLineEntities())
            {
                Member member = new Member() { id = "fff" + db.Members.Count(), password = "password", name = "new" + db.Members.Count() };
                db.Members.Add(member);
                try
                {
                    db.SaveChanges();

                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }

            }

            return View();
        }
    }
}

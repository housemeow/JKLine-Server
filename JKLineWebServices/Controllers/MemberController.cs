using JKLineWebServices.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace JKLineWebServices.Controllers
{
    public class MemberController : Controller
    {
        private JKLineDb jklineDb = new JKLineDb();

        [HttpPost]
        public string Register(string id, string phone, string password, string checkPassword, string name, string pushToken)
        {
            return jklineDb.Register(id, phone, password, checkPassword, name, pushToken);
        }

        [HttpPost]
        public string UpdatePushToken(int mid, string pushToken)
        {
            return jklineDb.UpdatePushToken(mid, pushToken);
        }

        [HttpPost]
        public string Login(string id, string password)
        {
            return jklineDb.Login(id, password);
        }

        [HttpPost]
        public ActionResult LoginGetMember(string id, string password)
        {
            return Json(jklineDb.LoginGetMember(id, password));
        }

        [HttpPost]
        public string SendFriendInvitation(int smid, string rid)
        {
            return jklineDb.SendFriendInvitation(smid, rid);
        }

        public string Test(int smid, int rmid)
        {
            DbSet<Member> members = jklineDb.GetMembers();
            Member member = members.Where(item => item.mid == smid).First();
            string s = "smid=" + smid + ",";
            foreach (Member m in member.Invitees)
            {
                s += m.mid + ",";
            }

            return s;
        }

        [HttpPost]
        public string AgreeInvitation(int smid, int rmid)
        {
            return jklineDb.AgreeInvitation(smid, rmid);
        }

        [HttpPost]
        public string DisagreeInvitation(int smid, int rmid)
        {
            return jklineDb.DisagreeInvitation(smid, rmid);
        }

        [HttpPost]
        public string EditMember(int mid, string name, string state)
        {
            return jklineDb.EditMember(mid, name, state);
        }

        [HttpPost]
        public string EditName(int mid, string name)
        {
            return jklineDb.EditName(mid, name);
        }

        [HttpPost]
        public string EditState(int mid, string state)
        {
            return jklineDb.EditState(mid, state);
        }

        [HttpPost]
        public ActionResult GetFriends(int mid)
        {
            return Json(jklineDb.GetFriends(mid), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetInvitations(int mid)
        {
            return Json(jklineDb.GetInvitations(mid), JsonRequestBehavior.AllowGet);
        }

        // GET api/member

        public ActionResult getArray(String name)
        {
            using (var db = new JKLineEntities())
            {

                var members = db.Members.ToArray();
                var newArray = Array.ConvertAll(members, item => new { name = item.name });
                return Json(newArray, JsonRequestBehavior.AllowGet);
                //return newArray;
            }
            //return new string[] { "value1", "value2" };
        }

        // GET api/member/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/member
        //public void Post([FromBody]string value)
        //{
        //}
    }
}

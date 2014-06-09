using JKLineWebServices.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;



using JKLineWebServices.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PushSharp;
using PushSharp.Android;

namespace JKLineWebServices.Controllers
{
    public class JKLineDb
    {
        private string MESSAGE_SUCCESS = "success";
        private string MESSAGE_ID_IS_EXISTING = "idIsExisting";
        private string MESSAGE_MEMBER_IS_NOT_EXISTING = "memberIsNotExisting";
        private string MESSAGE_PASSWORD_UNMATCHED = "passwordUnmatched";
        private string MESSAGE_INFO_IS_NOT_ENOUGH = "infoIsNotEnough";
        private string MESSAGE_WRONG_PASSWORD = "wrongPassword";
        private string MESSAGE_HAS_INVITED = "hasInvited";
        private string MESSAGE_HAS_BEEN_INVITED = "hasBeenInvited";
        private string MESSAGE_CAN_NOT_INVITE_SELF = "canNotInviteSelf";
        private string MESSAGE_INVITE_NOT_EXISTING = "inviteNotExisting";

        JKLineEntities jklineEntities;
        private string MESSAGE_NAME_IS_NULL = "messageNameIsNull";
        private string MESSAGE_HAS_BEEN_FRIEND = "alreadyBeenFriends";
        private string MESSAGE_PUSH_TOKEN_IS_NULL = "pushTokenIsNull";

        public JKLineEntities GetJKLineEntites()
        {
            if (jklineEntities == null)
            {

                jklineEntities = new JKLineEntities();
            }
            return jklineEntities;
        }

        public DbSet<Member> GetMembers()
        {
            return GetJKLineEntites().Members;
        }

        public System.Data.Entity.DbSet<MessageQueue> GetMessageQueues()
        {
            return GetJKLineEntites().MessageQueues;
        }

        public bool IsIdExist(string id)
        {
            return GetMembers().Where(item => item.id == id).Count() != 0;
        }

        public string Register(string id, string phone, string password, string checkPassword, string name, string pushToken)
        {
            if (id == null || phone == null || password == null || checkPassword == null || name == null || pushToken == null)
            {
                return MESSAGE_INFO_IS_NOT_ENOUGH;
            }
            else if (IsIdExist(id))
            {
                return MESSAGE_ID_IS_EXISTING;
            }
            else if (password != checkPassword)
            {
                return MESSAGE_PASSWORD_UNMATCHED;
            }

            Member member = new Member();
            member.id = id;
            member.phone = phone;
            member.password = password;
            member.name = name;
            member.pushToken = pushToken;
            GetJKLineEntites().Members.Add(member);
            GetJKLineEntites().SaveChanges();
            return MESSAGE_SUCCESS;
        }

        public string Login(string id, string password)
        {
            if (id == null || password == null)
            {
                return MESSAGE_INFO_IS_NOT_ENOUGH;
            } if (IsIdNotExist(id))
            {
                return MESSAGE_MEMBER_IS_NOT_EXISTING;
            } if (FindMemberById(id).password != password)
            {
                return MESSAGE_WRONG_PASSWORD;
            }
            return MESSAGE_SUCCESS;
        }

        private Member FindMemberById(string id)
        {
            return GetMembers().Where(item => item.id == id).First();
        }
        public Member FindMemberByIdAndPassword(string id, string password)
        {
            return GetMembers().Where(item => item.id == id && item.password == password).First();
        }

        public string SendFriendInvitation(int smid, string rid)
        {
            if (IsMemberNotExisting(smid) || IsIdNotExist(rid))
            {
                return MESSAGE_MEMBER_IS_NOT_EXISTING;
            }
            else if (FindMemberById(rid).mid == smid)
            {
                return MESSAGE_CAN_NOT_INVITE_SELF;
            }
            else if (HasInvited(smid, FindMemberById(rid).mid))
            {
                return MESSAGE_HAS_INVITED;
            }
            else if (HasBeenInvited(smid, FindMemberById(rid).mid))
            {
                return MESSAGE_HAS_BEEN_INVITED;
            }
            else if (HasBeenFriend(smid, rid))
            {
                return MESSAGE_HAS_BEEN_FRIEND;
            }

            FindMemberById(rid).Inviters.Add(GetMember(smid));
            GetMember(smid).Invitees.Add(FindMemberById(rid));
            GetJKLineEntites().SaveChanges();
            return MESSAGE_SUCCESS;
        }

        private bool HasBeenFriend(int smid, string rid)
        {
            return GetMember(smid).Friends.Where(item => item.mid == FindMemberById(rid).mid).Count()>0;
        }

        private bool IsMemberNotExisting(int mid)
        {
            return !IsMemberExisting(mid);
        }

        public bool HasBeenInvited(int smid, int rmid)
        {
            Member member = FindMemberByMid(smid);
            return member.Inviters.Where(m => m.mid == rmid).Count() > 0;
        }

        public Member FindMemberByMid(int mid)
        {
            if (IsMemberExisting(mid))
            {
                return GetMember(mid);
            }
            return null;
        }

        private Member GetMember(int mid)
        {
            return GetMembers().Where(item => item.mid == mid).First();
        }

        private int GetUnreceiveMessageCount(int smid, int rmid)
        {
            int count = 0;
            ICollection<MessageQueue> queue = FindMemberByMid(rmid).ReceiveMessageQueue;
            count = queue.Where(item => item.message!="").Count();
            return count;
        }

        private bool IsMemberExisting(int mid)
        {
            return GetMembers().Where(item => item.mid == mid).Count() > 0;
        }

        public bool HasInvited(int smid, int rmid)
        {
            Member member = FindMemberByMid(rmid);
            return member.Inviters.Where(m => m.mid == smid).Count() > 0;
        }

        public bool IsIdNotExist(string id)
        {
            return !IsIdExist(id);
        }

        public string AgreeInvitation(int smid, int rmid)
        {
            if (IsMemberNotExisting(smid) || IsMemberNotExisting(rmid))
            {
                return MESSAGE_MEMBER_IS_NOT_EXISTING;
            }
            else if (IsInviteNotExist(smid, rmid))
            {
                return MESSAGE_INVITE_NOT_EXISTING;
            }
            GetMember(smid).Inviters.Remove(GetMember(rmid));
            GetMember(rmid).Inviters.Remove(GetMember(smid));

            GetMember(smid).FriendOwners.Add(GetMember((rmid)));
            GetMember(rmid).FriendOwners.Add(GetMember((smid)));
            GetMember(rmid).FriendOwners.Add(GetMember((smid)));
            GetMember(smid).FriendOwners.Add(GetMember((rmid)));
            GetJKLineEntites().SaveChanges();
            return MESSAGE_SUCCESS;
        }
        public string DisagreeInvitation(int smid, int rmid)
        {
            if (IsMemberNotExisting(smid) || IsMemberNotExisting(rmid))
            {
                return MESSAGE_MEMBER_IS_NOT_EXISTING;
            }
            else if (IsInviteNotExist(smid, rmid))
            {
                return MESSAGE_INVITE_NOT_EXISTING;
            }
            GetMember(smid).Inviters.Remove(GetMember(rmid));
            GetMember(rmid).Inviters.Remove(GetMember(smid));
            GetJKLineEntites().SaveChanges();
            return MESSAGE_SUCCESS;
        }

        private bool IsInviteNotExist(int smid, int rmid)
        {
            return GetMember(rmid).Inviters.Where(item => item.mid == smid).Count() == 0;
        }


        public string EditName(int mid, string name)
        {
            if (IsMemberNotExisting(mid))
            {
                return MESSAGE_MEMBER_IS_NOT_EXISTING;
            }
            else if (name == null)
            {
                return MESSAGE_NAME_IS_NULL;
            }
            GetMember(mid).name = name;
            GetJKLineEntites().SaveChanges();
            return MESSAGE_SUCCESS;
        }

        public string EditState(int mid, string state)
        {
            if (IsMemberNotExisting(mid))
            {
                return MESSAGE_MEMBER_IS_NOT_EXISTING;
            }
            GetMember(mid).state = state;
            GetJKLineEntites().SaveChanges();
            return MESSAGE_SUCCESS;
        }

        public IEnumerable<dynamic> GetFriends(int mid)
        {
            return Array.ConvertAll(GetMember(mid).Friends.ToArray(), item => new { mid = item.mid, id = item.id, name = item.name, state = item.state });
        }

        public IEnumerable<dynamic> GetInvitations(int mid)
        {
            return Array.ConvertAll(GetMember(mid).Inviters.ToArray(), item => new { mid = item.mid, id = item.id, name = item.name });
        }

        internal dynamic LoginGetMember(string id, string password)
        {
            if (Login(id, password) == MESSAGE_SUCCESS)
            {
                Member member = FindMemberByIdAndPassword(id, password);
                return new { mid = member.mid, id = member.id, name = member.name, state = member.state };
            }
            else
            {
                return Login(id, password);
            }
        }

        internal string EditMember(int mid, string name, string state)
        {
            if (IsMemberNotExisting(mid))
            {
                return MESSAGE_MEMBER_IS_NOT_EXISTING;
            }
            else if (name == null || name == "")
            {
                return MESSAGE_INFO_IS_NOT_ENOUGH;
            }
            EditName(mid, name);
            EditState(mid, state);
            return MESSAGE_SUCCESS;
        }

        internal string SendMessage(int smid, int rmid, string message)
        {
            if (IsMemberNotExisting(smid) || IsMemberNotExisting(rmid))
            {
                return MESSAGE_MEMBER_IS_NOT_EXISTING;
            }
            MessageQueue messageQueue = new MessageQueue();
            messageQueue.message = message;
            messageQueue.MessageReceiver = GetMember(rmid);
            messageQueue.MessageSender = GetMember(smid);
            messageQueue.rmid = rmid;
            messageQueue.smid = smid;
            messageQueue.timeStamp = DateTime.Now;
            GetMessageQueues().Add(messageQueue);
            try
            {
                GetJKLineEntites().SaveChanges();
                DisposeJKLineEntities();
                string receiverPushToken = messageQueue.MessageReceiver.pushToken;
                PushBroker push = new PushBroker();
                push.RegisterGcmService(new GcmPushChannelSettings("AIzaSyA7dl25oulxDoTpExp9RdAlsCUR-tv61_U"));
                Dictionary<string, string> pushedMessage = new Dictionary<string, string>();
                int unreceiveMessageCount = GetUnreceiveMessageCount(smid, rmid);
                string jsonString = String.Format("{{\"message\":\"{0}\",\"senderName\":\"{1}\",\"timeStamp\":\"{2}\",\"count\":{3}}}", message, messageQueue.MessageSender.name, messageQueue.timeStamp.ToShortTimeString(), unreceiveMessageCount);
                push.QueueNotification(new GcmNotification().ForDeviceRegistrationId(receiverPushToken)
                                      .WithJson(jsonString));
                return jsonString;

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            GetJKLineEntites().SaveChanges();
            return DateTime.Now.ToString();
        }

        private void DisposeJKLineEntities()
        {
            jklineEntities.Dispose();
            jklineEntities = null;
        }

        internal object ReceiveMessage(int smid, int rmid)
        {
            IQueryable<MessageQueue> retrieveMessages = GetMessageQueues().Where(item => item.smid == smid && item.rmid == rmid);

            var array = retrieveMessages.ToArray();
            var result = Array.ConvertAll(array, item => new { smid = item.smid, rmid = item.rmid, name = item.MessageSender.name, message = item.message, timeStamp = item.timeStamp.ToString("YYYY-MM-DD HH:MM:SS.SSS") });
            DbSet<Models.MessageQueue> messageQueues = GetMessageQueues();
            foreach (MessageQueue messageQueue in retrieveMessages)
            {
                messageQueues.Remove(messageQueue);
            }
            GetJKLineEntites().SaveChanges();
            return result;
            throw new System.NotImplementedException();
        }

        internal string UpdatePushToken(int mid, string pushToken)
        {
            if (IsMemberNotExisting(mid))
            {
                return MESSAGE_MEMBER_IS_NOT_EXISTING;
            }
            else if (pushToken == null)
            {
                return MESSAGE_PUSH_TOKEN_IS_NULL;
            }
            GetMember(mid).pushToken = pushToken;
            GetJKLineEntites().SaveChanges();
            return MESSAGE_SUCCESS;
        }

        internal int getNewEid()
        {
            Event newEvent = new Event();
            GetJKLineEntites().Events.Add(newEvent);
            GetJKLineEntites().SaveChanges();
            return newEvent.eid;
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JKLineWebServices.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Member
    {
        public Member()
        {
            this.MessageQueueReceivers = new HashSet<MessageQueue>();
            this.MessageQueueSenders = new HashSet<MessageQueue>();
            this.FriendOwners = new HashSet<Member>();
            this.Friends = new HashSet<Member>();
            this.Inviters = new HashSet<Member>();
            this.Invitees = new HashSet<Member>();
        }
    
        public int mid { get; set; }
        public string id { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string pushToken { get; set; }
    
        public virtual ICollection<MessageQueue> MessageQueueReceivers { get; set; }
        public virtual ICollection<MessageQueue> MessageQueueSenders { get; set; }
        public virtual ICollection<Member> FriendOwners { get; set; }
        public virtual ICollection<Member> Friends { get; set; }
        public virtual ICollection<Member> Inviters { get; set; }
        public virtual ICollection<Member> Invitees { get; set; }
    }
}
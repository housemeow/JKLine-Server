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
    
    public partial class MessageQueue
    {
        public int smid { get; set; }
        public int rmid { get; set; }
        public System.DateTime timeStamp { get; set; }
        public string message { get; set; }
    
        public virtual Member MessageReceiver { get; set; }
        public virtual Member MessageSender { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
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

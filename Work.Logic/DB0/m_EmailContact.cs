//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProcCore.Business.DB0
{
    using System;
    using System.Collections.Generic;
    public partial class m_EmailContact :BaseEntityTable {
    public int email_contact_id { get; set; }
    public int category { get; set; }
    public string send_name { get; set; }
    public string email { get; set; }
    public string tel { get; set; }
    public string addr { get; set; }
    public Nullable<int> interest { get; set; }
    public Nullable<int> how_findout { get; set; }
    public System.DateTime i_InsertDateTime { get; set; }
    public string i_UpdateUserID { get; set; }
    public Nullable<int> i_UpdateDeptID { get; set; }
    public Nullable<System.DateTime> i_UpdateDateTime { get; set; }
    public string i_Lang { get; set; }
    }
}

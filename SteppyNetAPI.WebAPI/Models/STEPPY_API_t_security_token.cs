
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SteppyNetAPI.WebAPI.Models
{

using System;
    using System.Collections.Generic;
    
public partial class STEPPY_API_t_security_token
{

    public long id_token { get; set; }

    public Nullable<int> id_user { get; set; }

    public string security_token { get; set; }

    public Nullable<System.DateTime> request_date { get; set; }

    public Nullable<System.DateTime> expired_date { get; set; }

    public Nullable<bool> is_logout { get; set; }



    public virtual STEPPY_API_m_user STEPPY_API_m_user { get; set; }

}

}

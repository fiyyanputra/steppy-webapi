
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
    
public partial class STEPPY_API_t_user_profile
{

    public int id_profile { get; set; }

    public Nullable<decimal> hi_score { get; set; }

    public Nullable<int> current_experience { get; set; }

    public Nullable<int> current_level { get; set; }

    public Nullable<int> next_level_experience { get; set; }

    public Nullable<int> gold { get; set; }

    public Nullable<int> diamond { get; set; }

    public Nullable<int> id_user { get; set; }

    public Nullable<decimal> current_score { get; set; }

    public string profile_image { get; set; }



    public virtual STEPPY_API_m_user STEPPY_API_m_user { get; set; }

}

}
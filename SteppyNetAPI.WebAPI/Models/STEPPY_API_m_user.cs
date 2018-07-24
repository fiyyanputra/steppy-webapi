
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
    
public partial class STEPPY_API_m_user
{

    public STEPPY_API_m_user()
    {

        this.STEPPY_API_m_contact = new HashSet<STEPPY_API_m_contact>();

        this.STEPPY_API_t_security_token = new HashSet<STEPPY_API_t_security_token>();

        this.STEPPY_API_t_user_profile = new HashSet<STEPPY_API_t_user_profile>();

        this.STEPPY_API_t_measurement = new HashSet<STEPPY_API_t_measurement>();

    }


    public int id_user { get; set; }

    public string telp_number { get; set; }

    public string display_name { get; set; }

    public System.DateTime join_date { get; set; }

    public string password { get; set; }

    public Nullable<System.DateTime> last_login { get; set; }

    public Nullable<System.DateTime> last_logout { get; set; }

    public Nullable<int> id_country { get; set; }

    public int id_user_shesop { get; set; }



    public virtual ICollection<STEPPY_API_m_contact> STEPPY_API_m_contact { get; set; }

    public virtual ICollection<STEPPY_API_t_security_token> STEPPY_API_t_security_token { get; set; }

    public virtual STEPPY_API_m_country STEPPY_API_m_country { get; set; }

    public virtual ICollection<STEPPY_API_t_user_profile> STEPPY_API_t_user_profile { get; set; }

    public virtual ICollection<STEPPY_API_t_measurement> STEPPY_API_t_measurement { get; set; }

}

}
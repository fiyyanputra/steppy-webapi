
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
    
public partial class STEPPY_API_m_measurement_type
{

    public STEPPY_API_m_measurement_type()
    {

        this.STEPPY_API_t_measurement = new HashSet<STEPPY_API_t_measurement>();

    }


    public int id_measurement_type { get; set; }

    public string measurement_type_name { get; set; }

    public string measurement_type_unit { get; set; }

    public string measurement_description { get; set; }



    public virtual ICollection<STEPPY_API_t_measurement> STEPPY_API_t_measurement { get; set; }

}

}

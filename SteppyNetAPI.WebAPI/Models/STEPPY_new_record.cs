
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
    
public partial class STEPPY_new_record
{

    public int UserID { get; set; }

    public Nullable<System.DateTime> tanggal { get; set; }

    public Nullable<System.TimeSpan> jam_mulai { get; set; }

    public Nullable<System.TimeSpan> jam_akhir { get; set; }

    public Nullable<long> step { get; set; }

    public Nullable<long> kalori { get; set; }

    public string jenis_sensor { get; set; }

    public string keterangan { get; set; }

    public int id { get; set; }

    public Nullable<int> user_id_shesop { get; set; }

    public Nullable<decimal> distance { get; set; }

}

}

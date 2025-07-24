using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class SurchargePrice
{
    public int SurchargeId { get; set; }

    public string SampleType { get; set; } = null!;

    public decimal? Surcharge { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}

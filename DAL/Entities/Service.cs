using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string ServiceType { get; set; } = null!;

    public string PackageType { get; set; } = null!;

    public decimal Price { get; set; }

    public string Status { get; set; } = null!;

    public decimal? ExtraSampleFee { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<SurchargePrice> Surcharges { get; set; } = new List<SurchargePrice>();
}

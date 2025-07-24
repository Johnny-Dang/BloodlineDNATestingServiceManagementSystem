using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Booking
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int ServiceId { get; set; }

    public int NumberSample { get; set; }

    public DateOnly? BookingDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal? TotalPrice { get; set; }

    public DateOnly? AppointmentDate { get; set; }

    public string? Note { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int? UpdateBy { get; set; }

    public virtual Feedback? Feedback { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();

    public virtual Service Service { get; set; } = null!;

    public virtual TestResult? TestResult { get; set; }

    public virtual User? UpdateByNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}

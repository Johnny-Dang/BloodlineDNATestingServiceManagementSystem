using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Sample
{
    public int SampleId { get; set; }

    public int BookingId { get; set; }

    public int? UserId { get; set; }

    public int ParticipantId { get; set; }

    public string TypeOfCollection { get; set; } = null!;

    public string SampleType { get; set; } = null!;

    public DateOnly? ReceivedDate { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Participant Participant { get; set; } = null!;

    public virtual User? User { get; set; }
}

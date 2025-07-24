using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? BookingId { get; set; }

    public string? Comments { get; set; }

    public string? Answers { get; set; }

    public int? Rating { get; set; }

    public DateOnly? CreateDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public string? Status { get; set; }

    public virtual Booking? Booking { get; set; }
}

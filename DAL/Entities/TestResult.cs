using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class TestResult
{
    public int TestResultId { get; set; }

    public int BookingId { get; set; }

    public DateOnly? ResultDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public string? ResultConclution { get; set; }

    public string? ResultFile { get; set; }

    public string? UpdatedBy { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual ICollection<DetailResult> DetailResults { get; set; } = new List<DetailResult>();
}

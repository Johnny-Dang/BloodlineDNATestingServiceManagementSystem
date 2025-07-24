using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Consultant
{
    public int ConsultantId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateOnly ConsultantDate { get; set; }

    public string? Notes { get; set; }

    public int? ConfirmBy { get; set; }

    public string? Status { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public virtual User User { get; set; } = null!;
}

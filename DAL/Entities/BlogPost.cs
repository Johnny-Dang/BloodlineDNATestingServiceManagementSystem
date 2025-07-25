using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class BlogPost
{
    public int PostId { get; set; }

    public int? UserId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public string? Image { get; set; }

    public string? Status { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public int? UpdateBy { get; set; }

    public virtual User? User { get; set; }
}

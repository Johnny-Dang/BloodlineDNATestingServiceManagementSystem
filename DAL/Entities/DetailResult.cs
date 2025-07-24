using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class DetailResult
{
    public int DetailResultId { get; set; }

    public int TestResultId { get; set; }

    public string LocusName { get; set; } = null!;

    public string? P1Allele1 { get; set; }

    public string? P1Allele2 { get; set; }

    public string? P2Allele1 { get; set; }

    public string? P2Allele2 { get; set; }

    public decimal? PaternityIndex { get; set; }

    public virtual TestResult TestResult { get; set; } = null!;
}

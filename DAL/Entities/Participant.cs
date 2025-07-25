using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Participant
{
    public int ParticipantId { get; set; }

    public string QuestionalbleRelationship { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string? CollectionMethod { get; set; }

    public string? RelationshipToCustomer { get; set; }

    public string? IdentityNumber { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();
}

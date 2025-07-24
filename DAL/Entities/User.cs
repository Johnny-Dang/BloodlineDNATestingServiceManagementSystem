using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string Status { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<Booking> BookingUpdateByNavigations { get; set; } = new List<Booking>();

    public virtual ICollection<Booking> BookingUsers { get; set; } = new List<Booking>();

    public virtual ICollection<Consultant> Consultants { get; set; } = new List<Consultant>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();
}

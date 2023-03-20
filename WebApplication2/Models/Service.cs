using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int UserId { get; set; }

    public int MasterId { get; set; }
    public string Category { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual Master? Master { get; set; } = null;

    public virtual Admin? User { get; set; } = null;
}

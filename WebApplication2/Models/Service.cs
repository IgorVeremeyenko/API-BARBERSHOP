using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual Admin? User { get; set; } = null;
}

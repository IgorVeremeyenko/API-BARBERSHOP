using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Costumer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Language { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual ICollection<Statistic> Statistics { get; } = new List<Statistic>();

    public virtual Admin? User { get; set; } = null;
}

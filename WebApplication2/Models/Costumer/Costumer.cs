using System;
using System.Collections.Generic;
using static WebApplication2.Services.Costumers.CalculateRating;

namespace WebApplication2.Models.Costumer;

public partial class Costumer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Language { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();

    public virtual Admin? User { get; set; } = null;
}

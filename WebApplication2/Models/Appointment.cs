using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int CostumerId { get; set; }

    public int ServiceId { get; set; }

    public int UserId { get; set; }

    public DateTime Date { get; set; }

    public virtual Costumer Costumer { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual Admin User { get; set; } = null!;
}

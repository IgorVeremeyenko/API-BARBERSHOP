﻿using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int CostumerId { get; set; }

    public int ServiceId { get; set; }
    public string? ServiceName { get; set; } = null;
    public int? ServicePrice { get; set; } = null;

    public int UserId { get; set; }

    public int MasterId { get; set; }

    public int TimezoneOffset { get; set; }

    public DateTime Date { get; set; }

    public string? Status { get; set; }

    public virtual Costumer.Costumer? Costumer { get; set; } = null;

    public virtual Service? Service { get; set; } = null;

    public virtual Admin? User { get; set; } = null;
}

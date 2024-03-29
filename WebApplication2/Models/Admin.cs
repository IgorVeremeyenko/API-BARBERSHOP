﻿namespace WebApplication2.Models;

public partial class Admin
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Salt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual ICollection<Costumer.Costumer> Costumers { get; } = new List<Costumer.Costumer>();

    public virtual ICollection<Service> Services { get; } = new List<Service>();

    public virtual ICollection<Statistic> Statistics { get; } = new List<Statistic>();
}

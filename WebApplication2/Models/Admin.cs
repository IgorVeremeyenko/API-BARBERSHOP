using Microsoft.Build.Framework;
using Microsoft.VisualStudio.Web.CodeGeneration.CommandLine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace WebApplication2.Models;

public partial class Admin
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;

    [Option]
    public string? Salt { get; set; } = null;

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual ICollection<Costumer> Costumers { get; } = new List<Costumer>();

    public virtual ICollection<Service> Services { get; } = new List<Service>();

    public virtual ICollection<Statistic> Statistics { get; } = new List<Statistic>();
}

using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Master
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; } = null;

    public virtual ICollection<MasterSchedule> MasterSchedules { get; } = new List<MasterSchedule>();

    public virtual ICollection<Service> Services { get; } = new List<Service>();
}

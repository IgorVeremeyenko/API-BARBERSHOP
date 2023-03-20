using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Master
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Service> Services { get; } = new List<Service>();
}

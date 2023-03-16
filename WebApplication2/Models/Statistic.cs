using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Statistic
{
    public int Id { get; set; }

    public int CostumerId { get; set; }

    public int NumOfVisits { get; set; }

    public int UserId { get; set; }

    public virtual Costumer Costumer { get; set; } = null!;

    public virtual Admin? User { get; set; } = null;
}

using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class MasterSchedule
{
    public int Id { get; set; }

    public int MasterId { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public virtual Master? Master { get; set; } = null;
}

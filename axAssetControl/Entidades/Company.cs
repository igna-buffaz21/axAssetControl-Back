using System;
using System.Collections.Generic;

namespace axAssetControl.Entidades;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<ControlRecord> ControlRecords { get; set; } = new HashSet<ControlRecord>();

    public virtual ICollection<Sector> Sectors { get; set; } = new List<Sector>();

    public virtual ICollection<Subsector> Subsectors { get; set; } = new List<Subsector>();

    public virtual ICollection<Active> Actives { get; set; } = new List<Active>();

}

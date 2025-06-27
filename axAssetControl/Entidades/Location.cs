using System;
using System.Collections.Generic;

namespace axAssetControl.Entidades;

public partial class Location
{
    public int Id { get; set; }

    public int IdCompany { get; set; }

    public string Name { get; set; } = null!;

    public virtual Company IdCompanyNavigation { get; set; } = null!;

    public virtual ICollection<Sector> Sectors { get; set; } = new List<Sector>();

    public int Version { get; set; }

    public bool Status { get; set; } = true; ///true = activo, false = dado de baja
}

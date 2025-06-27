using System;
using System.Collections.Generic;

namespace axAssetControl.Entidades;

public partial class Sector
{
    public int Id { get; set; }

    public int IdLocation { get; set; }

    public string Name { get; set; } = null!;

    public string? TagRfid { get; set; }

    public virtual Location IdLocationNavigation { get; set; } = null!;

    public virtual ICollection<Subsector> Subsectors { get; set; } = new List<Subsector>();

    public int IdEmpresa { get; set; }

    public virtual Company Company { get; set; } = null!;

    public int Version { get; set; }

    public bool Status { get; set; } = true; ///true = activo, false = dado de baja
}

using System;
using System.Collections.Generic;

namespace axAssetControl.Entidades;

public partial class Subsector
{
    public int Id { get; set; }

    public int IdSector { get; set; }

    public string Name { get; set; } = null!;

    public string TagRfid { get; set; } = null!;

    public virtual ICollection<Active> Actives { get; set; } = new List<Active>();

    public virtual ICollection<ControlRecord> ControlRecords { get; set; } = new List<ControlRecord>();

    public virtual Sector IdSectorNavigation { get; set; } = null!;

    public int IdEmpresa { get; set; }

    public virtual Company Company { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace axAssetControl.Entidades;

public partial class Active
{
    public int Id { get; set; }

    public int IdSubsector { get; set; }

    public string Name { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string SeriaNumber { get; set; } = null!;

    public string TagRfid { get; set; } = null!;

    public int IdActiveType { get; set; }

    public virtual ICollection<DetailControl> DetailControls { get; set; } = new List<DetailControl>();

    public virtual ActiveType IdActiveTypeNavigation { get; set; } = null!;

    public virtual Subsector IdSubsectorNavigation { get; set; } = null!;

    public int IdEmpresa { get; set; }

    public virtual Company Company { get; set; }

    public int Version { get; set; }

    public bool Status { get; set; } ///true = activo, false = dado de baja
}

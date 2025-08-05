using System;
using System.Collections.Generic;

namespace axAssetControl.Entidades;

public partial class ControlRecord
{
    public int Id { get; set; }

    public int IdSubsector { get; set; }

    public long Date { get; set; }

    public virtual ICollection<DetailControl> DetailControls { get; set; } = new List<DetailControl>();

    public virtual Subsector IdSubsectorNavigation { get; set; } = null!;

    public string Status { get; set; } = "inProcces";

    public int IdCompany { get; set; }  // la FK

    public virtual Company IdCompanyNavigation { get; set; }  // navegación


}

using System;
using System.Collections.Generic;

namespace axAssetControl.Entidades;

public partial class DetailControl
{
    public int Id { get; set; }

    public int IdControl { get; set; }

    public int IdActivo { get; set; }

    public string Status { get; set; } = null!;

    public int IdAuditor { get; set; }

    public virtual Active IdActivoNavigation { get; set; } = null!;

    public virtual User IdAuditorNavigation { get; set; } = null!;

    public virtual ControlRecord IdControlNavigation { get; set; } = null!;
}

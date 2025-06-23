using System;
using System.Collections.Generic;

namespace axAssetControl.Entidades;

public partial class User
{
    public int Id { get; set; }

    public int IdCompany { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<DetailControl> DetailControls { get; set; } = new List<DetailControl>();

    public virtual Company IdCompanyNavigation { get; set; } = null!;
}

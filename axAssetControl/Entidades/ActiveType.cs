using System;
using System.Collections.Generic;

namespace axAssetControl.Entidades;

public partial class ActiveType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Active> Actives { get; set; } = new List<Active>();
}

using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared._WL.MODSuit;

[Prototype("modsuit")]
public sealed partial class MODSuitPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public string Name { get; private set; } = "Unknown";

    [DataField]
    public EntProtoId MODSuitHead { get; private set; } = default!;

    [DataField]
    public SpriteSpecifier? Sprite { get; private set; }
}

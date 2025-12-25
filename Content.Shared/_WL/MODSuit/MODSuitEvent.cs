using Content.Shared.RCD;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._WL.MODSuit;

public sealed class MODSuitSystemMassage(ProtoId<MODSuitPrototype> protoId) : BoundUserInterfaceMessage
{
    public ProtoId<MODSuitPrototype> ProtoId = protoId;
}

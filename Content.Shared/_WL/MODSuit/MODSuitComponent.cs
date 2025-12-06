using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._WL.MODSuit;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class MODSuitComponent : Component
{
    [DataField]
    public EntProtoId MODSuitActionId = "ActionMODSuit";

    [DataField, AutoNetworkedField]
    public EntityUid? MODSuitAction;

    [DataField]
    public HashSet<ProtoId<MODSuitPrototype>> MODSuit = default!;
}
[Serializable, NetSerializable]
public enum MODSuitUiKey : byte
{
    Key
}

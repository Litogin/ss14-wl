using Content.Server.Actions;
using Content.Shared._WL.MODSuit;
using Robust.Server.GameObjects;

namespace Content.Server._WL.MODSuit;

public sealed partial class MODSuitSystem : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _ui = default!;
    [Dependency] private readonly ActionsSystem _actions = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MODSuitComponent, MapInitEvent>(OnMODSuitMapInit);
        SubscribeLocalEvent<MODSuitComponent, ComponentShutdown>(OnMODSuitShutdown);

    }
    private void OnMODSuitMapInit(EntityUid uid, MODSuitComponent component, MapInitEvent args)
    {
        _actions.AddAction(uid, ref component.MODSuitAction, component.MODSuitActionId, uid);
    }

    private void OnMODSuitShutdown(EntityUid uid, MODSuitComponent component, ComponentShutdown args)
    {
        _actions.RemoveAction(uid, component.MODSuitAction);
    }
}


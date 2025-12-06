using Content.Client.Popups;
using Content.Client.UserInterface.Controls;
using Content.Shared._WL.MODSuit;
using Content.Shared.RCD;
using Content.Shared.RCD.Components;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Collections;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;


namespace Content.Client._WL.MODSuit;

[UsedImplicitly]
public sealed class MODSuitMenuBoundUserInterface : BoundUserInterface
{
    private SimpleRadialMenu? _menu;

    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly ISharedPlayerManager _playerManager = default!;
    public MODSuitMenuBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }
    protected override void Open()
    {
        base.Open();

        if (!EntMan.TryGetComponent<MODSuitComponent>(Owner, out var modsuit))
            return;

        _menu = this.CreateWindow<SimpleRadialMenu>();
        _menu.Track(Owner);
        var models = ConvertToButtons(modsuit.MODSuit);
        _menu.SetButtons(models);

        _menu.OpenOverMouseScreenPosition();
    }
    private IEnumerable<RadialMenuOptionBase> ConvertToButtons(HashSet<ProtoId<MODSuitPrototype>> prototypes)
    {
        Dictionary<string, List<RadialMenuActionOptionBase>> buttonsByCategory = new();
        ValueList<RadialMenuActionOptionBase> topLevelActions = new();
        foreach (var protoId in prototypes)
        {
            var prototype = _prototypeManager.Index(protoId);
            var topLevelActionOption = new RadialMenuActionOption<MODSuitPrototype>(HandleMenuOptionClick, prototype)
            {
                IconSpecifier = RadialMenuIconSpecifier.With(prototype.Sprite),
                ToolTip = prototype.Name
            };
            topLevelActions.Add(topLevelActionOption);
            continue;
        }

        var models = new RadialMenuOptionBase[buttonsByCategory.Count + topLevelActions.Count];
        var i = 0;
        foreach (var (key, list) in buttonsByCategory)
        {
            var groupInfo = PrototypesGroupingInfo[key];
            models[i] = new RadialMenuNestedLayerOption(list)
            {
                IconSpecifier = RadialMenuIconSpecifier.With(groupInfo.Sprite),
                ToolTip = Loc.GetString(groupInfo.Tooltip)
            };
            i++;
        }

        foreach (var action in topLevelActions)
        {
            models[i] = action;
            i++;
        }

        return models;
    }
    private void HandleMenuOptionClick(RCDPrototype proto)
    {
        // A predicted message cannot be used here as the RCD UI is closed immediately
        // after this message is sent, which will stop the server from receiving it
        SendMessage(new RCDSystemMessage(proto.ID));
    }
}

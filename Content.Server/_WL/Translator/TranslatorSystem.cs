using Content.Server._WL.Languages;
using Content.Shared.Chat;
using Content.Shared.Implants;
using Content.Shared.Inventory;
using Robust.Shared.Noise;
using System.Numerics;

namespace Content.Server._WL.Translator;

public sealed partial class TranslatorSystem : EntitySystem
{
    [Dependency] private LanguagesSystem _languages = default!;
    [Dependency] private InventorySystem _inventory = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TranslatorComponent, ImplantRelayEvent<TransformSpeechEvent>>(Translate);
        SubscribeLocalEvent<TranslatorComponent, InventoryRelayedEvent<TransformSpeechEvent>>(OnTranslate);

    }

    private void OnTranslate(Entity<TranslatorComponent> entity, ref InventoryRelayedEvent<TransformSpeechEvent> args)
    {

    }

    private void Translate(Entity<TranslatorComponent> entity, ref ImplantRelayEvent<TransformSpeechEvent> args)
    {

    }

    public bool IsTransladed(EntityUid uid)
    {
        if (HasComp<TranslatorComponent>(uid))
        {
            return true;
        }
        else { return false; }
    }

    public bool CanListenerTranslated(EntityUid listener)
    {
        if (!TryGetTranslato(listener, out var translator))
            return false;

        if (!TryComp<TranslatorComponent>(translator, out var comp))
            return false;

        return comp.ListenTranslate;

    }

    public bool CanSpeakerTranslate(EntityUid speaker)
    {
        if (!TryGetTranslato(speaker, out var translator))
            return false;

        if (!TryComp<TranslatorComponent>(translator, out var comp))
            return false;

        return comp.SpeakTranslation;
    }

    private bool TryGetTranslato(EntityUid owner, out EntityUid? translator)
    {
        translator = null;

        if (!TryComp<InventoryComponent>(owner, out var inventoryComp))
            return false;

        for (var indexer = 0; indexer < inventoryComp.Slots.Length; indexer++)
        {
            var slotEntity = inventoryComp.Containers[indexer].ContainedEntity;

            if (!TryComp<TranslatorComponent>(slotEntity, out var comp))
                continue;

            if (!_inventory.TryGetSlot(owner, comp.Slot, out var slotDef))
                continue;

            if (inventoryComp.Slots[indexer].SlotFlags != slotDef.SlotFlags)
                continue;

            translator = slotEntity;
            return true;

        }

        return false;

    }
}

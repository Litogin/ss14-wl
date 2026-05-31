
using System.Reflection.Emit;

namespace Content.Server._WL.Translator;

[RegisterComponent]
public sealed partial class TranslatorComponent : Component
{
    [DataField]
    public bool SpeakTranslation { get; set; }

    [DataField]
    public bool ListenTranslate { get; set; }

    [DataField]
    public string Slot = "neck";
}

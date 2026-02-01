using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Shared._WL.CCVars;
using Content.Shared.Humanoid.Prototypes;
using Content.Shared.Preferences;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Roles;

/// <summary>
/// Requires the character to be older or younger than a certain age (inclusive)
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class AgeRequirement : JobRequirement
{
    //WL-Changes-start
    [DataField]
    public int? MinAge;

    [DataField]
    public int? MaxAge;

    [DataField]
    public Dictionary<ProtoId<SpeciesPrototype>, int> SpeciesMinAge { get; private set; } = new();

    [DataField]
    public Dictionary<ProtoId<SpeciesPrototype>, int> SpeciesMaxAge { get; private set; } = new();

    //WL-Changes-end

    public override bool Check(
        IEntityManager entManager,
        IPrototypeManager protoManager,
        /*WL-Changes-start*/IConfigurationManager cfgMan,/*WL-Changes-end*/
        HumanoidCharacterProfile? profile,
        /*WL-Changes-start*/JobPrototype? job,/*WL-Changes-end*/
        IReadOnlyDictionary<string, TimeSpan> playTimes,
        [NotNullWhen(false)] out FormattedMessage? reason)
    {
        reason = new FormattedMessage();

        if (profile is null) //the profile could be null if the player is a ghost. In this case we don't need to block the role selection for ghostrole
            return true;

        //WL-Changes-start
        if (job is null)
            return true;

        if (cfgMan.GetCVar(WLCVars.IsAgeCheckNeeded) == false)
            return true;

        var isNeeded = true;
        if (profile.JobUnblockings.ContainsKey(job.ID))
            isNeeded = false;

        var minAgeForSpecies = 0;
        var maxAgeForSpecies = 0;

        if (SpeciesMinAge.TryGetValue(profile.Species, out var minAge))
            minAgeForSpecies = minAge;

        if (SpeciesMaxAge.TryGetValue(profile.Species, out var maxAge))
            maxAgeForSpecies = maxAge;


        if (isNeeded)
        {
            if (minAgeForSpecies != 0 && profile.Age < minAgeForSpecies)
            {
                reason = FormattedMessage.FromMarkupPermissive(Loc.GetString("role-timer-age-too-young",
                    ("age", minAgeForSpecies)));
                return false;
            }
            if (maxAgeForSpecies != 0 && profile.Age > maxAgeForSpecies)
            {
                reason = FormattedMessage.FromMarkupPermissive(Loc.GetString("role-timer-age-too-old",
                    ("age", maxAgeForSpecies)));
                return false;
            }

            if (MinAge != null && profile.Age < MinAge)
            {
                reason = FormattedMessage.FromMarkupPermissive(Loc.GetString("role-timer-age-too-young",
                    ("age", MinAge)));
                return false;
            }
            if (MaxAge != null && profile.Age > MaxAge)
            {
                reason = FormattedMessage.FromMarkupPermissive(Loc.GetString("role-timer-age-too-old",
                    ("age", MaxAge)));
                return false;
            }
        }
        //WL-Changes-end

        return true;
    }
}

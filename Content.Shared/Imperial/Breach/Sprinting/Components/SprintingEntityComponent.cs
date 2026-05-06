using Robust.Shared.GameStates;

namespace Content.Shared.Imperial.Breach.Sprinting.Components;

/// <summary>
/// Gotta go fast!
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]//, Access(typeof(SharedMeleeWeaponSystem))]
public sealed partial class SprintingEntityComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool IsSprinting = false;

    [DataField, AutoNetworkedField]
    public float SprintPerTick = 1f;

}
using Robust.Shared.GameStates;
using Robust.Shared.Input;

namespace Content.Shared.Imperial.Breach.GunZoom.Components;

/// <summary>
/// Go into ZOOOM mode when alt clicked
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]//, Access(typeof(SharedMeleeWeaponSystem))]
public sealed partial class GunZoomComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool InZoom = false;

}
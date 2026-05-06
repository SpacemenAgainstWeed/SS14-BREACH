using Content.Shared.Imperial.Breach.GunZoom.Components;
using Content.Shared.Imperial.Breach.GunZoom.Events;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Hands.Components;
using Robust.Shared.Utility;
using System.Diagnostics.CodeAnalysis;
using Content.Shared.Timing;
using Content.Shared.Camera;
using Content.Shared.Hands;

namespace Content.Shared.Imperial.Breach.GunZoom;

public abstract partial class SharedGunZoomSystem : EntitySystem
{
    [Dependency] private readonly SharedHandsSystem _handsSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<UpdateZoomStateEvent>(OnUpdateKeyState);
    }

    private void OnUpdateKeyState(UpdateZoomStateEvent ev, EntitySessionEventArgs args)
    {
        if (!GetGunZoomInHands(GetEntity(ev.User), out var gun, out var zoomComp))
            return;

        
    }

    protected bool GetGunZoomInHands(EntityUid user, [NotNullWhen(true)] out EntityUid? gun, [NotNullWhen(true)] out GunZoomComponent? comp)
    {
        gun = null;
        comp = null;

        if (!TryComp<HandsComponent>(user, out var handsComp))
            return false;

        if(!_handsSystem.TryGetActiveItem((user, handsComp), out var heldEntity))
            return false;

        if (TryComp(heldEntity, out comp))
        {
            gun = heldEntity;
            return true;
        }

        return false;
    }
}
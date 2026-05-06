using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using static Robust.Shared.Input.Binding.PointerInputCmdHandler;
using Robust.Client.Player;
using Content.Shared.Imperial.Breach.GunZoom.Components;
using Content.Shared.Imperial.Breach.GunZoom;
using Content.Shared.Imperial.Breach.GunZoom.Events;
using Content.Client.Movement.Systems;
using Content.Shared.Camera;
using Content.Shared.Hands;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Hands.Components;
using Content.Shared.Movement.Components;
using Content.Client.Movement.Components;
using Robust.Shared.Log;

namespace Content.Client.Imperial.Breach.GunZoom;

public sealed partial class ClientGunZoomSystem : SharedGunZoomSystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly EyeCursorOffsetSystem _eyeOffset = default!;
    [Dependency] private readonly ContentEyeSystem _eye = default!;

    public override void Initialize()
    {
        base.Initialize();

        CommandBinds.Builder
            .Bind(EngineKeyFunctions.UseSecondary,
                new PointerInputCmdHandler(OnUse))
            .Register<ClientGunZoomSystem>();

        SubscribeLocalEvent<GunZoomComponent, HeldRelayedEvent<GetEyeOffsetRelayedEvent>>(OnGetEyePvsScale);
    }

    private bool OnUse(in PointerInputCmdHandler.PointerInputCmdArgs args)
    {
        var user = _player.LocalEntity;
        if (user == null)
            return false;

        if (!GetGunZoomInHands(user.Value, out var gun, out var zoomComp))
            return false;

        if (args.State == BoundKeyState.Down)
        {
            Logger.Debug("НАЖАТО");

            if (!zoomComp.InZoom)
                zoomComp.InZoom = true;
            else if (zoomComp.InZoom)
                zoomComp.InZoom = false;

            _eye.UpdatePvsScale(user.Value);

            RaiseNetworkEvent(new UpdateZoomStateEvent(GetNetEntity(user.Value)));

            return true;
        }
        return false;
    }

    private void OnGetEyePvsScale(Entity<GunZoomComponent> entity, ref HeldRelayedEvent<GetEyeOffsetRelayedEvent> args)
    {
        if (!TryComp(entity, out EyeCursorOffsetComponent? eyeCursorOffset) || !TryComp(entity.Owner, out GunZoomComponent? zoomComp))
            return;
        if (!zoomComp.InZoom)
            return;
        var offset = _eyeOffset.OffsetAfterMouse(entity.Owner, null);
        if (offset == null)
            return;
        args.Args.Offset += offset.Value;
    }
}
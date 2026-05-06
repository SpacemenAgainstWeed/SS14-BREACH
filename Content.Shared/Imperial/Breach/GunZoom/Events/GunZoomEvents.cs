using Content.Shared.Imperial.Breach.GunZoom.Components;
using Robust.Shared.Input;
using Robust.Shared.Serialization;

namespace Content.Shared.Imperial.Breach.GunZoom.Events;

[Serializable, NetSerializable]
public sealed class UpdateZoomStateEvent : EntityEventArgs
{
    public NetEntity User;

    public UpdateZoomStateEvent(NetEntity user)
    {
        User = user;
    }
}
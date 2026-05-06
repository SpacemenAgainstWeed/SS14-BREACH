using Robust.Shared.Utility;
using Content.Shared.Imperial.Breach.Sprinting.Components;
using Robust.Shared.Timing;
using Content.Shared.Movement.Systems;
using Content.Shared.Damage.Components;
using Content.Shared.Movement.Components;
using Content.Shared.Damage.Systems;
using Content.Shared.Weapons.Ranged.Events;

namespace Content.Shared.Imperial.Breach.Sprinting;

public abstract partial class SharedSprintingEntitySystem : EntitySystem
{
    [Dependency] private readonly SharedStaminaSystem _stam = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SprintingEntityComponent, ShotAttemptedEvent>(OnShotAttempted);
    }
    private void OnShotAttempted(Entity<SprintingEntityComponent> ent, ref ShotAttemptedEvent args)
    {
        if (ent.Comp.IsSprinting)
            args.Cancel();
    }
    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<SprintingEntityComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            var stamComp = EnsureComp<StaminaComponent>(uid);
            var moveComp = EnsureComp<InputMoverComponent>(uid);

            if (stamComp.StaminaDamage >= stamComp.BaseCritThreshold - (stamComp.BaseCritThreshold * 0.09f))
            {
                continue;
            }

            // НЕ ПУГАЕМСЯ И НЕ ПУТАЕМСЯ, НА ШИФТ МЫ "КРАДЕМСЯ" ПО КОДУ, НО ПО ФАКТУ СКОРОСТЬ УВЕЛИЧИВАЕТСЯ
            if (!moveComp.Sprinting)
            {
                comp.IsSprinting = true;
                _stam.TryTakeStamina(uid, comp.SprintPerTick * frameTime, stamComp);
                continue;
            }

            comp.IsSprinting = false;
        }

    }
}
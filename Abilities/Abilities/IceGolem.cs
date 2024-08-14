using Terraria.ID;
using TShockAPI;

namespace Abilities
{
    public class IceGolem : Ability
    {
        public int BuffDurationInTicks;

        public IceGolem(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats = () =>
            {
                BuffDurationInTicks = 540 + 120 * AbilityLevel;
            };
        }

        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr);

            plr.SetBuff(BuffID.Dazed, BuffDurationInTicks);
            plr.SetBuff(BuffID.Chilled, BuffDurationInTicks);
            plr.SetBuff(BuffID.OgreSpit, BuffDurationInTicks);
            plr.SetBuff(BuffID.Ironskin, BuffDurationInTicks);
            plr.SetBuff(BuffID.NebulaUpLife1, BuffDurationInTicks);
            plr.SetBuff(BuffID.RapidHealing, BuffDurationInTicks);
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];
            Utils.SpawnProjectile(plr.X + 16, plr.Y + 16, 0, 0, ProjectileID.StardustGuardianExplosion, 0, 0);
        }
    }
}
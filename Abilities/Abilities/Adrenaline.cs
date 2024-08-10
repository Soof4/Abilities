using Terraria;
using Terraria.ID;
using TShockAPI;

namespace Abilities
{
    public class Adrenaline : Ability
    {
        public int BuffDurationInTicks;


        public Adrenaline(int abilityLevel) : base(abilityLevel) { }


        internal override void CalculateProperties()
        {
            BuffDurationInTicks = (int)(10 + 1.5 * (AbilityLevel - 1)) * 60;
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr);

            plr.SetBuff(BuffID.Swiftness, BuffDurationInTicks, true);
            plr.SetBuff(BuffID.Panic, BuffDurationInTicks, true);
            plr.SetBuff(BuffID.SugarRush, BuffDurationInTicks, true);
            plr.SetBuff(BuffID.Sunflower, BuffDurationInTicks, true);
            plr.SetBuff(BuffID.NebulaUpDmg1, BuffDurationInTicks, true);
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];

            Projectile.NewProjectile(Projectile.GetNoneSource(),
                        new(plr.X + 16, plr.Y + 16), new(0, 0),
                        Type: 443, Damage: 0, KnockBack: 0,
                        ai0: 0, ai1: 0, ai2: 0);
        }
    }
}
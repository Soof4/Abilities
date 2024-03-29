using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;

namespace Abilities
{
    public class Witch : Ability
    {
        private int RangeInBlocks;
        private List<int> BuffTypes = new List<int>();

        public Witch(int abilityLevel) : base(abilityLevel) { }

        internal override void CalculateProperties()
        {
            RangeInBlocks = 16 + (AbilityLevel - 1) * 2;
            BuffTypes = new List<int>();

            switch (AbilityLevel)
            {
                case 1:
                    BuffTypes.Add(BuffID.Frostburn);
                    break;
                case 2:
                    BuffTypes.Add(BuffID.Frostburn);
                    BuffTypes.Add(BuffID.Bleeding);
                    break;
                case 3:
                    BuffTypes.Add(BuffID.ShadowFlame);
                    break;
                case 4:
                    BuffTypes.Add(BuffID.Venom);
                    break;
                default:
                    BuffTypes.Add(BuffID.ShadowFlame);
                    BuffTypes.Add(BuffID.Venom);
                    break;
            }
        }

        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr);

            foreach (NPC npc in Main.npc)
            {
                if (npc != null && npc.active && npc.type != 0 && npc.position.WithinRange(plr.TPlayer.position, RangeInBlocks * 16))
                {
                    foreach (int buffType in BuffTypes)
                    {
                        npc.AddBuff(buffType, 60 * 30);
                    }
                    TSPlayer.All.SendData(PacketTypes.NpcUpdateBuff, number: npc.whoAmI);
                }
            }
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];

            Projectile.NewProjectile(Projectile.GetNoneSource(),
                       new(plr.X + 16, plr.Y - 16 * 4), new(0, 0),
                       Type: ProjectileID.DD2DarkMageRaise, Damage: 0, KnockBack: 0,
                       ai0: 0);
            Projectile.NewProjectile(Projectile.GetNoneSource(),
                       new(plr.X + 16, plr.Y - 16 * 4), new(0, 0),
                       Type: ProjectileID.PrincessWeapon, Damage: 0, KnockBack: 0,
                       ai0: 0);

            Shapes.DrawCircle(plr.X + 8, plr.Y + 24, RangeInBlocks * 16, 0.1963, ParticleOrchestraType.PrincessWeapon, (byte)plr.Index);
        }
    }
}
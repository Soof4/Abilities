using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;
using System.Drawing;

namespace Abilities
{
    // Author of this ability is @strangelad on Discord
    public class Shockwave : Ability
    {
        public int BaseDmg, Size, Knockback;

        public Shockwave(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats = () =>
            {
                BaseDmg = (int)((50 + (AbilityLevel - 1) * 20) * (1 + (AbilityLevel - 1) / 5f));
                Size = (int)(100 + (AbilityLevel - 1) * 10);
                Knockback = (int)(25 + (AbilityLevel - 1) * 10);
            };
        }

        protected override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr);
            Vector2 plrPos = new Vector2(plr.X, plr.Y) - new Vector2(16, 40);

            foreach (NPC npc in Main.npc)
            {
                if (Utils.CanDamageThisEnemy(npc) && npc.position.WithinRange(plrPos, Size))
                {
                    float distance = plrPos.Distance(npc.position);
                    TSPlayer.Server.StrikeNPC(npc.whoAmI, (int)Math.Max(BaseDmg - (distance / 8), BaseDmg / 5), 0, 0);
                    npc.velocity = npc.position.DirectionTo(plrPos) * (Math.Max(Knockback - (distance / 8), 10) * -1) + new Vector2(0, 160);
                    NetMessage.SendData((int)PacketTypes.NpcUpdate, -1, -1, null, npc.whoAmI);
                }
            }

            foreach (TSPlayer aplr in TShock.Players)
            {
                if (aplr.IsAlive() && aplr.TPlayer.position.WithinRange(plrPos, Size) && Utils.CanDamageThisPlayer(plr, aplr) == true)
                {
                    float distance = plrPos.Distance(aplr.TPlayer.position);
                    aplr.DamagePlayer((int)Math.Max(BaseDmg - (distance / 8), BaseDmg / 5));
                    aplr.TPlayer.velocity = aplr.TPlayer.position.DirectionTo(plrPos) * (Math.Max(Knockback - (distance / 8), 10) * -1) + new Vector2(0, 160);
                    TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", plr.Index);
                }
            }

            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.hostile && proj.position.WithinRange(plrPos, Size))
                {
                    int hp = Utils.Random.Next(0, 4);
                    Utils.SpawnProjectile(proj.position.X, proj.position.Y, 0, 0, ProjectileID.SpiritHeal, 0, 0, plr.Index, plr.Index, hp);
                    proj.type = 0;
                    NetMessage.SendData((int)PacketTypes.ProjectileNew, -1, -1, null, proj.whoAmI);
                }
            }
        }

        protected override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];
            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                PositionInWorld = new(plr.X + 16, plr.Y + 40),
                UniqueInfoPiece = ItemID.FragmentStardust
            };

            for (double i = 0; i < Math.Tau; i += 0.174533)
            {
                settings.MovementVector = new Vector2(
                    new Vector2(plr.X, plr.Y).DirectionTo(new Vector2(plr.X + 10 * (float)Math.Cos(i), plr.Y + 10 * (float)Math.Sin(i))).X * (Size + 50),
                    new Vector2(plr.X, plr.Y).DirectionTo(new Vector2(plr.X + 10 * (float)Math.Cos(i), plr.Y + 10 * (float)Math.Sin(i))).Y * (Size + 50));
                ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
            }
        }
    }
}

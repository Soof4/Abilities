using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;

namespace Abilities
{
    // Author of this ability is @strangelad on Discord
    public class HyperCrit : Ability
    {
        public static Dictionary<byte, int> HyperCritActive = new();
        public static Dictionary<byte, bool> HyperCritCooldown = new();

        private int Duration, ExtraChance;
        public HyperCrit(int abilityLevel)
        {
            CalculateProperties(abilityLevel);
        }


        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                Duration = 12 + ((abilityLevel - 1) * 4);
                ExtraChance = 3 + abilityLevel;
            }
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            CalculateProperties(abilityLevel);
            PlayVisuals(plr);
            plr.SetBuff(BuffID.SwordWhipNPCDebuff, Duration * 60, false);
            HyperCritActive.Add((byte)plr.Index, ExtraChance);
            Task.Run(async () =>
            {
                await Task.Delay(Duration * 1000);
                HyperCritActive.Remove((byte)plr.Index);
            });
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];

            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                MovementVector = new(0, -8),
                PositionInWorld = new(plr.X + 16, plr.Y + 16)
            };
            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerArrow, settings);
            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
        }

        public static void HyperCritHit(Vector2 pos, int DMG, int owner)
        {
            Extensions.SpawnProjectile(pos.X, pos.Y, 0, 0, ProjectileID.DD2ExplosiveTrapT3Explosion, DMG, 0, owner);
            Extensions.MakeSound(pos, 105, 68, 1.5f, 0f);

            Extensions.SpawnProjectile(pos.X, pos.Y, 6, -6, ProjectileID.LightsBane, 0, 0, -1, 1f);
            Extensions.SpawnProjectile(pos.X, pos.Y, 6, 6, ProjectileID.LightsBane, 0, 0, -1, 1f);
            Extensions.SpawnProjectile(pos.X, pos.Y, -6, 6, ProjectileID.LightsBane, 0, 0, -1, 1f);
            Extensions.SpawnProjectile(pos.X, pos.Y, -6, -6, ProjectileID.LightsBane, 0, 0, -1, 1f);
        }

    }
}

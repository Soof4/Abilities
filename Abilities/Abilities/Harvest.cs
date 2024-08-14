using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;

namespace Abilities
{
    public class Harvest : Ability
    {
        public int Damage, ProjSpawnInterval;

        public Harvest(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats = () =>
            {
                Damage = (int)(5 * AbilityLevel * (3 + Math.Pow(AbilityLevel, 5 / 2f) / 10f));
                ProjSpawnInterval = (int)(1000 / Math.Sqrt(AbilityLevel + 1));
            };
        }

        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr);

            Task.Run(async () =>
            {
                int loopsTillBuffLoop = 0;
                int ms = 0;
                while (ms < 10000)
                {
                    if (loopsTillBuffLoop == 0)
                    {
                        foreach (NPC npc in Main.npc)
                        {
                            if (npc != null && npc.active && npc.position.WithinRange(plr.TPlayer.position, 16 * 16))
                            {
                                npc.AddBuff(BuffID.ScytheWhipEnemyDebuff, 10);
                                TSPlayer.All.SendData(PacketTypes.NpcUpdateBuff, number: npc.whoAmI);
                            }
                        }
                        loopsTillBuffLoop = 6;
                    }
                    float randX = plr.X + 16 + Utils.Random.Next(-64, 64);
                    float randY = plr.Y + 16 + Utils.Random.Next(-64, 64);
                    Utils.SpawnProjectile(randX, randY, 0, 0, ProjectileID.ScytheWhipProj, Damage, 4, plr.Index);
                    ms += ProjSpawnInterval;
                    loopsTillBuffLoop--;
                    await Task.Delay(ProjSpawnInterval);
                }
            });
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];

            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                MovementVector = new(0, -8),
                PositionInWorld = new(plr.X + 16, plr.Y - 16),
                UniqueInfoPiece = ItemID.ScytheWhip
            };

            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
            settings.PositionInWorld.Y = plr.Y + 16;
            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
        }
    }
}
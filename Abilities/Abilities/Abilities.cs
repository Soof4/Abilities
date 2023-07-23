using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using TShockAPI;
using dw = Terraria.GameContent.Drawing;

namespace Abilities {
    public class Abilities {
        
        public static void DryadsRingOfHealing(TSPlayer caster) {

            dw.ParticleOrchestraSettings settings = new() {
                IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(caster.X, caster.Y),
                UniqueInfoPiece = 1
            };

            Shapes.DrawCircle(caster.X + 16, caster.Y + 24, 256, 0.1963, ParticleOrchestraType.ChlorophyteLeafCrystalPassive, (byte)caster.Index);

            foreach (TSPlayer plr in TShock.Players) {
                if (plr != null && plr.Active && plr.TPlayer.position.WithinRange(caster.TPlayer.position, 256)) {
                    plr.SetBuff(165, 600, true);
                    plr.Heal(5);
                }
            }
        }

        public static void TotemOfUndying(TSPlayer caster) {
            dw.ParticleOrchestraSettings settings = new() {
                IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(caster.X + 16, caster.Y + 16),
                UniqueInfoPiece = 1171
            };
            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.WaffleIron, settings);
            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings);
            settings.PositionInWorld = new(caster.X + 8, caster.Y - 16);
            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);

            TSPlayer.All.SendData(PacketTypes.PlayerSpawn, number: caster.Index, number2: caster.X, number3: caster.Y, number4: 0, number5: 0);
            caster.PlayerData.health = (int)(caster.PlayerData.maxHealth * 0.10);
            caster.SendData(PacketTypes.PlayerHp, number: caster.Index, number2: caster.PlayerData.health, number3: caster.PlayerData.maxHealth);
        }

        public static void RingOfDracula(TSPlayer caster) {
            dw.ParticleOrchestraSettings settings = new() {
                IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                MovementVector = new(0, -8),
                PositionInWorld = new(caster.X + 16, caster.Y + 16),
                UniqueInfoPiece = 3999
            };

            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.WaffleIron, settings);
            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
            settings.PositionInWorld = new(caster.X + 8, caster.Y - 24);
            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);

            Shapes.DrawCircle(caster.X + 16, caster.Y + 16, 384, 0.09817, ParticleOrchestraType.BlackLightningSmall, (byte)caster.Index);

            double healAmount = 1;

            foreach (TSPlayer plr in TShock.Players) {
                if (plr != null && plr.Active && plr.Index != caster.Index && plr.TPlayer.position.WithinRange(caster.TPlayer.position, 384)) {
                    healAmount += plr.PlayerData.health * 0.02;
                    plr.DamagePlayer((int)(plr.PlayerData.health * 0.02));
                }
            }
            caster.Heal((int)healAmount);

        }

        public static void SandFrames(TSPlayer caster) {
            dw.ParticleOrchestraSettings settings = new() {
                IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(caster.X + 16, caster.Y + 16),
                UniqueInfoPiece = 1
            };

            Projectile.NewProjectile(Projectile.GetNoneSource(),
                       caster.TPlayer.position, new(0,0),
                       Type: 704, Damage: 0, KnockBack: 0,
                       ai0: 0, ai1: 0, ai2: 0);

            if (!AbilityExtentions.SandFrameTicks.ContainsKey((byte)caster.Index)) {
                AbilityExtentions.SandFrameTicks.Add((byte)caster.Index, 220);
            }
        }

        public static void FlashBoy(TSPlayer caster) {
            Projectile.NewProjectile(Projectile.GetNoneSource(),
                        new(caster.X + 16, caster.Y + 16), new(0, 0),
                        Type: 443, Damage: 0, KnockBack: 0,
                        ai0: 0, ai1: 0, ai2: 0);

            caster.SetBuff(BuffID.Swiftness, 600, true);
            caster.SetBuff(BuffID.Panic, 600, true);
            caster.SetBuff(BuffID.SugarRush, 600, true);
            caster.SetBuff(BuffID.Sunflower, 600, true);
        }

        public static void Whipper(TSPlayer caster) {
            Projectile.NewProjectile(Projectile.GetNoneSource(),
                        new(caster.X + 16, caster.Y + 16), new(0,0),
                        Type: 950, Damage: 0, KnockBack: 0,
                        ai0: 0, ai1: 0, ai2: 0);
            caster.SetBuff(BuffID.ThornWhipPlayerBuff, 600, true);
        }
    }

    public class AbilityExtentions {
        internal static Dictionary<byte, int> SandFrameTicks = new();

        /// <summary>
        /// Register this into ServerApi.Hooks.GameUpdate in order to use SandFrames ability.
        /// </summary>
        public static void SandFrameTicker(EventArgs e) {
            foreach (var kvp in SandFrameTicks) {
                SandFrameTicks[kvp.Key]--;

                if (TShock.Players[kvp.Key].TPlayer.shadowDodgeCount < 0.01) {
                    TShock.Players[kvp.Key].SendData(PacketTypes.PlayerDodge, number: kvp.Key, number2: 2);
                }
                if (SandFrameTicks[kvp.Key] < 1) {
                    SandFrameTicks.Remove(kvp.Key);
                }
            }
        }
    }

    public enum AbilityType {
        DryadsRingOfHealing,
        TotemOfUndying,
        RingOfDracula,
        KinkyWhipper
    }
}

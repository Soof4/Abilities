using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using TShockAPI;
using dw = Terraria.GameContent.Drawing;
using OTAPI;
using System;
using System.Linq;
using System.IO.Streams;

namespace Abilities {
    public class Abilities {
        
        public static void DryadsRingOfHealing(TSPlayer caster) {
            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index)) {
                return;
            }
            caster.SetBuff(307, 5400, true);
            #endregion

            #region Visuals
            dw.ParticleOrchestraSettings settings = new() {
                IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(caster.X, caster.Y),
                UniqueInfoPiece = 1
            };

            Shapes.DrawCircle(caster.X + 16, caster.Y + 24, 256, 0.1963, ParticleOrchestraType.ChlorophyteLeafCrystalPassive, (byte)caster.Index);
            #endregion

            #region Functionality
            foreach (TSPlayer plr in TShock.Players) {
                if (plr != null && plr.Active && !plr.Dead && plr.TPlayer.position.WithinRange(caster.TPlayer.position, 256)) {
                    plr.SetBuff(165, 420, true);
                    plr.Heal((int)(plr.PlayerData.maxHealth * 0.05));
                }
            }
            #endregion
        }

        public static void TotemOfUndying(TSPlayer caster) {
            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index)) {
                return;
            }
            caster.SetBuff(307, 5400, true);
            #endregion

            #region Visuals
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
            #endregion

            #region Functionality
            TSPlayer.All.SendData(PacketTypes.PlayerSpawn, number: caster.Index, number2: caster.X, number3: caster.Y, number4: 0, number5: 0);
            caster.PlayerData.health = (int)(caster.PlayerData.maxHealth * 0.10);
            caster.SendData(PacketTypes.PlayerHp, number: caster.Index, number2: caster.PlayerData.health, number3: caster.PlayerData.maxHealth);
            #endregion
        }

        public static void RingOfDracula(TSPlayer caster) {
            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index)) {
                return;
            }
            caster.SetBuff(307, 5400, true);
            #endregion

            #region Visuals
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

            Shapes.DrawCircle(caster.X + 16, caster.Y + 16, 384, 0.1963, ParticleOrchestraType.BlackLightningSmall, (byte)caster.Index);
            #endregion

            #region Functionality
            int healAmount = 0;

            foreach (NPC npc in Main.npc) {
                if (npc != null && npc.active && npc.type != 0 && npc.type != NPCID.TargetDummy && npc.position.WithinRange(caster.TPlayer.position, 384)) {
                    healAmount += (int)(0.2 * Math.Pow(npc.life, 9 / 17f));
                    npc.life -= (int)(0.2 * Math.Pow(npc.life, 9 / 17f));
                    NetMessage.SendData((int)PacketTypes.NpcUpdate, -1, -1, null, npc.whoAmI);
                }
            }
            caster.Heal((int)healAmount);
            #endregion
        }


        /// <summary>
        /// Register AbilityExtentions.SandFrames into ServerApi.Hooks.GameUpdate in order to use this ability.
        /// </summary>
        public static void SandFrames(TSPlayer caster) {
            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index)) {
                return;
            }
            caster.SetBuff(307, 5400, true);
            #endregion

            #region Visuals
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
            #endregion

            #region Functionality
            if (!AbilityExtentions.SandFrameTicks.ContainsKey((byte)caster.Index)) {
                AbilityExtentions.SandFrameTicks.Add((byte)caster.Index, 100);
            }
            #endregion
        }

        public static void FlashBoy(TSPlayer caster) {
            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index)) {
                return;
            }
            caster.SetBuff(307, 5400, true);
            #endregion

            #region Visuals
            Projectile.NewProjectile(Projectile.GetNoneSource(),
                        new(caster.X + 16, caster.Y + 16), new(0, 0),
                        Type: 443, Damage: 0, KnockBack: 0,
                        ai0: 0, ai1: 0, ai2: 0);

            #endregion

            #region Functionality
            caster.SetBuff(BuffID.Swiftness, 600, true);
            caster.SetBuff(BuffID.Panic, 600, true);
            caster.SetBuff(BuffID.SugarRush, 600, true);
            caster.SetBuff(BuffID.Sunflower, 600, true);
            #endregion
        }

        public static void Witch(TSPlayer caster) {
            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index)) {
                return;
            }
            caster.SetBuff(307, 5400, true);
            #endregion

            #region Visuals
            Projectile.NewProjectile(Projectile.GetNoneSource(),
                       new(caster.X + 16, caster.Y - 16 * 4),new(0, 0),
                       Type: ProjectileID.DD2DarkMageRaise, Damage: 0, KnockBack: 0,
                       ai0: 0);
            Projectile.NewProjectile(Projectile.GetNoneSource(),
                       new(caster.X + 16, caster.Y - 16 * 4), new(0, 0),
                       Type: ProjectileID.PrincessWeapon, Damage: 0, KnockBack: 0,
                       ai0: 0);

            Shapes.DrawCircle(caster.X + 8, caster.Y + 24, 256, 0.1963, ParticleOrchestraType.PrincessWeapon, (byte)caster.Index);
            #endregion

            #region Functionality
            foreach (NPC npc in Main.npc) {
                if (npc != null && npc.active && npc.type != 0 && npc.position.WithinRange(caster.TPlayer.position, 256)) {
                    npc.AddBuff(BuffID.Frostburn, 60*30);
                    TSPlayer.All.SendData(PacketTypes.NpcUpdateBuff, number: npc.whoAmI);
                }
            }
            #endregion
        }


    }

    public class AbilityExtentions {
        internal static Dictionary<byte, DateTime> Cooldowns = new();
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

        internal static bool IsInCooldown(byte casterId) {
            if (!Cooldowns.ContainsKey(casterId)) {
                Cooldowns.Add(casterId, DateTime.UtcNow);
            }
            else if ((DateTime.UtcNow - Cooldowns[casterId]).TotalSeconds < 90) {
                return true;
            }
            else {
                Cooldowns[casterId] = DateTime.UtcNow;
            }

            return false;
        }
    }

    public enum AbilityType {
        DryadsRingOfHealing,
        RingOfDracula,
        SandFrames,
        FlashBoy,
        Witch
    }
}

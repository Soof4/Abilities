using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using TShockAPI;
using dw = Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;
using TerrariaApi.Server;

namespace Abilities {
    public class Abilities {
        
        public static void DryadsRingOfHealing(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Properties
            int buffDurationInTicks = 400 + 20 * abilityLevel;
            double healPercentage = 0.04 + abilityLevel * 0.01;
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown)) {
                return;
            }
            #endregion

            #region Visuals
            dw.ParticleOrchestraSettings settings = new() {
                IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(caster.X, caster.Y),
                UniqueInfoPiece = 1
            };

            Shapes.DrawCircle(caster.X + 16, caster.Y + 24, 384, 0.1963, ParticleOrchestraType.ChlorophyteLeafCrystalPassive, (byte)caster.Index);
            #endregion

            #region Functionality
            foreach (TSPlayer plr in TShock.Players) {
                if (plr != null && plr.Active && !plr.Dead && plr.TPlayer.position.WithinRange(caster.TPlayer.position, 384)) {
                    plr.SetBuff(165, buffDurationInTicks, true);
                    plr.Heal((int)(plr.PlayerData.maxHealth * healPercentage));
                }
            }
            #endregion
        }


        /// <summary>
        /// This ability is deprecated.
        /// </summary>
        public static void TotemOfUndying(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown)) {
                return;
            }
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

        public static void RingOfDracula(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Properties
            double healFactorIncrease = (abilityLevel - 1) * 0.01;
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown)) {
                return;
            }
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
                    int stolenHealth = (int)((0.2 + healFactorIncrease) * Math.Pow(npc.life, 9 / 17f));
                    healAmount += stolenHealth;
                    npc.life -= stolenHealth;
                    NetMessage.SendData((int)PacketTypes.NpcUpdate, -1, -1, null, npc.whoAmI);
                }
            }
            caster.Heal((int)healAmount);
            #endregion
        }

        public static void SandFrames(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Properties
            double dodgeDurationInSecs = 3 + abilityLevel * 0.2;
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown)) {
                return;
            }
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
            Task.Run(async () => {
                while (dodgeDurationInSecs >= 1.33) {
                    caster.SendData(PacketTypes.PlayerDodge, number: caster.Index, number2: 2);
                    dodgeDurationInSecs -= 0.1;
                    await Task.Delay(100);
                }
            });
            #endregion
        }

        public static void Adrenaline(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Properties
            int buffDurationInTicks = (10 + 5 * (abilityLevel - 1))*60;
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown)) {
                return;
            }
            #endregion

            #region Visuals
            Projectile.NewProjectile(Projectile.GetNoneSource(),
                        new(caster.X + 16, caster.Y + 16), new(0, 0),
                        Type: 443, Damage: 0, KnockBack: 0,
                        ai0: 0, ai1: 0, ai2: 0);

            #endregion

            #region Functionality
            caster.SetBuff(BuffID.Swiftness, buffDurationInTicks, true);
            caster.SetBuff(BuffID.Panic, buffDurationInTicks, true);
            caster.SetBuff(BuffID.SugarRush, buffDurationInTicks, true);
            caster.SetBuff(BuffID.Sunflower, buffDurationInTicks, true);
            caster.SetBuff(BuffID.NebulaUpDmg1, buffDurationInTicks, true);
            #endregion
        }

        public static void Witch(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Properties
            int rangeInBlocks = 16 + (abilityLevel - 1) * 2;
            int buffType = BuffID.Frostburn;

            if (abilityLevel > 4) {
                buffType = BuffID.Venom;
            }
            else if (abilityLevel > 2) {
                buffType = BuffID.ShadowFlame;
            }

            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown)) {
                return;
            }
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
                if (npc != null && npc.active && npc.type != 0 && npc.position.WithinRange(caster.TPlayer.position, rangeInBlocks * 16)) {
                    npc.AddBuff(buffType, 60*30);
                    TSPlayer.All.SendData(PacketTypes.NpcUpdateBuff, number: npc.whoAmI);
                }
            }
            #endregion
        }

        /// <summary>
        /// This ability is deprecated.
        /// </summary>
        public static void DragonMaster(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Properties
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown)) {
                return;
            }
            #endregion
        
            #region Visuals
            #endregion
        
            #region Functionality
            
            Projectile.NewProjectile(Projectile.GetNoneSource(), 
                new(caster.X + WorldGen.genRand.Next(-64, 64), caster.Y - WorldGen.genRand.Next(48)),
                new(0,0), 
                ProjectileID.StardustDragon1,
                200, 0, caster.Index, 0, 4, 0);
            #endregion
        }

        public static void Marthymr(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Properties
            float speedFactor = 9 + abilityLevel;
            int damage = 250 + 100 * abilityLevel;
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown)) {
                return;
            }
            #endregion
            
            #region Visuals
            #endregion
            
            #region Functionality
            for (double i = 0; i < Math.Tau; i += 0.3926) {
                Projectile.NewProjectile(Projectile.GetNoneSource(), 
                    new Vector2(caster.X + 16*(float)Math.Cos(i), caster.Y + 16*(float)Math.Sin(i)),
                    new Vector2(speedFactor * (float)Math.Cos(i), speedFactor * (float)Math.Sin(i)), 
                    684, damage, 30);
            }
            #endregion
        }

        public static void RandomTeleport(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Properties
            int rangeInBlocks = 50 + abilityLevel * 10;
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown)) {
                return;
            }
            #endregion
            
            #region Visuals
            #endregion
            
            #region Functionality
            int d = WorldGen.genRand.Next(160, rangeInBlocks * 16);
            double tetha = WorldGen.genRand.NextDouble() * Math.Tau;
            float x = d * (float)Math.Cos(tetha);
            float y = d * (float)Math.Sin(tetha);
            
            caster.Teleport(x + caster.X, y + caster.Y);
            #endregion
        
        }

        public static void EmpressOfLight(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Properties
            int lanceDmg = 50 + (abilityLevel - 1) * 15;
            int dashDmg = 100 + (abilityLevel - 1) * 30;
            int boltDmg = 10 + (abilityLevel - 1) * 10;
            int boltSpawnInterval = 20 - abilityLevel * 2;
            int danceDmg = 55 + (abilityLevel - 1) * 20;
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown)) {
                return;
            }
            #endregion

            #region Visuals
            #endregion

            #region Functionality
            if (!AbilityExtentions.EmpressCycles.ContainsKey(caster.Name)) {
                AbilityExtentions.EmpressCycles.Add(caster.Name, 1);
            }

            switch (AbilityExtentions.EmpressCycles[caster.Name]) {
                case 1:    // Ethereal Lance
                    float startingYPos = caster.Y - 8 * 16;
                    for (int i = 0; i < 19; i+=3) {

                        Projectile.NewProjectile(Projectile.GetNoneSource(),
                            new Vector2(caster.X + (i % 2 * 60 - 30) * 16, startingYPos + i * 16),
                            new Vector2(i % 2 * 60 * -1 + 30, 0),
                            932, lanceDmg, 5);
                    }
                    break;
                case 2:    // Dash
                    caster.SendData(PacketTypes.PlayerDodge, number: (byte)caster.Index, number2: 2);

                    caster.TPlayer.velocity.X = caster.TPlayer.direction * 45;
                    TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", caster.Index);

                    Task.Run(async () => {
                        for (int i=0; i<15; i++) {
                            Projectile.NewProjectile(Projectile.GetNoneSource(),
                                new Vector2(caster.X + 16, caster.Y + 1 % 2 * 32),
                                new Vector2(0, 0),
                                929, dashDmg, 14);
                            await Task.Delay(14);
                        }
                        caster.TPlayer.velocity.X = caster.TPlayer.maxRunSpeed * caster.TPlayer.direction;
                        TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", caster.Index);
                    });
                    break;
                case 3:    // Prismatic Bolts
                    Task.Run(async () => {
                        int ms = 0;
                        while(ms < 2500) {
                            Projectile.NewProjectile(Projectile.GetNoneSource(),
                                new Vector2(caster.X + WorldGen.genRand.Next(33), caster.Y + WorldGen.genRand.Next(-8, 33)),
                                new Vector2(WorldGen.genRand.Next(-4, 5), WorldGen.genRand.Next(-4, 5)),
                                931, boltDmg, 10);

                            ms += 300;
                            await Task.Delay(300);
                        }
                        
                    });
                    break;
                case 4:    // Sundance
                    caster.SetBuff(BuffID.Webbed, 180, true);

                    Task.Run(async () => {
                        for (double i = 0.7853; i < Math.Tau; i += 1.5707) {
                            Projectile.NewProjectile(Projectile.GetNoneSource(),
                                new Vector2(caster.X + 16 + 64 * (float)Math.Cos(i), caster.Y + 16 + 64 * (float)Math.Sin(i)),
                                new Vector2(2 * (float)Math.Cos(i), 2 * (float)Math.Sin(i)),
                                950, danceDmg, 5);
                        }
                        await Task.Delay(500);
                        for (double i = 1.5707; i < Math.Tau; i += 1.5707) {
                            Projectile.NewProjectile(Projectile.GetNoneSource(),
                                new Vector2(caster.X + 16 + 64 * (float)Math.Cos(i), caster.Y + 16 + 64 * (float)Math.Sin(i)),
                                new Vector2(2 * (float)Math.Cos(i), 2 * (float)Math.Sin(i)),
                                950, danceDmg, 5);
                        }
                        await Task.Delay(500);
                        for (double i = 0.7853; i < Math.Tau; i += 1.5707) {
                            Projectile.NewProjectile(Projectile.GetNoneSource(),
                                new Vector2(caster.X + 16 + 64 * (float)Math.Cos(i), caster.Y + 16 + 64 * (float)Math.Sin(i)),
                                new Vector2(2 * (float)Math.Cos(i), 2 * (float)Math.Sin(i)),
                                950, danceDmg, 5);
                        }
                        
                    });

                    break;
            }

            if (AbilityExtentions.EmpressCycles[caster.Name] < 4) {
                AbilityExtentions.EmpressCycles[caster.Name]++;
            }
            else {
                AbilityExtentions.EmpressCycles[caster.Name] = 1;
            }
            #endregion
        }

        public static void Twilight(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Properties
            int eyesDmg = 8 + (abilityLevel - 1) * 8;
            int eyesBuffDuration = 720 + (abilityLevel - 1) * 120;
            int judgeBaseDmg = 10 + (abilityLevel - 1) * 10;
            int judgeRange = 30 + (abilityLevel - 1) * 10;
            int judgeDuration = 420 + (abilityLevel - 1) * 60;
            int punishDmg = 45 + (abilityLevel - 1) * 35;
            int punishBuffDuration = 300 + (abilityLevel - 1) * 60;
            int punishKB = 30 + (abilityLevel - 1);
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown)) {
                return;
            }
            #endregion

            #region Visuals
            #endregion

            #region Functionality
            if (!AbilityExtentions.TwilightCycles.ContainsKey(caster.Name)) {
                AbilityExtentions.TwilightCycles.Add(caster.Name, 1);
            }

            switch (AbilityExtentions.TwilightCycles[caster.Name]) {
                case 1: //Brilliant Eyes
                    caster.SetBuff(11, eyesBuffDuration, false);
                    caster.SetBuff(12, eyesBuffDuration, false);
                    caster.SetBuff(17, eyesBuffDuration, false);
                    Task.Run(async () => {
                        Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(0, 4), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(0, -4), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(4, 0), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(-4, 0), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        await Task.Delay(150);
                        Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(2, 4), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(4, -2), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(-2, -4), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(-4, 2), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        await Task.Delay(150);
                        Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(4, 4), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(4, -4), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(-4, -4), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(-4, 4), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        if (abilityLevel > 2) {
                            await Task.Delay(150);
                            Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(4, 2), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                            Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(2, -4), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                            Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(-4, -2), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                            Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(-2, 4), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        }
                        if (abilityLevel > 3) {
                            await Task.Delay(150);
                            Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(0, 4), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                            Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(0, -4), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                            Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(4, 0), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                            Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X, caster.TPlayer.position.Y), new Vector2(-4, 0), ProjectileID.LostSoulFriendly, eyesDmg, 0);
                        }
                    });
                    break;
                case 2: //Judgement
                    caster.SetBuff(160, 180, true);
                    Task.Run(async () => {
                        await Task.Delay(3000);
                        foreach (NPC npc in Main.npc) {
                            int judgeActualDmg = judgeBaseDmg;
                            if (npc != null && npc.active && npc.type != 0 && npc.netID != 400 && npc.netID != 70 && npc.netID != 72 && !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(caster.TPlayer.position, judgeRange * 16)) {
                                judgeActualDmg += npc.life / 20;
                                if (judgeActualDmg > judgeBaseDmg * 8) judgeActualDmg = judgeBaseDmg * 8;
                                string judgelist = "24 49 51 60 62 66 93 137 150 151 153 156 158 159 281 282 283 284 285 286 315 329 421 469";
                                if (judgelist.Contains(npc.netID.ToString())) judgeActualDmg *= 2;
                                if (npc.aiStyle == 6 || npc.netID == 267) judgeActualDmg /= 10;
                                else if (npc.aiStyle == 37) judgeActualDmg /= 15;
                                if (npc.defDefense < 999) judgeActualDmg += npc.defDefense / 2;
                                TSPlayer.Server.StrikeNPC(npc.whoAmI, judgeActualDmg, 0, 0);
                                npc.AddBuff(69, judgeDuration);
                                if (abilityLevel > 3) {
                                    npc.AddBuff(203, judgeDuration);
                                }
                                TSPlayer.All.SendData(PacketTypes.NpcUpdateBuff, number: npc.whoAmI);
                                if (npc.life <= 0) {
                                    Terraria.Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(npc.position.X, npc.position.Y), new Vector2(0, 0), ProjectileID.FairyQueenMagicItemShot, judgeBaseDmg * 3, 0);
                                }
                            }
                        }
                    });
                    break;
                case 3: //Punishment
                    Terraria.Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X + 70, caster.TPlayer.position.Y), new Vector2(3, 0), ProjectileID.DD2ExplosiveTrapT3Explosion, punishDmg, punishKB);
                    Terraria.Projectile.NewProjectile(Projectile.GetNoneSource(), new Vector2(caster.TPlayer.position.X - 70, caster.TPlayer.position.Y), new Vector2(-3, 0), ProjectileID.DD2ExplosiveTrapT3Explosion, punishDmg, punishKB);
                    caster.SetBuff(173, punishBuffDuration, false);
                    caster.SetBuff(176, punishBuffDuration, false);
                    caster.SetBuff(179, punishBuffDuration, false);
                    break;

            }

            if (AbilityExtentions.TwilightCycles[caster.Name] < 3) {
                AbilityExtentions.TwilightCycles[caster.Name]++;
            }
            else {
                AbilityExtentions.TwilightCycles[caster.Name] = 1;
            }
            #endregion
        }
    }

    public class AbilityExtentions {
        internal static Dictionary<byte, DateTime> Cooldowns = new();
        internal static Dictionary<byte, int> CooldownLengths = new();
        internal static Dictionary<string, int> EmpressCycles = new();
        internal static Dictionary<string, int> TwilightCycles = new();


        /// <summary>
        /// Register this into ServerApi.Hooks.NetSendData in order to have respawn buff timers.
        /// </summary>
        public static void RespawnCooldownBuffAdder(SendDataEventArgs args) {
            
            if (args.MsgId != PacketTypes.PlayerSpawn || 
                !Cooldowns.ContainsKey((byte)args.number) ||
                (DateTime.UtcNow - Cooldowns[(byte)args.number]).TotalSeconds < 0) {
                return;
            }
            
            TShock.Players[(byte)args.number].SetBuff(307, (int)((CooldownLengths[(byte)args.number] - (DateTime.UtcNow - Cooldowns[(byte)args.number]).TotalSeconds) * 60), true);
            
        }

        internal static bool IsInCooldown(byte casterId, int cooldown) {
            if (Cooldowns.ContainsKey(casterId)) {
                if ((DateTime.UtcNow - Cooldowns[casterId]).TotalSeconds < cooldown) {
                return true;
                }

                Cooldowns[casterId] = DateTime.UtcNow;
                CooldownLengths[casterId] = cooldown;
            }
            else {
                Cooldowns.Add(casterId, DateTime.UtcNow);
                CooldownLengths.Add(casterId, cooldown);
            }

            TShock.Players[casterId].SetBuff(307, cooldown * 60, true);
            return false;
        }
    }

    public enum AbilityType {
        DryadsRingOfHealing,
        RingOfDracula,
        SandFrames,
        Adrenaline,
        Witch,
        Marthymr,
        RandomTeleport,
        EmpressOfLight,
        Twilight
    }
}

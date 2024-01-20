using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using TShockAPI;
using dw = Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;
using TerrariaApi.Server;

// Version 1.2.7

namespace Abilities
{
    public class Abilities
    {

        public static void DryadsRingOfHealing(TSPlayer caster, int cooldown, int abilityLevel = 1)
        {
            #region Properties
            int buffDurationInTicks = 400 + 20 * abilityLevel;
            double healPercentage = 0.04 + abilityLevel * 0.01;
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown))
            {
                return;
            }
            #endregion

            #region Visuals
            dw.ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(caster.X, caster.Y),
                UniqueInfoPiece = 1
            };

            Shapes.DrawCircle(caster.X + 16, caster.Y + 24, 384, 0.1963, ParticleOrchestraType.ChlorophyteLeafCrystalPassive, (byte)caster.Index);
            #endregion

            #region Functionality
            foreach (TSPlayer plr in TShock.Players)
            {
                if (plr != null && plr.Active && !plr.Dead && plr.TPlayer.position.WithinRange(caster.TPlayer.position, 384))
                {
                    plr.SetBuff(BuffID.DryadsWard, buffDurationInTicks, true);
                    plr.Heal((int)(plr.PlayerData.maxHealth * healPercentage));
                }
            }
            #endregion
        }

        /// <summary>
        /// This ability is deprecated.
        /// </summary>
        public static void TotemOfUndying(TSPlayer caster, int cooldown, int abilityLevel = 1)
        {
            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown))
            {
                return;
            }
            #endregion

            #region Visuals
            dw.ParticleOrchestraSettings settings = new()
            {
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

        /// <summary>
        /// This ability is deprecated.
        /// </summary>
        public static void DragonMaster(TSPlayer caster, int cooldown, int abilityLevel = 1)
        {
            #region Properties
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown))
            {
                return;
            }
            #endregion

            #region Visuals
            #endregion

            #region Functionality

            Projectile.NewProjectile(Projectile.GetNoneSource(),
                new(caster.X + WorldGen.genRand.Next(-64, 64), caster.Y - WorldGen.genRand.Next(48)),
                new(0, 0),
                ProjectileID.StardustDragon1,
                200, 0, caster.Index, 0, 4, 0);
            #endregion
        }


        public static void EmpressOfLight(TSPlayer caster, int cooldown, int abilityLevel = 1)
        {
            #region Properties
            int lanceDmg = (int)((50 + (abilityLevel - 1) * 15) * (1 + abilityLevel / 10f));
            int dashDmg = (int)((100 + (abilityLevel - 1) * 30) * (1 + abilityLevel / 10f));
            int boltDmg = (int)((10 + (abilityLevel - 1) * 10) * (1 + abilityLevel / 10));
            int boltSpawnInterval = 400 * (10 - abilityLevel) / 10;
            int danceDmg = (int)((55 + (abilityLevel - 1) * 20) * (1 + abilityLevel / 10f));
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown))
            {
                return;
            }
            #endregion

            #region Visuals
            #endregion

            #region Functionality
            if (!AbilityExtentions.EmpressCycles.ContainsKey(caster.Name))
            {
                AbilityExtentions.EmpressCycles.Add(caster.Name, 1);
            }

            switch (AbilityExtentions.EmpressCycles[caster.Name])
            {
                case 1:    // Ethereal Lance
                    float startingYPos = caster.Y - 8 * 16;
                    for (int i = 0; i < 19; i += 3)
                    {
                        AbilityExtentions.SpawnProjectile(
                            posX: caster.X + (i % 2 * 60 - 30) * 16,
                            posY: startingYPos + i * 16,
                            speedX: i % 2 * 60 * -1 + 30,
                            speedY: 0,
                            type: ProjectileID.FairyQueenRangedItemShot,
                            damage: lanceDmg,
                            knockback: 5,
                            owner: caster.Index,
                            ai_1: AbilityExtentions.GetNextHallowedWeaponColor()
                            );
                    }
                    break;
                case 2:    // Dash
                    caster.SendData(PacketTypes.PlayerDodge, number: (byte)caster.Index, number2: 2);

                    int direction = AbilityExtentions.GetVelocityXDirection(caster.TPlayer);

                    Task.Run(async () =>
                    {
                        caster.TPlayer.velocity.X = 36 * direction;
                        TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", caster.Index);

                        for (int i = 0; i < 12; i++)
                        {
                            AbilityExtentions.SpawnProjectile(
                                posX: caster.X + 16,
                                posY: caster.Y + 16,
                                speedX: 0,
                                speedY: i % 2 == 0 ? -0.5f : +0.5f,
                                type: ProjectileID.PrincessWeapon,
                                damage: dashDmg,
                                knockback: 14,
                                owner: caster.Index
                                );

                            if (i == 9)
                            {
                                caster.SetBuff(BuffID.Webbed, 1);
                            }

                            await Task.Delay(17);
                        }
                    });
                    break;
                case 3:    // Prismatic Bolts
                    Task.Run(async () =>
                    {
                        int ms = 0;
                        while (ms < 2500)
                        {
                            AbilityExtentions.SpawnProjectile(
                                posX: caster.X + WorldGen.genRand.Next(33),
                                posY: caster.Y + WorldGen.genRand.Next(-8, 33),
                                speedX: WorldGen.genRand.Next(-4, 5),
                                speedY: WorldGen.genRand.Next(-4, 5),
                                type: ProjectileID.FairyQueenMagicItemShot,
                                damage: boltDmg,
                                knockback: 6,
                                owner: caster.Index,
                                ai_1: AbilityExtentions.GetNextHallowedWeaponColor()
                                );

                            ms += boltSpawnInterval;
                            await Task.Delay(boltSpawnInterval);
                        }
                    });

                    break;
                case 4:    // Sundance
                    caster.SetBuff(BuffID.Webbed, 140, true);

                    Task.Run(async () =>
                    {
                        for (int i = 0; i < 3; i++)
                        {    // TODO: Rewrite this without trigonometric functions.
                            for (double j = i % 2 == 0 ? 0.7853 : 1.5707; j < Math.Tau; j += 1.5707)
                            {
                                AbilityExtentions.SpawnProjectile(
                                posX: caster.X + 16 + 64 * (float)Math.Cos(j),
                                posY: caster.Y + 16 + 64 * (float)Math.Sin(j),
                                speedX: 2 * (float)Math.Cos(j),
                                speedY: 2 * (float)Math.Sin(j),
                                type: ProjectileID.PrincessWeapon,
                                damage: danceDmg,
                                knockback: 5,
                                owner: caster.Index
                                );
                            }
                            await Task.Delay(500);
                        }
                    });
                    break;
            }

            if (AbilityExtentions.EmpressCycles[caster.Name] < 4)
            {
                AbilityExtentions.EmpressCycles[caster.Name]++;
            }
            else
            {
                AbilityExtentions.EmpressCycles[caster.Name] = 1;
            }
            #endregion
        }

        public static void Twilight(TSPlayer caster, int cooldown, int abilityLevel = 1)
        {
            #region Properties
            int eyesDmg = (int)((16 + (abilityLevel - 1) * 16) * (1 + abilityLevel / 10f));
            int eyesBuffDuration = 720 + (abilityLevel - 1) * 120;
            int judgeBaseDmg = (int)((15 + (abilityLevel - 1) * 15) * (1 + abilityLevel / 10f));
            int judgeRange = 30 + (abilityLevel - 1) * 10;
            int judgeDuration = 420 + (abilityLevel - 1) * 60;
            int punishDmg = (int)((50 + (abilityLevel - 1) * 35) * (1 + abilityLevel / 10f));
            int punishBuffDuration = 300 + (abilityLevel - 1) * 60;
            int punishKB = 20 + (abilityLevel - 1) * 5;
            cooldown -= (int)(cooldown * (abilityLevel - 1) * 0.1);
            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown))
            {
                return;
            }
            #endregion

            #region Visuals
            dw.ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(caster.X, caster.Y),
                UniqueInfoPiece = 1
            };
            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
            #endregion

            #region Functionality
            if (!AbilityExtentions.TwilightCycles.ContainsKey(caster.Name))
            {
                AbilityExtentions.TwilightCycles.Add(caster.Name, 1);
            }

            switch (AbilityExtentions.TwilightCycles[caster.Name])
            {
                case 1: //Brilliant Eyes
                    caster.SetBuff(11, eyesBuffDuration, false);
                    caster.SetBuff(12, eyesBuffDuration, false);
                    caster.SetBuff(17, eyesBuffDuration, false);
                    switch (abilityLevel)
                    {
                        case 1:
                            Task.Run(async () =>
                            {
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 0, -4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(250);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 4, 0, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(250);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 0, 4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(250);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -4, 0, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                            });
                            break;
                        case 2:
                            Task.Run(async () =>
                            {
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 0, -4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(150);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 4, -2, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(150);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 4, 2, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(150);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 0, 4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(150);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -4, 2, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(150);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -4, -2, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                            });
                            break;
                        case 3:
                            Task.Run(async () =>
                            {
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 2, -4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(107);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 4, -2, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(107);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 4, 2, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(107);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 2, 4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(107);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -2, 4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(107);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -4, 2, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(107);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -4, -2, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(107);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -2, -4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                            });
                            break;
                        case 4:
                            Task.Run(async () =>
                            {
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 2, -4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(83);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 4, -2.5f, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(83);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 4, 0, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(83);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 4, 2.5f, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(83);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 2, 4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(83);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -2, 4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(83);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -4, 2.5f, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(83);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -4, 0, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(83);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -4, -2.5f, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(83);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -2, -4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                            });
                            break;
                        case 5:
                            Task.Run(async () =>
                            {
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 0, -4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(68);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 2.5f, -4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(68);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 4, -2.5f, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(68);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 4, 0, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(68);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 4, 2.5f, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(68);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 2.5f, 4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(68);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, 0, 4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(68);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -2.5f, 4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(68);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -4, 2.5f, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(68);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -4, 0, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(68);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -4, -2.5f, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                                await Task.Delay(68);
                                AbilityExtentions.SpawnProjectile(caster.X, caster.Y, -2.5f, -4, ProjectileID.LostSoulFriendly, eyesDmg, 0, caster.Index);
                            });
                            break;
                    }
                    break;
                case 2: //Judgement
                    caster.SetBuff(160, 180, true);
                    Task.Run(async () =>
                    {
                        await Task.Delay(3000);
                        dw.ParticleOrchestraSettings Judgsettings = new()
                        {
                            IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                            MovementVector = new(0, 0),
                            PositionInWorld = new(caster.X, caster.Y),
                            UniqueInfoPiece = 1
                        };
                        dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, Judgsettings);
                        foreach (NPC npc in Main.npc)
                        {
                            int judgeActualDmg = judgeBaseDmg;
                            if (npc != null && npc.active && npc.type != 0 && npc.netID != 400 && npc.netID != 70 && npc.netID != 72 && !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(caster.TPlayer.position, judgeRange * 16))
                            {
                                judgeActualDmg += npc.life / 20;
                                if (judgeActualDmg > judgeBaseDmg * 8) judgeActualDmg = judgeBaseDmg * 8;
                                string judgelist = "24 49 51 60 62 66 93 137 150 151 153 156 158 159 281 282 283 284 285 286 315 329 421 469";
                                if (judgelist.Contains(npc.netID.ToString())) judgeActualDmg *= 2;
                                if (npc.aiStyle == 6 || npc.netID == 267) judgeActualDmg /= 10;
                                else if (npc.aiStyle == 37) judgeActualDmg /= 15;
                                if (npc.defDefense < 999) judgeActualDmg += npc.defDefense / 2;
                                TSPlayer.Server.StrikeNPC(npc.whoAmI, judgeActualDmg, 0, 0);
                                npc.AddBuff(69, judgeDuration);
                                if (abilityLevel > 2)
                                {
                                    npc.AddBuff(203, judgeDuration);
                                }
                                dw.ParticleOrchestraSettings Hitsettings = new()
                                {
                                    IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                                    MovementVector = new(0, 0),
                                    PositionInWorld = new(npc.position.X, npc.position.Y),
                                    UniqueInfoPiece = 1
                                };
                                dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPCSend, Hitsettings);
                                TSPlayer.All.SendData(PacketTypes.NpcUpdateBuff, number: npc.whoAmI);
                                if (npc.life <= 0)
                                {
                                    AbilityExtentions.SpawnProjectile(npc.position.X, npc.position.Y, 0, 0, ProjectileID.FairyQueenMagicItemShot, judgeBaseDmg * 3, 0, caster.Index);
                                }
                            }
                        }
                    });
                    break;
                case 3: //Punishment
                    AbilityExtentions.SpawnProjectile(caster.X + 70, caster.Y, 3, 0, ProjectileID.DD2ExplosiveTrapT3Explosion, punishDmg, punishKB, caster.Index);
                    AbilityExtentions.SpawnProjectile(caster.X - 70, caster.Y, -3, 0, ProjectileID.DD2ExplosiveTrapT3Explosion, punishDmg, punishKB, caster.Index);
                    caster.SetBuff(173, punishBuffDuration, false);
                    caster.SetBuff(176, punishBuffDuration, false);
                    caster.SetBuff(179, punishBuffDuration, false);
                    break;

            }

            if (AbilityExtentions.TwilightCycles[caster.Name] < 3)
            {
                AbilityExtentions.TwilightCycles[caster.Name]++;
            }
            else
            {
                AbilityExtentions.TwilightCycles[caster.Name] = 1;
            }
            #endregion
        }



        public static void MagicDice(TSPlayer caster, int cooldown, int abilityLevel = 1)
        {
            #region Properties
            cooldown -= WorldGen.genRand.Next(0, 16);
            int roll1DMG = (int)((60 + (abilityLevel - 1) * 35) * (1 + abilityLevel / 5f));
            int roll2HP = 75 * abilityLevel;
            int roll3DMG = (int)((15 + (abilityLevel - 1) * 15) * (1 + abilityLevel / 10f));
            int[] roll4List = { BuffID.Blackout, BuffID.VortexDebuff, BuffID.WitheredArmor, BuffID.WitheredWeapon, BuffID.CursedInferno, BuffID.MoonLeech, BuffID.Ichor, BuffID.Lucky, BuffID.RapidHealing, BuffID.Endurance, BuffID.Thorns, BuffID.Honey, BuffID.Ironskin, BuffID.Regeneration, BuffID.ShadowDodge, BuffID.Invisibility };
            int roll4Duration = (int)(600 + (abilityLevel - 1) * 120);
            int roll4Amount = (int)(2 + (abilityLevel / 2));
            int roll5Amount = (int)(2 + abilityLevel);
            int roll6DMG = (int)((200 + (abilityLevel - 1) * 100) * (1 + abilityLevel / 5f));
            int roll7Duration = (int)(5000 + (abilityLevel - 1) * 200);
            int roll7DMG = (int)(15 + (abilityLevel - 1) * 10);
            int roll8Duration = (int)(300 + (abilityLevel - 1) * 120);
            int roll8HP = (int)(20 + (abilityLevel - 1) * 10);
            int roll8DMG = (int)((25 + (abilityLevel - 1) * 15) * (1 + abilityLevel / 10f));
            float roll8Scale = 1f + (abilityLevel - 1) * 0.5f;
            int roll9Count = (int)(3 + abilityLevel);
            int roll9DMG = (int)((60 + (abilityLevel - 1) * 35) * (1 + abilityLevel / 5f));
            int roll10Ticks = (int)(420 + (abilityLevel - 1) * 60);
            float roll10Duration = (float)(7 + (abilityLevel - 1) * 1);
            int roll11Cap = (int)(250 + (abilityLevel - 1) * 500);
            int roll12Duration = (int)(10000 + (abilityLevel - 1) * 3000);
            int roll15DMG = (int)(25 + (abilityLevel - 1) * 15);
            int i = 0;
            float i2 = 0;

            #endregion

            #region Cooldown
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown))
            {
                return;
            }
            #endregion

            #region Visuals
            NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1,
            Terraria.Localization.NetworkText.FromLiteral("You roll..."), (int)new Color(255, 255, 255).PackedValue,
            caster.X + 16, caster.Y - 20);
            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(caster.X + 16, caster.Y),
                UniqueInfoPiece = 1
            };
            ParticleOrchestraSettings why = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(caster.X + 16, caster.Y + 16),
                UniqueInfoPiece = 1
            };
            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
            #endregion

            #region Functionality
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                if (caster.Dead) return;
                Vector2 savedPos = caster.LastNetPosition;
                ParticleOrchestraSettings settings = new()
                {
                    IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                    MovementVector = new(0, 0),
                    PositionInWorld = new(caster.X + 16, caster.Y),
                    UniqueInfoPiece = 1
                };
                switch (WorldGen.genRand.Next(1, 16))
                {
                    case 1:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Explosion?"), (int)new Color(255, 255, 75).PackedValue, caster.X + 16, caster.Y);
                        caster.SetBuff(BuffID.Dazed, 650);
                        caster.SetBuff(BuffID.WitheredArmor, 650);
                        caster.SetBuff(BuffID.WitheredWeapon, 650);
                        int explosionTimer = 10;
                        await Task.Delay(1000);
                        while (explosionTimer > 0 && !caster.Dead)
                        {
                            if (caster.Dead) return;
                            why.PositionInWorld.X = caster.X + 16;
                            why.PositionInWorld.Y = caster.Y + 16;
                            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, why);
                            NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral(explosionTimer.ToString()), (int)new Color(255, 25, 25).PackedValue, caster.X + 16, caster.Y + 33);
                            explosionTimer--;
                            await Task.Delay(1000);
                        }
                        if (!caster.Dead)
                        {
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y, 7, 0, 950, roll1DMG, 8, caster.Index);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y, 5.5f, 5.5f, 950, roll1DMG, 8, caster.Index);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y, 0, 7, 950, roll1DMG, 8, caster.Index);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y, -5.5f, 5.5f, 950, roll1DMG, 8, caster.Index);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y, -7, 0, 950, roll1DMG, 8, caster.Index);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y, -5.5f, -5.5f, 950, roll1DMG, 8, caster.Index);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y, 0, -7, 950, roll1DMG, 8, caster.Index);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y, 5.5f, -5.5f, 950, roll1DMG, 8, caster.Index);
                        }

                        break;
                    case 2:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Heal!"), (int)new Color(75, 255, 75).PackedValue, caster.X + 16, caster.Y - 20);
                        caster.Heal(WorldGen.genRand.Next(10, roll2HP));
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings);
                        break;
                    case 3:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Stone."), (int)new Color(255, 75, 75).PackedValue, caster.X + 16, caster.Y);
                        caster.SetBuff(BuffID.Stoned, 100);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PetExchange, settings);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PetExchange, settings);
                        i = 0;
                        while (i < 15)
                        {
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y, WorldGen.genRand.Next(-70, 70) / 10f, WorldGen.genRand.Next(-70, 70) / 10f, ProjectileID.PewMaticHornShot, roll3DMG, 1.75f, caster.Index);
                            i++;
                            await Task.Delay(50);
                        }
                        break;
                    case 4:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Buff Shuffle?"), (int)new Color(255, 255, 75).PackedValue, caster.X + 16, caster.Y);
                        i = roll4Amount;
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPCSend, settings);
                        while (i > 0)
                        {
                            caster.SetBuff(roll4List[WorldGen.genRand.Next(0, roll4List.Length)], roll4Duration);
                            i--;
                        }
                        break;
                    case 5:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Nebula Burst!"), (int)new Color(75, 255, 75).PackedValue, caster.X + 16, caster.Y);
                        i = roll5Amount;
                        while (i > 0)
                        {
                            switch (WorldGen.genRand.Next(1, 4))
                            {
                                case 1:
                                    Terraria.Item.NewItem(new Terraria.DataStructures.EntitySource_DebugCommand(), (int)caster.X + 16, (int)caster.Y, (int)caster.TPlayer.Size.X, (int)caster.TPlayer.Size.X, Terraria.ID.ItemID.NebulaPickup1, 1);
                                    break;
                                case 2:
                                    Terraria.Item.NewItem(new Terraria.DataStructures.EntitySource_DebugCommand(), (int)caster.X + 16, (int)caster.Y, (int)caster.TPlayer.Size.X, (int)caster.TPlayer.Size.X, Terraria.ID.ItemID.NebulaPickup2, 1);
                                    break;
                                case 3:
                                    Terraria.Item.NewItem(new Terraria.DataStructures.EntitySource_DebugCommand(), (int)caster.X + 16, (int)caster.Y, (int)caster.TPlayer.Size.X, (int)caster.TPlayer.Size.X, Terraria.ID.ItemID.NebulaPickup3, 1);
                                    break;
                            }
                            i--;
                        }
                        break;
                    case 6:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("The dice exploded..?"), (int)new Color(255, 75, 75).PackedValue, caster.X + 16, caster.Y);
                        AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y, 0, 0, ProjectileID.DD2ExplosiveTrapT3Explosion, roll6DMG, 20, caster.Index);
                        caster.DamagePlayer(roll6DMG / 4);
                        break;
                    case 7:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Overcharge?"), (int)new Color(255, 255, 75).PackedValue, caster.X + 16, caster.Y);
                        i = roll7Duration;
                        caster.SetBuff(BuffID.NebulaUpDmg3, 600, true);
                        caster.SetBuff(BuffID.NebulaUpLife3, 600, true);
                        while (i > 0 && !caster.Dead)
                        {
                            why.PositionInWorld.X = caster.X + 16;
                            why.PositionInWorld.Y = caster.Y + 16;
                            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, why);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y + 16, 0, 0, ProjectileID.FairyQueenMagicItemShot, roll7DMG, 2, caster.Index, 0f, 0.45f);
                            await Task.Delay(100);
                            i -= 100;
                        }
                        if (!caster.Dead) caster.KillPlayer();
                        break;
                    case 8:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Rooted?"), (int)new Color(255, 255, 75).PackedValue, caster.X + 16, caster.Y);
                        i = roll8Duration;
                        caster.SetBuff(BuffID.Webbed, roll8Duration);
                        while (i > 0 && !caster.Dead)
                        {
                            caster.Heal(roll8HP);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y + 16, WorldGen.genRand.Next(-10, 11) / 10f, WorldGen.genRand.Next(-10, 11) / 10f, ProjectileID.SharpTears, roll8DMG, 7, caster.Index, 0f, roll8Scale);
                            await Task.Delay(250);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y + 16, WorldGen.genRand.Next(-10, 11) / 10f, WorldGen.genRand.Next(-10, 11) / 10f, ProjectileID.SharpTears, roll8DMG, 7, caster.Index, 0f, roll8Scale);
                            await Task.Delay(250);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y + 16, WorldGen.genRand.Next(-10, 11) / 10f, WorldGen.genRand.Next(-10, 11) / 10f, ProjectileID.SharpTears, roll8DMG, 7, caster.Index, 0f, roll8Scale);
                            await Task.Delay(250);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y + 16, WorldGen.genRand.Next(-10, 11) / 10f, WorldGen.genRand.Next(-10, 11) / 10f, ProjectileID.SharpTears, roll8DMG, 7, caster.Index, 0f, roll8Scale);
                            await Task.Delay(250);
                            i -= 60;
                        }
                        break;
                    case 9:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Teleportation?"), (int)new Color(255, 255, 75).PackedValue, caster.X + 16, caster.Y);
                        i = roll9Count;
                        await Task.Delay(750);
                        while (i > 0 && !caster.Dead)
                        {
                            why.PositionInWorld.X = caster.X + 16;
                            why.PositionInWorld.Y = caster.Y + 16;
                            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, why);
                            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, why);
                            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, why);
                            caster.Teleport(WorldGen.genRand.Next(-480, 481) + caster.X, WorldGen.genRand.Next(-480, 481) + caster.Y);
                            AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y + 16, 0, 0, 950, roll9DMG, 14, caster.Index);
                            await Task.Delay(750);
                            i--;
                        }
                        why.PositionInWorld.X = caster.X + 16;
                        why.PositionInWorld.Y = caster.Y + 16;
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, why);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, why);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, why);
                        caster.Teleport(savedPos.X, savedPos.Y);
                        AbilityExtentions.SpawnProjectile(caster.X + 16, caster.Y + 16, 0, 0, 950, roll9DMG, 14, caster.Index);
                        break;
                    case 10:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Smoke Bomb!"), (int)new Color(75, 255, 75).PackedValue, caster.X + 16, caster.Y);
                        i2 = roll10Duration;
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PetExchange, settings);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PetExchange, settings);
                        caster.SetBuff(BuffID.Invisibility, roll10Ticks);
                        caster.SetBuff(BuffID.WitheredWeapon, roll10Ticks);
                        while (i2 >= 1.33)
                        {
                            caster.SendData(PacketTypes.PlayerDodge, number: caster.Index, number2: 2);
                            i2 -= 0.1f;
                            await Task.Delay(100);
                        }
                        break;
                    case 11:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Magic Cleaver!"), (int)new Color(75, 255, 75).PackedValue, caster.X + 16, caster.Y);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
                        foreach (NPC npc in Main.npc)
                        {
                            int cleaveDMG = 0;
                            if (npc != null && npc.active && npc.type != 0 && npc.netID != 400 && !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(caster.TPlayer.position, 60 * 16))
                            {
                                if (npc.lifeMax > roll11Cap) cleaveDMG += npc.life / 8;
                                else cleaveDMG += npc.life / 2;
                                if (npc.aiStyle == 6 || npc.netID == 267) cleaveDMG /= 10;
                                else if (npc.aiStyle == 37) cleaveDMG /= 100;
                                TSPlayer.Server.StrikeNPC(npc.whoAmI, cleaveDMG, 0, 0);
                                AbilityExtentions.SpawnProjectile(npc.position.X + 16, npc.position.Y - 160, 0, 12, ProjectileID.LightsBane, 0, 0, caster.Index, 2.5f);
                            }
                        }
                        break;
                    case 12:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Explosive Effect!"), (int)new Color(75, 255, 75).PackedValue, caster.X + 16, caster.Y);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.RainbowRodHit, settings);
                        AbilityExtentions.ExplosiveEffectState++;
                        await Task.Delay(roll12Duration);
                        AbilityExtentions.ExplosiveEffectState--;
                        break;
                    case 13:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Inked."), (int)new Color(255, 75, 75).PackedValue, caster.X + 16, caster.Y);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                        caster.SetBuff(BuffID.Obstructed, 600);
                        caster.SetBuff(BuffID.Blackout, 600);
                        break;
                    case 14:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Withered."), (int)new Color(255, 75, 75).PackedValue, caster.X + 16, caster.Y);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.StellarTune, settings);
                        caster.SetBuff(BuffID.WitheredArmor, 900);
                        caster.SetBuff(BuffID.WitheredWeapon, 900);
                        break;
                    case 15:
                        NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("Zapped."), (int)new Color(255, 75, 75).PackedValue, caster.X + 16, caster.Y);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerArrow, settings);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, settings);
                        caster.DamagePlayer(roll15DMG);
                        caster.SetBuff(BuffID.Electrified, 600);
                        break;
                }
            });
            #endregion
        }
    }
    public class AbilityExtentions
    {
        internal static Dictionary<byte, DateTime> Cooldowns = new();
        internal static Dictionary<byte, int> CooldownLengths = new();
        internal static Dictionary<string, int> EmpressCycles = new();
        internal static Dictionary<string, int> TwilightCycles = new();
        internal static float HallowedWeaponColor = 0;
        internal static int ExplosiveEffectState = 0;
        internal static bool IsPosTeleportable(int x, int y)
        {
            return x + 1 < Main.maxTilesX && y + 2 < Main.maxTilesY &&
                   Main.tile[x, y].collisionType == 0 &&
                   Main.tile[x, y + 1].collisionType == 0 &&
                   Main.tile[x, y + 2].collisionType == 0 &&
                   Main.tile[x + 1, y].collisionType == 0 &&
                   Main.tile[x + 1, y + 1].collisionType == 0 &&
                   Main.tile[x + 1, y + 2].collisionType == 0;
        }

        internal static float GetNextHallowedWeaponColor()
        {
            if (HallowedWeaponColor >= 1)
            {
                HallowedWeaponColor = 0;
            }
            else
            {
                HallowedWeaponColor += 0.015f;
            }
            return HallowedWeaponColor;
        }

        public static int SpawnProjectile(float posX, float posY, float speedX, float speedY, int type,
            int damage, float knockback, int owner = -1, float ai_0 = 0, float ai_1 = 0, float ai_2 = 0)
        {
            int projIndex = Projectile.NewProjectile(
                                spawnSource: Projectile.GetNoneSource(),
                                X: posX,
                                Y: posY,
                                SpeedX: speedX,
                                SpeedY: speedY,
                                Type: type,
                                Damage: damage,
                                KnockBack: knockback,
                                Owner: owner,
                                ai0: ai_0,
                                ai1: ai_1,
                                ai2: ai_2
                            );

            Main.projectile[projIndex].ai[0] = ai_0;
            Main.projectile[projIndex].ai[1] = ai_1;
            Main.projectile[projIndex].ai[2] = ai_2;

            if (owner != -1)
            {
                NetMessage.SendData((int)PacketTypes.ProjectileNew, -1, -1, null, projIndex);
            }

            return projIndex;
        }

        public static void ExplosiveEffectEffect(Vector2 pos, int HP)
        {
            if (ExplosiveEffectState <= 0) return;
            AbilityExtentions.SpawnProjectile(pos.X + 16, pos.Y, 5, 0, 950, HP, 8, -1);
            AbilityExtentions.SpawnProjectile(pos.X + 16, pos.Y, 4, 4, 950, HP, 8, -1);
            AbilityExtentions.SpawnProjectile(pos.X + 16, pos.Y, 0, 5, 950, HP, 8, -1);
            AbilityExtentions.SpawnProjectile(pos.X + 16, pos.Y, -4, 4, 950, HP, 8, -1);
            AbilityExtentions.SpawnProjectile(pos.X + 16, pos.Y, -5, 0, 950, HP, 8, -1);
            AbilityExtentions.SpawnProjectile(pos.X + 16, pos.Y, -4, -4, 950, HP, 8, -1);
            AbilityExtentions.SpawnProjectile(pos.X + 16, pos.Y, 0, -5, 950, HP, 8, -1);
            AbilityExtentions.SpawnProjectile(pos.X + 16, pos.Y, 4, -4, 950, HP, 8, -1);
        }

        public static int GetVelocityXDirection(Player player)
        {
            return (player.velocity.X == 0) ? player.direction : (int)(player.velocity.X / Math.Abs(player.velocity.X));
        }

        /// <summary>
        /// Register this into ServerApi.Hooks.NetSendData in order to have respawn buff timers.
        /// </summary>
        public static void RespawnCooldownBuffAdder(SendDataEventArgs args)
        {

            if (args.MsgId != PacketTypes.PlayerSpawn ||
                !Cooldowns.ContainsKey((byte)args.number) ||
                (DateTime.UtcNow - Cooldowns[(byte)args.number]).TotalSeconds < 0)
            {
                return;
            }

            TShock.Players[(byte)args.number].SetBuff(307, (int)((CooldownLengths[(byte)args.number] - (DateTime.UtcNow - Cooldowns[(byte)args.number]).TotalSeconds) * 60), true);

        }

        internal static bool IsInCooldown(byte casterId, int cooldown)
        {
            if (Cooldowns.ContainsKey(casterId))
            {
                if ((DateTime.UtcNow - Cooldowns[casterId]).TotalSeconds < cooldown)
                {
                    return true;
                }

                Cooldowns[casterId] = DateTime.UtcNow;
                CooldownLengths[casterId] = cooldown;
            }
            else
            {
                Cooldowns.Add(casterId, DateTime.UtcNow);
                CooldownLengths.Add(casterId, cooldown);
            }

            TShock.Players[casterId].SetBuff(307, cooldown * 60, true);
            return false;
        }
    }
}

using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using TShockAPI;
using dw = Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;
using TerrariaApi.Server;
using IL.Terraria.GameContent;

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
                    plr.SetBuff(BuffID.DryadsWard, buffDurationInTicks, true);
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
            int targetIndex = -1;

            foreach (NPC npc in Main.npc) {
                if (npc != null && npc.active && npc.type != 0 && npc.type != NPCID.TargetDummy && npc.position.WithinRange(caster.TPlayer.position, 384)) {

                    int stolenHealth = (int)(600 / (1 + Math.Pow(1.0001, -npc.life)) - 300);

                    if (stolenHealth > healAmount) {
                        targetIndex = npc.whoAmI;
                        healAmount = stolenHealth;
                    }
                }
            }

            if (targetIndex != -1) {
                NPC npc = Main.npc[targetIndex];
                npc.life -= healAmount;
                NetMessage.SendData((int)PacketTypes.NpcUpdate, -1, -1, null, targetIndex);
                caster.Heal((int)healAmount);

                settings.PositionInWorld = new Vector2(npc.position.X + npc.width / 2 + 8, npc.position.Y - 8);
                dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
            }
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
            List<int> buffTypes = new List<int>();
            
            switch (abilityLevel) {
                case 1:
                    buffTypes.Add(BuffID.Frostburn);
                    break;
                case 2:
                    buffTypes.Add(BuffID.Frostburn);
                    buffTypes.Add(BuffID.Bleeding);
                    break;
                case 3:
                    buffTypes.Add(BuffID.ShadowFlame);
                    break;
                case 4:
                    buffTypes.Add(BuffID.Venom);
                    break;
                default:
                    buffTypes.Add(BuffID.ShadowFlame);
                    buffTypes.Add(BuffID.Venom);
                    break;
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

            Shapes.DrawCircle(caster.X + 8, caster.Y + 24, rangeInBlocks * 16, 0.1963, ParticleOrchestraType.PrincessWeapon, (byte)caster.Index);
            #endregion

            #region Functionality
            foreach (NPC npc in Main.npc) { 
                if (npc != null && npc.active && npc.type != 0 && npc.position.WithinRange(caster.TPlayer.position, rangeInBlocks * 16)) {
                    foreach(int buffType in buffTypes) {
                        npc.AddBuff(buffType, 60 * 30);
                    }
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
                AbilityExtentions.SpawnProjectile(
                    posX: caster.X + 16 * (float)Math.Cos(i),
                    posY: caster.Y + 16 * (float)Math.Sin(i),
                    speedX: speedFactor * (float)Math.Cos(i),
                    speedY: speedFactor * (float)Math.Sin(i),
                    type: ProjectileID.DD2SquireSonicBoom,
                    damage: damage,
                    knockback: 30,
                    owner: caster.Index
                    );
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
            dw.ParticleOrchestraSettings settings = new() {
                IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(caster.X + 16, caster.Y + 16),
                UniqueInfoPiece = 1
            };

            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPCSend, settings);
            #endregion

            #region Functionality
            float x = 0;
            float y = 0;

            for (int i = 0; i < 100; i++) {
                int d = WorldGen.genRand.Next(160, rangeInBlocks * 16);
                double tetha = WorldGen.genRand.NextDouble() * Math.Tau;
                x = d * (float)Math.Cos(tetha);
                y = d * (float)Math.Sin(tetha);

                if (AbilityExtentions.IsPosTeleportable((int)((x + caster.X) / 16), (int)((y + caster.Y) / 16))) {
                    break;
                }
            }

            settings.PositionInWorld.X = x + caster.X + 16;
            settings.PositionInWorld.Y = y + caster.Y + 16;

            caster.Teleport(x + caster.X, y + caster.Y);

            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPCSend, settings);
            #endregion
        }

        public static void EmpressOfLight(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Properties
            int lanceDmg = (int)((50 + (abilityLevel - 1) * 15) * (1 + abilityLevel / 10f));
            int dashDmg = (int)((100 + (abilityLevel - 1) * 30) * (1 + abilityLevel / 10f));
            int boltDmg = (int)((10 + (abilityLevel - 1) * 10) * (1 + abilityLevel / 10));
            int boltSpawnInterval = 400 * (10 - abilityLevel) / 10;
            int danceDmg = (int)((55 + (abilityLevel - 1) * 20) * (1 + abilityLevel / 10f));
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
                    for (int i = 0; i < 19; i += 3) {
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

                    Task.Run(async () => {
                        caster.TPlayer.velocity.X = 36 * direction;
                        TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", caster.Index);

                        for (int i = 0; i < 12; i++) {
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

                            if (i == 9) {
                                caster.SetBuff(BuffID.Webbed, 1);
                            }

                            await Task.Delay(17);
                        }
                    });
                    break;
                case 3:    // Prismatic Bolts
                    Task.Run(async () => {
                        int ms = 0;
                        while(ms < 2500) {
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

                    Task.Run(async () => {
                        for (int i = 0; i < 3; i++) {    // TODO: Rewrite this without trigonometric functions.
                            for (double j = i%2 == 0 ? 0.7853 : 1.5707; j < Math.Tau; j += 1.5707) {
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
            if (AbilityExtentions.IsInCooldown((byte)caster.Index, cooldown)) {
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
            if (!AbilityExtentions.TwilightCycles.ContainsKey(caster.Name)) {
                AbilityExtentions.TwilightCycles.Add(caster.Name, 1);
            }

            switch (AbilityExtentions.TwilightCycles[caster.Name]) {
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
                    Task.Run(async () => {
                        await Task.Delay(3000);
                        dw.ParticleOrchestraSettings Judgsettings = new()
                        {
                            IndexOfPlayerWhoInvokedThis = (byte)caster.Index,
                            MovementVector = new(0, 0),
                            PositionInWorld = new(caster.X, caster.Y),
                            UniqueInfoPiece = 1
                        };
                        dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, Judgsettings);
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
                                if (abilityLevel > 2) {
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
                                if (npc.life <= 0) {
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

            if (AbilityExtentions.TwilightCycles[caster.Name] < 3) {
                AbilityExtentions.TwilightCycles[caster.Name]++;
            }
            else {
                AbilityExtentions.TwilightCycles[caster.Name] = 1;
            }
            #endregion
        }

        public static void Harvest(TSPlayer caster, int cooldown, int abilityLevel = 1) {
            #region Properties
            int damage = (int)((20 + abilityLevel * 15) * (1 + abilityLevel / 10f));
            int projSpawnInterval = (int)(1000 / Math.Sqrt(abilityLevel));
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
                PositionInWorld = new(caster.X + 16, caster.Y - 16),
                UniqueInfoPiece = ItemID.ScytheWhip
            };

            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ItemTransfer, settings);
            settings.PositionInWorld.Y = caster.Y + 16;
            dw.ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
            #endregion

            #region Functionality
            Task.Run(async () => {
                int ms = 0;
                while (ms < 10000) {
                    float randX = caster.X + 16 + WorldGen.genRand.Next(-64, 64);
                    float randY = caster.Y + 16 + WorldGen.genRand.Next(-64, 64);
                    AbilityExtentions.SpawnProjectile(randX, randY, 0, 0, ProjectileID.ScytheWhipProj, damage, 6, caster.Index);
                    ms += projSpawnInterval;
                    await Task.Delay(projSpawnInterval);
                }
            });
            #endregion
        }
    }

    public class AbilityExtentions {
        internal static Dictionary<byte, DateTime> Cooldowns = new();
        internal static Dictionary<byte, int> CooldownLengths = new();
        internal static Dictionary<string, int> EmpressCycles = new();
        internal static Dictionary<string, int> TwilightCycles = new();
        internal static float HallowedWeaponColor = 0;

        internal static bool IsPosTeleportable(int x, int y) {
            return x + 1 < Main.maxTilesX && y + 2 < Main.maxTilesY &&
                   Main.tile[x, y].collisionType == 0 &&
                   Main.tile[x, y + 1].collisionType == 0 &&
                   Main.tile[x, y + 2].collisionType == 0 &&
                   Main.tile[x + 1, y].collisionType == 0 &&
                   Main.tile[x + 1, y + 1].collisionType == 0 &&
                   Main.tile[x + 1, y + 2].collisionType == 0;
        }

        internal static float GetNextHallowedWeaponColor() {
            if (HallowedWeaponColor >= 1) {
                HallowedWeaponColor = 0;
            }
            else {
                HallowedWeaponColor += 0.015f;
            }
            return HallowedWeaponColor;
        }

        public static int SpawnProjectile(float posX, float posY, float speedX, float speedY, int type, 
            int damage, float knockback, int owner = -1, float ai_0 = 0, float ai_1 = 0, float ai_2 = 0) {
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

            if (owner != -1) {
                NetMessage.SendData((int)PacketTypes.ProjectileNew, -1, -1, null, projIndex);
            }

            return projIndex;
        }

        public static int GetVelocityXDirection(Player player) {
            return (player.velocity.X == 0) ? player.direction : (int)(player.velocity.X / Math.Abs(player.velocity.X));
        }

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
        Twilight,
        Harvest
    }
}

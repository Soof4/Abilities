using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;

namespace Abilities
{
    // Author of this ability is @strangelad on Discord
    public class Twilight : Ability
    {
        private int EyesDmg, EyesBuffDuration, JudgeBaseDmg, JudgeRange, JudgeDuration, PunishDmg, PunishBuffDuration, PunishKB;
        private string Judgelist = "24 49 51 60 62 66 93 137 150 151 153 156 158 159 281 282 283 284 285 286 315 329 421 469";


        public Twilight(int abilityLevel)
        {
            CalculateProperties(abilityLevel);
        }


        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                EyesDmg = (int)((16 + (abilityLevel - 1) * 16) * (1 + abilityLevel / 10f));
                EyesBuffDuration = 720 + (abilityLevel - 1) * 120;
                JudgeBaseDmg = (int)((15 + (abilityLevel - 1) * 15) * (1 + abilityLevel / 10f));
                JudgeRange = 30 + (abilityLevel - 1) * 10;
                JudgeDuration = 420 + (abilityLevel - 1) * 60;
                PunishDmg = (int)((50 + (abilityLevel - 1) * 35) * (1 + abilityLevel / 10f));
                PunishBuffDuration = 300 + (abilityLevel - 1) * 60;
                PunishKB = 20 + (abilityLevel - 1) * 5;
            }
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            CalculateProperties(abilityLevel);
            PlayVisuals(plr, 0);

            if (!Extensions.TwilightCycles.ContainsKey(plr.Name))
            {
                Extensions.TwilightCycles.Add(plr.Name, 1);
            }

            switch (Extensions.TwilightCycles[plr.Name])
            {
                case 1:    //Brilliant Eyes
                    plr.SetBuff(BuffID.Shine, EyesBuffDuration, false);
                    plr.SetBuff(BuffID.NightOwl, EyesBuffDuration, false);
                    plr.SetBuff(BuffID.Hunter, EyesBuffDuration, false);
                    switch (abilityLevel)
                    {
                        case 1:
                            Task.Run(async () =>
                            {
                                Extensions.SpawnProjectile(plr.X, plr.Y, 0, -4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(250);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 4, 0, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(250);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 0, 4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(250);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -4, 0, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                            });
                            break;
                        case 2:
                            Task.Run(async () =>
                            {
                                Extensions.SpawnProjectile(plr.X, plr.Y, 0, -4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(150);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 4, -2, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(150);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 4, 2, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(150);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 0, 4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(150);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -4, 2, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(150);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -4, -2, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                            });
                            break;
                        case 3:
                            Task.Run(async () =>
                            {
                                Extensions.SpawnProjectile(plr.X, plr.Y, 2, -4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(107);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 4, -2, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(107);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 4, 2, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(107);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 2, 4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(107);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -2, 4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(107);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -4, 2, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(107);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -4, -2, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(107);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -2, -4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                            });
                            break;
                        case 4:
                            Task.Run(async () =>
                            {
                                Extensions.SpawnProjectile(plr.X, plr.Y, 2, -4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(83);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 4, -2.5f, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(83);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 4, 0, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(83);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 4, 2.5f, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(83);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 2, 4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(83);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -2, 4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(83);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -4, 2.5f, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(83);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -4, 0, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(83);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -4, -2.5f, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(83);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -2, -4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                            });
                            break;
                        case 5:
                            Task.Run(async () =>
                            {
                                Extensions.SpawnProjectile(plr.X, plr.Y, 0, -4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(68);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 2.5f, -4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(68);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 4, -2.5f, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(68);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 4, 0, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(68);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 4, 2.5f, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(68);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 2.5f, 4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(68);
                                Extensions.SpawnProjectile(plr.X, plr.Y, 0, 4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(68);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -2.5f, 4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(68);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -4, 2.5f, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(68);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -4, 0, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(68);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -4, -2.5f, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                                await Task.Delay(68);
                                Extensions.SpawnProjectile(plr.X, plr.Y, -2.5f, -4, ProjectileID.LostSoulFriendly, EyesDmg, 0, plr.Index);
                            });
                            break;
                    }
                    break;
                case 2:    //Judgement
                    plr.SetBuff(BuffID.Dazed, 180, true);
                    Task.Run(async () =>
                    {
                        await Task.Delay(3000);

                        PlayVisuals(plr, 1);

                        foreach (NPC npc in Main.npc)
                        {
                            int judgeActualDmg = JudgeBaseDmg;
                            if (npc != null && npc.active && npc.type != 0 && npc.netID != 400 && npc.netID != 70 && npc.netID != 72 &&
                                !npc.friendly && !npc.CountsAsACritter && npc.position.WithinRange(plr.TPlayer.position, JudgeRange * 16))
                            {
                                judgeActualDmg += npc.life / 20;
                                if (judgeActualDmg > JudgeBaseDmg * 8) judgeActualDmg = JudgeBaseDmg * 8;
                                if (Judgelist.Contains(npc.netID.ToString())) judgeActualDmg *= 2;
                                if (npc.aiStyle == 6 || npc.netID == 267) judgeActualDmg /= 10;
                                else if (npc.aiStyle == 37) judgeActualDmg /= 15;
                                if (npc.defDefense < 999) judgeActualDmg += npc.defDefense / 2;
                                TSPlayer.Server.StrikeNPC(npc.whoAmI, judgeActualDmg, 0, 0);
                                npc.AddBuff(69, JudgeDuration);
                                if (abilityLevel > 2)
                                {
                                    npc.AddBuff(203, JudgeDuration);
                                }

                                PlayVisuals(plr, 2, npc);

                                TSPlayer.All.SendData(PacketTypes.NpcUpdateBuff, number: npc.whoAmI);
                                if (npc.life <= 0)
                                {
                                    Extensions.SpawnProjectile(npc.position.X, npc.position.Y, 0, 0, ProjectileID.FairyQueenMagicItemShot, JudgeBaseDmg * 3, 0, plr.Index);
                                }
                            }
                        }
                    });
                    break;
                case 3:    //Punishment
                    Extensions.SpawnProjectile(plr.X + 70, plr.Y, 3, 0, ProjectileID.DD2ExplosiveTrapT3Explosion, PunishDmg, PunishKB, plr.Index);
                    Extensions.SpawnProjectile(plr.X - 70, plr.Y, -3, 0, ProjectileID.DD2ExplosiveTrapT3Explosion, PunishDmg, PunishKB, plr.Index);
                    plr.SetBuff(BuffID.NebulaUpLife1, PunishBuffDuration, false);
                    plr.SetBuff(BuffID.NebulaUpMana1, PunishBuffDuration, false);
                    plr.SetBuff(BuffID.NebulaUpDmg1, PunishBuffDuration, false);
                    break;

            }

            if (Extensions.TwilightCycles[plr.Name] < 3)
            {
                Extensions.TwilightCycles[plr.Name]++;
            }
            else
            {
                Extensions.TwilightCycles[plr.Name] = 1;
            }
        }

        internal override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];
            int num = (int)args[1];


            switch (num)
            {
                case 0:
                    ParticleOrchestraSettings settings = new()
                    {
                        IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                        MovementVector = new(0, 0),
                        PositionInWorld = new(plr.X, plr.Y),
                        UniqueInfoPiece = 1
                    };
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                    break;
                case 1:
                    ParticleOrchestraSettings Judgsettings = new()
                    {
                        IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                        MovementVector = new(0, 0),
                        PositionInWorld = new(plr.X, plr.Y),
                        UniqueInfoPiece = 1
                    };
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, Judgsettings);
                    break;
                case 2:
                    NPC npc = (NPC)args[2];

                    ParticleOrchestraSettings Hitsettings = new()
                    {
                        IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                        MovementVector = new(0, 0),
                        PositionInWorld = new(npc.position.X, npc.position.Y),
                        UniqueInfoPiece = 1
                    };
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPCSend, Hitsettings);
                    break;


            }
        }
    }
}
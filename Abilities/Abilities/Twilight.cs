using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.Audio;
using Org.BouncyCastle.Asn1.X509;
//using System.Drawing;

namespace Abilities
{
    // Author of this ability is @strangelad on Discord
    public class Twilight : Ability
    {
        private static Dictionary<string, int> TwilightCycles = new Dictionary<string, int>();
        public int EyesDmg, EyesBuffDuration, EyesCount, JudgeBaseDmg, JudgeRange, PunishDmg, PunishKB;
        private int[] ignore = { 8, 9, 11, 12, 14, 15, 40, 41, 68, 70, 72, 88, 89, 90, 91, 92, 96, 97, 99, 100, 114, 118, 119, 135, 136, 249, 263, 267, 328, 379, 380, 400, 403, 404, 412, 413, 437, 440, 438, 455, 456, 457, 458, 459, 491, 511, 512, 514, 515, 549, 622, 623 };
        private int[] evil = { -43, -42, -41, -40, -39, -38, -25, -24, -23, -22, -12, -11, -2, -1, 6, 7, 24, 34, 47, 57, 62, 66, 79, 81, 82, 83, 94, 98, 101, 109, 121, 156, 158, 159, 168, 173, 174, 179, 181, 182, 183, 189, 190, 191, 192, 193, 194, 195, 196, 239, 240, 241, 242, 253, 268, 281, 282, 283, 284, 288, 289, 464, 465, 470, 473, 474, 489, 490, 525, 526, 529, 533, 534, 543, 544, 630 };
        private int[] kindaevil = { -21, -20, -19, -18, 4, 5, 13, 35, 36, 60, 104, 113, 115, 116, 117, 133, 151, 162, 166, 176, 266, 315, 329, 330, 351, 460, 461, 462, 463, 466, 467, 468, 469, 472, 477, 479, 523, 586, 587, 618, 619, 620, 621, 662 };
        private int[] notevil = { 75, 80, 84, 86, 120, 122, 137, 138, 475, 527, 545, 636, 657, 658, 659, 660 };


        public Twilight(int abilityLevel) : base(abilityLevel) { }

        internal override void CalculateProperties()
        {
            EyesDmg = (int)((10 + (AbilityLevel - 1) * 5) * (1 + AbilityLevel / 10f));
            EyesBuffDuration = 720 + (AbilityLevel - 1) * 240;
            EyesCount = 10 + (AbilityLevel - 1) * 4;
            JudgeBaseDmg = (int)(25 + (AbilityLevel - 1) * 25);
            JudgeRange = 50 + (AbilityLevel - 1) * 15;
            PunishDmg = (int)((50 + (AbilityLevel - 1) * 25) * (1 + AbilityLevel / 5f));
            PunishKB = 25 + (AbilityLevel - 1) * 10;
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr, 0);

            if (!TwilightCycles.ContainsKey(plr.Name))
            {
                TwilightCycles.Add(plr.Name, 0);
            }

            switch (TwilightCycles[plr.Name])
            {
                case 0:    //Brilliant Eyes
                    plr.SetBuff(BuffID.Shine, EyesBuffDuration, false);
                    plr.SetBuff(BuffID.NightOwl, EyesBuffDuration, false);
                    plr.SetBuff(BuffID.Hunter, EyesBuffDuration, false);
                    Extensions.MakeSound(plr.TPlayer.position, 105, 164, 0.6f, -0.2f);
                    int i = EyesCount;
                    Task.Run(async () =>
                    {
                        while (i > 0 && !plr.Dead)
                        {
                            float random = Extensions.Random.Next(-30, 31) / 10f;
                            Extensions.SpawnProjectile(plr.X + 16, plr.Y + 16, plr.TPlayer.direction * -6, random, ProjectileID.FairyQueenMagicItemShot, EyesDmg, 2, plr.Index, 0f, 0.67f);
                            Extensions.SpawnProjectile(plr.X + 16 + (plr.TPlayer.direction * -25), plr.Y + 16, plr.TPlayer.direction * -3f, random / 2f, ProjectileID.LightsBane, 0, 0, -1, 0.67f);
                            PlayVisuals(plr, 0);
                            await Task.Delay(125);
                            i--;
                        }
                    });
                    break;
                case 1:    //Judgement
                    plr.SetBuff(BuffID.Dazed, 240, true);
                    plr.SetBuff(BuffID.WitheredArmor, 240, true);
                    Task.Run(async () =>
                    {
                        int timer = 4000;
                        while (timer > 0)
                        {
                            PlayVisuals(plr, 1);
                            timer -= 200;
                            await Task.Delay(200);
                        }
                        if (plr.Dead) return;
                        PlayVisuals(plr, 2);
                        Extensions.MakeSound(plr.TPlayer.position, 105, 35, 2f, 0f);
                        foreach (NPC npc in Main.npc)
                        {
                            int judgeActualDmg = JudgeBaseDmg;
                            if (npc != null && npc.active && npc.type != 0 && !npc.friendly && !ignore.Contains(npc.type) && !npc.CountsAsACritter && npc.position.WithinRange(plr.TPlayer.position, JudgeRange * 16))
                            {
                                judgeActualDmg += npc.lifeMax / 5;
                                if (judgeActualDmg > JudgeBaseDmg * 10) judgeActualDmg = JudgeBaseDmg * 10;
                                if (evil.Contains(npc.type))
                                {
                                    judgeActualDmg *= 2;
                                    NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("FATAL"), (int)new Color(0, 255, 255).PackedValue, npc.position.X + npc.width / 2, npc.position.Y - 16);

                                }
                                else if (kindaevil.Contains(npc.type))
                                {
                                    judgeActualDmg = (int)(judgeActualDmg * 1.25);
                                    NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("WEAK"), (int)new Color(0, 255, 255).PackedValue, npc.position.X + npc.width / 2, npc.position.Y - 16);

                                }
                                else if (notevil.Contains(npc.type))
                                {
                                    judgeActualDmg = (int)(judgeActualDmg * 0.5);
                                    NetMessage.SendData((int)PacketTypes.CreateCombatTextExtended, -1, -1, Terraria.Localization.NetworkText.FromLiteral("RESIST"), (int)new Color(0, 255, 255).PackedValue, npc.position.X + npc.width / 2, npc.position.Y - 16);

                                }
                                if (npc.defDefense < 999) judgeActualDmg += npc.defDefense / 2;
                                TSPlayer.Server.StrikeNPC(npc.whoAmI, judgeActualDmg, 0, 0);
                                PlayVisuals(plr, 3, npc);
                            }
                        }
                    });
                    break;
                case 2:    //Punishment
                    Extensions.MakeSound(plr.TPlayer.position, 288, 62, 1f, -0.4f);
                    Extensions.MakeSound(plr.TPlayer.position, 105, 105, 1f, -0.15f);
                    Extensions.SpawnProjectile(plr.X + 16 + (plr.TPlayer.direction * 32), plr.Y + 16, plr.TPlayer.direction * 1, 0, ProjectileID.SharpTears, PunishDmg, PunishKB, plr.Index, 0f, 3f);
                    Extensions.SpawnProjectile(plr.X + 16 + (plr.TPlayer.direction * 32), plr.Y + 16, plr.TPlayer.direction * 1, -0.3f, ProjectileID.SharpTears, (int)(PunishDmg * 0.80), PunishKB, plr.Index, 0f, 2f);
                    Extensions.SpawnProjectile(plr.X + 16 + (plr.TPlayer.direction * 32), plr.Y + 16, plr.TPlayer.direction * 1, 0.3f, ProjectileID.SharpTears, (int)(PunishDmg * 0.80), PunishKB, plr.Index, 0f, 2f);
                    Extensions.SpawnProjectile(plr.X + 16 + (plr.TPlayer.direction * 32), plr.Y + 16, plr.TPlayer.direction * 1, -0.9f, ProjectileID.SharpTears, (int)(PunishDmg * 0.60), PunishKB, plr.Index, 0f, 1f);
                    Extensions.SpawnProjectile(plr.X + 16 + (plr.TPlayer.direction * 32), plr.Y + 16, plr.TPlayer.direction * 1, 0.9f, ProjectileID.SharpTears, (int)(PunishDmg * 0.60), PunishKB, plr.Index, 0f, 1f);
                    break;

            }

            TwilightCycles[plr.Name] += TwilightCycles[plr.Name] < 2 ? 1 : -TwilightCycles[plr.Name];    // Loop sub-ability
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
                        PositionInWorld = new(plr.X + 16, plr.Y + 16),
                        UniqueInfoPiece = 1
                    };
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.BlackLightningHit, settings);
                    break;
                case 1:
                    ParticleOrchestraSettings JudgeSparklesettings = new()
                    {
                        IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                        MovementVector = new(0, 0),
                        PositionInWorld = new(plr.X + 16 + plr.TPlayer.direction * 16, plr.Y + 16),
                        UniqueInfoPiece = 1
                    };
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PaladinsHammer, JudgeSparklesettings);
                    break;
                case 2:
                    ParticleOrchestraSettings JudgeTriggersettings = new()
                    {
                        IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                        MovementVector = new(0, 0),
                        PositionInWorld = new(plr.X + 16 + plr.TPlayer.direction * 16, plr.Y + 16),
                        UniqueInfoPiece = 1
                    };
                    ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.Keybrand, JudgeTriggersettings);
                    break;
                case 3:
                    NPC npc = (NPC)args[2];
                    ParticleOrchestraSettings JudgeHitsettings = new()
                    {
                        PositionInWorld = new(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2)
                    };

                    for (double i = 0; i < Math.Tau; i += 0.785415617)
                    {
                        JudgeHitsettings.MovementVector = new Vector2(
                            new Vector2(JudgeHitsettings.PositionInWorld.X, JudgeHitsettings.PositionInWorld.Y).DirectionTo(new Vector2(JudgeHitsettings.PositionInWorld.X + 10 * (float)Math.Cos(i), JudgeHitsettings.PositionInWorld.Y + 10 * (float)Math.Sin(i))).X * 15,
                            new Vector2(JudgeHitsettings.PositionInWorld.X, JudgeHitsettings.PositionInWorld.Y).DirectionTo(new Vector2(JudgeHitsettings.PositionInWorld.X + 10 * (float)Math.Cos(i), JudgeHitsettings.PositionInWorld.Y + 10 * (float)Math.Sin(i))).Y * 15);
                        ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.PaladinsHammer, JudgeHitsettings);
                    }
                    break;


            }
        }

        internal override void CycleLogic(TSPlayer plr)
        {
            TwilightCycles[plr.Name] += TwilightCycles[plr.Name] < 2 ? 1 : -TwilightCycles[plr.Name];
            Extensions.SendFloatingMessage("Cycled the ability!", plr.TPlayer.position.X + 16, plr.TPlayer.position.Y - 16, 115, 10, 115, plr.Index);
        }
    }
}
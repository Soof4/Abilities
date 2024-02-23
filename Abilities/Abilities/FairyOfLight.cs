using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;

namespace Abilities
{
    public class FairyOfLight : Ability
    {
        private static Dictionary<string, int> FairyOfLightCycles = new Dictionary<string, int>();
        private int LanceDmg, DashDmg, BoltDmg, BoltSpawnInterval, DanceDmg;

        public FairyOfLight(int abilityLevel)
        {
            CalculateProperties(abilityLevel);
        }

        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                LanceDmg = (int)((50 + (abilityLevel - 1) * 15) * (1 + abilityLevel / 10f));
                DashDmg = (int)((100 + (abilityLevel - 1) * 30) * (1 + abilityLevel / 10f));
                BoltDmg = (int)((10 + (abilityLevel - 1) * 10) * (1 + abilityLevel / 10));
                BoltSpawnInterval = 400 * (10 - abilityLevel) / 10;
                DanceDmg = (int)((55 + (abilityLevel - 1) * 20) * (1 + abilityLevel / 10f));
            }
        }

        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            CalculateProperties(abilityLevel);

            if (!FairyOfLightCycles.ContainsKey(plr.Name))
            {
                FairyOfLightCycles.Add(plr.Name, 0);
            }

            switch (FairyOfLightCycles[plr.Name])
            {
                case 0:    // Ethereal Lance
                    float startingYPos = plr.Y - 8 * 16;
                    for (int i = 0; i < 19; i += 3)
                    {
                        Extensions.SpawnProjectile(
                            posX: plr.X + (i % 2 * 60 - 30) * 16,
                            posY: startingYPos + i * 16,
                            speedX: i % 2 * 60 * -1 + 30,
                            speedY: 0,
                            type: ProjectileID.FairyQueenRangedItemShot,
                            damage: LanceDmg,
                            knockback: 5,
                            owner: plr.Index,
                            ai_1: Extensions.HallowedWeaponColor
                            );
                    }
                    break;
                case 1:    // Dash
                    plr.SendData(PacketTypes.PlayerDodge, number: (byte)plr.Index, number2: 2);

                    int direction = Extensions.GetVelocityXDirection(plr.TPlayer);

                    Task.Run(async () =>
                    {
                        plr.TPlayer.velocity.X = 36 * direction;
                        TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", plr.Index);
                        float startX = plr.X;
                        float startY = plr.Y;
                        float xVelocity = plr.TPlayer.velocity.X;

                        for (int i = 0; i < 12; i++)
                        {
                            Extensions.SpawnProjectile(
                                posX: plr.X + 16,
                                posY: plr.Y + 16,
                                speedX: 0,
                                speedY: 0,
                                type: ProjectileID.TrueExcalibur,
                                damage: DashDmg,
                                knockback: 4,
                                owner: plr.Index,
                                ai_0: 16,     // laps
                                ai_1: 12,     // time
                                ai_2: 0.4f    // size
                                );

                            if (i == 9)
                            {
                                plr.SetBuff(BuffID.Webbed, 1);
                            }

                            await Task.Delay(17);
                        }
                    });
                    break;
                case 2:    // Prismatic Bolts
                    Task.Run(async () =>
                    {
                        int ms = 0;
                        while (ms < 2500)
                        {
                            Extensions.SpawnProjectile(
                                posX: plr.X + Extensions.Random.Next(33),
                                posY: plr.Y + Extensions.Random.Next(-8, 33),
                                speedX: Extensions.Random.Next(-4, 5),
                                speedY: Extensions.Random.Next(-4, 5),
                                type: ProjectileID.FairyQueenMagicItemShot,
                                damage: BoltDmg,
                                knockback: 6,
                                owner: plr.Index,
                                ai_1: Extensions.HallowedWeaponColor
                                );

                            ms += BoltSpawnInterval;
                            await Task.Delay(BoltSpawnInterval);
                        }
                    });
                    break;
                case 3:    // Sundance
                    plr.SetBuff(BuffID.Webbed, 140, true);

                    Task.Run(async () =>
                    {
                        for (int i = 0; i < 3; i++)
                        {    // TODO: Rewrite this without trigonometric functions.
                            for (double j = i % 2 == 0 ? 0.7853 : 1.5707; j < Math.Tau; j += 1.5707)
                            {
                                Extensions.SpawnProjectile(
                                posX: plr.X + 16 + 64 * (float)Math.Cos(j),
                                posY: plr.Y + 16 + 64 * (float)Math.Sin(j),
                                speedX: 2 * (float)Math.Cos(j),
                                speedY: 2 * (float)Math.Sin(j),
                                type: ProjectileID.PrincessWeapon,
                                damage: DanceDmg,
                                knockback: 5,
                                owner: plr.Index
                                );
                            }
                            await Task.Delay(500);
                        }
                    });
                    break;
            }

            FairyOfLightCycles[plr.Name] += FairyOfLightCycles[plr.Name] < 3 ? 1 : -FairyOfLightCycles[plr.Name];    // Loop sub-ability
        }

        internal override void PlayVisuals(params object[] args) { }
    }
}
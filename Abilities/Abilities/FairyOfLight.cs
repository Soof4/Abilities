using Terraria.ID;
using TShockAPI;

namespace Abilities.Abilities
{
    public class FairyOfLight : Ability
    {
        public static Dictionary<string, int> FairyOfLightCycles = new Dictionary<string, int>();
        public int LanceDmg, DashDmg, BoltDmg, BoltSpawnInterval, DanceDmg;

        public FairyOfLight(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats = () =>
            {
                LanceDmg = (int)((50 + (AbilityLevel - 1) * 15) * (1 + AbilityLevel / 10f));
                DashDmg = (int)((100 + (AbilityLevel - 1) * 30) * (1 + AbilityLevel / 10f));
                BoltDmg = (int)((10 + (AbilityLevel - 1) * 10) * (1 + AbilityLevel / 10));
                BoltSpawnInterval = 400 * (10 - AbilityLevel) / 10;
                DanceDmg = (int)((55 + (AbilityLevel - 1) * 20) * (1 + AbilityLevel / 10f));
            };

            UpdateStats();
        }

        protected override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
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
                        Utils.SpawnProjectile(
                            posX: plr.X + (i % 2 * 60 - 30) * 16,
                            posY: startingYPos + i * 16,
                            speedX: i % 2 * 60 * -1 + 30,
                            speedY: 0,
                            type: ProjectileID.FairyQueenRangedItemShot,
                            damage: LanceDmg,
                            knockback: 5,
                            owner: plr.Index,
                            ai_1: Utils.HallowedWeaponColor
                            );
                    }
                    break;
                case 1:    // Dash
                    plr.SendData(PacketTypes.PlayerDodge, number: (byte)plr.Index, number2: 2);

                    int direction = Utils.GetVelocityXDirection(plr.TPlayer);

                    Task.Run(async () =>
                    {
                        plr.TPlayer.velocity.X = 36 * direction;
                        TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", plr.Index);
                        float startX = plr.X;
                        float startY = plr.Y;
                        float xVelocity = plr.TPlayer.velocity.X;

                        Utils.SpawnProjectile(
                            posX: plr.X + 16,
                            posY: plr.Y + 16,
                            speedX: 0,
                            speedY: 0,
                            type: ProjectileID.TrueExcalibur,
                            damage: DashDmg,
                            knockback: 9,
                            owner: plr.Index,
                            ai_0: 16,     // laps
                            ai_1: 12,     // time
                            ai_2: 0.4f    // size
                            );

                        await Task.Delay(12 * 16);
                        plr.SetBuff(BuffID.Webbed, 1);
                    });
                    break;
                case 2:    // Prismatic Bolts
                    Task.Run(async () =>
                    {
                        int ms = 0;
                        while (ms < 2500)
                        {
                            Utils.SpawnProjectile(
                                posX: plr.X + Utils.Random.Next(33),
                                posY: plr.Y + Utils.Random.Next(-8, 33),
                                speedX: Utils.Random.Next(-4, 5),
                                speedY: Utils.Random.Next(-4, 5),
                                type: ProjectileID.FairyQueenMagicItemShot,
                                damage: BoltDmg,
                                knockback: 6,
                                owner: plr.Index,
                                ai_1: Utils.HallowedWeaponColor
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
                                Utils.SpawnProjectile(
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

        protected override void PlayVisuals(params object[] args) { }

        protected override void CycleLogic(TSPlayer plr)
        {
            FairyOfLightCycles[plr.Name] += FairyOfLightCycles[plr.Name] < 3 ? 1 : -FairyOfLightCycles[plr.Name];
            Utils.SendFloatingMessage("Cycled the ability!", plr.TPlayer.position.X + 16, plr.TPlayer.position.Y - 16, 255, 40, 255, plr.Index);
        }
    }
}
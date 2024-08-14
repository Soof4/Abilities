using Terraria.ID;
using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;

namespace Abilities
{
    public class Paranoia : Ability
    {
        public int DurationInSecs, RangeInBlocks;

        public Paranoia(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats = () =>
            {
                DurationInSecs = 4 + AbilityLevel;
                RangeInBlocks = 25 + AbilityLevel * 10;
            };
        }

        protected override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr);

            foreach (TSPlayer p in TShock.Players)
            {
                if (p.IsAlive() && p.TPlayer.hostile && p.Index != plr.Index && p.TPlayer.WithinRange(plr.TPlayer.position, 16 * RangeInBlocks))
                {
                    double gameModeBuffFactor = 1;
                    if (Main.GameMode == GameModeID.Master)
                    {
                        gameModeBuffFactor = 2.5;
                    }
                    else if (Main.GameMode == GameModeID.Expert)
                    {
                        gameModeBuffFactor = 2;
                    }

                    p.SetBuff(BuffID.Obstructed, DurationInSecs * 60);
                    p.SetBuff(BuffID.Silenced, (int)(DurationInSecs * 60 / gameModeBuffFactor));
                    p.SetBuff(BuffID.Bleeding, (int)(DurationInSecs * 60 / gameModeBuffFactor));
                    p.SetBuff(BuffID.WitheredArmor, DurationInSecs * 60);
                    p.SetBuff(BuffID.WitheredWeapon, DurationInSecs * 60);
                    PlaySounds(p, DurationInSecs * 1000);
                }
            }
        }

        protected override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];

            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                MovementVector = new(0, -8),
                PositionInWorld = new(plr.X + 16, plr.Y - 16),
                UniqueInfoPiece = ItemID.ScytheWhip
            };

            Shapes.DrawCircle(plr.X + 16, plr.Y + 16, RangeInBlocks * 16, 0.1963, ParticleOrchestraType.NightsEdge, (byte)plr.Index);
        }

        private static void PlaySounds(TSPlayer plr, int duration)
        {
            (ushort, int) deerStep = Utils.GetSoundIndexAndId(42, 250);
            (ushort, int) abigailCry0 = Utils.GetSoundIndexAndId(42, 256);
            (ushort, int) abigailCry1 = Utils.GetSoundIndexAndId(42, 257);
            (ushort, int) abigailUpgrade0 = Utils.GetSoundIndexAndId(42, 260);
            (ushort, int) abigailUpgrade1 = Utils.GetSoundIndexAndId(42, 261);
            (ushort, int) abigailUpgrade2 = Utils.GetSoundIndexAndId(42, 262);
            (ushort, int) dreadCharge = Utils.GetSoundIndexAndId(2, 170);
            (ushort, int) koboldFlyerDeath1 = Utils.GetSoundIndexAndId(42, 129);

            Task.Run(async () =>
            {
                if (Utils.Random.Next(2) == 0)
                {
                    NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, abigailCry0.Item1, abigailCry0.Item2, 10), plr.Index);
                }
                else
                {
                    NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, abigailCry1.Item1, abigailCry1.Item2, 10), plr.Index);
                }

                int ms = 0;
                while (ms < duration)
                {
                    if (Utils.Random.Next(3) == 0)
                    {
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, deerStep.Item1, deerStep.Item2), plr.Index);
                    }

                    if (Utils.Random.Next(20) == 0)
                    {
                        NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, koboldFlyerDeath1.Item1, koboldFlyerDeath1.Item2, 0.2f, -0.99f), plr.Index);
                    }

                    if (ms % 2000 == 0)
                    {
                        switch (Utils.Random.Next(4))
                        {
                            case 0:
                                NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, abigailUpgrade1.Item1, abigailUpgrade1.Item2, 10), plr.Index);
                                break;
                            case 1:
                                NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, abigailUpgrade1.Item1, abigailUpgrade1.Item2, 10), plr.Index);
                                break;
                            case 2:
                                NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, abigailUpgrade1.Item1, abigailUpgrade1.Item2, 10), plr.Index);
                                break;
                            case 3:
                                NetMessage.PlayNetSound(new NetMessage.NetSoundInfo(plr.TPlayer.position, dreadCharge.Item1, dreadCharge.Item2, 10), plr.Index);
                                break;
                        }
                    }

                    ms += 500;
                    await Task.Delay(500);
                }
            });
        }
    }
}
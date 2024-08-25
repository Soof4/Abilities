using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;

namespace Abilities.Abilities
{
    public class SetsBlessing : Ability
    {
        public double DodgeDurationInSeconds;

        public SetsBlessing(int abilityLevel) : base(abilityLevel)
        {
            UpdateStats = () =>
            {
                DodgeDurationInSeconds = 3 + AbilityLevel * 0.2;
            };

            UpdateStats();
        }

        protected override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            PlayVisuals(plr);

            double secs = DodgeDurationInSeconds;

            Task.Run(async () =>
            {
                while (secs >= 1.33)
                {
                    plr.SendData(PacketTypes.PlayerDodge, number: plr.Index, number2: 2);
                    secs -= 0.1;
                    await Task.Delay(100);
                }
            });
        }

        protected override void PlayVisuals(params object[] args)
        {
            TSPlayer plr = (TSPlayer)args[0];

            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = (byte)plr.Index,
                MovementVector = new(0, 0),
                PositionInWorld = new(plr.X + 16, plr.Y + 16),
                UniqueInfoPiece = 1
            };

            Projectile.NewProjectile(Projectile.GetNoneSource(),
                       plr.TPlayer.position, new Vector2(0, 0),
                       Type: 704, Damage: 0, KnockBack: 0,
                       ai0: 0, ai1: 0, ai2: 0);
        }
    }
}
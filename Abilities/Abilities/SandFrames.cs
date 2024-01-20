using TShockAPI;
using Terraria;
using Terraria.GameContent.Drawing;
using Microsoft.Xna.Framework;

namespace Abilities
{
    public class SandFrames : Ability
    {
        private double DodgeDurationInSeconds;

        public SandFrames(int abilityLevel)
        {
            CalculateProperties(abilityLevel);
        }


        internal override void CalculateProperties(params object[] args)
        {
            int abilityLevel = (int)args[0];

            if (abilityLevel != AbilityLevel)
            {
                AbilityLevel = abilityLevel;
                DodgeDurationInSeconds = 3 + abilityLevel * 0.2;
            }
        }


        internal override void Function(TSPlayer plr, int cooldown, int abilityLevel = 1)
        {
            CalculateProperties(abilityLevel);
            PlayVisuals(plr);

            Task.Run(async () =>
            {
                while (DodgeDurationInSeconds >= 1.33)
                {
                    plr.SendData(PacketTypes.PlayerDodge, number: plr.Index, number2: 2);
                    DodgeDurationInSeconds -= 0.1;
                    await Task.Delay(100);
                }
            });
        }

        internal override void PlayVisuals(params object[] args)
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
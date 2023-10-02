using dw = Terraria.GameContent.Drawing;
using System;

namespace Abilities {
    public class Shapes {
        /// <summary>
        /// Draws a circle.
        /// </summary>
        /// <param name="posX">This is pixel coordinates.</param>
        /// <param name="posY">This is pixel coordinates.</param>
        /// <param name="range">This is in pixels. (One tile length = 16 pixels)</param>
        /// <param name="deltaRadian">This changes how tight the particles will spawn. Lower the value means more particles. If set too low might cause "Bad header lead to buffer overflow for clients."</param>
        /// <param name="particleType">The type of particles.</param>
        public static void DrawCircle(float posX, float posY, int range, double deltaRadian, dw.ParticleOrchestraType particleType, 
            byte indexOfPlayerWhoInvokedThis, int uniqueInfoPiece = 1) {

            dw.ParticleOrchestraSettings settings = new() {
                IndexOfPlayerWhoInvokedThis = indexOfPlayerWhoInvokedThis,
                MovementVector = new(0, 0),
                PositionInWorld = new(posX, posY),
                UniqueInfoPiece = uniqueInfoPiece
            };

            for (double i = 0; i < Math.Tau; i += deltaRadian) {
                settings.PositionInWorld = new Microsoft.Xna.Framework.Vector2(posX + range*(float)Math.Cos(i), posY + range*(float)Math.Sin(i));
                dw.ParticleOrchestrator.BroadcastParticleSpawn(particleType, settings);
            }
        }

        /// <summary>
        /// This has not been properly built yet.
        /// </summary>
        public static void DrawLine(float posX1, float posY1, float posX2, float posY2, float deltaLength, dw.ParticleOrchestraType particleType,
            byte indexOfPlayerWhoInvokedThis, int uniqueInfoPiece = 1) {

            dw.ParticleOrchestraSettings settings = new() {
                IndexOfPlayerWhoInvokedThis = indexOfPlayerWhoInvokedThis,
                MovementVector = new(0, 0),
                PositionInWorld = new(posX1, posY1),
                UniqueInfoPiece = uniqueInfoPiece
            };

            float tetha = (float)Math.Atan2(posY2 - posY1, posX2 - posX1);
            float dx = deltaLength * (float)Math.Cos(tetha);
            float dy = deltaLength * (float)Math.Sin(tetha);

            for (double i = 0, j = 0; i < posX2 && j < posY2; i += dx, j += dy) {
                settings.PositionInWorld = new(posX1 + dx, posY1 + dy);
                dw.ParticleOrchestrator.BroadcastParticleSpawn(particleType, settings);
            }
        }
    }
}

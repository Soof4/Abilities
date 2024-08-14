using System;
using System.Numerics;
using Terraria.GameContent.Drawing;

namespace Abilities
{
    public static class Shapes
    {
        /// <summary>
        /// Draws a circle.
        /// </summary>
        /// <param name="posX">This is pixel coordinates.</param>
        /// <param name="posY">This is pixel coordinates.</param>
        /// <param name="range">This is in pixels. (One tile length = 16 pixels)</param>
        /// <param name="deltaRadian">This changes how tight the particles will spawn. Lower the value means more particles. If set too low might cause "Bad header lead to buffer overflow for clients."</param>
        /// <param name="particleType">The type of particles.</param>
        public static void DrawCircle(float posX, float posY, int range, double deltaRadian, ParticleOrchestraType particleType,
            byte indexOfPlayerWhoInvokedThis, int uniqueInfoPiece = 1)
        {

            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = indexOfPlayerWhoInvokedThis,
                MovementVector = new(0, 0),
                PositionInWorld = new(posX, posY),
                UniqueInfoPiece = uniqueInfoPiece
            };

            for (double i = 0; i < Math.Tau; i += deltaRadian)
            {
                settings.PositionInWorld = new Microsoft.Xna.Framework.Vector2(posX + range * (float)Math.Cos(i), posY + range * (float)Math.Sin(i));
                ParticleOrchestrator.BroadcastParticleSpawn(particleType, settings);
            }
        }

        public static void DrawLine(float posX1, float posY1, float posX2, float posY2, float deltaLength, ParticleOrchestraType particleType,
            byte indexOfPlayerWhoInvokedThis, int uniqueInfoPiece = 0)
        {

            ParticleOrchestraSettings settings = new()
            {
                IndexOfPlayerWhoInvokedThis = indexOfPlayerWhoInvokedThis,
                MovementVector = new(0, 0),
                PositionInWorld = new(posX1, posY1),
                UniqueInfoPiece = uniqueInfoPiece
            };

            float tetha = (float)Math.Atan2(posY2 - posY1, posX2 - posX1);
            float dx = deltaLength * (float)Math.Cos(tetha);
            float dy = deltaLength * (float)Math.Sin(tetha);
            int loopCount = Math.Max((int)Math.Abs((posX2 - posX1) / dx), (int)Math.Abs((posY2 - posY1) / dy));

            for (double i = 0; i < loopCount; i++)
            {
                posX1 += dx;
                posY1 += dy;
                settings.PositionInWorld = new(posX1, posY1);
                ParticleOrchestrator.BroadcastParticleSpawn(particleType, settings);
            }
        }

        public static void DrawStar(float posX, float posY, int size, float deltaLength, ParticleOrchestraType particleType, byte indexOfPlayerWhoInvokedThis, int uniqueInfoPiece = 1)
        {
            float sin18 = (float)Math.Sin(0.314159);
            float cos18 = (float)Math.Cos(0.314159);
            float sin54 = (float)Math.Sin(0.942478);
            float cos54 = (float)Math.Cos(0.942478);
            Vector2 topPos = new Vector2(posX, posY - size);
            Vector2 topLeft = new Vector2(posX - size * cos18, posY - size * sin18);
            Vector2 bottomLeft = new Vector2(posX - size * cos54, posY + size * sin54);
            Vector2 bottomRight = new Vector2(posX + size * cos54, posY + size * sin54);
            Vector2 topRight = new Vector2(posX + size * cos18, posY - size * sin18);

            DrawLine(topPos.X, topPos.Y, bottomLeft.X, bottomLeft.Y, deltaLength, particleType, indexOfPlayerWhoInvokedThis, uniqueInfoPiece);
            DrawLine(bottomLeft.X, bottomLeft.Y, topRight.X, topRight.Y, deltaLength, particleType, indexOfPlayerWhoInvokedThis, uniqueInfoPiece);
            DrawLine(topRight.X, topRight.Y, topLeft.X, topLeft.Y, deltaLength, particleType, indexOfPlayerWhoInvokedThis, uniqueInfoPiece);
            DrawLine(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y, deltaLength, particleType, indexOfPlayerWhoInvokedThis, uniqueInfoPiece);
            DrawLine(bottomRight.X, bottomRight.Y, topPos.X, topPos.Y, deltaLength, particleType, indexOfPlayerWhoInvokedThis, uniqueInfoPiece);
        }
    }
}

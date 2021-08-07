using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kalkatos.Cycles
{
    public static class GameVariables
    {
        public const int ScreenWidth = 1200;
        public const int ScreenHeight = 800;
        public const float MinX = -6.5f;
        public const float MaxX = 6.5f;
        public const float MinY = -4f;
        public const float MaxY = 4f;
        public const float StartingX = -100f;
        public const float StartingY = 0;
        public const float StartingZ = 100;

        public static Vector2 GetScreenPercent (Vector2 point)
        {
            return new Vector2(point.x / ScreenWidth, (ScreenHeight - point.y) / ScreenHeight);
        }

        public static float XCoord (float positionX)
		{
            return StartingX + MinX + (MaxX - MinX) * positionX;
        }

        public static float YCoord (float positionY)
        {
            return StartingY + MinY + (MaxY - MinY) * positionY;
        }

        public static Vector3 CoordToWorld (Vector2 position)
		{
            return new Vector3(XCoord(position.x), YCoord(position.y), StartingZ);
		}
    }
}

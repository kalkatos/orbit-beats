using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kalkatos.Cycles
{
    public static class GameVariables
    {
        public const int ScreenWidth = 1200;
        public const int ScreenHeight = 800;

        public static Vector2 GetScreenPercent (Vector2 point)
        {
            return new Vector2(point.x / ScreenWidth, (ScreenHeight - point.y) / ScreenHeight);
        }
    }
}

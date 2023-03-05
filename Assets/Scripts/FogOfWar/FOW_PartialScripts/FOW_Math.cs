using UnityEngine;

namespace VladB.Doka.FogOfWar
{
    public partial class FOW_Controller
    {
        // private bool IsBlocker(MapState state)
        // {
        //     return state == MapState.Blocker || state == MapState.VisibleBlocker;
        // }

        // private bool IsVisible(MapState state)
        // {
        //     return state == MapState.Light || state == MapState.VisibleBlocker;
        // }

        private Vector2Int ConvertFrom3DTo2D(Vector3 vec)
        {
            Vector2Int pos2D = new Vector2Int(
                Mathf.RoundToInt(vec.x * SizesMultiplier.x),
                Mathf.RoundToInt(vec.z * SizesMultiplier.y));
            return Clamp(pos2D);
        }

        private Vector2Int Clamp(Vector2Int pos2D)
        {
            pos2D.x = Mathf.Clamp(pos2D.x, 0, MapSizeX - 1);
            pos2D.y = Mathf.Clamp(pos2D.y, 0, MapSizeY - 1);
            return pos2D;
        }

        private bool IsInMap(Vector2Int pos2D)
        {
            return pos2D.x >= 0 && pos2D.y >= 0 && pos2D.x < MapSizeX && pos2D.y < MapSizeY;
        }

        private bool IsInMap(int finalPosX, int finalPosY)
        {
            return finalPosX >= 0 && finalPosY >= 0 && finalPosX < MapSizeX && finalPosY < MapSizeY;
        }

        private static float FindDistanceToSegment( //Поискать аналоги
            Vector2 pt, Vector2 p1, Vector2 p2, out Vector2 closest)
        {
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            if ((dx == 0) && (dy == 0))
            {
                // Это точка не отрезка.
                closest = p1;
                dx = pt.x - p1.x;
                dy = pt.y - p1.y;
                return Mathf.Sqrt(dx * dx + dy * dy);
            }

            // Вычислим t, который минимизирует расстояние.
            float t = ((pt.x - p1.x) * dx + (pt.y - p1.y) * dy) /
                      (dx * dx + dy * dy);

            // Посмотрим, представляет ли это один из сегментов
            // конечные точки или точка в середине.
            if (t < 0)
            {
                closest = new Vector2(p1.x, p1.y);
                dx = pt.x - p1.x;
                dy = pt.y - p1.y;
            }
            else if (t > 1)
            {
                closest = new Vector2(p2.x, p2.y);
                dx = pt.x - p2.x;
                dy = pt.y - p2.y;
            }
            else
            {
                closest = new Vector2(p1.x + t * dx, p1.y + t * dy);
                dx = pt.x - closest.x;
                dy = pt.y - closest.y;
            }

            return Mathf.Sqrt(dx * dx + dy * dy);
        }
    }
}
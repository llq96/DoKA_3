using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VladB.Doka
{
    public partial class WarFogController
    {
        private void CalculateLight()
        {
            var playerPos3D = MainController.Instance.UnitsManager.Player.transform.position;
            Vector2Int startPos = ConvertFrom3DTo2D(playerPos3D);
            _info[startPos.x, startPos.y] = 2;
            var lightDistance = 30; //TODO

            //TODO Подумать над алгоритмом
            for (int a = 0; a < 360; a += _perAngle)
            {
                var rad = a * Mathf.Deg2Rad;
                CalculateLightForRay(startPos, startPos
                                               + Vector2Int.right * Mathf.RoundToInt(Mathf.Cos(rad) * lightDistance)
                                               + Vector2Int.up * Mathf.RoundToInt(Mathf.Sin(rad) * lightDistance)
                );
            }
        }

        private void CalculateLightForRay(Vector2Int start, Vector2Int end)
        {
            start = Clamp(start);
            end = Clamp(end);
            int minX = Mathf.Min(start.x, end.x);
            int minY = Mathf.Min(start.y, end.y);
            int maxX = Mathf.Max(start.x, end.x);
            int maxY = Mathf.Max(start.y, end.y);
            var sqrt2 = Mathf.Sqrt(2);
            List<Vector2Int> pointsOnRay = new();

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var distance = FindDistanceToSegment(
                        new Vector2(x, y),
                        new Vector2(start.x, start.y),
                        new Vector2(end.x, end.y),
                        out var closest
                    );

                    if (distance <= sqrt2)
                    {
                        pointsOnRay.Add(new Vector2Int(x, y));
                    }
                }
            }

            var iEnumerable = pointsOnRay.OrderBy(pos2D => (pos2D - start).sqrMagnitude);
            foreach (var pos2D in iEnumerable)
            {
                if (_info[pos2D.x, pos2D.y] == 1) break;
                _info[pos2D.x, pos2D.y] = 3;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VladB.Doka.FogOfWar
{
    public partial class FOW_Controller
    {
        [Header("PreparedAngles")]
        [SerializeField]
        private List<PreparedAngle> _preparedAngles = new();

        [Serializable]
        public class PreparedAngle
        {
            public float Angle;
            public List<PointData> Points;

            [Serializable]
            public class PointData
            {
                public int PosX;
                public int PosY;
                public float Distance;
            }
        }

        private void CalculateLight()
        {
            foreach (var lightPoint in _lights)
            {
                if (lightPoint == null) continue;
                if (lightPoint.transform == null) continue;
                if (!lightPoint.gameObject.activeInHierarchy) continue;

                Vector2Int startPos = ConvertFrom3DTo2D(lightPoint.transform.position);
                int startPosX = startPos.x;
                int startPosY = startPos.y;
                var lightDistance = lightPoint.Radius * SizesMultiplier.x;

                foreach (var preparedAngle in _preparedAngles)
                {
                    foreach (var pointData in preparedAngle.Points)
                    {
                        if (pointData.Distance > lightDistance) break;
                        int finalPosX = startPosX + pointData.PosX;
                        int finalPosY = startPosY + pointData.PosY;

                        if (!IsInMap(finalPosX, finalPosY)) break;

                        _map_isLight[finalPosX, finalPosY] = true;

                        if (_map_isBlocker[finalPosX, finalPosY])
                        {
                            break;
                        }
                    }
                }
            }
        }

        #region PreparedAngle

        [ContextMenu(nameof(GeneratePreparedAngles))]
        private void GeneratePreparedAngles()
        {
            _preparedAngles.Clear();

            Vector2Int startPos = new Vector2Int(0, 0);
            var lightDistance = 1000;

            for (int a = 0; a < 360; a += _perAngle)
            {
                var rad = a * Mathf.Deg2Rad;
                var list = CalculateLightForPreparedRay(startPos, startPos
                                                                  + Vector2Int.right *
                                                                  Mathf.RoundToInt(Mathf.Cos(rad) * lightDistance)
                                                                  + Vector2Int.up *
                                                                  Mathf.RoundToInt(Mathf.Sin(rad) * lightDistance)
                );

                var preparedAngle = new PreparedAngle
                {
                    Angle = a,
                    Points = list
                };
                _preparedAngles.Add(preparedAngle);
            }
        }


        private List<PreparedAngle.PointData> CalculateLightForPreparedRay(Vector2Int start, Vector2Int end)
        {
            int minX = Mathf.Min(start.x, end.x);
            int minY = Mathf.Min(start.y, end.y);
            int maxX = Mathf.Max(start.x, end.x);
            int maxY = Mathf.Max(start.y, end.y);
            var sqrt2 = Mathf.Sqrt(2);
            List<PreparedAngle.PointData> pointsOnRay = new();

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
                        var pointData = new PreparedAngle.PointData
                        {
                            PosX = x,
                            PosY = y,
                            Distance = new Vector2Int(x, y).magnitude
                        };
                        pointsOnRay.Add(pointData);
                    }
                }
            }

            return pointsOnRay.OrderBy(pointData => pointData.Distance).ToList();
        }

        #endregion
    }
}
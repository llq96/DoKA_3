using UnityEngine;

namespace VladB.Doka.FogOfWar
{
    public class BlockerInfo
    {
        public FOW_BlockerPoint BlockerPoint { get; }
        public Vector3 WorldPosition { get; private set; }
        public float Radius { get; private set; }
        // public List<Vector2Int> Points = new();
        public bool IsVisible;

        public BlockerInfo(FOW_BlockerPoint blockerPoint)
        {
            BlockerPoint = blockerPoint;
        }

        public void UpdateData()
        {
            WorldPosition = BlockerPoint.transform.position;
            Radius = BlockerPoint.Radius;
        }
    }
}
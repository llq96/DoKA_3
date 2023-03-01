using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

namespace VladB.Doka
{
    public partial class WarFogController : MonoBehaviour
    {
        [Header("Size")] [SerializeField] private int MapSizeX = 40;
        [SerializeField] private int MapSizeY = 40;
        [SerializeField] private int MapRealSizeX = 20;
        [SerializeField] private int MapRealSizeY = 20;

        [Header("Other Settings")]
        [SerializeField]
        private float _updateDelay = 0.1f;

        [SerializeField] [Range(1, 10)] private int _perAngle = 5;

        [Header("Debug")] [SerializeField] private Material _fogDebugMat;
        private Texture2D _debugTex;
        [SerializeField] private Color[] _debugColors;

        public Vector2Int MapSize { get; private set; }
        public Vector2Int MapRealSize { get; private set; }
        public Vector2 SizesMultiplier { get; private set; }

        private List<PointForWarFog> _pointsForFog;

        private int[,] _info;


        public void Init()
        {
            MapSize = new Vector2Int(MapSizeX, MapSizeY);
            MapRealSize = new Vector2Int(MapRealSizeX, MapRealSizeY);
            SizesMultiplier = new Vector2(MapSizeX / (float)MapRealSize.x, MapSizeY / (float)MapRealSize.y);

            _info = new int[MapSizeX, MapSizeY];

            _debugTex = new Texture2D(MapSizeX, MapSizeY, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point
            };
            _fogDebugMat.mainTexture = _debugTex;

            _pointsForFog = FindObjectsOfType<PointForWarFog>(true).ToList();
            _pointsForFog.ForEach(x => x.Init());

            StartCoroutine(UpdateCor());
        }

        private IEnumerator UpdateCor()
        {
            while (true)
            {
                yield return new WaitForSeconds(_updateDelay);
                ForceUpdateFog();
            }
        }

        private void ForceUpdateFog()
        {
            _info = new int[MapSizeX, MapSizeY];

            AddBlockers();
            CalculateLight();

#if UNITY_EDITOR
            UpdateDebugTexture();
#endif
        }

        private void AddBlockers()
        {
            foreach (var point in _pointsForFog)
            {
                if (point == null) continue;
                if (point.transform == null) continue;

                Vector2Int center = ConvertFrom3DTo2D(point.transform.position);
                Vector2Int leftTop =
                    ConvertFrom3DTo2D(point.transform.position + point.Radius * (Vector3.left + Vector3.forward));
                Vector2Int rightBot =
                    ConvertFrom3DTo2D(point.transform.position + point.Radius * (Vector3.right + Vector3.back));

                for (int x = leftTop.x; x <= rightBot.x; x++)
                {
                    for (int y = rightBot.y; y <= leftTop.y; y++)
                    {
                        //TODO types
                        var deltaX = x - center.x;
                        var deltaY = y - center.y;
                        var sqrDistance = deltaX * deltaX + deltaY * deltaY;
                        if (sqrDistance <= point.Radius * point.Radius)
                        {
                            _info[x, y] = 1;
                        }
                    }
                }
            }
        }

        private void UpdateDebugTexture()
        {
            var colors = new Color[MapSizeX * MapSizeY];
            for (int x = 0; x < MapSizeX; x++)
            {
                for (int y = 0; y < MapSizeY; y++)
                {
                    var value = _info[x, y];
                    colors[y * MapSizeX + x] = _debugColors[value];
                }
            }

            _debugTex.SetPixels(colors);
            _debugTex.Apply(false);
        }
    }
}
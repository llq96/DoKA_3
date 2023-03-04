using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

namespace VladB.Doka.WarFog
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

        [Header("Debug")] [SerializeField] private bool _isNeedUpdateDebugTexture;
        [SerializeField] private Material _fogDebugMat;

        [SerializeField] private Color[] _debugColors;
        private Texture2D _debugTex;
        [SerializeField] private GameObject _debugPlane;

        public Vector2Int MapSize { get; private set; }
        public Vector2Int MapRealSize { get; private set; }
        public Vector2 SizesMultiplier { get; private set; }

        private List<WarFog_BlockerPoint> _blockers;
        private List<WarFog_LightPoint> _lights;

        private MapState[,] _info;

        public enum MapState
        {
            Nothing,
            Blocker,
            Light
        }

        public void Init()
        {
            MapSize = new Vector2Int(MapSizeX, MapSizeY);
            MapRealSize = new Vector2Int(MapRealSizeX, MapRealSizeY);
            SizesMultiplier = new Vector2(MapSizeX / (float)MapRealSize.x, MapSizeY / (float)MapRealSize.y);

            _info = new MapState[MapSizeX, MapSizeY];

            _debugTex = new Texture2D(MapSizeX, MapSizeY, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point
            };
            _fogDebugMat.mainTexture = _debugTex;
            Vector3 _debugPlanePos = _debugPlane.transform.position;
            _debugPlanePos.x = MapRealSizeX / 2f - 1 / SizesMultiplier.x * 0.5f;
            _debugPlanePos.z = MapRealSizeY / 2f - 1 / SizesMultiplier.y * 0.5f;
            _debugPlane.transform.position = _debugPlanePos;

            _blockers = FindObjectsOfType<WarFog_BlockerPoint>(true).ToList();
            _blockers.ForEach(x => x.Init());

            _lights = FindObjectsOfType<WarFog_LightPoint>(true).ToList();
            _lights.ForEach(x => x.Init());

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
            _info = new MapState[MapSizeX, MapSizeY];

            AddBlockers();
            CalculateLight();

#if UNITY_EDITOR
            if (_isNeedUpdateDebugTexture)
            {
                UpdateDebugTexture();
            }
#endif
        }

        private void AddBlockers()
        {
            foreach (var point in _blockers)
            {
                if (point == null) continue;
                if (point.transform == null) continue;

                Vector2Int center = ConvertFrom3DTo2D(point.transform.position);
                var pointRadius = point.Radius * SizesMultiplier.x;

                Vector2Int leftTop =
                    ConvertFrom3DTo2D(point.transform.position + pointRadius * (Vector3.left + Vector3.forward));
                Vector2Int rightBot =
                    ConvertFrom3DTo2D(point.transform.position + pointRadius * (Vector3.right + Vector3.back));

                for (int x = leftTop.x; x <= rightBot.x; x++)
                {
                    for (int y = rightBot.y; y <= leftTop.y; y++)
                    {
                        //TODO types
                        var deltaX = x - center.x;
                        var deltaY = y - center.y;
                        var sqrDistance = deltaX * deltaX + deltaY * deltaY;
                        if (sqrDistance <= pointRadius * pointRadius)
                        {
                            _info[x, y] = MapState.Blocker;
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
                    colors[y * MapSizeX + x] = _debugColors[(int)value];
                }
            }

            _debugTex.SetPixels(colors);
            _debugTex.Apply(false);
        }
    }
}
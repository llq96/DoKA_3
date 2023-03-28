using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Color = UnityEngine.Color;

namespace VladB.Doka.FogOfWar
{
    public partial class FOW_Controller : MonoBehaviour
    {
        [Header("Size")] [SerializeField] private int MapSizeX = 40;
        [SerializeField] private int MapSizeY = 40;
        [SerializeField] private int MapRealSizeX = 20;
        [SerializeField] private int MapRealSizeY = 20;

        [Header("Other Settings")]
        [SerializeField]
        private float _updateDelay = 0.1f;

        [SerializeField] [Range(1, 10)] private int _perAngle = 5;

        [Header("Mask")] [SerializeField] private MeshRenderer _floorRenderer;
        [SerializeField] private Material _fogMaskMaterial;
        [SerializeField] private Color[] _maskColors;
        [SerializeField] private GameObject _debugPlane;
        private Texture2D _fogMaskTexture;

        public Vector2Int MapSize { get; private set; }
        public Vector2Int MapRealSize { get; private set; }
        public Vector2 SizesMultiplier { get; private set; }

        private List<BlockerInfo> _blockers = new();
        private List<FOW_BlockerPoint> _blockerPoints;
        private List<FOW_LightPoint> _lights;
        private List<FOW_VisibilityChangingObject> _visibilityChangingObjects;

        private class BlockerInfo
        {
            public FOW_BlockerPoint BlockerPoint;
            public List<Vector2Int> Points = new();
            public bool IsVisible;
        }

        private MapState[,] _info;
        private List<BlockerInfo>[,] _blockersAtMap;
        private static readonly int DissolveMap = Shader.PropertyToID("_DissolveMap");

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

            _fogMaskTexture = new Texture2D(MapSizeX, MapSizeY, TextureFormat.RGBA32, false);
            // _fogMaskMaterial.mainTexture = _fogMaskTexture;
            _fogMaskMaterial.SetTexture(DissolveMap, _fogMaskTexture);
            _floorRenderer.materials = new[] { _floorRenderer.material, _fogMaskMaterial };

            Vector3 _debugPlanePos = _debugPlane.transform.position;
            _debugPlanePos.x = MapRealSizeX / 2f - 1 / SizesMultiplier.x * 0.5f;
            _debugPlanePos.z = MapRealSizeY / 2f - 1 / SizesMultiplier.y * 0.5f;
            _debugPlane.transform.position = _debugPlanePos;

            _blockerPoints = FindObjectsOfType<FOW_BlockerPoint>(true).ToList();
            _blockerPoints.ForEach(x => x.Init());

            _lights = FindObjectsOfType<FOW_LightPoint>(true).ToList();
            _lights.ForEach(x => x.Init());

            _visibilityChangingObjects = FindObjectsOfType<FOW_VisibilityChangingObject>(true).ToList();
            _visibilityChangingObjects.ForEach(x => x.Init());

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
            _blockersAtMap = new List<BlockerInfo>[MapSizeX, MapSizeY];

            AddBlockers();
            CalculateLight();
            UpdateUnitsVisibility();
            UpdateVisibility();

            UpdateMaskTexture();
        }

        private void UpdateVisibility() //TODO
        {
            foreach (var obj in _visibilityChangingObjects)
            {
                Vector2Int startPos = ConvertFrom3DTo2D(obj.transform.position);
                if (IsInMap(startPos))
                {
                    var isVisible = _info[startPos.x, startPos.y] == MapState.Light;
                    obj.SetVisibility(isVisible);
                }
                else
                {
                    obj.SetVisibility(false);
                }
            }

            foreach (var blocker in _blockers.Where(x => x.IsVisible))
            {
                blocker.BlockerPoint.VisibilityChangingObjects.ForEach(x => x.SetVisibility(true));
            }
        }

        private void UpdateUnitsVisibility()
        {
            foreach (var unit in MainController.Instance.UnitsManager.AllUnits)
            {
                if (unit is Player) continue; //TODO
                Vector2Int startPos = ConvertFrom3DTo2D(unit.transform.position);
                if (IsInMap(startPos))
                {
                    var isVisible = _info[startPos.x, startPos.y] == MapState.Light;
                    unit.Visibility.SetVisibility(isVisible);
                }
                else
                {
                    unit.Visibility.SetVisibility(false);
                }
            }
        }

        private void AddBlockers()
        {
            _blockers.Clear();
            _blockerPoints.ForEach(x =>
            {
                var blockerInfo = new BlockerInfo
                {
                    BlockerPoint = x
                };
                _blockers.Add(blockerInfo);
            });

            foreach (var blocker in _blockers)
            {
                var point = blocker.BlockerPoint;
                if (point == null) continue;
                if (point.transform == null) continue;
                if (!point.gameObject.activeInHierarchy) continue;

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

                        _blockersAtMap[x, y] ??= new List<BlockerInfo>();
                        _blockersAtMap[x, y].Add(blocker);
                    }
                }
            }
        }

        private void UpdateMaskTexture()
        {
            var colors = new Color[MapSizeX * MapSizeY];
            for (int x = 0; x < MapSizeX; x++)
            {
                for (int y = 0; y < MapSizeY; y++)
                {
                    var value = _info[x, y];
                    colors[y * MapSizeX + x] = _maskColors[(int)value];
                }
            }

            _fogMaskTexture.SetPixels(colors);
            _fogMaskTexture.Apply(false);
        }
    }
}
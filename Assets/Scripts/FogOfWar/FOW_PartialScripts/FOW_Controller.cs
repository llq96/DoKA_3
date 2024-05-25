using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Color = UnityEngine.Color;

namespace VladB.Doka.FogOfWar
{
    public partial class FOW_Controller : MonoBehaviour //TODO Разбить на большее количество скриптов
    {
        [Inject] private UnitsManager _unitsManager;

        [Header("Size")] [SerializeField] private int MapSizeX = 40;
        [SerializeField] private int MapSizeY = 40;
        [SerializeField] private int MapRealSizeX = 20;
        [SerializeField] private int MapRealSizeY = 20;

        [Header("Other Settings")]
        [SerializeField]
        private float _updateDelay = 0.1f;

        [SerializeField] private float _updateColorsSpeed = 5f;
        [SerializeField] [Range(1, 10)] private int _perAngle = 5;

        [Header("Mask")] [SerializeField] private MeshRenderer _floorRenderer;
        [SerializeField] private Material _fogMaskMaterial;
        [SerializeField] private GameObject _debugPlane;
        private Texture2D _fogMaskTexture;

        public Vector2Int MapSize { get; private set; }
        public Vector2Int MapRealSize { get; private set; }
        public Vector2 SizesMultiplier { get; private set; }

        private List<BlockerInfo> _blockers;
        private List<FOW_BlockerPoint> _blockerPoints;
        private List<FOW_LightPoint> _lights;
        private List<FOW_VisibilityChangingObject> _visibilityChangingObjects;

        private class BlockerInfo
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

        private MapState[,] _info;
        private float[,] _visibleProgress;
        // private List<BlockerInfo>[,] _blockersAtMap;
        private static readonly int DissolveMap = Shader.PropertyToID("_DissolveMap");

        private readonly CancellationTokenSource _updateCancellationSource = new();

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
            _visibleProgress = new float[MapSizeX, MapSizeY];


            _fogMaskTexture = new Texture2D(MapSizeX, MapSizeY, TextureFormat.RGBA32, false);
            // _fogMaskMaterial.mainTexture = _fogMaskTexture;
            _fogMaskMaterial.SetTexture(DissolveMap, _fogMaskTexture);
            _floorRenderer.materials = new[] { _floorRenderer.material, _fogMaskMaterial };

            Vector3 debugPlanePos = _debugPlane.transform.position;
            debugPlanePos.x = MapRealSizeX / 2f - 1 / SizesMultiplier.x * 0.5f;
            debugPlanePos.z = MapRealSizeY / 2f - 1 / SizesMultiplier.y * 0.5f;
            _debugPlane.transform.position = debugPlanePos;

            _blockerPoints = FindObjectsOfType<FOW_BlockerPoint>(true).ToList();
            _blockerPoints.ForEach(x => x.Init());
            _blockers = _blockerPoints.Select(x => new BlockerInfo(x)).ToList();


            _lights = FindObjectsOfType<FOW_LightPoint>(true).ToList();
            _lights.ForEach(x => x.Init());

            _visibilityChangingObjects = FindObjectsOfType<FOW_VisibilityChangingObject>(true).ToList();
            _visibilityChangingObjects.ForEach(x => x.Init());

            UpdateFogCircle(_updateCancellationSource.Token).Forget();
        }


        private async UniTaskVoid UpdateFogCircle(CancellationToken cancellationToken)
        {
            while (true)
            {
                await UniTask.Delay((int)_updateDelay * 1000, DelayType.DeltaTime, PlayerLoopTiming.Update,
                    cancellationToken);
                if (cancellationToken.IsCancellationRequested) return;
                ForceUpdateFog();
            }
        }

        [SerializeField] private bool _isActiveUpdate;

        private void ForceUpdateFog()
        {
            for (int x = 0; x < MapSizeX; x++)
            {
                for (int y = 0; y < MapSizeY; y++)
                {
                    _info[x, y] = MapState.Nothing;
                }
            }


            if (_isActiveUpdate)
            {
                AddBlockers();
                CalculateLight();
                UpdateUnitsVisibility();
                UpdateVisibility();
                UpdateBlockersVisibility();
                UpdateMaskTexture();
            }
        }

        private void UpdateBlockersVisibility()
        {
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

                blocker.IsVisible = IsVisible();

                bool IsVisible()
                {
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
                                if (_info[x, y] == MapState.Light)
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    return false;
                }
            }

            foreach (var blocker in _blockers.Where(x => x.IsVisible))
            {
                blocker.BlockerPoint.VisibilityChangingObjects.ForEach(x => x.SetVisibility(true));
            }
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
        }

        private void UpdateUnitsVisibility()
        {
            foreach (var unit in _unitsManager.AllUnits)
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
            _blockers.ForEach(x => x.UpdateData());
          
            _blockers
                .Where(blocker => blocker.BlockerPoint != null 
                                  && blocker.BlockerPoint.gameObject.activeInHierarchy)
                .ToList()
                .AsParallel()
                .ForAll(AddBlocker);

        }

        private void AddBlocker(BlockerInfo blocker)
        {
            Vector2Int center = ConvertFrom3DTo2D(blocker.WorldPosition);
            var centerX = center.x;
            var centerY = center.y;
            var pointRadius = blocker.Radius * SizesMultiplier.x;

            Vector2Int leftTop =
                ConvertFrom3DTo2D(blocker.WorldPosition + pointRadius * (Vector3.left + Vector3.forward));
            Vector2Int rightBot =
                ConvertFrom3DTo2D(blocker.WorldPosition + pointRadius * (Vector3.right + Vector3.back));

            for (int x = leftTop.x; x <= rightBot.x; x++)
            {
                for (int y = rightBot.y; y <= leftTop.y; y++)
                {
                    //TODO types
                    var deltaX = x - centerX;
                    var deltaY = y - centerY;
                    var sqrDistance = deltaX * deltaX + deltaY * deltaY;
                    if (sqrDistance <= pointRadius * pointRadius)
                    {
                        _info[x, y] = MapState.Blocker;
                    }

                    // _blockersAtMap[x, y] ??= new List<BlockerInfo>();
                    // _blockersAtMap[x, y].Add(blocker);
                }
            }
        }

        private Color[] colors;
            
        private async void UpdateMaskTexture()
        {
            await UpdateColors();
            if(_updateCancellationSource.IsCancellationRequested) return;
            _fogMaskTexture.SetPixels(colors);
            _fogMaskTexture.Apply(false);
        }

        private async Task UpdateColors()
        {
            var deltaTime = Time.deltaTime;
            await Task.Delay(1).ConfigureAwait(false);
            colors = new Color[MapSizeX * MapSizeY];
            for (int x = 0; x < MapSizeX; x++)
            {
                for (int y = 0; y < MapSizeY; y++)
                {
                    var progress = _visibleProgress[x, y];

                    switch (_info[x, y])
                    {
                        case MapState.Nothing:
                            progress += deltaTime * _updateColorsSpeed;
                            break;
                        case MapState.Blocker:
                            progress += deltaTime * _updateColorsSpeed;
                            break;
                        case MapState.Light:
                            progress -= deltaTime * _updateColorsSpeed;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    progress = Mathf.Clamp01(progress);
                    colors[y * MapSizeX + x] = new Color(progress, progress, progress, progress);
                    _visibleProgress[x, y] = progress;
                }
            }
        }

        private void OnDestroy()
        {
            _updateCancellationSource.Cancel();
        }
    }
}
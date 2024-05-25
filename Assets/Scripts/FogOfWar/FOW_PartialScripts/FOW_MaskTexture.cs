using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;

namespace VladB.Doka.FogOfWar
{
    public partial class FOW_Controller : MonoBehaviour
    {
        private async void UpdateMaskTexture()
        {
            await UpdateColors();
            if (_updateCancellationSource.IsCancellationRequested) return;
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

                    if (_map_isLight[x, y])
                        progress -= deltaTime * _updateColorsSpeed;
                    else
                        progress += deltaTime * _updateColorsSpeed;

                    progress = Mathf.Clamp01(progress);
                    colors[y * MapSizeX + x] = new Color(progress, progress, progress, progress);
                    _visibleProgress[x, y] = progress;
                }
            }
        }
    }
}